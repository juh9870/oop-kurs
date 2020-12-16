using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Data.Bundles
{
    public class XmlBundle : Bundle
    {
        private XElement _document;

        public XmlBundle()
        {
            _document = new XElement("root");
        }

        protected override T GetInternal<T>(string key)
        {
            if (!Contains(key)) return default;
            return (T) Restore<T>(_document.Element(key));
        }

        protected override List<T> GetListInternal<T>(string key)
        {
            if (!Contains(key)) return null;
            var list = new List<T>();
            var parent = _document.Element(key);
            if (parent == null) return null;
            foreach (var xElement in parent.Elements()) list.Add((T) Restore<T>(xElement));

            return list;
        }

        protected override void PutInternal<T>(string key, T value)
        {
            if (Contains(key)) throw new DuplicatingBundleKeyException(key);
            XNode node;
            if (!(value is string) && !(value is IBundlable) && value is IEnumerable enumerable)
            {
                var parent = new XElement(key);
                foreach (var o in enumerable)
                {
                    var el = Store(o, o.GetType().Name);
                    parent.Add(el);
                }

                node = parent;
            }
            else
            {
                node = Store(value, key);
            }

            _document.Add(node);
        }

        public override bool Contains(string key)
        {
            return !(_document.Element(key) is null);
        }

        protected override Bundle BundleFromBundlable(IBundlable bundlable)
        {
            var b = new XmlBundle();
            b.Put(bundlable);
            return b;
        }

        public override byte[] Serialize()
        {
            var str = _document.ToString(SaveOptions.DisableFormatting);
            return Encoding.ASCII.GetBytes(str);
        }

        public override void Deserialize(byte[] data)
        {
            var str = Encoding.ASCII.GetString(data);
            _document = XElement.Parse(str);
        }

        public override BundleFactory Factory()
        {
            return XmlBundleFactory.Instance;
        }

        private static XNode Store(object value, string name)
        {
            var node = new XElement(name);
            if (value is XmlBundle b)
            {
                node.Add(new XElement(b._document));
                return node;
            }

            if (value is string s) value = Escape(s);
            node.SetValue(value);
            return node;
        }

        private static object Restore<T>(XElement node)
        {
            if (node is null) return default;
            try
            {
                if (typeof(T) == typeof(short)) return XmlConvert.ToInt16(node.Value);
                if (typeof(T) == typeof(int)) return XmlConvert.ToInt32(node.Value);
                if (typeof(T) == typeof(long)) return XmlConvert.ToInt64(node.Value);
                if (typeof(T) == typeof(ushort)) return XmlConvert.ToUInt16(node.Value);
                if (typeof(T) == typeof(uint)) return XmlConvert.ToUInt32(node.Value);
                if (typeof(T) == typeof(ulong)) return XmlConvert.ToUInt64(node.Value);
                if (typeof(T) == typeof(float)) return XmlConvert.ToSingle(node.Value);
                if (typeof(T) == typeof(double)) return XmlConvert.ToDouble(node.Value);
                if (typeof(T) == typeof(decimal)) return XmlConvert.ToDecimal(node.Value);
                if (typeof(T) == typeof(byte)) return XmlConvert.ToByte(node.Value);
                if (typeof(T) == typeof(sbyte)) return XmlConvert.ToSByte(node.Value);
                if (typeof(T) == typeof(string)) return Unescape(node.Value);
                if (typeof(T) == typeof(bool)) return XmlConvert.ToBoolean(node.Value);
                if (typeof(T) == typeof(DateTime))
                    return XmlConvert.ToDateTime(node.Value, XmlDateTimeSerializationMode.Utc);
                if (typeof(T) == typeof(Bundle))
                {
                    var b = new XmlBundle {_document = new XElement(node.Element("root"))};
                    return b;
                }
            }
            catch
            {
                return default;
            }

            return default;
        }

        private static string Escape(string value)
        {
            var sb = new StringBuilder();
            foreach (var c in value)
                if (c > 127)
                {
                    var encodedValue = "\\u" + ((int) c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }

            return sb.ToString();
        }

        private static string Unescape(string source)
        {
            return Regex.Unescape(source);
        }
    }

    public class XmlBundleFactory : BundleFactory
    {
        public static XmlBundleFactory Instance = new XmlBundleFactory();

        private XmlBundleFactory()
        {
        }

        public override Bundle CreateBundle()
        {
            return new XmlBundle();
        }
    }
}