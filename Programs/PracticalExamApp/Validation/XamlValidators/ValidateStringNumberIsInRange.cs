using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace PracticalExamApp.Validation.XamlValidators
{
    class ValidateStringNumberIsInRange : ValidationRule
    {
        public string ErrorMessage { get; set; }

        /*public int ErrorMessage
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }*/

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MaximumProperty =
        //    DependencyProperty.Register("ErrorMessage", typeof(int), typeof(ValidateStringNumberIsInRange), new UIPropertyMetadata(0));

        public int LowRange { get; set; }
        public int HighRange { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value.ToString(), out int intValue) && (intValue <= LowRange || intValue >= HighRange))
                return new ValidationResult(false, ErrorMessage);
            else
                return ValidationResult.ValidResult;
        }

        //public bool Validate(string value, out string message)
        //{
        //    message = "";
        //    if (int.TryParse(value, out int intValue) && (intValue <= lowRange || intValue >= highRange))
        //    {
        //        message = $"Podany wartość jest spoza zakresu <{lowRange} ; {highRange}>";
        //        return false;
        //    }
        //    return true;
        //}
    }
}
