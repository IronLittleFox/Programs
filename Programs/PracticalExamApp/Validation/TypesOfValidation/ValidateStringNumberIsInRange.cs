using System;
using System.Collections.Generic;
using System.Text;

namespace PracticalExamApp.Validation.TypesOfValidation
{
    class ValidateStringNumberIsInRange: ISpecyficValidation<string>
    {
        private int lowRange;
        private int highRange;

        public ValidateStringNumberIsInRange(int lowRange, int highRange)
        {
            this.lowRange = lowRange;
            this.highRange = highRange;
        }

        public bool Validate(string value, out string message)
        {
            message = "";
            if (int.TryParse(value, out int intValue) && (intValue <= lowRange || intValue >= highRange))
            {
                message = $"Podany wartość jest spoza zakresu <{lowRange} ; {highRange}>";
                return false;
            }
            return true;
        }
    }
}
