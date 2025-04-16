using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PlanMyNight.Converters {
    /// <summary>
    /// CONVERTISSEUR QUI TRANSFORME UN NIVEAU D’ALERTE (EX. : "Red", "Orange") EN COULEUR VISUELLE.
    /// UTILISÉ POUR AFFICHER LES MESSAGES DE WARNINGS AVEC UN CODE COULEUR.
    /// </summary>
    public class WarningLevelToBrushConverter : IValueConverter {
        /// <summary>
        /// CONVERSION D’UNE CHAÎNE DE NIVEAU ("Red", "Orange", ...) EN BRUSH (COULEUR D’AFFICHAGE).
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value switch {
                "Red" => Brushes.OrangeRed,         // GRAVITÉ ÉLEVÉE
                "Orange" => Brushes.DarkOrange,     // GRAVITÉ MOYENNE
                "Yellow" => Brushes.DarkGoldenrod,  // INFO OU PRÉCAUTION
                "Green" => Brushes.ForestGreen,     // ÉTAT OK / SUCCÈS
                _ => Brushes.Gray                   // PAR DÉFAUT (INCONNU OU NON SPÉCIFIÉ)
            };
        }

        /// <summary>
        /// NON UTILISÉ - LA CONVERSION INVERSE N’EST PAS NÉCESSAIRE ICI.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
