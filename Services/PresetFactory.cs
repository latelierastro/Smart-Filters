using SmartFilters.Models;
using System.Collections.Generic;
using System.Linq;

namespace SmartFilters.Services {
    /// <summary>
    /// Factory to create default exposure presets.
    /// </summary>
    public static class PresetFactory {
        /// <summary>
        /// Returns the list of default preset profiles with their names.
        /// </summary>
        public static List<(string Name, ExposureProfile Profile)> GetDefaultPresets() {
            return new List<(string, ExposureProfile)>
            {
        ("★ LRGB Standard", new ExposureProfile {
            FiltersSelected = new() { ["L"] = true, ["R"] = true, ["G"] = true, ["B"] = true },
            TargetProportion = new() { ["L"] = 52, ["R"] = 16, ["G"] = 16, ["B"] = 16 }
        }),
        ("★ LRGB Sharp", new ExposureProfile {
            FiltersSelected = new() { ["L"] = true, ["R"] = true, ["G"] = true, ["B"] = true },
            TargetProportion = new() { ["L"] = 64, ["R"] = 12, ["G"] = 12, ["B"] = 12 }
        }),
        ("★ LRGB Pop", new ExposureProfile {
            FiltersSelected = new() { ["L"] = true, ["R"] = true, ["G"] = true, ["B"] = true },
            TargetProportion = new() { ["L"] = 40, ["R"] = 20, ["G"] = 20, ["B"] = 20 }
        }),
        ("★ SHO Standard", new ExposureProfile {
            FiltersSelected = new() { ["Ha"] = true, ["S"] = true, ["O"] = true },
            TargetProportion = new() { ["Ha"] = 33.3, ["S"] = 33.3, ["O"] = 33.3 }
        }),
        ("★ SHO + RGB Stars", new ExposureProfile {
            FiltersSelected = new() { ["Ha"] = true, ["S"] = true, ["O"] = true, ["R"] = true, ["G"] = true, ["B"] = true },
            TargetProportion = new() { ["Ha"] = 28, ["S"] = 28, ["O"] = 28, ["R"] = 5.3, ["G"] = 5.3, ["B"] = 5.3 }
        }),
        ("★ LRGB + Ha", new ExposureProfile {
            FiltersSelected = new() { ["L"] = true, ["R"] = true, ["G"] = true, ["B"] = true, ["Ha"] = true },
            TargetProportion = new() { ["L"] = 35, ["R"] = 15, ["G"] = 15, ["B"] = 15, ["Ha"] = 20 }
        }),
        ("★ HOO", new ExposureProfile {
            FiltersSelected = new() { ["Ha"] = true, ["O"] = true },
            TargetProportion = new() { ["Ha"] = 50, ["O"] = 50 }
        }),
        ("🧹 Clear All", new ExposureProfile {
            FiltersSelected = new() {
                ["L"] = false, ["R"] = false, ["G"] = false, ["B"] = false,
                ["Ha"] = false, ["S"] = false, ["O"] = false
            },
            ExposurePerFilter = new() {
                ["L"] = 0, ["R"] = 0, ["G"] = 0, ["B"] = 0,
                ["Ha"] = 0, ["S"] = 0, ["O"] = 0
            },
            AlreadyAcquiredPerFilter = new() {
                ["L"] = 0, ["R"] = 0, ["G"] = 0, ["B"] = 0,
                ["Ha"] = 0, ["S"] = 0, ["O"] = 0
            },
            TargetProportion = new() {
                ["L"] = 0, ["R"] = 0, ["G"] = 0, ["B"] = 0,
                ["Ha"] = 0, ["S"] = 0, ["O"] = 0
            },
            StartHour = 22,
            StartMinute = 0,
            EndHour = 4,
            EndMinute = 0,
            SafetyTolerance = 10,
            AutofocusFrequency = 60,
            EnableAutofocusRGB = false,
            EnableAutofocusSHO = false,
            AutofocusDurationRGB = 60,
            AutofocusDurationSHO = 60,
            EnableMeridianFlip = false,
            FlipDuration = 10,
            EnableDithering = false,
            DitheringFrequency = 8,
            DitheringDuration = 3,
            EnablePauseBetweenFrames = false,
            PauseBetweenExposures = 2
        }),
        ("User Preset", new ExposureProfile())
    };
        }


        public static ExposureProfile? GetPresetByName(string name) {
            return GetDefaultPresets().FirstOrDefault(p => p.Name == name).Profile;
        }

    }
}
