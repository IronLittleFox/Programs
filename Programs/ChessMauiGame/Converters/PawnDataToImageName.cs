using System.Globalization;

namespace ChessMauiGame.Converters
{
    public class PawnDataToImageName : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return Binding.DoNothing;

            string? color = values[0]?.ToString()?.ToLower();
            string? type = values[1]?.ToString()?.ToLower();

            string imageName = "chess_" + color + "_"+ type + ".png";

            return imageName;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
