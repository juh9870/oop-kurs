using System;
using System.Collections.Generic;

namespace Presentation.Interface.Readers
{
    public class ElementReader<T> : Reader<T>
    {
        private readonly ListPicker<T> _picker;

        public ElementReader(string name, TableOutput<T> table, IEnumerable<T> source) : base(name, null)
        {
            Message = $"Select {name}";
            _picker = new ListPicker<T>(table, Message, source);
        }

        public override T Read(object defaultValue = null)
        {
            if (defaultValue != null) _picker.HaveDefault = true;
            _picker.Start();
            return _picker.Picked ?? CastValue(defaultValue);
        }

        private class ListPicker<T> : ConsoleListUtils<T>
        {
            private readonly string _message;
            public bool HaveDefault;
            public T Picked;

            public ListPicker(TableOutput<T> table, string message, IEnumerable<T> list) : base(table, null)
            {
                _message = message;
                AddAction(new ConsoleAction(PickItem, $"Select {ObjectName}"), true, '1');
                List.AddRange(list);
                if (List.Count == 0) throw new EmptySourceException();
            }

            protected override void WriteControls()
            {
                Console.WriteLine(_message);
                AddAction(new ConsoleAction(Terminate, "Use default (old) value", int.MaxValue, () => HaveDefault),
                    true, '0');

                base.WriteControls();
            }

            private void PickItem()
            {
                Picked = List[IdReader.Read()];
                Terminate();
            }
        }

        public class EmptySourceException : Exception
        {
        }
    }
}