using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperMauiGame.Converters
{
    public class NumberToImageName : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string strNumber = value?.ToString() ?? "";
            if (strNumber == "")
                strNumber = "empty_field.png";
            else if (strNumber == "?")
                strNumber = "question_mark.png";
            else if (strNumber == "M")
                strNumber = "mine.png";
            else if (strNumber == "0")
                strNumber = "zero.png";
            else if (strNumber == "0")
                strNumber = "zero.png";
            else if (strNumber == "1")
                strNumber = "one.png";
            else if (strNumber == "2")
                strNumber = "two.png";
            else if (strNumber == "3")
                strNumber = "three.png";
            else if (strNumber == "4")
                strNumber = "four.png";
            else if (strNumber == "5")
                strNumber = "five.png";
            else if (strNumber == "6")
                strNumber = "six.png";
            else if (strNumber == "7")
                strNumber = "seven.png";
            else if (strNumber == "8")
                strNumber = "eight.png";


            return strNumber;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
