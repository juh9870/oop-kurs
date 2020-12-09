using System;

namespace Presentation.Interface.Readers
{
    public class ReaderPostProcessor<T, TU> : Reader<T>
    {
        private readonly Func<TU, T> _processor;
        private readonly Reader<TU> _reader;
        private Func<T, TU> _placeholderProcessor;

        public ReaderPostProcessor(Reader<TU> baseReader, Func<TU, T> processor) : base(baseReader.Name, null)
        {
            _processor = processor;
            _reader = baseReader;
        }

        public ReaderPostProcessor<T, TU> Default(Func<T, TU> defaultValueProcessor)
        {
            _placeholderProcessor = defaultValueProcessor;
            return this;
        }

        public override T Read(object defaultValue = null)
        {
            TU middleType;
            if (defaultValue != null)
            {
                if (_placeholderProcessor == null)
                    throw new NoDefaultValueProcessorException();
                middleType = _reader.Read(_placeholderProcessor.Invoke(CastValue(defaultValue)));
            }
            else
            {
                middleType = _reader.Read();
            }

            return _processor.Invoke(middleType);
        }

        public override Reader<T> CustomMessage(string message)
        {
            _reader.CustomMessage(message);
            return this;
        }

        public class NoDefaultValueProcessorException : Exception
        {
        }
    }
}