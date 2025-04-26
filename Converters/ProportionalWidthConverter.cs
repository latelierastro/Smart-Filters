// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


using System;
using System.Globalization;
using System.Windows.Data;

namespace SmartFilters.Converters {
    /// <summary>
    /// CONVERTISSEUR QUI CALCULE UNE LARGEUR PROPORTIONNELLE À PARTIR D’UNE VALEUR ENTRE 0 ET 1 ET DE LA LARGEUR TOTALE DISPONIBLE.
    /// </summary>
    public class ProportionalWidthConverter : IMultiValueConverter {
        /// <summary>
        /// PREND EN ENTRÉE UNE PROPORTION (EX : 0.3) ET UNE LARGEUR TOTALE (EX : 200), ET RENVOIE LA LARGEUR EFFECTIVE (EX : 60).
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            // VÉRIFIE QUE LES DEUX VALEURS SONT BIEN DES DOUBLES
            if (values.Length >= 2 && values[0] is double proportion && values[1] is double totalWidth) {
                return proportion * totalWidth;
            }

            // SI LES VALEURS SONT INVALIDES, RENVOIE 0
            return 0;
        }

        /// <summary>
        /// NON UTILISÉ - LA CONVERSION INVERSE N’EST PAS REQUISE POUR CE SCÉNARIO.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
