using System;
using System.Globalization;

namespace Szakdolgozat.Specials
{

    public class DateTimeParser
    {
        public static DateTime? ParseDateTime(string dateString)
        {
            // Define possible date formats
            string[] formats = new[]
            {
            "yyyyMMdd",       // 20240616
            "yyyy.MM.dd",     // 2024.06.16
            "yyyy-MM-dd",     // 2024-06-16
            "yyyy/MM/dd",     // 2024/06/16
            "yyyy.MM-dd",     // 2024.06-16
            "yyyy-MM/dd",     // 2024-06/16
            "yyyy/MM-dd",     // 2024/06-16
            "yyyy.MMdd",      // 2024.0616
            "yyyy/MMdd",      // 2024/0616
            "yyyy-MMdd"       // 2024-0616
        };

            // Try to parse the string into a DateTime object
            if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                return result.Date; // Return the parsed DateTime object
            }

            return null; // Return null if parsing failed
        }
    }

}
