﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


using SmartFilters.Calculations;
using SmartFilters.Models;
using SmartFilters.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;





namespace SmartFilters.SmartFiltersDockables.ViewModels {
    public class SmartFiltersDockableViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event Action<string>? ToastRequested;


        // =====================
        // GESTION DES HORAIRES
        // =====================
        private int _startHour;
        public int StartHour { get => _startHour; set { _startHour = value; OnPropertyChanged(); } }
        private int _startMinute;
        public int StartMinute { get => _startMinute; set { _startMinute = value; OnPropertyChanged(); } }
        private int _endHour;
        public int EndHour { get => _endHour; set { _endHour = value; OnPropertyChanged(); } }
        private int _endMinute;
        public int EndMinute { get => _endMinute; set { _endMinute = value; OnPropertyChanged(); } }


        // =====================
        // PARAMÈTRES DE SESSION
        // =====================
        private double _tolerancePercent;
        public double TolerancePercent { get => _tolerancePercent; set { _tolerancePercent = value; OnPropertyChanged(); } }

        private double _autofocusFrequency;
        public double AutofocusFrequency { get => _autofocusFrequency; set { _autofocusFrequency = value; OnPropertyChanged(); } }

        private bool _enableAutofocusRGB;
        public bool EnableAutofocusRGB { get => _enableAutofocusRGB; set { _enableAutofocusRGB = value; OnPropertyChanged(); } }

        private bool _enableAutofocusSHO;
        public bool EnableAutofocusSHO { get => _enableAutofocusSHO; set { _enableAutofocusSHO = value; OnPropertyChanged(); } }

        private bool _enableMeridianFlip;
        public bool EnableMeridianFlip { get => _enableMeridianFlip; set { _enableMeridianFlip = value; OnPropertyChanged(); } }

        private bool _enableDithering;
        public bool EnableDithering { get => _enableDithering; set { _enableDithering = value; OnPropertyChanged(); } }

        private bool _enablePauseBetweenFrames;
        public bool EnablePauseBetweenFrames { get => _enablePauseBetweenFrames; set { _enablePauseBetweenFrames = value; OnPropertyChanged(); } }

        private double _autofocusDurationRGB;
        public double AutofocusDurationRGB { get => _autofocusDurationRGB; set { _autofocusDurationRGB = value; OnPropertyChanged(); } }

        private double _autofocusDurationSHO;
        public double AutofocusDurationSHO { get => _autofocusDurationSHO; set { _autofocusDurationSHO = value; OnPropertyChanged(); } }

        private double _meridianFlipDuration;
        public double MeridianFlipDuration { get => _meridianFlipDuration; set { _meridianFlipDuration = value; OnPropertyChanged(); } }

        private double _ditherEvery;
        public double DitheringFrequency { get => _ditherEvery; set { _ditherEvery = value; OnPropertyChanged(); } }

        private double _ditherDuration;
        public double DitheringDuration { get => _ditherDuration; set { _ditherDuration = value; OnPropertyChanged(); } }

        private double _pauseBetweenFrames;
        public double PauseBetweenFrames { get => _pauseBetweenFrames; set { _pauseBetweenFrames = value; OnPropertyChanged(); } }



        // =====================
        // FILTRES SÉLECTIONNÉS ET ENTRÉES UTILISATEUR
        // =====================
        private bool _includeL; public bool IncludeL { get => _includeL; set { _includeL = value; OnPropertyChanged(); UpdateRemainingPercent(); } }
        private bool _includeR; public bool IncludeR { get => _includeR; set { _includeR = value; OnPropertyChanged(); UpdateRemainingPercent(); } }
        private bool _includeG; public bool IncludeG { get => _includeG; set { _includeG = value; OnPropertyChanged(); UpdateRemainingPercent(); } }
        private bool _includeB; public bool IncludeB { get => _includeB; set { _includeB = value; OnPropertyChanged(); UpdateRemainingPercent(); } }
        private bool _includeHa; public bool IncludeHa { get => _includeHa; set { _includeHa = value; OnPropertyChanged(); UpdateRemainingPercent(); } }
        private bool _includeS; public bool IncludeS { get => _includeS; set { _includeS = value; OnPropertyChanged(); UpdateRemainingPercent(); } }
        private bool _includeO; public bool IncludeO { get => _includeO; set { _includeO = value; OnPropertyChanged(); UpdateRemainingPercent(); } }

