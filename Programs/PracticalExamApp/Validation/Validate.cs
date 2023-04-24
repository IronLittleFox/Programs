using System;
using System.Collections.Generic;
using System.Text;

namespace PracticalExamApp.Validation
{
    public class Validate
    {
        private List<IValidationTypes> listOfValidators;

        public Validate()
        {
            listOfValidators = new List<IValidationTypes>();
        }

        public void AddValidator(IValidationTypes validator)
        {
            listOfValidators.Add(validator);
        }

        public bool Validation(out string message)
        {
            foreach(IValidationTypes validator in listOfValidators)
            {
                if (!validator.Validate(out message))
                {
                    return false;
                }
            }

            message = "";
            return true;
        }

        public bool Validation(out Dictionary<string, string> dictonaryError)
        {
            dictonaryError = new Dictionary<string, string>();

            foreach (IValidationTypes validator in listOfValidators)
            {
                if (!validator.Validate(out string message))
                {
                    if (!dictonaryError.ContainsKey(validator.Name))
                    {
                        dictonaryError.Add(validator.Name, "");
                    }
                    dictonaryError[validator.Name] = message;
                }
            }

            return dictonaryError.Count == 0;
        }
    }
}
