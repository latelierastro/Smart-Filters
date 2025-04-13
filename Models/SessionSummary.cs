using System.Collections.Generic;

namespace PlanMyNight.Models {
    public class SessionSummary {
        public Dictionary<string, double> TimePerFilter { get; set; } = new();
        public double TotalDithers { get; set; }
        public double TotalAutofocusRGB { get; set; }
        public double TotalAutofocusSHO { get; set; }
        public double UnusedTime { get; set; }

        public double TotalAutofocus => TotalAutofocusRGB + TotalAutofocusSHO;
    }
}
