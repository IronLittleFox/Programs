using System;
using System.Collections.Generic;
using System.Text;

namespace PracticalExamApp.Validation.TypesOfValidation
{
    public class ValidateStringEmpty: ISpecyficValidation<string>
    {
        public bool Validate(string value, out string message)
        {
            message = "";
            if (string.IsNullOrWhiteSpace(value))
            {
                message = "Ciąg znaków jest pusty";
                return false;
            }
            return true;
        }
    }
}