        private double _exposureTimeL; public double ExposureTimeL { get => _exposureTimeL; set { _exposureTimeL = value; OnPropertyChanged(); } }
        private int _hoursDoneL; public int HoursDoneL { get => _hoursDoneL; set { _hoursDoneL = value; OnPropertyChanged(); } }
        private int _minutesDoneL; public int MinutesDoneL { get => _minutesDoneL; set { _minutesDoneL = value; OnPropertyChanged(); } }
        private double _percentL; public double PercentL { get => _percentL; set { if (_percentL != value) { _percentL = value; OnPropertyChanged(); UpdateRemainingPercent(); } } }
       

        private double _exposureTimeR; public double ExposureTimeR { get => _exposureTimeR; set { _exposureTimeR = value; OnPropertyChanged(); } }
        private int _hoursDoneR; public int HoursDoneR { get => _hoursDoneR; set { _hoursDoneR = value; OnPropertyChanged(); } }
        private int _minutesDoneR; public int MinutesDoneR { get => _minutesDoneR; set { _minutesDoneR = value; OnPropertyChanged(); } }
        private double _percentR; public double PercentR { get => _percentR; set { if (_percentR != value) { _percentR = value; OnPropertyChanged(); UpdateRemainingPercent(); } } }
        
        private double _exposureTimeG; public double ExposureTimeG { get => _exposureTimeG; set { _exposureTimeG = value; OnPropertyChanged(); } }
        private int _hoursDoneG; public int HoursDoneG { get => _hoursDoneG; set { _hoursDoneG = value; OnPropertyChanged(); } }
        private int _minutesDoneG; public int MinutesDoneG { get => _minutesDoneG; set { _minutesDoneG = value; OnPropertyChanged(); } }
        private double _percentG; public double PercentG { get => _percentG; set { if (_percentG != value) { _percentG = value; OnPropertyChanged(); UpdateRemainingPercent(); } } }
        
        private double _exposureTimeB; public double ExposureTimeB { get => _exposureTimeB; set { _exposureTimeB = value; OnPropertyChanged(); } }
        private int _hoursDoneB; public int HoursDoneB { get => _hoursDoneB; set { _hoursDoneB = value; OnPropertyChanged(); } }
        private int _minutesDoneB; public int MinutesDoneB { get => _minutesDoneB; set { _minutesDoneB = value; OnPropertyChanged(); } }
        private double _percentB; public double PercentB { get => _percentB; set { if (_percentB != value) { _percentB = value; OnPropertyChanged(); UpdateRemainingPercent(); } } }
        
        private double _exposureTimeHa; public double ExposureTimeHa { get => _exposureTimeHa; set { _exposureTimeHa = value; OnPropertyChanged(); } }
        private int _hoursDoneHa; public int HoursDoneHa { get => _hoursDoneHa; set { _hoursDoneHa = value; OnPropertyChanged(); } }
        private int _minutesDoneHa; public int MinutesDoneHa { get => _minutesDoneHa; set { _minutesDoneHa = value; OnPropertyChanged(); } }
        private double _percentHa; public double PercentHa { get => _percentHa; set { if (_percentHa != value) { _percentHa = value; OnPropertyChanged(); UpdateRemainingPercent(); } } }
       
        private double _exposureTimeS; public double ExposureTimeS { get => _exposureTimeS; set { _exposureTimeS = value; OnPropertyChanged(); } }
        private int _hoursDoneS; public int HoursDoneS { get => _hoursDoneS; set { _hoursDoneS = value; OnPropertyChanged(); } }
        private int _minutesDoneS; public int MinutesDoneS { get => _minutesDoneS; set { _minutesDoneS = value; OnPropertyChanged(); } }
        private double _percentS; public double PercentS { get => _percentS; set { if (_percentS != value) { _percentS = value; OnPropertyChanged(); UpdateRemainingPercent(); } } }
        
