// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.


using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PlanMyNight.Converters {
    /// <summary>
    /// CONVERTISSEUR QUI AFFICHE UN ÉLÉMENT UNIQUEMENT SI LA LISTE CONTIENT AU MOINS UN ÉLÉMENT.
    /// </summary>
    public class ListCountToVisibilityConverter : IValueConverter {
        /// <summary>
        /// SI LA LISTE CONTIENT DES ÉLÉMENTS → VISIBILITY.VISIBLE, SINON → VISIBILITY.COLLAPSED.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is ICollection collection) {
                return collection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            // SI LA VALEUR N'EST PAS UNE COLLECTION, ON CACHE PAR DÉFAUT
            return Visibility.Collapsed;
        }

        /// <summary>
        /// NON UTILISÉ - LA CONVERSION INVERSE N'EST PAS NÉCESSAIRE.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
