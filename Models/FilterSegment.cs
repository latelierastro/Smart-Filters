using System.Windows.Media;

namespace PlanMyNight.Models {
    public class FilterSegment {
        public string FilterName { get; set; }
        public Brush Color { get; set; }
        public double Proportion { get; set; }
        public string Label { get; set; } // pour le tooltip
    }
}

