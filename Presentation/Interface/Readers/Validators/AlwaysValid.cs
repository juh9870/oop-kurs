namespace Presentation.Interface.Readers.Validators
{
    public class AlwaysValid<T> : Validator<T>
    {
        public override bool Validate(T value, out string errorMessage)
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}