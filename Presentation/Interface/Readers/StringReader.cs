using System;
using Presentation.Interface.Readers.Validators;

namespace Presentation.Interface.Readers
{
    public class StringReader : Reader<string>
    {
        public StringReader(string name, Validator<string> validator = null) : base(name, validator)
        {
        }

        public override string Read(object defaultValue = null)
        {
            if (!(defaultValue is null)) Console.WriteLine($"Press enter to input '{defaultValue}'");
            ConsoleUtils.ClearLine();
            Console.Write(Message + ": ");
            do
            {
                var text = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(text))
                {
                    if (!(defaultValue is null)) return defaultValue.ToString();
                    Console.CursorTop--;
                    ConsoleUtils.ClearLine();
                    Console.Write($"Input string empty. {Message}: ");
                }
                else
                {
                    if (!Validator.Validate(text, out var errorMessage))
                    {
                        Console.CursorTop--;
                        ConsoleUtils.ClearLine();
                        Console.Write($"{errorMessage} {Message}:");
                    }
                    else
                    {
                        return text;
                    }
                }
            } while (true);
        }
    }
}