        private double _exposureTimeO; public double ExposureTimeO { get => _exposureTimeO; set { _exposureTimeO = value; OnPropertyChanged(); } }
        private int _hoursDoneO; public int HoursDoneO { get => _hoursDoneO; set { _hoursDoneO = value; OnPropertyChanged(); } }
        private int _minutesDoneO; public int MinutesDoneO { get => _minutesDoneO; set { _minutesDoneO = value; OnPropertyChanged(); } }
        private double _percentO; public double PercentO { get => _percentO; set { if (_percentO != value) { _percentO = value; OnPropertyChanged(); UpdateRemainingPercent(); } } }


        // =========================
        // AFFICHAGE DES % RESTANTS 
        //==========================
        private double _remainingPercent;
        public double RemainingPercent {
            get => _remainingPercent;
            set {
                if (_remainingPercent != value) {
                    _remainingPercent = value;
                    OnPropertyChanged(nameof(RemainingPercent));
                }
            }
        }
        // mise à jour de la case de % restants
        private void UpdateRemainingPercent() {
            try {
                double sum = 0;
                if (IncludeL) sum += PercentL;
                if (IncludeR) sum += PercentR;
                if (IncludeG) sum += PercentG;
                if (IncludeB) sum += PercentB;
                if (IncludeS) sum += PercentS;
                if (IncludeHa) sum += PercentHa;
                if (IncludeO) sum += PercentO;

                RemainingPercent = Math.Round(100 - sum, 1);
            } catch {
                // En cas de crash au chargement initial, on affiche 100%
                RemainingPercent = 100;
            }
        }



        // =====================
        // RÉSULTATS CALCULÉS (NOMBRE D'IMAGES À ACQUÉRIR)
        // =====================
        private int _resultL;
        public int ResultL { get => _resultL; set { _resultL = value; OnPropertyChanged(); } }

        private int _resultR;
        public int ResultR { get => _resultR; set { _resultR = value; OnPropertyChanged(); } }

        private int _resultG;
        public int ResultG { get => _resultG; set { _resultG = value; OnPropertyChanged(); } }

        private int _resultB;
        public int ResultB { get => _resultB; set { _resultB = value; OnPropertyChanged(); } }

        private int _resultHa;
        public int ResultHa { get => _resultHa; set { _resultHa = value; OnPropertyChanged(); } }

        private int _resultS;
        public int ResultS { get => _resultS; set { _resultS = value; OnPropertyChanged(); } }

        private int _resultO;
        public int ResultO { get => _resultO; set { _resultO = value; OnPropertyChanged(); } }


        // =====================
        // SEGMENTS VISUELS DE LA ROUE À FILTRES
        // =====================
        public ObservableCollection<FilterSegment> FilterSegments { get; set; } // <-- Collection for binding




