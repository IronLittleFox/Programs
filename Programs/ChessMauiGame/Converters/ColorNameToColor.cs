using Microsoft.Maui.Graphics.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMauiGame.Converters
{
    public class ColorNameToColor : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string colorName = value?.ToString() ?? "";

            ColorTypeConverter converter = new ColorTypeConverter();
            Color color = converter.ConvertFromInvariantString(colorName) as Color;
            return color;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
