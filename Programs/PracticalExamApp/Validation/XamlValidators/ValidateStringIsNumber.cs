using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace PracticalExamApp.Validation.XamlValidators
{
    public class ValidateStringIsNumber : ValidationRule
    {
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(value.ToString(), out _))
                return new ValidationResult(false, ErrorMessage);
            else
                return ValidationResult.ValidResult;
        }
    }
}
