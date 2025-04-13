using System;
using System.Collections.Generic;

namespace PlanMyNight.Models {
    /// <summary>
    /// Represents a saved exposure planning profile with all user-defined settings.
    /// </summary>
    public class ExposureProfile {
        public string ProfileName { get; set; } = "New Profile";

        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }

        public Dictionary<string, bool> FiltersSelected { get; set; } = new();
        public Dictionary<string, double> ExposurePerFilter { get; set; } = new();
        public Dictionary<string, double> AlreadyAcquiredPerFilter { get; set; } = new();
        public Dictionary<string, double> TargetProportion { get; set; } = new();

        public double SafetyTolerance { get; set; }

        public bool EnableAutofocusRGB { get; set; }
        public bool EnableAutofocusSHO { get; set; }
        public int AutofocusFrequency { get; set; }
        public double AutofocusDurationRGB { get; set; }
        public double AutofocusDurationSHO { get; set; }

        public bool EnableMeridianFlip { get; set; }
        public double FlipDuration { get; set; }

        public bool EnableDithering { get; set; }
        public int DitheringFrequency { get; set; }
        public double DitheringDuration { get; set; }

        public bool EnablePauseBetweenFrames { get; set; }
        public double PauseBetweenExposures { get; set; }
    }
}
