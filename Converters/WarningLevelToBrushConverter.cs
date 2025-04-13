using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PlanMyNight.Converters {
    public class WarningLevelToBrushConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value switch {
                "Red" => Brushes.OrangeRed,
                "Orange" => Brushes.DarkOrange,
                "Yellow" => Brushes.DarkGoldenrod,
                _ => Brushes.Gray
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}

