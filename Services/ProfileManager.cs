// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using SmartFilters.Models;

namespace SmartFilters.Services {
    /// <summary>
    /// Provides functionality to save, load, list and delete exposure planning profiles.
    /// </summary>
    public static class ProfileManager {
        private static readonly string ProfileDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SmartFilters", "Profiles");

        static ProfileManager() {
            // Ensure the profile directory exists
            Directory.CreateDirectory(ProfileDirectory);
        }

        /// <summary>
        /// Saves a profile with the given name.
        /// </summary>
        public static void SaveProfile(string name, ExposureProfile profile) {
            string path = GetProfilePath(name);
            string json = JsonSerializer.Serialize(profile, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Loads a profile by name.
        /// </summary>
        public static ExposureProfile? LoadProfile(string name) {
            string path = GetProfilePath(name);
            if (!File.Exists(path))
                return null;

            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<ExposureProfile>(json);
        }

        /// <summary>
        /// Returns the list of all saved profile names (without extension).
        /// </summary>
        public static List<string> GetAvailableProfiles() {
            var files = Directory.GetFiles(ProfileDirectory, "*.json");
            var names = new List<string>();

            foreach (var file in files)
                names.Add(Path.GetFileNameWithoutExtension(file));

            return names;
        }

        /// <summary>
        /// Deletes a saved profile.
        /// </summary>
        public static void DeleteProfile(string name) {
            string path = GetProfilePath(name);
            if (File.Exists(path))
                File.Delete(path);
        }

        private static string GetProfilePath(string name) {
            return Path.Combine(ProfileDirectory, name + ".json");
        }

        /// Création Presets Repartition
        
    }
}
