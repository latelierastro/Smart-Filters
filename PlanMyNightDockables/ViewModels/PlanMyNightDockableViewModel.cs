using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using PlanMyNight.Calculations;
using System.Linq;
using System.Collections.Generic;

namespace PlanMyNight.PlanMyNightDockables.ViewModels {
    public class PlanMyNightDockableViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // --- TIME ---
        private int _startHour;
        public int StartHour { get => _startHour; set { _startHour = value; OnPropertyChanged(); } }

        private int _startMinute;
        public int StartMinute { get => _startMinute; set { _startMinute = value; OnPropertyChanged(); } }

        private int _endHour;
        public int EndHour { get => _endHour; set { _endHour = value; OnPropertyChanged(); } }

        private int _endMinute;
        public int EndMinute { get => _endMinute; set { _endMinute = value; OnPropertyChanged(); } }

        // --- SETTINGS ---
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

        // --- FILTER FLAGS + INPUT ---
        private bool _includeL; public bool IncludeL { get => _includeL; set { _includeL = value; OnPropertyChanged(); } }
        private bool _includeR; public bool IncludeR { get => _includeR; set { _includeR = value; OnPropertyChanged(); } }
        private bool _includeG; public bool IncludeG { get => _includeG; set { _includeG = value; OnPropertyChanged(); } }
        private bool _includeB; public bool IncludeB { get => _includeB; set { _includeB = value; OnPropertyChanged(); } }
        private bool _includeHa; public bool IncludeHa { get => _includeHa; set { _includeHa = value; OnPropertyChanged(); } }
        private bool _includeS; public bool IncludeS { get => _includeS; set { _includeS = value; OnPropertyChanged(); } }
        private bool _includeO; public bool IncludeO { get => _includeO; set { _includeO = value; OnPropertyChanged(); } }

        private double _exposureTimeL; public double ExposureTimeL { get => _exposureTimeL; set { _exposureTimeL = value; OnPropertyChanged(); } }
        private int _hoursDoneL; public int HoursDoneL { get => _hoursDoneL; set { _hoursDoneL = value; OnPropertyChanged(); } }
        private int _minutesDoneL; public int MinutesDoneL { get => _minutesDoneL; set { _minutesDoneL = value; OnPropertyChanged(); } }
        private double _percentL; public double PercentL { get => _percentL; set { _percentL = value; OnPropertyChanged(); } }

        private double _exposureTimeR; public double ExposureTimeR { get => _exposureTimeR; set { _exposureTimeR = value; OnPropertyChanged(); } }
        private int _hoursDoneR; public int HoursDoneR { get => _hoursDoneR; set { _hoursDoneR = value; OnPropertyChanged(); } }
        private int _minutesDoneR; public int MinutesDoneR { get => _minutesDoneR; set { _minutesDoneR = value; OnPropertyChanged(); } }
        private double _percentR; public double PercentR { get => _percentR; set { _percentR = value; OnPropertyChanged(); } }

        private double _exposureTimeG; public double ExposureTimeG { get => _exposureTimeG; set { _exposureTimeG = value; OnPropertyChanged(); } }
        private int _hoursDoneG; public int HoursDoneG { get => _hoursDoneG; set { _hoursDoneG = value; OnPropertyChanged(); } }
        private int _minutesDoneG; public int MinutesDoneG { get => _minutesDoneG; set { _minutesDoneG = value; OnPropertyChanged(); } }
        private double _percentG; public double PercentG { get => _percentG; set { _percentG = value; OnPropertyChanged(); } }

        private double _exposureTimeB; public double ExposureTimeB { get => _exposureTimeB; set { _exposureTimeB = value; OnPropertyChanged(); } }
        private int _hoursDoneB; public int HoursDoneB { get => _hoursDoneB; set { _hoursDoneB = value; OnPropertyChanged(); } }
        private int _minutesDoneB; public int MinutesDoneB { get => _minutesDoneB; set { _minutesDoneB = value; OnPropertyChanged(); } }
        private double _percentB; public double PercentB { get => _percentB; set { _percentB = value; OnPropertyChanged(); } }

        private double _exposureTimeHa; public double ExposureTimeHa { get => _exposureTimeHa; set { _exposureTimeHa = value; OnPropertyChanged(); } }
        private int _hoursDoneHa; public int HoursDoneHa { get => _hoursDoneHa; set { _hoursDoneHa = value; OnPropertyChanged(); } }
        private int _minutesDoneHa; public int MinutesDoneHa { get => _minutesDoneHa; set { _minutesDoneHa = value; OnPropertyChanged(); } }
        private double _percentHa; public double PercentHa { get => _percentHa; set { _percentHa = value; OnPropertyChanged(); } }

        private double _exposureTimeS; public double ExposureTimeS { get => _exposureTimeS; set { _exposureTimeS = value; OnPropertyChanged(); } }
        private int _hoursDoneS; public int HoursDoneS { get => _hoursDoneS; set { _hoursDoneS = value; OnPropertyChanged(); } }
        private int _minutesDoneS; public int MinutesDoneS { get => _minutesDoneS; set { _minutesDoneS = value; OnPropertyChanged(); } }
        private double _percentS; public double PercentS { get => _percentS; set { _percentS = value; OnPropertyChanged(); } }

        private double _exposureTimeO; public double ExposureTimeO { get => _exposureTimeO; set { _exposureTimeO = value; OnPropertyChanged(); } }
        private int _hoursDoneO; public int HoursDoneO { get => _hoursDoneO; set { _hoursDoneO = value; OnPropertyChanged(); } }
        private int _minutesDoneO; public int MinutesDoneO { get => _minutesDoneO; set { _minutesDoneO = value; OnPropertyChanged(); } }
        private double _percentO; public double PercentO { get => _percentO; set { _percentO = value; OnPropertyChanged(); } }

        // --- RESULTS ---
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

            // 8. Update results
            ResultL = result.FramesToAcquire.GetValueOrDefault("L", 0);
            ResultR = result.FramesToAcquire.GetValueOrDefault("R", 0);
            ResultG = result.FramesToAcquire.GetValueOrDefault("G", 0);
            ResultB = result.FramesToAcquire.GetValueOrDefault("B", 0);
            ResultHa = result.FramesToAcquire.GetValueOrDefault("Ha", 0);
            ResultS = result.FramesToAcquire.GetValueOrDefault("S", 0);
            ResultO = result.FramesToAcquire.GetValueOrDefault("O", 0);

            Debug.WriteLine(result.Comment);
        }

    }
}
