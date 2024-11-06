using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicAPI.Custom
{
    public class FontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double width = (double)value;

            // Scale font size based on window width
            if (width >= 1920) return 25; // Desktop Large
            if (width >= 1366) return 20; // Laptop Large
            if (width >= 1024) return 17.5; // Laptop Small / Tablet Landscape
            if (width >= 768) return 15; // Tablet Portrait
            if (width >= 414) return 12; // Mobile Large
            return 10; // Mobile Small
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
