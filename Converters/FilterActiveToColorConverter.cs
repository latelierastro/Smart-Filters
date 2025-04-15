using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PlanMyNight.Converters {
    public class FilterActiveToColorConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length < 2)
                return Brushes.Transparent;

            bool isActive = values[0] is bool b && b;
            string colorCode = values[1]?.ToString() ?? "#00000000";

            return isActive ? (SolidColorBrush)(new BrushConverter().ConvertFrom(colorCode))! : Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
