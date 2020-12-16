using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Data.Bundles
{
    [Serializable]
    public class JsonBundle : Bundle
    {
        public JsonBundle()
        {
            Values = new Dictionary<string, object>();
        }

        public Dictionary<string, object> Values { get; set; }

        protected override T GetInternal<T>(string key)
        {
            if (!Values.TryGetValue(key, out var obj)) return default;

            switch (obj)
            {
                case T t:
                    return t;
                case JToken token:
                    return (T) JsonGet<T>(token);
                default:
                    try
                    {
                        return (T) Convert.ChangeType(obj, typeof(T));
                    }
                    catch
                    {
                        return default;
                    }
            }
        }

        protected override List<T> GetListInternal<T>(string key)
        {
            if (!Values.TryGetValue(key, out var obj)) return null;
            var result = new List<T>();
            if (obj is JArray array)
            {
                foreach (var t in array) result.Add((T) JsonGet<T>(t));

                return result;
            }

            if (!(obj is IEnumerable<T> enumerable)) return null;
            foreach (var x in enumerable) result.Add(x);

            return result;
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
            var bundle = new JsonBundle();
            bundle.Put(bundlable);
            return bundle;
        }


        public override byte[] Serialize()
        {
            var str = JsonConvert.SerializeObject(this);
            return Encoding.ASCII.GetBytes(str);
        }

        public override void Deserialize(byte[] data)
        {
            var str = Encoding.ASCII.GetString(data);
            var bundle = JsonConvert.DeserializeObject<JsonBundle>(str);
            Values = bundle.Values;
        }

        public override BundleFactory Factory()
        {
            return JsonBundleFactory.Instance;
        }

        private static object JsonGet<T>(JToken element)
        {
            if (typeof(T) == typeof(DateTime)) return element.ToObject<DateTime>();
            if (typeof(T) == typeof(Bundle)) return element.ToObject<JsonBundle>();
            return element.Value<T>();
        }

        public static T CloneBundlable<T>(T source) where T : IBundlable
        {
            var b = new JsonBundle();
            b.Put(source);
            return b.Get<T>();
        }
    }

    public class JsonBundleFactory : BundleFactory
    {
        public static JsonBundleFactory Instance = new JsonBundleFactory();

        private JsonBundleFactory()
        {
        }

        public override Bundle CreateBundle()
        {
            return new JsonBundle();
        }
    }
}