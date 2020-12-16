using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Data.Bundles
{
    [Serializable]
    public class BinaryBundle : Bundle
    {
        public BinaryBundle()
        {
            Values = new Dictionary<string, object>();
        }

        private Dictionary<string, object> Values { get; set; }

        protected override T GetInternal<T>(string key)
        {
            if (!Values.TryGetValue(key, out var obj)) return default;

            if (obj is T t) return t;
            try
            {
                return (T) Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        protected override List<T> GetListInternal<T>(string key)
        {
            if (!Values.TryGetValue(key, out var obj)) return null;
            if (!(obj is IEnumerable enumerable)) return null;
            
            var list = new List<T>();
            foreach (var o in enumerable)
            {
                if (!(o is T t)) return null;
                list.Add(t);
            }

            return list;

        }

        protected override void PutInternal<T>(string key, T value)
        {
            if (Values.ContainsKey(key)) throw new DuplicatingBundleKeyException(key);
            Values[key] = value;
        }

        public override bool Contains(string key)
        {
            return Values.ContainsKey(key);
        }

        protected override Bundle BundleFromBundlable(IBundlable bundlable)
        {
            var bundle = new BinaryBundle();
            bundle.Put(bundlable);
            return bundle;
        }


        public override byte[] Serialize()
        {
            using var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            Write(writer);
            return stream.ToArray();
        }

        private void Write(BinaryWriter writer)
        {
            writer.Write(Values.Count);
            foreach (var (key, value) in Values)
            {
                writer.Write(key);
                WriteElement(value,writer);
            }
        }
        private void WriteEnumerable(IEnumerable enumerable, BinaryWriter writer)
        {
            var os = enumerable as object[] ?? enumerable.Cast<object>().ToArray();
            writer.Write(os.Length);
            foreach (var o in os)
            {
                WriteElement(o,writer);
            }
        }
        private void WriteElement(object value, BinaryWriter writer)
        {
            switch (value)
                {
                    case short _short:
                        writer.Write((int)BinaryType.Short);
                        writer.Write(_short);
                        break;
                    case int _int:
                        writer.Write((int)BinaryType.Int);
                        writer.Write(_int);
                        break;
                    case long _long:
                        writer.Write((int)BinaryType.Long);
                        writer.Write(_long);
                        break;
                    case ushort _ushort:
                        writer.Write((int)BinaryType.Ushort);
                        writer.Write(_ushort);
                        break;
                    case uint _uint:
                        writer.Write((int)BinaryType.Uint);
                        writer.Write(_uint);
                        break;
                    case ulong _ulong:
                        writer.Write((int)BinaryType.Ulong);
                        writer.Write(_ulong);
                        break;
                    case float _float:
                        writer.Write((int)BinaryType.Float);
                        writer.Write(_float);
                        break;
                    case double _double:
                        writer.Write((int)BinaryType.Double);
                        writer.Write(_double);
                        break;
                    case decimal _decimal:
                        writer.Write((int)BinaryType.Decimal);
                        writer.Write(_decimal);
                        break;
                    case byte _byte:
                        writer.Write((int)BinaryType.Byte);
                        writer.Write(_byte);
                        break;
                    case sbyte _sbyte:
                        writer.Write((int)BinaryType.Sbyte);
                        writer.Write(_sbyte);
                        break;
                    case string _string:
                        writer.Write((int)BinaryType.String);
                        writer.Write(_string);
                        break;
                    case bool _bool:
                        writer.Write((int)BinaryType.Bool);
                        writer.Write(_bool);
                        break;
                    case DateTime dateTime:
                        writer.Write((int)BinaryType.DateTime);
                        writer.Write(dateTime.ToString("O"));
                        break;
                    case BinaryBundle bundle:
                        writer.Write((int)BinaryType.BinaryBundle);
                        bundle.Write(writer);
                        break;
                    case IEnumerable enumerable:
                        writer.Write((int)BinaryType.Enumerable);
                        WriteEnumerable(enumerable,writer);
                        break;
                    default:
                        throw new InvalidBinaryDataTypeException(value.GetType());
                }
        }

        public override void Deserialize(byte[] data)
        {
            using var stream = new MemoryStream(data);
            var bundle = Read(new BinaryReader(stream));
            Values = bundle.Values;
        }

        private static BinaryBundle Read(BinaryReader reader)
        {
            var bundle = new BinaryBundle();
            
            var l = reader.ReadInt32();
            for (var i = 0; i < l; i++)
            {
                var key = reader.ReadString();
                bundle.PutInternal(key,ReadElement(reader));
            }

            return bundle;
        }
        private static IEnumerable ReadEnumerable(BinaryReader reader)
        {
            var list = new ArrayList();
            var l = reader.ReadInt32();
            for (var i = 0; i < l; i++)
            {
                list.Add(ReadElement(reader));
            }

            return list;
        }

        private static object ReadElement(BinaryReader reader)
        {
            var num = reader.ReadInt32();
            var type = (BinaryType) num;
            switch (type)
            {
                case BinaryType.Short:
                    return reader.ReadInt16();
                case BinaryType.Int:
                    return reader.ReadInt32();
                case BinaryType.Long:
                    return reader.ReadInt64();
                case BinaryType.Ushort:
                    return reader.ReadUInt16();
                case BinaryType.Uint:
                    return reader.ReadUInt32();
                case BinaryType.Ulong:
                    return reader.ReadUInt64();
                case BinaryType.Float:
                    return reader.ReadSingle();
                case BinaryType.Double:
                    return reader.ReadDouble();
                case BinaryType.Decimal:
                    return reader.ReadDouble();
                case BinaryType.Byte:
                    return reader.ReadByte();
                case BinaryType.Sbyte:
                    return reader.ReadSByte();
                case BinaryType.String:
                    return reader.ReadString();
                case BinaryType.Bool:
                    return reader.ReadBoolean();
                case BinaryType.DateTime:
                    return DateTime.Parse(reader.ReadString(), CultureInfo.InvariantCulture,
                        DateTimeStyles.RoundtripKind);
                case BinaryType.BinaryBundle:
                    return Read(reader);
                case BinaryType.Enumerable:
                    return ReadEnumerable(reader);
                default:
                    throw new InvalidBinaryDataTypeException(num);
            }
        }

        public override BundleFactory Factory()
        {
            return BinaryBundleFactory.Instance;
        }

        private enum BinaryType
        {
            Short,
            Int,
            Long,
            Ushort,
            Uint,
            Ulong,
            Float,
            Double,
            Decimal,
            Byte,
            Sbyte,
            String,
            Bool,
            DateTime,
            BinaryBundle,
            Enumerable
        }
        public class InvalidBinaryDataTypeException : Exception
        {
            public InvalidBinaryDataTypeException(int type) : base($"Binary type {type} is unknown")
            {
            }
            public InvalidBinaryDataTypeException(Type type) : base($"Can't store {type} type objects")
            {
            }
        }
    }

    public class BinaryBundleFactory : BundleFactory
    {
        public static BinaryBundleFactory Instance = new BinaryBundleFactory();

        private BinaryBundleFactory()
        {
        }

        public override Bundle CreateBundle()
        {
            return new BinaryBundle();
        }
    }
}