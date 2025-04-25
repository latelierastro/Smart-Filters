// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using PlanMyNight.Models;

namespace PlanMyNight.Services {
    /// <summary>
    /// Static wrapper for profile access and management.
    /// </summary>
    public static class ProfileStorage {
        /// <summary>
        /// Save a profile under a given name.
        /// </summary>
        public static void Save(string name, ExposureProfile profile) {
            ProfileManager.SaveProfile(name, profile);
        }

        /// <summary>
        /// Load a profile by name.
        /// </summary>
        public static ExposureProfile? Load(string name) {
            return ProfileManager.LoadProfile(name);
        }

        /// <summary>
        /// Get all saved (user-created) profile names.
        /// </summary>
        public static List<string> List() {
            return ProfileManager.GetAvailableProfiles();
        }

        /// <summary>
        /// Get a combined list of presets and saved profiles.
        /// </summary>
        public static List<string> ListAll() {
            var presets = PresetFactory.GetDefaultPresets().Select(p => p.Name);
            var saved = ProfileManager.GetAvailableProfiles();
            return presets.Concat(saved).Distinct().ToList();
        }

        /// <summary>
        /// Delete a profile by name.
        /// </summary>
        public static void Delete(string name) {
            ProfileManager.DeleteProfile(name);
        }
    }
}
