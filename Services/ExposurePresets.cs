using System.Collections.Generic;

namespace PlanMyNight.Models {
    public static class ExposurePresets {
        public static Dictionary<string, Dictionary<string, double>> PresetDefinitions = new() {
            ["LRGB Standard"] = new() { ["L"] = 52, ["R"] = 16, ["G"] = 16, ["B"] = 16 },
            ["LRGB Sharp"] = new() { ["L"] = 64, ["R"] = 12, ["G"] = 12, ["B"] = 12 },
            ["LRGB Pop"] = new() { ["L"] = 40, ["R"] = 20, ["G"] = 20, ["B"] = 20 },
            ["LRGB + Ha"] = new() { ["L"] = 35, ["R"] = 15, ["G"] = 15, ["B"] = 15, ["Ha"] = 20 },
            ["SHO"] = new() { ["Ha"] = 33.3, ["S"] = 33.3, ["O"] = 33.3 },
            ["SHO + RGB Stars"] = new() {
                ["Ha"] = 28,
                ["S"] = 28,
                ["O"] = 28,
                ["R"] = 5.3,
                ["G"] = 5.3,
                ["B"] = 5.3
            },
            ["User Preset 1"] = new() { } // à remplir manuellement
        };
    }
}
