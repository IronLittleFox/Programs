using System.Globalization;

namespace TicTacToeMauiGame.Converters
{
    public class DataToImageName : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string imageName = "empty.png";

            if (value is null)
            {
                return imageName;
            }

            if (value.ToString() == "X") 
            {
                imageName = "cross.png";
            }
            if (value.ToString() == "O")
            {
                imageName = "circle.png";
            }
            if (value.ToString() == "Draw")
            {
                imageName = "draw.png";
            }

            return imageName;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
