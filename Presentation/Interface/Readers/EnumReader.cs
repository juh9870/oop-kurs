using System;

namespace Presentation.Interface.Readers
{
    public class EnumReader<T> : Reader<T> where T : IComparable, IFormattable, IConvertible
    {
        private readonly Reader<int> _idReader;
        private readonly Array _values;

        public EnumReader(string name) : base(name, null)
        {
            if (!typeof(T).IsEnum) throw new NotAnEnumException();
            _values = Enum.GetValues(typeof(T));
            _idReader = new IntReader("Select a value", 0, _values.Length - 1);
        }

        public override T Read(object defaultValue = null)
        {
            var hasPlaceholder = defaultValue != null;
            if (hasPlaceholder)
                if (typeof(T) != defaultValue.GetType() && typeof(int) != defaultValue.GetType())
                    throw new InvalidPlaceholderTypeException(typeof(T), defaultValue.GetType());
            Console.WriteLine(Message);
            for (var i = 0; i < _values.Length; i++)
                Console.WriteLine($"{i}: {Enum.GetName(typeof(T), _values.GetValue(i))}");
            var index = hasPlaceholder ? _idReader.Read((int) defaultValue) : _idReader.Read();
            return (T) _values.GetValue(index);
        }

        public class NotAnEnumException : Exception
        {
        }
    }
}