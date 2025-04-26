using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SmartFilters.Converters {
    public class SegmentCornerRadiusConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            var item = values[0];
            var itemsControl = values[1] as System.Windows.Controls.ItemsControl;

            if (item == null || itemsControl == null)
                return new CornerRadius(0);

            int index = itemsControl.Items.IndexOf(item);
            int totalCount = itemsControl.Items.Count;

            if (index == 0) // Premier segment
                return new CornerRadius(6, 0, 0, 6);
            else if (index == totalCount - 1) // Dernier segment
                return new CornerRadius(0, 6, 6, 0);
            else
                return new CornerRadius(0); // Segments intermédiaires
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
