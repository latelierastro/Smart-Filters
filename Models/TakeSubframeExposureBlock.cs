using System;
using System.Text.Json.Serialization;

namespace PlanMyNight.Models {
    public class TakeSubframeExposureBlock {
        [JsonPropertyName("$type")]
        public string Type => "NINA.Sequencer.SequenceItem.Imaging.TakeSubframeExposure, NINA.Sequencer";

        public double ExposureTime { get; set; } = 300;
        public int Gain { get; set; } = 120;
        public int Offset { get; set; } = 15;

        public BinningMode Binning { get; set; } = new BinningMode { X = 1, Y = 1 };

        public string ImageType { get; set; } = "LIGHT";
        public int ExposureCount { get; set; } = 0;
        public double ROI { get; set; } = 0.8;

        public MetaData MetaData { get; set; } = new MetaData {
            Name = "TakeSubframeExposure",
            InstructionId = Guid.NewGuid().ToString()
        };
    }

    public class BinningMode {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class MetaData {
        public string Name { get; set; }
        public string InstructionId { get; set; }
    }
}
