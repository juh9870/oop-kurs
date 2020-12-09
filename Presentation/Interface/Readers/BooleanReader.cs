using System;

namespace Presentation.Interface.Readers
{
    public class BooleanReader : Reader<bool>
    {
        public BooleanReader(string name) : base(name, null)
        {
        }

        public override bool Read(object ignored = null)
        {
            ConsoleUtils.ClearLine();
            Console.WriteLine(Message);
            Console.Write("Press ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("y");
            Console.ResetColor();
            Console.Write(" for yes or ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("n");
            Console.ResetColor();
            Console.Write(" for no: ");

            var pos = Console.CursorLeft;
            do
            {
                if (Console.CursorLeft != pos)
                {
                    Console.CursorLeft = pos;
                    Console.Write(" ");
                    Console.CursorLeft = pos;
                }

                var key = Console.ReadKey();
                var ch = key.KeyChar;

                switch (ch)
                {
                    case 'y':
                    case 'Y':
                    case '\r':
                        Console.WriteLine();
                        return true;
                    case 'n':
                    case 'N':
                        Console.WriteLine();
                        return false;
                }
            } while (true);
        }
    }
}