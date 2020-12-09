using System.Collections.Generic;

namespace Presentation.Interface.Readers.Validators
{
    public class MultiValidator<T> : Validator<T>
    {
        private readonly List<Validator<T>> _validators;

        public MultiValidator(params Validator<T>[] validators)
        {
            _validators = new List<Validator<T>>(validators);
        }


        public override bool Validate(T value, out string errorMessage)
        {
            foreach (var validator in _validators)
            {
                if (validator.Validate(value, out var message)) continue;
                errorMessage = message;
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}