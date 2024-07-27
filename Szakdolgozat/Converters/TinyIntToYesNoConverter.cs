using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Szakdolgozat.Converters
{
    public class TinyIntToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Int16 intValue)
            {
                return intValue == 1 ? "Igen" : "Nem";
            }
            return "Nem";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ComboBoxItem comboBoxItem)
            {
                if (comboBoxItem.Content is string strValue)
                {

                    return strValue == "Igen" ? 1 : 0;
                }
            }

            return 0;
        }
    }
}
