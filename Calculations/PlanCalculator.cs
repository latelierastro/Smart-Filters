// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

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
            double afRGB = 0;
            double afSHO = 0;
            int totalDithers = 0;

            var selectedFilters = request.FiltersSelected.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();

            double flipLoss = request.EnableMeridianFlip ? request.FlipDuration : 0;
            double safeTime = request.TotalAvailableMinutes * (1 - request.SafetyTolerance / 100.0);
            double timeAvailable = safeTime - flipLoss;

            double totalAlreadyAcquired = selectedFilters.Sum(f => request.AlreadyAcquiredPerFilter.GetValueOrDefault(f, 0));
            double totalWeight = selectedFilters.Sum(f => request.TargetProportion.GetValueOrDefault(f, 0));
            if (totalWeight == 0) totalWeight = 1;

            double finalTargetTotal = totalAlreadyAcquired + timeAvailable;

            var targetTotalPerFilter = new Dictionary<string, double>();
            foreach (var filter in selectedFilters) {
                double proportion = request.TargetProportion.GetValueOrDefault(filter, 0) / totalWeight;
                targetTotalPerFilter[filter] = finalTargetTotal * proportion;
            }

            foreach (var filter in selectedFilters) {
                double alreadyAcquired = request.AlreadyAcquiredPerFilter.GetValueOrDefault(filter, 0);
                double remainingTarget = targetTotalPerFilter[filter] - alreadyAcquired;
                if (remainingTarget <= 0) {
                    result.FramesToAcquire[filter] = 0;
                    result.TimePlannedPerFilter[filter] = 0;
                    continue;
                }

                double exposureSec = request.ExposurePerFilter.GetValueOrDefault(filter, 0);
                if (exposureSec <= 0) {
                    result.FramesToAcquire[filter] = 0;
                    result.TimePlannedPerFilter[filter] = 0;
                    continue;
                }

                double pauseSec = request.PauseBetweenExposures;
                double exposureMin = exposureSec / 60.0;
                double pauseMin = pauseSec / 60.0;

                bool isRGB = new[] { "L", "R", "G", "B" }.Contains(filter);
                bool isSHO = new[] { "Ha", "S", "O" }.Contains(filter);
                bool enableAF = (isRGB && request.EnableAutofocusRGB) || (isSHO && request.EnableAutofocusSHO);
                double afDuration = isRGB ? request.AutofocusDurationRGB : request.AutofocusDurationSHO;
                afDuration /= 60.0;

                double ditherDuration = request.DitheringDuration / 60.0;

                int frames = 0;
                double elapsed = 0;
                double minutesSinceLastAF = 0;
                int framesSinceLastDither = 0;
                int localDithers = 0;

                // Initial AF
                if (enableAF && afDuration > 0) {
                    elapsed += afDuration;
                    minutesSinceLastAF = 0;
                    if (isRGB) afRGB++;
                    if (isSHO) afSHO++;
                }

                while (true) {
                    bool willNeedAF = enableAF && request.AutofocusFrequency > 0 && minutesSinceLastAF >= request.AutofocusFrequency;
                    bool willNeedDither = request.EnableDithering && request.DitheringFrequency > 0 && framesSinceLastDither >= request.DitheringFrequency;

                    double blockCost = exposureMin + pauseMin;
                    if (willNeedAF) blockCost += afDuration;
                    if (willNeedDither) blockCost += ditherDuration;

                    if (elapsed + blockCost > remainingTarget) break;

                    // Apply AF if needed
                    if (willNeedAF) {
                        elapsed += afDuration;
                        minutesSinceLastAF = 0;
                        if (isRGB) afRGB++;
                        if (isSHO) afSHO++;
                    }

                    // Pose et pause
                    elapsed += exposureMin + pauseMin;
                    minutesSinceLastAF += exposureMin + pauseMin;
                    framesSinceLastDither++;
                    frames++;

                    // Apply dither if needed
                    if (willNeedDither) {
                        elapsed += ditherDuration;
                        framesSinceLastDither = 0;
                        localDithers++;
                    }
                }

                totalDithers += localDithers;
                result.FramesToAcquire[filter] = frames;
                result.TimePlannedPerFilter[filter] = elapsed;
            }

            result.TotalUsedMinutes = result.TimePlannedPerFilter.Values.Sum();
            result.UnusedMinutes = timeAvailable - result.TotalUsedMinutes;
            result.TotalDithers = totalDithers;
            result.TotalAutofocusRGB = afRGB;
            result.TotalAutofocusSHO = afSHO;
            result.TotalLostMinutes = request.TotalAvailableMinutes - safeTime + flipLoss;
            result.Comment = $"Time usage: {Math.Round(result.TotalUsedMinutes, 1)} / {Math.Round(request.TotalAvailableMinutes, 1)} min";

            double toleranceLostMinutes = request.TotalAvailableMinutes * (request.SafetyTolerance / 100.0);
            result.Summary = new SessionSummary {
                TimePerFilter = result.TimePlannedPerFilter.ToDictionary(entry => entry.Key, entry => entry.Value),
                TotalDithers = totalDithers,
                TotalAutofocusRGB = afRGB,
                TotalAutofocusSHO = afSHO,
                UnusedTime = result.UnusedMinutes,
                ToleranceLostMinutes = toleranceLostMinutes
            };

            return result;
        }
    }
}
