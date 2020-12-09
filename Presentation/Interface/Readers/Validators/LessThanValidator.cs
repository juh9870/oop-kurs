using System;

namespace Presentation.Interface.Readers.Validators
{
    public class LessThanValidator<T> : Validator<T> where T : IComparable
    {
        private readonly bool _allowEqual;
        private T _value;

        public LessThanValidator(T value, bool allowEqual)
        {
            _value = value;
            _allowEqual = allowEqual;
        }

        public override bool Validate(T value, out string errorMessage)
        {
            errorMessage = $"Value {value} must be lower than {(_allowEqual ? "or equal to " : "")} {_value}. ";
            var compResult = _value.CompareTo(value);
            return compResult > 0 || _allowEqual && compResult == 0;
        }
    }
}