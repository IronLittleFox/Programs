using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MinesweeperWpfGame.Converters
{
    public class NumberToImageName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strNumber = value?.ToString()?.ToLower();
            if (strNumber == "")
                strNumber = "empty";
            if (strNumber == "m")
                strNumber = "mine";
            if (strNumber == "?")
                strNumber = "questionMark";

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri("Images\\" + strNumber + ".png", UriKind.RelativeOrAbsolute);
            bitmapImage.EndInit();

            return bitmapImage;

            //return color + type + ".png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
