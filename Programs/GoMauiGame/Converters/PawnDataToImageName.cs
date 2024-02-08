using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GoMauiGame.Converters
{
    public class PawnDataToImageName : IValueConverter
    {
        public object Convert(object? values, Type targetType, object? parameter, CultureInfo culture)
        {

            string color = values?.ToString()?.ToLower() ?? "";

            return color + "_go_pawn.png";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
