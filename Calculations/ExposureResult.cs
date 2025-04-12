using System.Collections.Generic;

namespace PlanMyNight.Calculations {
    /// <summary>
    /// Résultat du calcul de répartition des temps d’exposition par filtre.
    /// </summary>
    public class ExposureResult {
        public Dictionary<string, int> FramesToAcquire { get; set; } = new();
        public Dictionary<string, double> TimePlannedPerFilter { get; set; } = new();

        public double TotalUsedMinutes { get; set; }
        public double UnusedMinutes { get; set; }

        public string Comment { get; set; } = string.Empty;

        // Display loss of dithers ands AFs
        public int TotalDithers { get; set; }
        public double TotalAutofocusRGB { get; set; }
        public double TotalAutofocusSHO { get; set; }
        public double TotalLostMinutes { get; set; }

    }
}
