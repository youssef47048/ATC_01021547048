using System.Globalization;

namespace Event_Management_System.Helpers
{
    public static class DateTimeHelper
    {
        public static string FormatDateTime(DateTime dateTime, string format = "MMMM dd, yyyy - h:mm tt")
        {
            var culture = CultureInfo.CurrentCulture;
            
            if (culture.Name == "ar-SA")
            {
                // For Arabic, use Arabic numerals and format
                var arCulture = new CultureInfo("ar-SA");
                return dateTime.ToString(format, arCulture);
            }
            
            // For other cultures, use default formatting
            return dateTime.ToString(format);
        }
        
        public static string FormatDate(DateTime dateTime, string format = "MMMM dd, yyyy")
        {
            var culture = CultureInfo.CurrentCulture;
            
            if (culture.Name == "ar-SA")
            {
                // For Arabic, use Arabic numerals and format
                var arCulture = new CultureInfo("ar-SA");
                return dateTime.ToString(format, arCulture);
            }
            
            // For other cultures, use default formatting
            return dateTime.ToString(format);
        }
    }
} 