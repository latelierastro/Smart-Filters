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

            // 1. Calcule le total déjà acquis pour les filtres sélectionnés
            double totalAlreadyAcquired = selectedFilters.Sum(f => request.AlreadyAcquiredPerFilter.GetValueOrDefault(f, 0));

            // 2. Calcule le poids total des pourcentages assignés
            double totalWeight = selectedFilters.Sum(f => request.TargetProportion.GetValueOrDefault(f, 0));
            if (totalWeight == 0) totalWeight = 1;

            // 3. Objectif global à atteindre = déjà acquis + ce qu'on peut faire ce soir
            double finalTargetTotal = totalAlreadyAcquired + timeAvailable;

            // 4. On calcule la cible finale par filtre (en tenant compte de ce qui a déjà été fait)
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
                double timePerFrameMin = (exposureSec + pauseSec) / 60.0;

                bool isRGB = new[] { "L", "R", "G", "B" }.Contains(filter);
                bool isSHO = new[] { "Ha", "S", "O" }.Contains(filter);
                bool enableAF = (isRGB && request.EnableAutofocusRGB) || (isSHO && request.EnableAutofocusSHO);
                double afDuration = isRGB ? request.AutofocusDurationRGB : request.AutofocusDurationSHO;
                afDuration /= 60.0;

                int frames = 0;
                double elapsed = 0;
                double nextAF = request.AutofocusFrequency;
                int localDithers = 0;

                if (enableAF && afDuration > 0 && remainingTarget > 0) {
                    elapsed += afDuration;
                    if (isRGB) afRGB++;
                    if (isSHO) afSHO++;
                }

                while (elapsed + timePerFrameMin <= remainingTarget) {
                    elapsed += timePerFrameMin;
                    frames++;

                    if (request.EnableDithering && request.DitheringFrequency > 0 && frames % request.DitheringFrequency == 0) {
                        elapsed += request.DitheringDuration / 60.0;
                        localDithers++;
                    }

                    if (enableAF && request.AutofocusFrequency > 0 && elapsed >= nextAF) {
                        elapsed += afDuration;
                        nextAF += request.AutofocusFrequency;
                        if (isRGB) afRGB++;
                        if (isSHO) afSHO++;
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

            // Warnings (inchangés)
            double sumPercent = request.TargetProportion
                .Where(kvp => request.FiltersSelected.GetValueOrDefault(kvp.Key, false))
                .Sum(kvp => kvp.Value);

            if (sumPercent < 99.0 || sumPercent > 101.0) {
                result.Warnings.Add(new WarningMessage(
                    $"The total percentage of selected filters is {Math.Round(sumPercent, 1)}%. It should be 100%.",
                    "Orange"
                ));
            }

            if (result.UnusedMinutes > 1.0) {
                double totalPercent = request.TargetProportion
                    .Where(kvp => request.FiltersSelected.GetValueOrDefault(kvp.Key, false))
                    .Sum(kvp => kvp.Value);

                bool proportionsMatch = Math.Abs(totalPercent - 100.0) <= request.SafetyTolerance;

                if (proportionsMatch) {
                    result.Warnings.Add(new WarningMessage(
                        $"🎯 Objectives reached. You still have {Math.Round(result.UnusedMinutes, 1)} minutes available. You could add extra frames on a selected filter.",
                        "Green"
                    ));
                } else {
                    result.Warnings.Add(new WarningMessage(
                        $"There are {Math.Round(result.UnusedMinutes, 1)} unused minutes at the end of the session. Consider optimizing your plan.",
                        "Yellow"
                    ));
                }
            }

            foreach (var filter in request.FiltersSelected.Where(f => f.Value).Select(f => f.Key)) {
                if (result.FramesToAcquire.GetValueOrDefault(filter, 0) == 0) {
                    result.Warnings.Add(new WarningMessage(
                        $"Filter {filter} is selected but no exposure time can be allocated within the available session time.",
                        "Red"
                    ));
                }
            }

            if (!request.FiltersSelected.Any(kvp => kvp.Value)) {
                result.Warnings.Add(new WarningMessage(
                    "No filter is selected. Please select at least one filter to plan exposures.",
                    "Red"
                ));
            }

            if (request.FiltersSelected.Any(kvp => kvp.Value)) {
                bool allZeroExp = request.FiltersSelected
                    .Where(kvp => kvp.Value)
                    .All(kvp => request.ExposurePerFilter.GetValueOrDefault(kvp.Key, 0) <= 0);

                if (allZeroExp) {
                    result.Warnings.Add(new WarningMessage(
                        "All exposure durations are set to 0. Please define at least one valid exposure time.",
                        "Red"
                    ));
                }
            }

            foreach (var filter in selectedFilters) {
                double exp = request.ExposurePerFilter.GetValueOrDefault(filter, 0);
                if (exp <= 0) {
                    result.Warnings.Add(new WarningMessage(
                        $"Filter {filter} is selected but its exposure time is set to 0 s.",
                        "Red"
                    ));
                }
            }

            foreach (var filter in selectedFilters) {
                double exp = request.ExposurePerFilter.GetValueOrDefault(filter, 0);
                if (exp > 0 && request.PauseBetweenExposures > exp) {
                    result.Warnings.Add(new WarningMessage(
                        $"Pause between frames is longer than the exposure time for filter {filter}.",
                        "Red"
                    ));
                }
            }

            if (timeAvailable < 1) {
                result.Warnings.Add(new WarningMessage(
                    "The available session time is too short to plan any exposures.",
                    "Red"
                ));
            }

            bool hasSelectedFiltersWithZeroPercent = request.FiltersSelected
                .Where(kvp => kvp.Value)
                .Any(kvp => request.TargetProportion.GetValueOrDefault(kvp.Key, 0) == 0);

            if (hasSelectedFiltersWithZeroPercent) {
                result.Warnings.Add(new WarningMessage(
                    "Selected filters have no percentage assigned. Please adjust the distribution.",
                    "Red"
                ));
            }

            if (request.EnableDithering && request.DitheringFrequency <= 0) {
                result.Warnings.Add(new WarningMessage(
                    "Dithering is enabled but no frequency is defined.",
                    "Yellow"
                ));
            }

            return result;
        }
    }
}
