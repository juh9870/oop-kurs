using System;
using System.Collections;
using System.Collections.Generic;
using Presentation.Interface.Readers;

namespace Presentation.Interface
{
    public class ClassManipulator<T>
    {
        private readonly List<string> _fieldNames;
        private readonly Func<T> _initializer;
        private readonly List<Reader> _readers;
        private readonly Dictionary<Reader, string> _readersNames;

        public ClassManipulator(Func<T> initializer)
        {
            _readersNames = new Dictionary<Reader, string>();
            _fieldNames = new List<string>();
            _readers = new List<Reader>();
            _initializer = initializer;
        }

        public ClassManipulator<T> AddReader<TU>(string fieldName, Reader<TU> reader)
        {
            _readersNames[reader] = fieldName;
            _fieldNames.Add(fieldName);
            _readers.Add(reader);
            return this;
        }

        public ClassManipulator<T> AddReader(MultiFieldReader reader)
        {
            _readers.Add(reader);
            _fieldNames.AddRange(reader.FieldNames());
            return this;
        }

        public T NewInstance()
        {
            return _initializer.Invoke();
        }

        public void Modify(T target)
        {
            Assign(target, InputValues());
        }

        public void Assign(T target, Dictionary<string, object> values)
        {
            var type = target.GetType();
            foreach (var key in _fieldNames)
            {
                var property = type.GetProperty(key);
                if (values.ContainsKey(key))
                    property?.SetValue(target, values[key]);
            }
        }

        public Dictionary<string, object> InputValues(Dictionary<string, object> defaultValues = null,
            bool askConfirmation = false)
        {
            var dict = new Dictionary<string, object>();
            foreach (var reader in _readers)
            {
                if (askConfirmation)
                    if (!new BooleanReader("").CustomMessage($"Input {reader.Name}?").Read())
                        continue;

                if (reader is MultiFieldReader multiReader)
                {
                    foreach (DictionaryEntry o in (IDictionary) multiReader.ReadRaw(defaultValues))
                        dict[(string) o.Key] = o.Value;
                }
                else
                {
                    var key = _readersNames[reader];
                    dict[key] = reader.ReadRaw(defaultValues?[key]);
                }

                Console.WriteLine();
            }

            return dict;
        }

        public Dictionary<string, object> GetValues(T source)
        {
            var type = source.GetType();
            var dict = new Dictionary<string, object>();
            foreach (var key in _fieldNames)
            {
                var property = type.GetProperty(key);
                if (property != null)
                    dict[key] = property.GetValue(source);
            }

            return dict;
        }

        public bool Match(T target, Dictionary<string, object> fields)
        {
            var type = target.GetType();
            foreach (var (key, value) in fields)
            {
                var prop = type.GetProperty(key);
                if (prop is null) continue;
                var curValue = prop.GetValue(target);
                if (!Equals(curValue, value) && (curValue is null || !curValue.ToString().Contains(value.ToString())))
                    return false;
            }

            return true;
        }
    }
}