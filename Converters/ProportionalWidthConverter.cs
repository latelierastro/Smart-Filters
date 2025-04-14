using System;
using System.Globalization;
using System.Windows.Data;

namespace PlanMyNight.Converters {
    public class ProportionalWidthConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if (values.Length >= 2 && values[0] is double proportion && values[1] is double totalWidth) {
                return proportion * totalWidth;
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
