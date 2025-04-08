using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlanMyNight.PlanMyNightDockables.ViewModels {
    /// <summary>
    /// ViewModel for the Plan My Night dockable window. Handles user input and data binding.
    /// </summary>
    public class PlanMyNightDockableViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Start time (hour and minute)
        private int _startHour;
        public int StartHour {
            get => _startHour;
            set { _startHour = value; OnPropertyChanged(); }
        }

        private int _startMinute;
        public int StartMinute {
            get => _startMinute;
            set { _startMinute = value; OnPropertyChanged(); }
        }

        // End time (hour and minute)
        private int _endHour;
        public int EndHour {
            get => _endHour;
            set { _endHour = value; OnPropertyChanged(); }
        }

        private int _endMinute;
        public int EndMinute {
            get => _endMinute;
            set { _endMinute = value; OnPropertyChanged(); }
        }

        // Safety tolerance percentage
        private double _tolerancePercent;
        public double TolerancePercent {
            get => _tolerancePercent;
            set { _tolerancePercent = value; OnPropertyChanged(); }
        }

        // Autofocus durations (RGB and SHO)
        private double _autofocusDurationRGB;
        public double AutofocusDurationRGB {
            get => _autofocusDurationRGB;
            set { _autofocusDurationRGB = value; OnPropertyChanged(); }
        }

        private double _autofocusDurationSHO;
        public double AutofocusDurationSHO {
            get => _autofocusDurationSHO;
            set { _autofocusDurationSHO = value; OnPropertyChanged(); }
        }

        // Meridian flip duration
        private double _meridianFlipDuration;
        public double MeridianFlipDuration {
            get => _meridianFlipDuration;
            set { _meridianFlipDuration = value; OnPropertyChanged(); }
        }

        // Dithering settings
        private double _ditherEvery;
        public double DitherEvery {
            get => _ditherEvery;
            set { _ditherEvery = value; OnPropertyChanged(); }
        }

        private double _ditherDuration;
        public double DitherDuration {
            get => _ditherDuration;
            set { _ditherDuration = value; OnPropertyChanged(); }
        }

        // Pause between frames
        private double _pauseBetweenFrames;
        public double PauseBetweenFrames {
            get => _pauseBetweenFrames;
            set { _pauseBetweenFrames = value; OnPropertyChanged(); }
        }

        // Filters selection (checkboxes)
        public bool IncludeL { get; set; }
        public bool IncludeR { get; set; }
        public bool IncludeG { get; set; }
        public bool IncludeB { get; set; }
        public bool IncludeHa { get; set; }
        public bool IncludeS { get; set; }
        public bool IncludeO { get; set; }

        // TODO: Add exposure time and already done values per filter
        // These can be implemented as dictionaries or separate properties per filter if preferred
    }
}
