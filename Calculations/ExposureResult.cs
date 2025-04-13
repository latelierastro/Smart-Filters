using PlanMyNight.Models;
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

        //Display Warnings :
        public List<WarningMessage> Warnings { get; set; } = new();

        //Display Summary:
        public SessionSummary Summary { get; set; } = new();
    }

    public class SessionSummary {
        public Dictionary<string, double> TimePerFilter { get; set; } = new();
        public double TotalDithers { get; set; }
        public double TotalAutofocusRGB { get; set; }
        public double TotalAutofocusSHO { get; set; }
        public double UnusedTime { get; set; }
        public double ToleranceLostMinutes { get; set; }  // ✅ Ici maintenant
    }
}
