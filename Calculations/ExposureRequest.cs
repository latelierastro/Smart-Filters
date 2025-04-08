using System.Collections.Generic;

namespace PlanMyNight.Calculations {
    /// <summary>
    /// Données saisies par l'utilisateur pour planifier les temps d'exposition.
    /// </summary>
    public class ExposureRequest {
        public double TotalAvailableMinutes { get; set; }

        public double AutofocusDurationRGB { get; set; }
        public double AutofocusDurationSHO { get; set; }
        public double AutofocusFrequency { get; set; }

        public double FlipDuration { get; set; }
        public double DitheringDuration { get; set; }
        public double DitheringFrequency { get; set; }
        public double PauseBetweenExposures { get; set; }

        public double SafetyTolerance { get; set; }

        public Dictionary<string, bool> FiltersSelected { get; set; } = new();
        public Dictionary<string, double> ExposurePerFilter { get; set; } = new();
        public Dictionary<string, double> AlreadyAcquiredPerFilter { get; set; } = new();
        public Dictionary<string, double> TargetProportion { get; set; } = new();
    }
}
