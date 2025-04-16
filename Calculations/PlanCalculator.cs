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
                double timeRemaining = Math.Max(0, targetTotal - alreadyAcquired); // en minutes
                double exposureSec = request.ExposurePerFilter.GetValueOrDefault(filter, 1);
                double pauseSec = request.PauseBetweenExposures;
                double timePerFrameMin = (exposureSec + pauseSec) / 60.0;

                if (timePerFrameMin <= 0) {
                    result.FramesToAcquire[filter] = 0;
                    result.TimePlannedPerFilter[filter] = alreadyAcquired;
                    continue;
                }

                // Déterminer les pertes autofocus pour ce filtre
                bool isRGB = new[] { "L", "R", "G", "B" }.Contains(filter);
                bool isSHO = new[] { "Ha", "S", "O" }.Contains(filter);
                double autofocusLossPerFilter = 0;
                double autofocusDurationMin = 0;

                if ((isRGB && request.EnableAutofocusRGB) || (isSHO && request.EnableAutofocusSHO)) {
                    autofocusDurationMin = (isRGB ? request.AutofocusDurationRGB : request.AutofocusDurationSHO) / 60.0;
                    autofocusLossPerFilter += autofocusDurationMin;

                    if (request.AutofocusFrequency > 0) {
                        double remainingAfterFirstAF = timeRemaining - autofocusLossPerFilter;
                        int additionalAFs = (int)(remainingAfterFirstAF / request.AutofocusFrequency);
                        autofocusLossPerFilter += additionalAFs * autofocusDurationMin;

                        if (isRGB) afRGB += additionalAFs + 1;
                        if (isSHO) afSHO += additionalAFs + 1;
                    } else {
                        if (isRGB) afRGB += 1;
                        if (isSHO) afSHO += 1;
                    }
                }

                // Temps disponible pour les frames après autofocus
                double availableTime = timeRemaining - autofocusLossPerFilter;
                if (availableTime <= 0) {
                    result.FramesToAcquire[filter] = 0;
                    result.TimePlannedPerFilter[filter] = alreadyAcquired;
                    continue;
                }

                // Estimation brute
                int estimatedFrames = (int)Math.Floor(availableTime / timePerFrameMin);
                int finalFrames = estimatedFrames;
                double ditherLoss = 0;

                // Réduction itérative si pertes totales dépassent le temps dispo
                while (finalFrames > 0) {
                    ditherLoss = 0;
                    if (request.EnableDithering && request.DitheringFrequency > 0 && request.DitheringDuration > 0) {
                        int numDithers = finalFrames / (int)request.DitheringFrequency;
                        ditherLoss = numDithers * (request.DitheringDuration / 60.0);
                    }

                    double totalTime = finalFrames * timePerFrameMin + ditherLoss + autofocusLossPerFilter;

                    if (totalTime <= timeRemaining)
                        break;

                    finalFrames--; // on réduit jusqu’à être dans les clous
                }

                // Résultats finaux
                double totalPlanned = finalFrames * timePerFrameMin + ditherLoss + autofocusLossPerFilter;
                totalDithers += request.EnableDithering && request.DitheringFrequency > 0 && request.DitheringDuration > 0
                    ? finalFrames / (int)request.DitheringFrequency
                    : 0;

                result.FramesToAcquire[filter] = finalFrames;
                result.TimePlannedPerFilter[filter] = totalPlanned;
            }




            // Final result summary
            result.TotalUsedMinutes = result.TimePlannedPerFilter.Values.Sum();
            result.UnusedMinutes = timeAvailableAfterLoss - result.TotalUsedMinutes;
            if (result.TotalUsedMinutes > timeAvailableAfterLoss + 0.1) {
                result.Warnings.Add(new WarningMessage(
                    "The planned exposures exceed the available session time. Please reduce exposure durations or adjust your plan.",
                    "Red"
                ));
            }

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
            if (result.UnusedMinutes > 1.0) {
                // Vérifie si la somme des pourcentages est cohérente
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

            // 4. No filter selected
            if (!request.FiltersSelected.Any(kvp => kvp.Value)) {
                result.Warnings.Add(new WarningMessage(
                    "No filter is selected. Please select at least one filter to plan exposures.",
                    "Red"
                ));
            }

            // 5. All exposure durations are 0
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

            // 6. Filter selected with exposure = 0
            foreach (var filter in selectedFilters) {
                double exp = request.ExposurePerFilter.GetValueOrDefault(filter, 0);
                if (exp <= 0) {
                    result.Warnings.Add(new WarningMessage(
                        $"Filter {filter} is selected but its exposure time is set to 0 s.",
                        "Red"
                    ));
                }
            }

            // 7. Pause > exposure
            foreach (var filter in selectedFilters) {
                double exp = request.ExposurePerFilter.GetValueOrDefault(filter, 0);
                if (exp > 0 && request.PauseBetweenExposures > exp) {
                    result.Warnings.Add(new WarningMessage(
                        $"Pause between frames is longer than the exposure time for filter {filter}.",
                        "Red"
                    ));
                }
            }

            // 8. Time too short
            if (timeAvailableAfterLoss < 1) {
                result.Warnings.Add(new WarningMessage(
                    "The available session time is too short to plan any exposures.",
                    "Red"
                ));
            }

            // 9. Selected filters with 0% proportion
            bool hasSelectedFiltersWithZeroPercent = request.FiltersSelected
                .Where(kvp => kvp.Value)
                .Any(kvp => request.TargetProportion.GetValueOrDefault(kvp.Key, 0) == 0);

            if (hasSelectedFiltersWithZeroPercent) {
                result.Warnings.Add(new WarningMessage(
                    "Selected filters have no percentage assigned. Please adjust the distribution.",
                    "Red"
                ));
            }
            // Dithering enabled but no frequency
            if (request.EnableDithering && request.DitheringFrequency <= 0) {
                result.Warnings.Add(new WarningMessage(
                    "Dithering is enabled but no frequency is defined.",
                    "Yellow"
                ));
            }

            double toleranceLostMinutes = request.TotalAvailableMinutes * (request.SafetyTolerance / 100.0);

            // 10. Session Summary 
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
