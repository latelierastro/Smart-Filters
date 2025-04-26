// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


using System.Windows.Media;

namespace SmartFilters.Models {
    /// <summary>
    /// REPRÉSENTE UN SEGMENT DE FILTRE POUR LA VISUALISATION (EX : DANS LA ROUE DE COULEURS OU LA BARRE DE DISTRIBUTION).
    /// CHAQUE SEGMENT CONTIENT UN NOM DE FILTRE, UNE COULEUR, UNE PROPORTION ET UN LIBELLÉ POUR L'AFFICHAGE.
    /// </summary>
    
    
    public class FilterSegment {
        // NOM DU FILTRE (EX : "L", "R", "G", "Ha", ETC.)
        public string FilterName { get; set; }

        // COULEUR ASSOCIÉE À CE FILTRE (UTILISÉE POUR L'AFFICHAGE GRAPHIQUE)
        public Brush Color { get; set; }

        // PROPORTION DU TEMPS TOTAL REPRÉSENTÉE PAR CE FILTRE (VALEUR ENTRE 0 ET 1)
        public double Proportion { get; set; }

        // TEXTE ASSOCIÉ POUR AFFICHAGE (EX : "R (25%)") - UTILISÉ POUR LES TOOLTIPS OU LES LIBELLÉS
        public string Label { get; set; }
    }
}