        // Méthode principale de calcul : évalue les résultats d'exposition en fonction des paramètres saisis
        public void CalculateResults() {
            // 1. Compute total available minutes
            int start = StartHour * 60 + StartMinute;
            int end = EndHour * 60 + EndMinute;
            if (end <= start) end += 24 * 60; // handles night sessions over midnight
            int totalAvailableMinutes = end - start;

            // 2. Build exposure request object
            var request = new ExposureRequest {
                TotalAvailableMinutes = totalAvailableMinutes,
                EnableAutofocusRGB = EnableAutofocusRGB,
                EnableAutofocusSHO = EnableAutofocusSHO,
                EnableDithering = EnableDithering,
                EnableMeridianFlip = EnableMeridianFlip,
                EnablePauseBetweenFrames = EnablePauseBetweenFrames,
                AutofocusDurationRGB = AutofocusDurationRGB,
                AutofocusDurationSHO = AutofocusDurationSHO,
                AutofocusFrequency = AutofocusFrequency,
                FlipDuration = MeridianFlipDuration,
                DitheringDuration = DitheringDuration,
                DitheringFrequency = DitheringFrequency,
                PauseBetweenExposures = EnablePauseBetweenFrames ? PauseBetweenFrames : 0,
                SafetyTolerance = TolerancePercent
            };

            // 3. Fill per-filter flags
            var flags = request.FiltersSelected;
            flags["L"] = IncludeL;
            flags["R"] = IncludeR;
            flags["G"] = IncludeG;
            flags["B"] = IncludeB;
            flags["Ha"] = IncludeHa;
            flags["S"] = IncludeS;
            flags["O"] = IncludeO;

            // 4. Fill exposure times
            var expos = request.ExposurePerFilter;
            expos["L"] = ExposureTimeL;
            expos["R"] = ExposureTimeR;
            expos["G"] = ExposureTimeG;
            expos["B"] = ExposureTimeB;
            expos["Ha"] = ExposureTimeHa;
            expos["S"] = ExposureTimeS;
            expos["O"] = ExposureTimeO;

            // 5. Fill already acquired durations (in minutes)
            var done = request.AlreadyAcquiredPerFilter;
            done["L"] = HoursDoneL * 60 + MinutesDoneL;
            done["R"] = HoursDoneR * 60 + MinutesDoneR;
            done["G"] = HoursDoneG * 60 + MinutesDoneG;
            done["B"] = HoursDoneB * 60 + MinutesDoneB;
            done["Ha"] = HoursDoneHa * 60 + MinutesDoneHa;
            done["S"] = HoursDoneS * 60 + MinutesDoneS;
            done["O"] = HoursDoneO * 60 + MinutesDoneO;

            // 6. Fill target percentages
            var targets = request.TargetProportion;
            targets["L"] = PercentL;
            targets["R"] = PercentR;
            targets["G"] = PercentG;
            targets["B"] = PercentB;
            targets["Ha"] = PercentHa;
            targets["S"] = PercentS;
            targets["O"] = PercentO;

            // 7. Perform the calculation
            var result = PlanCalculator.Calculate(request);
            Warnings = result.Warnings;

            // 8. Fill session summary
            TimePerFilter = result.Summary.TimePerFilter;
            TotalDithers = result.Summary.TotalDithers;
            TotalAutofocusRGB = (int)result.Summary.TotalAutofocusRGB;
            TotalAutofocusSHO = (int)result.Summary.TotalAutofocusSHO;
            TotalAutofocus = result.Summary.TotalAutofocusRGB + result.Summary.TotalAutofocusSHO;
            UnusedTime = result.Summary.UnusedTime;
            ToleranceLostMinutes = result.Summary.ToleranceLostMinutes;



            // 9. Update results
            ResultL = result.FramesToAcquire.GetValueOrDefault("L", 0);
            ResultR = result.FramesToAcquire.GetValueOrDefault("R", 0);
            ResultG = result.FramesToAcquire.GetValueOrDefault("G", 0);
            ResultB = result.FramesToAcquire.GetValueOrDefault("B", 0);
            ResultHa = result.FramesToAcquire.GetValueOrDefault("Ha", 0);
            ResultS = result.FramesToAcquire.GetValueOrDefault("S", 0);
            ResultO = result.FramesToAcquire.GetValueOrDefault("O", 0);

            Debug.WriteLine(result.Comment);
            OnPropertyChanged(nameof(AutofocusSummary));

            UpdateFilterSegments();

        }


        // =====================
        // MESSAGES D'AVERTISSEMENT
        // =====================
        private List<WarningMessage> _warnings = new();
        public List<WarningMessage> Warnings {
            get => _warnings; set {
                _warnings = value; OnPropertyChanged(nameof(Warnings));
            }
        }


        // =====================
        // BILAN DE LA SESSION
        // =====================

        private Dictionary<string, double> _timePerFilter = new();
        public Dictionary<string, double> TimePerFilter {
            get => _timePerFilter;
            set { _timePerFilter = value; OnPropertyChanged(); }
        }

