using System;

namespace Presentation.Interface.Readers.Validators
{
    public class GreaterThanValidator<T> : Validator<T> where T : IComparable
    {
        protected readonly bool AllowEqual;
        protected T Value;

        public GreaterThanValidator(T value, bool allowEqual)
        {
            Value = value;
            AllowEqual = allowEqual;
        }

        public override bool Validate(T value, out string errorMessage)
        {
            errorMessage = $"Value {value} must be greater than {(AllowEqual ? "or equal to" : "")} {Value}. ";
            var compResult = Value.CompareTo(value);
            return compResult < 0 || AllowEqual && compResult == 0;
        }
    }
}