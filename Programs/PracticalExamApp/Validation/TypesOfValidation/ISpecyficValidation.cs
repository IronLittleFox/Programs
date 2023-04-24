using System;
using System.Collections.Generic;
using System.Text;

namespace PracticalExamApp.Validation.TypesOfValidation
{
    public interface ISpecyficValidation<T>
    {
        bool Validate(T value, out string message);
    }
}
