using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace MultiKonwersje.Converters
{
    class DoubleColorToDesctriptionConverter : IValueConverter
    {
        public string ColorName { get; set; }

        //z kodu do widoku
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Double colorComponent)
            {
                return "Wartość koloru " + parameter?.ToString() + " " + colorComponent.ToString();
            }
            return "";
        }

        //z widoku do kodu
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
