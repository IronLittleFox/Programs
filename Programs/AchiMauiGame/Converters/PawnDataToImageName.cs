using System.Globalization;

namespace AchiMauiGame.Converters
{
    public class PawnDataToImageName : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
                return Binding.DoNothing;

            string? color = value.ToString()?.ToLower();

            string imageName = "achi_" + color + ".png";

            return imageName;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
