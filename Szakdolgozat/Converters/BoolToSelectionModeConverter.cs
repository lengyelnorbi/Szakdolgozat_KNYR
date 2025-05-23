using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Szakdolgozat.Converters
{
    public class BoolToSelectionModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? DataGridSelectionMode.Extended : DataGridSelectionMode.Single;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(DataGridSelectionMode.Extended);
        }
    }
}
