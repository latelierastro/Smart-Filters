// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


namespace PlanMyNight.Models {
    /// <summary>
    /// REPRÉSENTE UN MESSAGE D’AVERTISSEMENT À AFFICHER DANS L’INTERFACE UTILISATEUR.
    /// UTILISÉ POUR SIGNALER DES INCOHÉRENCES OU INFORMATIONS IMPORTANTES APRÈS LE CALCUL.
    /// </summary>
    public class WarningMessage {
        // CONTENU DU MESSAGE D'AVERTISSEMENT À AFFICHER
        public string Message { get; set; }

        // NIVEAU DE GRAVITÉ : "Red" (GRAVE), "Orange" (IMPORTANT), "Yellow" (INFORMATIF)
        public string Level { get; set; }

        /// <summary>
        /// CONSTRUCTEUR DE LA CLASSE WarningMessage
        /// </summary>
        /// <param name="message">TEXTE DU MESSAGE À AFFICHER</param>
        /// <param name="level">NIVEAU DE GRAVITÉ : "Red", "Orange" OU "Yellow"</param>
        public WarningMessage(string message, string level) {
            Message = message;
            Level = level;
        }
    }
}
