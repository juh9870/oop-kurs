using System.Collections.Generic;

namespace Presentation.Interface.Readers
{
    public abstract class MultiFieldReader : Reader
    {
        protected MultiFieldReader(string name) : base(name)
        {
            Message = $"Input {name}";
        }

        public abstract List<string> FieldNames();
    }

    public abstract class MultiFieldReader<T> : MultiFieldReader
    {
        protected MultiFieldReader(string name) : base(name)
        {
        }

        public abstract Dictionary<string, T> Read(Dictionary<string, object> defaultValues = null);

        public override object ReadRaw(object defaultValue = null)
        {
            return Read((Dictionary<string, object>) defaultValue);
        }
    }
}