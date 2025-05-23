using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace Szakdolgozat.Converters
{
    // ChartTypeToVisibilityConverter.cs
    public class ChartTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string chartType = value as string;
            string targetChartType = parameter as string;

            if (chartType == targetChartType)
                return Visibility.Visible;
            else if(chartType != "PieSeries" && targetChartType == "RowSeries")
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
