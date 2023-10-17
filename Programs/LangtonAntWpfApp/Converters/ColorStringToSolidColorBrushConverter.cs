using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace LangtonAntWpfApp.Converters
{
    public class ColorStringToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString)
            {
                try
                {
                    Color color = (Color)ColorConverter.ConvertFromString(colorString);
                    SolidColorBrush brush = new SolidColorBrush(color);
                    return brush;
                }
                catch (Exception)
                {
                    return new SolidColorBrush(Colors.Black);
                }
            }

            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
