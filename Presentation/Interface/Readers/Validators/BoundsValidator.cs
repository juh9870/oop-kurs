using System;

namespace Presentation.Interface.Readers.Validators
{
    public class BoundsValidator<T> : Validator<T> where T : IComparable
    {
        private T _max;
        private T _min;

        public BoundsValidator(T min, T max)
        {
            _min = min;
            _max = max;
        }

        public override bool Validate(T value, out string errorMessage)
        {
            errorMessage = $"Value {value} is out of [{_min};{_max}] range.";
            return _min.CompareTo(value) <= 0 && _max.CompareTo(value) >= 0;
        }
    }
}