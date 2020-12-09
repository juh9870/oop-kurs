using System;
using System.Collections.Generic;

namespace Presentation.Interface.Readers
{
    public class ListReader<T> : Reader<List<T>>
    {
        private readonly Reader<T> _reader;

        public ListReader(string name, Reader<T> elementsReader) : base(name, null)
        {
            _reader = elementsReader;
        }

        public override List<T> Read(object defaultValue = null)
        {
            Console.WriteLine(Message);
            Console.WriteLine("Press enter to start inputting values.");
            List<T> defaultList = null;
            List<T> list = null;
            if (!(defaultValue is null))
            {
                defaultList = CastValue(defaultValue);
                Console.Write($"Press escape to input {{{string.Join(',', defaultList)}}}.");
            }
            else
            {
                Console.Write("Press escape to input empty list");
                list = new List<T>();
            }

            do
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape) return list ?? defaultList;
                if (key.Key == ConsoleKey.Enter)
                {
                    list ??= new List<T>();
                    var value = _reader.Read();
                    list.Add(value);

                    Console.WriteLine("Press enter to input more or escape to finish");
                    continue;
                }

                Console.CursorLeft--;
            } while (true);
        }
    }
}