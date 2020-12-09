namespace Presentation.Interface.Readers.Validators
{
    public abstract class Validator<T>
    {
        public abstract bool Validate(T value, out string errorMessage);
    }
}