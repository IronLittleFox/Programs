using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MultiKonwersje.Converters
{
    public class IntsToColorConwerters: IMultiValueConverter
    {
        private Color ExtractColorFrom(object[] values)
        {
            byte red = System.Convert.ToByte((double)values[0]);
            byte green = System.Convert.ToByte((double)values[1]);
            byte blue = System.Convert.ToByte((double)values[2]);
            return Color.FromRgb(red, green, blue);
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var x = ExtractColorFrom(values);
            return new SolidColorBrush(x);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
