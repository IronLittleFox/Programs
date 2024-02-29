using Microsoft.Maui.Graphics.Converters;
using System.Globalization;

namespace ConnectFourMauiGame.Converters
{
    public class ColorToSolidColorBrush : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string colorName = value?.ToString() ?? "";

            ColorTypeConverter converter = new ColorTypeConverter();
            Color? color = converter.ConvertFromInvariantString(colorName) as Color;
            Brush brush = new SolidColorBrush(color);
            
            return brush;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