        private double _totalDithers;
        public double TotalDithers {
            get => _totalDithers;
            set { _totalDithers = value; OnPropertyChanged(); }
        }


        private double _totalAutofocus;
        public double TotalAutofocus {
            get => _totalAutofocus;
            set { _totalAutofocus = value; OnPropertyChanged(); }
        }


        private double _unusedTime;
        public double UnusedTime {
            get => _unusedTime;
            set { _unusedTime = value; OnPropertyChanged(); }
        }

        private double _toleranceLostMinutes;
        public double ToleranceLostMinutes {
            get => _toleranceLostMinutes;
            set { _toleranceLostMinutes = value; OnPropertyChanged(); }
        }

        private int _totalAutofocusRGB;
        public int TotalAutofocusRGB {
            get => _totalAutofocusRGB;
            set { _totalAutofocusRGB = value; OnPropertyChanged(); }
        }

        private int _totalAutofocusSHO;
        public int TotalAutofocusSHO {
            get => _totalAutofocusSHO;
            set { _totalAutofocusSHO = value; OnPropertyChanged(); }
        }

        public string AutofocusSummary {
            get {
                // Durée totale : chaque autofocus RGB ou SHO dure un certain temps
                double totalMinutes =
                    Math.Round(TotalAutofocusRGB * AutofocusDurationRGB / 60.0 + TotalAutofocusSHO * AutofocusDurationSHO / 60.0);

                int totalCount = (int)(TotalAutofocusRGB + TotalAutofocusSHO);

                return $"Autofocus: {totalCount} operations ({totalMinutes} min)";
            }
        }




