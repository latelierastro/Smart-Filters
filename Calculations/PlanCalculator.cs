using Accord.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanMyNight.Calculations {
    /// <summary>
    /// Main computation class for the exposure planning plugin.
    /// </summary>
    public static class PlanCalculator {
        public static ExposureResult Calculate(ExposureRequest request) {
            var result = new ExposureResult();
            int totalDithers = 0;
            double afRGB = 0;
            double afSHO = 0;

            // Get selected filters
            var selectedFilters = request.FiltersSelected
                .Where(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();

            // Total already acquired for selected filters
            double totalAlreadyAcquired = selectedFilters
                .Sum(f => request.AlreadyAcquiredPerFilter.GetValueOrDefault(f, 0));

            // Loss due to meridian flip (already in minutes)
            double flipLoss = request.EnableMeridianFlip ? request.FlipDuration : 0;

            // Compute autofocus loss
            double autofocusLossRGB = 0;
            double autofocusLossSHO = 0;

            if (request.AutofocusFrequency > 0) {
                double numAutofocus = request.TotalAvailableMinutes / request.AutofocusFrequency;

                // Définir les groupes de filtres
                var rgbFilters = new[] { "L", "R", "G", "B" };
                var shoFilters = new[] { "Ha", "S", "O" };

                // Sommes des poids pour RGB et SHO
                double weightRGB = rgbFilters.Where(f => selectedFilters.Contains(f)).Sum(f => request.TargetProportion.GetValueOrDefault(f, 0));
                double weightSHO = shoFilters.Where(f => selectedFilters.Contains(f)).Sum(f => request.TargetProportion.GetValueOrDefault(f, 0));
                double totalWeightAF = weightRGB + weightSHO;
                if (totalWeightAF == 0) totalWeightAF = 1;

                // Répartition du nombre d’autofocus
                if (request.EnableAutofocusRGB) {
                    afRGB = numAutofocus * (weightRGB / totalWeightAF);
                    autofocusLossRGB = afRGB * (request.AutofocusDurationRGB / 60.0);
                }

                if (request.EnableAutofocusSHO) {
                    afSHO = numAutofocus * (weightSHO / totalWeightAF);
                    autofocusLossSHO = afSHO * (request.AutofocusDurationSHO / 60.0);
                }
            }


            // Total fixed losses
            double totalLoss = flipLoss + autofocusLossRGB + autofocusLossSHO;

            // Apply safety margin
            double safeTime = request.TotalAvailableMinutes * (1 - request.SafetyTolerance / 100.0);
            double timeAvailableAfterLoss = safeTime - totalLoss;

            // Total time that we want to reach overall (acquired + tonight)
            double finalTargetTotal = totalAlreadyAcquired + timeAvailableAfterLoss;

            // Total weight for distribution (only on selected filters)
            double totalWeight = selectedFilters.Sum(f => request.TargetProportion.GetValueOrDefault(f, 0));
            if (totalWeight <= 0) totalWeight = 1;

            // Target total per filter = proportion * finalTargetTotal
            var targetTotalPerFilter = new Dictionary<string, double>();
            foreach (var filter in selectedFilters) {
                double weight = request.TargetProportion.GetValueOrDefault(filter, 0);
                double target = finalTargetTotal * (weight / totalWeight);
                targetTotalPerFilter[filter] = target;
            }


            foreach (var filter in selectedFilters) {
                double alreadyAcquired = request.AlreadyAcquiredPerFilter.GetValueOrDefault(filter, 0);
                double targetTotal = targetTotalPerFilter[filter];
                double timeRemaining = Math.Max(0, targetTotal - alreadyAcquired);
                double exposureSec = request.ExposurePerFilter.GetValueOrDefault(filter, 1);
                double pauseSec = request.PauseBetweenExposures;
                double timePerFrameMin = (exposureSec + pauseSec) / 60.0;

                if (timePerFrameMin <= 0) {
                    result.FramesToAcquire[filter] = 0;
                    result.TimePlannedPerFilter[filter] = alreadyAcquired; // <- NE PAS OUBLIER ce temps déjà acquis
                    continue;
                }

                int estimatedFrames = (int)Math.Floor(timeRemaining / timePerFrameMin);

                double ditherLoss = 0;
                int finalFrames = estimatedFrames;
                if (request.EnableDithering && request.DitheringFrequency > 0 && request.DitheringDuration > 0) {
                    int numDithers = estimatedFrames / (int)request.DitheringFrequency;
                    totalDithers += numDithers;
                    ditherLoss = numDithers * (request.DitheringDuration / 60.0);
                    double timeLeft = timeRemaining - ditherLoss;
                    finalFrames = (int)Math.Floor(timeLeft / timePerFrameMin);
                }

                // TOTAL pour CE FILTRE = temps déjà acquis + nouveaux frames + pertes par dithering
                double totalPlanned = finalFrames * timePerFrameMin + ditherLoss;
                result.FramesToAcquire[filter] = finalFrames;
                result.TimePlannedPerFilter[filter] = totalPlanned;
            }



            // Final result summary
            result.TotalUsedMinutes = result.TimePlannedPerFilter.Values.Sum();
            result.UnusedMinutes = timeAvailableAfterLoss - result.TotalUsedMinutes;
            result.Comment = $"Time usage: {Math.Round(result.TotalUsedMinutes, 1)} / {Math.Round(request.TotalAvailableMinutes, 1)} min";
            
            //Display losses
            result.TotalDithers = totalDithers;
            result.TotalAutofocusRGB = afRGB;
            result.TotalAutofocusSHO = afSHO;
            result.TotalLostMinutes = request.TotalAvailableMinutes * (1 - request.SafetyTolerance / 100.0)
                                      - result.TotalUsedMinutes;

            
            return result;
        }
    }
}
