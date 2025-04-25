using System.Reflection;
using System.Runtime.InteropServices;

// [MANDATORY] The following GUID is used as a unique identifier of the plugin. Generate a fresh one for your plugin!
[assembly: Guid("d81d3cc9-c02f-4743-9008-753d36368a33")]

// [MANDATORY] The assembly versioning
//Should be incremented for each new release build of a plugin
[assembly: AssemblyVersion("1.0.1.0")]
[assembly: AssemblyFileVersion("1.0.1.0")]

// [MANDATORY] The name of your plugin
[assembly: AssemblyTitle("Plan My Night")]
// [MANDATORY] A short description of your plugin
[assembly: AssemblyDescription("PlanMyNight is a plugin designed to optimize your astrophotography sessions by calculating the ideal number of exposures for each filter.")]

// The following attributes are not required for the plugin per se, but are required by the official manifest meta data

// Your name
[assembly: AssemblyCompany("Benoit SAINTOT")]
// The product name that this plugin is part of
[assembly: AssemblyProduct("Plan My Night")]
[assembly: AssemblyCopyright("Copyright © 2025 Benoit SAINTOT")]

// The minimum Version of N.I.N.A. that this plugin is compatible with
[assembly: AssemblyMetadata("MinimumApplicationVersion", "3.0.0.2017")]

// The license your plugin code is using
[assembly: AssemblyMetadata("License", "MPL-2.0")]
// The url to the license
[assembly: AssemblyMetadata("LicenseURL", "https://www.mozilla.org/en-US/MPL/2.0/")]
// The repository where your pluggin is hosted
[assembly: AssemblyMetadata("Repository", "https://github.com/latelierastro/Plan-My-Night/releases/tag/PlanMyNightV1.0.1.0")]

// The following attributes are optional for the official manifest meta data

//[Optional] Your plugin homepage URL - omit if not applicaple

//[Optional] Common tags that quickly describe your plugin
[assembly: AssemblyMetadata("Tags", "")]

//[Optional] A link that will show a log of all changes in between your plugin's versions
[assembly: AssemblyMetadata("ChangelogURL", "https://github.com/latelierastro/Plan-My-Night/blob/master/CHANGELOG.md")]

//[Optional] The url to a featured logo that will be displayed in the plugin list next to the name
[assembly: AssemblyMetadata("FeaturedImageURL", "https://github.com/latelierastro/Plan-My-Night/blob/master/PLANMYNIGHTIMAGE.png?raw=true\r\n")]
//[Optional] A url to an example screenshot of your plugin in action
[assembly: AssemblyMetadata("ScreenshotURL", "https://raw.githubusercontent.com/latelierastro/Plan-My-Night/refs/heads/master/Capture%20d'%C3%A9cran%202025-04-25%20184827.png")]
//[Optional] An additional url to an example example screenshot of your plugin in action
[assembly: AssemblyMetadata("AltScreenshotURL", "https://github.com/latelierastro/Plan-My-Night/blob/master/Capture%20d'%C3%A9cran%202025-04-25%20184921.png?raw=true")]
//[Optional] An in-depth description of your plugin
[assembly: AssemblyMetadata("LongDescription", @"")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
// [Unused]
[assembly: AssemblyConfiguration("")]
// [Unused]
[assembly: AssemblyTrademark("")]
// [Unused]
[assembly: AssemblyCulture("")]