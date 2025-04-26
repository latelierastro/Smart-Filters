// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Generic;

namespace SmartFilters.Models {
    /// <summary>
    /// REPRÉSENTE UN PROFIL D’EXPOSITION ENREGISTRÉ CONTENANT TOUS LES RÉGLAGES DÉFINIS PAR L’UTILISATEUR.
    /// CE PROFIL PEUT ÊTRE SAUVEGARDÉ ET RECHARGÉ POUR FACILITER LA PLANIFICATION DE SESSIONS D’ASTROPHOTOGRAPHIE.
    /// </summary>
    public class ExposureProfile {
        // NOM DU PROFIL (POUR IDENTIFICATION DANS LA LISTE)
        public string ProfileName { get; set; } = "New Profile";

        // HEURE DE DÉBUT ET DE FIN DE LA SESSION (FORMAT 24H)
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }

        // INDICATEUR D’ACTIVATION PAR FILTRE (L, R, G, B, Ha, S, O)
        public Dictionary<string, bool> FiltersSelected { get; set; } = new();

        // TEMPS D’EXPOSITION UNITAIRE (EN SECONDES) POUR CHAQUE FILTRE
        public Dictionary<string, double> ExposurePerFilter { get; set; } = new();

        // DURÉE DÉJÀ ACQUISE (EN MINUTES) POUR CHAQUE FILTRE
        public Dictionary<string, double> AlreadyAcquiredPerFilter { get; set; } = new();

        // POURCENTAGE CIBLE POUR CHAQUE FILTRE (RÉPARTITION DE LA SESSION)
        public Dictionary<string, double> TargetProportion { get; set; } = new();

        // TOLÉRANCE DE SÉCURITÉ (POUR LES AJUSTEMENTS LIÉS AUX DÉCALAGES, PAUSES, ETC.)
        public double SafetyTolerance { get; set; }

        // AUTOFOCUS POUR FILTRES RGB
        public bool EnableAutofocusRGB { get; set; }

        // AUTOFOCUS POUR FILTRES SHO (NARROWBAND)
        public bool EnableAutofocusSHO { get; set; }

        // FRÉQUENCE D’AUTOFOCUS (TOUTES LES X MINUTES)
        public int AutofocusFrequency { get; set; }

        // DURÉE DE L’AUTOFOCUS RGB ET SHO (EN SECONDES)
        public double AutofocusDurationRGB { get; set; }
        public double AutofocusDurationSHO { get; set; }

        // FLIP MÉRIDIEN (ACTIVATION + DURÉE EN MINUTES)
        public bool EnableMeridianFlip { get; set; }
        public double FlipDuration { get; set; }

        // DITHERING (FREQUENCE ET DUREE)
        public bool EnableDithering { get; set; }
        public int DitheringFrequency { get; set; }       // NOMBRE DE FRAMES ENTRE CHAQUE DITHER
        public double DitheringDuration { get; set; }     // DURÉE DE CHAQUE DITHER (EN SECONDES)

        // PAUSE ENTRE LES EXPOSITIONS
        public bool EnablePauseBetweenFrames { get; set; }
        public double PauseBetweenExposures { get; set; } // EN SECONDES
    }
}
