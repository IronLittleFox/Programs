using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ValidatorApp
{
    public class IntegerValidation : ValidationRule
    {
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int i;
            if (!int.TryParse(value.ToString(), out i))
                return new ValidationResult(false, "Podana wartość nie jest liczbą!");
            else
                if (i < (MinValue ?? i) || i > (MaxValue ?? i))
                return new ValidationResult(false, ErrorMessage);
            else
                return ValidationResult.ValidResult;
        }
    }
}
