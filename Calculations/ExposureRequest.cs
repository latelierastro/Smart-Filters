// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


using System.Collections.Generic;

namespace PlanMyNight.Calculations {
    /// <summary>
    /// Données saisies par l'utilisateur pour planifier les temps d'exposition.
    /// </summary>
    public class ExposureRequest {
        public double TotalAvailableMinutes { get; set; }

        // --- Réglages liés aux pertes de temps ---
        public bool EnableAutofocusRGB { get; set; }
        public bool EnableAutofocusSHO { get; set; }
        public bool EnableDithering { get; set; }
        public bool EnableMeridianFlip { get; set; }
        public bool EnablePauseBetweenFrames { get; set; }


        public double AutofocusDurationRGB { get; set; }
        public double AutofocusDurationSHO { get; set; }
        public double AutofocusFrequency { get; set; }

        public double FlipDuration { get; set; }
        public double DitheringDuration { get; set; }
        public double DitheringFrequency { get; set; }
        public double PauseBetweenExposures { get; set; }

        public double SafetyTolerance { get; set; }

        // --- Données par filtre ---
        public Dictionary<string, bool> FiltersSelected { get; set; } = new();
        public Dictionary<string, double> ExposurePerFilter { get; set; } = new();
        public Dictionary<string, double> AlreadyAcquiredPerFilter { get; set; } = new();
        public Dictionary<string, double> TargetProportion { get; set; } = new();
    }
}
