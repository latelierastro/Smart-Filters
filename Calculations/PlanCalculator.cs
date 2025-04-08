using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanMyNight.Calculations {
    /// <summary>
    /// Classe principale de calcul du plugin.
    /// </summary>
    public static class PlanCalculator {
        public static ExposureResult Calculate(ExposureRequest request) {
            var result = new ExposureResult();

            var selectedFilters = request.FiltersSelected
                .Where(kvp => kvp.Value)
                .Select(kvp => kvp.Key)
                .ToList();

            double flipLoss = request.FlipDuration;
            double numAutofocus = request.TotalAvailableMinutes / request.AutofocusFrequency;
            double autofocusLossRGB = numAutofocus * request.AutofocusDurationRGB;
            double autofocusLossSHO = 0;

            if (selectedFilters.Any(f => f is "Ha" or "S" or "O")) {
                autofocusLossSHO = numAutofocus * request.AutofocusDurationSHO;
            }

            double totalLoss = flipLoss + autofocusLossRGB + autofocusLossSHO;
            double timeAvailableAfterLoss = request.TotalAvailableMinutes * (1 - request.SafetyTolerance / 100.0) - totalLoss;

            var rgb = new[] { "R", "G", "B" };
            var sho = new[] { "Ha", "S", "O" };

            double totalWeight = selectedFilters.Sum(f => request.TargetProportion.ContainsKey(f) ? request.TargetProportion[f] : 0);
            if (totalWeight <= 0) totalWeight = 1;

            foreach (var filter in selectedFilters) {
                double weight = request.TargetProportion.GetValueOrDefault(filter, 0);
                double timeForFilter = timeAvailableAfterLoss * (weight / totalWeight);

                double exposureSec = request.ExposurePerFilter.GetValueOrDefault(filter, 1);
                double pauseSec = request.PauseBetweenExposures;
                double timePerFrameMin = (exposureSec + pauseSec) / 60.0;

                int frames = (int)Math.Floor(timeForFilter / timePerFrameMin);
                double totalPlanned = frames * timePerFrameMin;

                result.FramesToAcquire[filter] = frames;
                result.TimePlannedPerFilter[filter] = totalPlanned;
            }

            result.TotalUsedMinutes = result.TimePlannedPerFilter.Values.Sum();
            result.UnusedMinutes = timeAvailableAfterLoss - result.TotalUsedMinutes;
            result.Comment = $"Utilisation du temps : {Math.Round(result.TotalUsedMinutes, 1)} / {Math.Round(request.TotalAvailableMinutes, 1)} min";

            return result;
        }
    }
}