        // =====================
        // GESTION DES PROFILS UTILISATEUR
        // =====================
        private bool ContainsInvalidChars(string name) {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars()) {
                if (name.Contains(c))
                    return true;
            }
            return false;
        }

        private List<string> _profileNames = new();
        public List<string> ProfileNames {
            get => _profileNames;
            set { _profileNames = value; OnPropertyChanged(); }
        }

        private string _selectedProfileName;
        public string SelectedProfileName {
            get => _selectedProfileName;
            set { _selectedProfileName = value; OnPropertyChanged(); }
        }


        // Chargement d’un profil sélectionné dans la liste déroulante
        public void OnLoadProfileClicked() {
            if (!string.IsNullOrEmpty(SelectedProfileName)) {
                ExposureProfile? loaded = null;

                // 1. Tenter de charger depuis les fichiers utilisateurs
                loaded = ProfileStorage.Load(SelectedProfileName);

                // 2. Si non trouvé, tenter via les presets par nom
                if (loaded == null) {
                    var preset = PresetFactory.GetDefaultPresets()
                                              .FirstOrDefault(p => p.Name == SelectedProfileName);
                    if (preset.Profile != null)
                        loaded = preset.Profile;
                }

                // 3. Si on a un profil valide, on le charge
                if (loaded != null) {
                    LoadFromProfile(loaded);
                    ToastRequested?.Invoke($"📂 Profile '{SelectedProfileName}' loaded.");
                } else {
                    ToastRequested?.Invoke($"❌ Could not load profile '{SelectedProfileName}'.");
                }
            }
        }


        // Presets


        // Gestion des caractères invalides
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

        private bool IsProfileNameValid(string name) {
            return !string.IsNullOrWhiteSpace(name) && !name.Any(c => InvalidFileNameChars.Contains(c));
        }


        // Sauvegarde d’un profil, avec gestion des doublons et du nom de fichier
        public void OnSaveProfileClicked() {
            if (string.IsNullOrWhiteSpace(SelectedProfileName))
                return;

            // Check if profile name is valid
            if (SelectedProfileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) {
                ToastRequested?.Invoke("❌ Invalid profile name.");
                return;
            }

            bool profileExists = ProfileNames.Contains(SelectedProfileName);

            if (profileExists) {
                var result = MessageBox.Show(
                    $"A profile named '{SelectedProfileName}' already exists. Do you want to overwrite it?",
                    "Overwrite confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result != MessageBoxResult.Yes)
                    return;
            }

            var profile = CreateCurrentProfile();
            ProfileStorage.Save(SelectedProfileName, profile);

            // 🔁 Rafraîchir la liste des profils
            var updatedList = ProfileStorage.List();
            if (!ProfileNames.SequenceEqual(updatedList))
                ProfileNames = ProfileStorage.ListAll();


            ToastRequested?.Invoke($"📂 Profile '{SelectedProfileName}' saved.");
        }







        // Suppression d’un profil existant avec confirmation
        public void OnDeleteProfileClicked() {
            if (!string.IsNullOrEmpty(SelectedProfileName)) {
                bool profileExists = ProfileNames.Contains(SelectedProfileName);

                if (profileExists) {
                    var result = MessageBox.Show(
                        $"Are you sure you want to delete the profile '{SelectedProfileName}'?",
                        "Delete Confirmation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning
                    );

                    if (result == MessageBoxResult.Yes) {
                        ProfileStorage.Delete(SelectedProfileName);

                        // Refresh list
                        ProfileNames = ProfileStorage.ListAll(); // recharge tous les noms visibles

                        SelectedProfileName = ""; // Clear selection
                    }
                }
            }
        }




        // Génère un objet ExposureProfile à partir des données courantes (pour sauvegarde)
        private ExposureProfile CreateCurrentProfile() {
            return new ExposureProfile {
                StartHour = StartHour,
                StartMinute = StartMinute,
                EndHour = EndHour,
                EndMinute = EndMinute,
                SafetyTolerance = TolerancePercent,
                AutofocusFrequency = (int)AutofocusFrequency,
                EnableAutofocusRGB = EnableAutofocusRGB,
                EnableAutofocusSHO = EnableAutofocusSHO,
                AutofocusDurationRGB = AutofocusDurationRGB,
                AutofocusDurationSHO = AutofocusDurationSHO,
                EnableMeridianFlip = EnableMeridianFlip,
                FlipDuration = MeridianFlipDuration,
                EnableDithering = EnableDithering,
                DitheringFrequency = (int)DitheringFrequency,
                DitheringDuration = DitheringDuration,
                EnablePauseBetweenFrames = EnablePauseBetweenFrames,
                PauseBetweenExposures = PauseBetweenFrames,

                FiltersSelected = new Dictionary<string, bool> {
                    ["L"] = IncludeL,
                    ["R"] = IncludeR,
                    ["G"] = IncludeG,
                    ["B"] = IncludeB,
                    ["Ha"] = IncludeHa,
                    ["S"] = IncludeS,
                    ["O"] = IncludeO,
                },
                ExposurePerFilter = new Dictionary<string, double> {
                    ["L"] = ExposureTimeL,
                    ["R"] = ExposureTimeR,
                    ["G"] = ExposureTimeG,
                    ["B"] = ExposureTimeB,
                    ["Ha"] = ExposureTimeHa,
                    ["S"] = ExposureTimeS,
                    ["O"] = ExposureTimeO,
                },
                AlreadyAcquiredPerFilter = new Dictionary<string, double> {
                    ["L"] = HoursDoneL * 60 + MinutesDoneL,
                    ["R"] = HoursDoneR * 60 + MinutesDoneR,
                    ["G"] = HoursDoneG * 60 + MinutesDoneG,
                    ["B"] = HoursDoneB * 60 + MinutesDoneB,
                    ["Ha"] = HoursDoneHa * 60 + MinutesDoneHa,
                    ["S"] = HoursDoneS * 60 + MinutesDoneS,
                    ["O"] = HoursDoneO * 60 + MinutesDoneO,
                },
                TargetProportion = new Dictionary<string, double> {
                    ["L"] = PercentL,
                    ["R"] = PercentR,
                    ["G"] = PercentG,
                    ["B"] = PercentB,
                    ["Ha"] = PercentHa,
                    ["S"] = PercentS,
                    ["O"] = PercentO,
                }
            };
        }


        // Recharge toutes les données à partir d’un profil sauvegardé
        private void LoadFromProfile(ExposureProfile profile) {
            StartHour = profile.StartHour;
            StartMinute = profile.StartMinute;
            EndHour = profile.EndHour;
            EndMinute = profile.EndMinute;

            TolerancePercent = profile.SafetyTolerance;
            AutofocusFrequency = profile.AutofocusFrequency;
            EnableAutofocusRGB = profile.EnableAutofocusRGB;
            EnableAutofocusSHO = profile.EnableAutofocusSHO;
            AutofocusDurationRGB = profile.AutofocusDurationRGB;
            AutofocusDurationSHO = profile.AutofocusDurationSHO;

            EnableMeridianFlip = profile.EnableMeridianFlip;
            MeridianFlipDuration = profile.FlipDuration;
            EnableDithering = profile.EnableDithering;
            DitheringFrequency = profile.DitheringFrequency;
            DitheringDuration = profile.DitheringDuration;

            EnablePauseBetweenFrames = profile.EnablePauseBetweenFrames;
            PauseBetweenFrames = profile.PauseBetweenExposures;

            IncludeL = profile.FiltersSelected.GetValueOrDefault("L");
            IncludeR = profile.FiltersSelected.GetValueOrDefault("R");
            IncludeG = profile.FiltersSelected.GetValueOrDefault("G");
            IncludeB = profile.FiltersSelected.GetValueOrDefault("B");
            IncludeHa = profile.FiltersSelected.GetValueOrDefault("Ha");
            IncludeS = profile.FiltersSelected.GetValueOrDefault("S");
            IncludeO = profile.FiltersSelected.GetValueOrDefault("O");

            ExposureTimeL = profile.ExposurePerFilter.GetValueOrDefault("L");
            ExposureTimeR = profile.ExposurePerFilter.GetValueOrDefault("R");
            ExposureTimeG = profile.ExposurePerFilter.GetValueOrDefault("G");
            ExposureTimeB = profile.ExposurePerFilter.GetValueOrDefault("B");
            ExposureTimeHa = profile.ExposurePerFilter.GetValueOrDefault("Ha");
            ExposureTimeS = profile.ExposurePerFilter.GetValueOrDefault("S");
            ExposureTimeO = profile.ExposurePerFilter.GetValueOrDefault("O");

            var done = profile.AlreadyAcquiredPerFilter;
            HoursDoneL = (int)(done.GetValueOrDefault("L") / 60);
            MinutesDoneL = (int)(done.GetValueOrDefault("L") % 60);
            HoursDoneR = (int)(done.GetValueOrDefault("R") / 60);
            MinutesDoneR = (int)(done.GetValueOrDefault("R") % 60);
            HoursDoneG = (int)(done.GetValueOrDefault("G") / 60);
            MinutesDoneG = (int)(done.GetValueOrDefault("G") % 60);
            HoursDoneB = (int)(done.GetValueOrDefault("B") / 60);
            MinutesDoneB = (int)(done.GetValueOrDefault("B") % 60);
            HoursDoneHa = (int)(done.GetValueOrDefault("Ha") / 60);
            MinutesDoneHa = (int)(done.GetValueOrDefault("Ha") % 60);
            HoursDoneS = (int)(done.GetValueOrDefault("S") / 60);
            MinutesDoneS = (int)(done.GetValueOrDefault("S") % 60);
            HoursDoneO = (int)(done.GetValueOrDefault("O") / 60);
            MinutesDoneO = (int)(done.GetValueOrDefault("O") % 60);

            PercentL = profile.TargetProportion.GetValueOrDefault("L");
            PercentR = profile.TargetProportion.GetValueOrDefault("R");
            PercentG = profile.TargetProportion.GetValueOrDefault("G");
            PercentB = profile.TargetProportion.GetValueOrDefault("B");
            PercentS = profile.TargetProportion.GetValueOrDefault("S");
            PercentHa = profile.TargetProportion.GetValueOrDefault("Ha");
            PercentO = profile.TargetProportion.GetValueOrDefault("O");

            UpdateFilterSegments();
            UpdateRemainingPercent();
        }


        // Constructeur : initialise la liste des noms de profils et la roue de filtres
        public SmartFiltersDockableViewModel() {
            // CHARGE LA LISTE DES PROFILS EXISTANTS AU DÉMARRAGE
            ProfileNames = ProfileStorage.ListAll();


            byte opacity = 205; // OPACITÉ À 80 %

            // INITIALISE LA LISTE DES SEGMENTS DE COULEURS POUR LA ROUE ET LA BARRE DE RÉPARTITION
            FilterSegments = new ObservableCollection<FilterSegment>
            {
                new FilterSegment { FilterName = "L", Color = CreateBrush(opacity, 255, 255, 255), Proportion = PercentL },
                new FilterSegment { FilterName = "R", Color = CreateBrush(opacity, 255, 0, 0),     Proportion = PercentR },
                new FilterSegment { FilterName = "G", Color = CreateBrush(opacity, 0, 255, 0),     Proportion = PercentG },
                new FilterSegment { FilterName = "B", Color = CreateBrush(opacity, 0, 0, 255),     Proportion = PercentB },
                new FilterSegment { FilterName = "S", Color = CreateBrush(opacity, 255, 255, 0),   Proportion = PercentS },
                new FilterSegment { FilterName = "Ha", Color = CreateBrush(opacity, 255, 0, 255),  Proportion = PercentHa },
                new FilterSegment { FilterName = "O", Color = CreateBrush(opacity, 0, 255, 255),   Proportion = PercentO }
            }; UpdateRemainingPercent();

        }



        // MÉTHODE PRIVÉE POUR CRÉER UNE COULEUR AVEC OPACITÉ PERSONNALISÉE
        private SolidColorBrush CreateBrush(byte alpha, byte r, byte g, byte b) => new(Color.FromArgb(alpha, r, g, b));

        // MET À JOUR LA LISTE DES SEGMENTS DE FILTRES POUR LA ROUE ET LA BARRE DE RÉPARTITION
        public void UpdateFilterSegments() {
            FilterSegments.Clear();

            byte opacity = 205;

            FilterSegments.Add(new FilterSegment { FilterName = "L", Color = CreateBrush(opacity, 255, 255, 255), Proportion = IncludeL ? PercentL / 100.0 : 0, Label = $"L ({PercentL}%)" });
            FilterSegments.Add(new FilterSegment { FilterName = "R", Color = CreateBrush(opacity, 255, 0, 0), Proportion = IncludeR ? PercentR / 100.0 : 0, Label = $"R ({PercentR}%)" });
            FilterSegments.Add(new FilterSegment { FilterName = "G", Color = CreateBrush(opacity, 0, 255, 0), Proportion = IncludeG ? PercentG / 100.0 : 0, Label = $"G ({PercentG}%)" });
            FilterSegments.Add(new FilterSegment { FilterName = "B", Color = CreateBrush(opacity, 0, 0, 255), Proportion = IncludeB ? PercentB / 100.0 : 0, Label = $"B ({PercentB}%)" });
            FilterSegments.Add(new FilterSegment { FilterName = "S", Color = CreateBrush(opacity, 255, 255, 0), Proportion = IncludeS ? PercentS / 100.0 : 0, Label = $"S ({PercentS}%)" });
            FilterSegments.Add(new FilterSegment { FilterName = "Ha", Color = CreateBrush(opacity, 255, 0, 255), Proportion = IncludeHa ? PercentHa / 100.0 : 0, Label = $"Ha ({PercentHa}%)" });
            FilterSegments.Add(new FilterSegment { FilterName = "O", Color = CreateBrush(opacity, 0, 255, 255), Proportion = IncludeO ? PercentO / 100.0 : 0, Label = $"O ({PercentO}%)" });

            
        }


    }

}