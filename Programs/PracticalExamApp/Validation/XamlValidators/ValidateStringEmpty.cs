using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace PracticalExamApp.Validation.XamlValidators
{
    public class ValidateStringEmpty : ValidationRule
    {
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, ErrorMessage);
            else
                return ValidationResult.ValidResult;
        }

        //public bool Validate(string value, out string message)
        //{
        //    message = "";
        //    if (string.IsNullOrWhiteSpace(value))
        //    {
        //        message = "Ciąg znaków jest pusty";
        //        return false;
        //    }
        //    return true;
        //}
    }
}
