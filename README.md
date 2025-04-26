# Smart Filters for N.I.N.A.

**Smart Filters** is a plugin for [N.I.N.A.](https://nighttime-imaging.eu/), designed to intelligently optimize your astrophotography sessions by dynamically distributing exposure time across different filters (L, R, G, B, Ha, OIII, SII).

Take full control of your imaging nights, maximize efficiency, and hit your target ratios — even under real-world constraints like meridian flips, autofocus routines, and unpredictable weather.

---

## ✨ Features

- ⚡ Automatic optimization of exposure time based on:
  - Available imaging window
  - Already acquired data
  - Meridian flips and their timing
  - Autofocus frequency and duration
  - Dithering needs
  - Pauses between exposures
- 🖥️ Seamless integration into N.I.N.A.'s Imaging tab
- 💾 Profile management: save and reload your favorite session plans
- 🏭 Factory presets for common astrophotography setups
- 📈 Interactive visualization:
  - Filter wheel display
  - Proportional filter segment chart
- 🔄 (Coming soon) Direct automatic sequence generation for N.I.N.A.'s **Advanced Sequencer**

---

## 📋 How to Use

### Set Acquisition Parameters

- Define the session start and end times.
- Select the filters to be used (L, R, G, B, Ha, SII, OIII).
- For each filter:
  - Enter the exposure time already acquired
  - Specify the unit exposure time
- Define the desired target percentage distribution for each filter.

### Fine-Tune Advanced Settings

- Tolerance (leave some buffer time)
- Autofocus frequency and duration per filter type
- Meridian flip timing
- Dithering frequency and duration
- Pause between frames

Then simply click **"Calculate"** to generate an optimized, efficient imaging plan!

---

## ❓ Support

Found a bug? Have suggestions?  
Feel free to [open an issue](https://github.com/latelierastro/SmartFiltersIssues) or reach out directly via **latelierastro@gmail.com**.

---

## 📜 License

This project is licensed under the **Mozilla Public License 2.0** (MPL-2.0).
