using System;
using Presentation.Interface.Readers.Validators;

namespace Presentation.Interface.Readers
{
    public abstract class NumericReader<T> : Reader<T> where T : IComparable
    {
        public NumericReader(string name, Validator<T> validator) : base(name, validator)
        {
        }

        public NumericReader(string name, T min, T max) : base(name, new BoundsValidator<T>(min, max))
        {
        }


        public override T Read(object defaultValue = null)
        {
            var haveDefault = defaultValue != null;
            if (haveDefault) Console.WriteLine($"Press Enter to input {defaultValue}");

            T num;
            Console.Write(Message + ": ");
            var text = Console.ReadLine();
            bool parsed;
            var errorMessage = string.Empty;
            while (!(parsed = TryParse(text, out num)) || !Validator.Validate(num, out errorMessage) ||
                   string.IsNullOrEmpty(text) && haveDefault)
            {
                if (string.IsNullOrEmpty(text) && haveDefault) return CastValue(defaultValue);
                Console.CursorTop--;
                ConsoleUtils.ClearLine();
                if (!parsed)
                    Console.Write($"Invalid number. {Message}: ");
                else Console.Write($"{errorMessage} {Message}: ");
                text = Console.ReadLine();
            }

            return num;
        }

        protected abstract bool TryParse(string text, out T result);
    }
}