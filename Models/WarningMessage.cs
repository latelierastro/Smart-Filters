namespace PlanMyNight.Models {
    public class WarningMessage {
        public string Message { get; set; }
        public string Level { get; set; } // "Red", "Orange", "Yellow"

        public WarningMessage(string message, string level) {
            Message = message;
            Level = level;
        }
    }
}
