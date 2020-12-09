using System;
using Presentation.Interface.Readers.Validators;

namespace Presentation.Interface.Readers
{
    public abstract class Reader
    {
        public readonly string Name;
        protected string Message="";

        protected Reader(string name)
        {
            Name = name;
        }

        public abstract object ReadRaw(object defaultValue = null);
    }

    public abstract class Reader<T> : Reader
    {
        protected readonly Validator<T> Validator;

        public Reader(string name, Validator<T> validator) : base(name)
        {
            Validator = validator ?? new AlwaysValid<T>();
            Message = $"Input {name}";
        }

        public ReaderPostProcessor<TU, T> Process<TU>(Func<T, TU> processor)
        {
            return new ReaderPostProcessor<TU, T>(this, processor);
        }

        public abstract T Read(object defaultValue = null);

        public override object ReadRaw(object defaultValue = null)
        {
            return Read(defaultValue);
        }

        protected T CastValue(object value)
        {
            if (!(value is T tValue))
                throw new InvalidPlaceholderTypeException(typeof(T), value.GetType());
            return tValue;
        }
        
        public virtual Reader<T> CustomMessage(string message)
        {
            Message = message;
            return this;
        }

        public class InvalidPlaceholderTypeException : Exception
        {
            public InvalidPlaceholderTypeException(Type expected, Type got) : base(
                $"Expected {expected} but got {got}.")
            {
            }
        }
    }
}