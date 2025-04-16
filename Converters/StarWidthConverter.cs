using System;
using System.Globalization;
using System.Windows.Data;

namespace PlanMyNight.Converters {
    /// <summary>
    /// CONVERTISSEUR QUI CALCULE UNE LARGEUR PROPORTIONNELLE EN FONCTION D’UNE VALEUR ET DE LA LARGEUR TOTALE.
    /// UTILISÉ POUR LE SLIDER.
    /// </summary>
    public class StarWidthConverter : IMultiValueConverter {
        /// <summary>
        /// CONVERTIT UNE PROPORTION (ENTRE 0 ET 1) ET UNE LARGEUR TOTALE EN UNE LARGEUR EFFECTIVE.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            // VÉRIFIE QUE LES DEUX VALEURS SONT BIEN DES DOUBLES
            if (values.Length >= 2 && values[0] is double proportion && values[1] is double totalWidth)
                return proportion * totalWidth;

            // EN CAS D’ERREUR OU DE DONNÉES MANQUANTES, RENVOIE 0
            return 0;
        }

        /// <summary>
        /// NON UTILISÉ - CONVERSION INVERSE NON SUPPORTÉE.
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
