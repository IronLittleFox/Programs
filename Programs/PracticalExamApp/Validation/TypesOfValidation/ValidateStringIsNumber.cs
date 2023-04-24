using System;
using System.Collections.Generic;
using System.Text;

namespace PracticalExamApp.Validation.TypesOfValidation
{
    class ValidateStringIsNumber: ISpecyficValidation<string>
    {
        public bool Validate(string value, out string message)
        {
            message = "";
            if (!int.TryParse(value, out _))
            {
                message = "Podana wartość nie jest liczbą";
                return false;
            }
            return true;
        }
    }
}
