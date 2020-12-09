using System;
using System.Collections.Generic;
using Presentation.Interface.Readers.Validators;

namespace Presentation.Interface.Readers
{
    public abstract class RangeReader<T> : MultiFieldReader<T> where T : IComparable
    {
        private readonly Reader<T> _firstReader;
        private readonly string _maxFieldName;
        private readonly string _minFieldName;
        private readonly MinMaxValidator _minMaxValidator;
        private readonly Reader<T> _secondReader;

        public RangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true,
            Validator<T> validator = null) : base(name)
        {
            var validator1 = validator;
            _minFieldName = minFieldName;
            _maxFieldName = maxFieldName;
            _minMaxValidator = new MinMaxValidator(default, allowZeroLengthRange);
            _firstReader = GetReader("Input min value", validator1);
            _secondReader = GetReader("Input max value", new MultiValidator<T>(validator1, _minMaxValidator));
        }

        public RangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, T min,
            T max) : this(name, minFieldName, maxFieldName, allowZeroLengthRange)
        {
            Validator<T> validator = new BoundsValidator<T>(min, max);
        }

        public override Dictionary<string, T> Read(Dictionary<string, object> defaultValues = null)
        {
            Console.WriteLine(Message);
            var min = _firstReader.Read(defaultValues?[_minFieldName]);
            _minMaxValidator.SetValue(min);
            var max = _secondReader.Read(defaultValues?[_maxFieldName]);
            return new Dictionary<string, T>
            {
                {_minFieldName, min},
                {_maxFieldName, max}
            };
        }

        public override List<string> FieldNames()
        {
            return new List<string>(new[] {_minFieldName, _maxFieldName});
        }

        protected abstract Reader<T> GetReader(string message, Validator<T> validator);

        private class MinMaxValidator : GreaterThanValidator<T>
        {
            public MinMaxValidator(T value, bool allowEqual) : base(value, allowEqual)
            {
            }

            public override bool Validate(T value, out string errorMessage)
            {
                if (base.Validate(value, out _))
                {
                    errorMessage = null;
                    return true;
                }

                errorMessage =
                    $"Max value must be greater than {(AllowEqual ? "or equal to " : "")} the min value ({Value})";
                return false;
            }

            public void SetValue(T value)
            {
                Value = value;
            }
        }
    }
}