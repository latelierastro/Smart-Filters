using System.Collections.Generic;
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
        /// Get all available profile names.
        /// </summary>
        public static List<string> List() {
            return ProfileManager.GetAvailableProfiles();
        }

        /// <summary>
        /// Delete a profile by name.
        /// </summary>
        public static void Delete(string name) {
            ProfileManager.DeleteProfile(name);
        }
    }
}
