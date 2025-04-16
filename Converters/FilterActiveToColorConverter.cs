// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PlanMyNight.Converters {
    /// <summary>
    /// CONVERTISSEUR MULTI-VALEURS QUI RENVOIE UNE COULEUR UNIQUEMENT SI LE FILTRE EST ACTIF.
    /// VALEUR [0] : BOOLÉEN (FILTRE ACTIF OU NON)  VALEUR [1] : PINCEAU (COULEUR À UTILISER SI ACTIF)
    /// </summary>
    public class FilterActiveToColorConverter : IMultiValueConverter {
        /// <summary>
        /// MÉTHODE DE CONVERSION : RETOURNE LA COULEUR SI LE FILTRE EST ACTIF, SINON TRANSPARENT.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            // VÉRIFIE QUE DEUX VALEURS SONT PRÉSENTES
            if (values.Length < 2)
                return Brushes.Transparent;

            // EXTRAIT L'ÉTAT D'ACTIVATION ET LA COULEUR
            bool isActive = values[0] is bool b && b;
            Brush brush = values[1] as Brush;

            // RETOURNE LA COULEUR SI ACTIF, SINON TRANSPARENT
            return isActive && brush != null ? brush : Brushes.Transparent;
        }

        /// <summary>
        /// NON UTILISÉ - LA CONVERSION INVERSE N'EST PAS IMPLÉMENTÉE.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
