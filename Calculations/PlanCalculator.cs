using Accord.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using PlanMyNight.Models;


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

                // Determine if this filter is RGB or SHO
                bool isRGB = new[] { "L", "R", "G", "B" }.Contains(filter);
                bool isSHO = new[] { "Ha", "S", "O" }.Contains(filter);

                double autofocusLossPerFilter = 0;

                // Add autofocus at the beginning (systematic)
                if ((isRGB && request.EnableAutofocusRGB) || (isSHO && request.EnableAutofocusSHO)) {
                    double autofocusDuration = isRGB ? request.AutofocusDurationRGB : request.AutofocusDurationSHO;
                    autofocusLossPerFilter += autofocusDuration / 60.0; // convert to minutes

                    // Add periodic autofocus if frequency is set
                    if (request.AutofocusFrequency > 0) {
                        double timeLeftAfterFirstAF = timeRemaining - autofocusLossPerFilter;
                        int additionalAF = (int)(timeLeftAfterFirstAF / request.AutofocusFrequency);
                        autofocusLossPerFilter += additionalAF * (autofocusDuration / 60.0);

                        // Update autofocus count summary
                        if (isRGB) afRGB += additionalAF + 1;
                        if (isSHO) afSHO += additionalAF + 1;
                    } else {
                        // Just the first autofocus
                        if (isRGB) afRGB += 1;
                        if (isSHO) afSHO += 1;
                    }
                }


                // TOTAL pour CE FILTRE = temps déjà acquis + nouveaux frames + pertes par dithering
                double totalPlanned = finalFrames * timePerFrameMin + ditherLoss + autofocusLossPerFilter;
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

            // ---------- WARNINGS ----------

            // 1. Sum of selected filter percentages ≠ 100%
            double sumPercent = request.TargetProportion
                .Where(kvp => request.FiltersSelected.GetValueOrDefault(kvp.Key, false))
                .Sum(kvp => kvp.Value);

            if (sumPercent < 99.0 || sumPercent > 101.0) {
                result.Warnings.Add(new WarningMessage(
                    $"The total percentage of selected filters is {Math.Round(sumPercent, 1)}%. It should be 100%.",
                    "Orange"
                ));
            }

            // 2. Unused minutes at the end of the session
            if (result.UnusedMinutes > 10.0) {
                result.Warnings.Add(new WarningMessage(
                    $"There are {Math.Round(result.UnusedMinutes, 1)} unused minutes at the end of the session. Consider optimizing your plan.",
                    "Yellow"
                ));
            }

            // 3. Selected filter with no exposure time planned
            foreach (var filter in request.FiltersSelected.Where(f => f.Value).Select(f => f.Key)) {
                double timePlanned = result.TimePlannedPerFilter.GetValueOrDefault(filter, 0);
                if (result.FramesToAcquire.GetValueOrDefault(filter, 0) == 0) {
                    double alreadyAcquired = request.AlreadyAcquiredPerFilter.GetValueOrDefault(filter, 0);
                    double target = targetTotalPerFilter.GetValueOrDefault(filter, 0);

                    if (alreadyAcquired >= target - 0.5) {
                        result.Warnings.Add(new WarningMessage(
                            $"Filter {filter} is selected but no additional exposure time is needed to reach your target.",
                            "Yellow"
                        ));
                    } else {
                        result.Warnings.Add(new WarningMessage(
                            $"Filter {filter} is selected but no exposure time can be allocated within the available session time.",
                            "Red"
                        ));
                    }
                }
            }

            // 4. Session Summary 
            result.Summary = new SessionSummary {
                TimePerFilter = result.TimePlannedPerFilter.ToDictionary(entry => entry.Key, entry => entry.Value),
                TotalDithers = totalDithers,
                TotalAutofocusRGB = afRGB,
                TotalAutofocusSHO = afSHO,
                UnusedTime = result.UnusedMinutes
            };

            return result;
        }
    }
}
