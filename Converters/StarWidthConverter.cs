using System;
using System.Globalization;
using System.Windows.Data;

namespace PlanMyNight.Converters {
    public class StarWidthConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is double d)
                return d * 400; // Adaptable
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
