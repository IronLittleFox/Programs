using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CheckersWpfGame.Converters
{
    public class PawnDataToImageName : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return Binding.DoNothing;

            string? color = values[0]?.ToString()?.ToLower();
            string? type = values[1]?.ToString()?.ToLower();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri("Images\\" + color + type + ".png", UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            return bitmapImage;

            //return color + type + ".png";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
