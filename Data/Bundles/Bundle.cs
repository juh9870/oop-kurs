

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Data.Bundles {
    public abstract class Bundle {
        protected const string TypeName = "__typeName";

        public virtual short GetShort ( string key )=>GetInternal<short>( key );
        public virtual List<short> GetShortList ( string key )=>GetListInternal<short>( key );
        public virtual void Put ( string key, short value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<short> enumerable )=>PutInternal( key, enumerable );
        public virtual int GetInt ( string key )=>GetInternal<int>( key );
        public virtual List<int> GetIntList ( string key )=>GetListInternal<int>( key );
        public virtual void Put ( string key, int value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<int> enumerable )=>PutInternal( key, enumerable );
        public virtual long GetLong ( string key )=>GetInternal<long>( key );
        public virtual List<long> GetLongList ( string key )=>GetListInternal<long>( key );
        public virtual void Put ( string key, long value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<long> enumerable )=>PutInternal( key, enumerable );
        public virtual ushort GetUshort ( string key )=>GetInternal<ushort>( key );
        public virtual List<ushort> GetUshortList ( string key )=>GetListInternal<ushort>( key );
        public virtual void Put ( string key, ushort value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<ushort> enumerable )=>PutInternal( key, enumerable );
        public virtual uint GetUint ( string key )=>GetInternal<uint>( key );
        public virtual List<uint> GetUintList ( string key )=>GetListInternal<uint>( key );
        public virtual void Put ( string key, uint value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<uint> enumerable )=>PutInternal( key, enumerable );
        public virtual ulong GetUlong ( string key )=>GetInternal<ulong>( key );
        public virtual List<ulong> GetUlongList ( string key )=>GetListInternal<ulong>( key );
        public virtual void Put ( string key, ulong value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<ulong> enumerable )=>PutInternal( key, enumerable );
        public virtual float GetFloat ( string key )=>GetInternal<float>( key );
        public virtual List<float> GetFloatList ( string key )=>GetListInternal<float>( key );
        public virtual void Put ( string key, float value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<float> enumerable )=>PutInternal( key, enumerable );
        public virtual double GetDouble ( string key )=>GetInternal<double>( key );
        public virtual List<double> GetDoubleList ( string key )=>GetListInternal<double>( key );
        public virtual void Put ( string key, double value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<double> enumerable )=>PutInternal( key, enumerable );
        public virtual decimal GetDecimal ( string key )=>GetInternal<decimal>( key );
        public virtual List<decimal> GetDecimalList ( string key )=>GetListInternal<decimal>( key );
        public virtual void Put ( string key, decimal value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<decimal> enumerable )=>PutInternal( key, enumerable );
        public virtual string GetString ( string key )=>GetInternal<string>( key );
        public virtual List<string> GetStringList ( string key )=>GetListInternal<string>( key );
        public virtual void Put ( string key, string value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<string> enumerable )=>PutInternal( key, enumerable );
        public virtual bool GetBool ( string key )=>GetInternal<bool>( key );
        public virtual List<bool> GetBoolList ( string key )=>GetListInternal<bool>( key );
        public virtual void Put ( string key, bool value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<bool> enumerable )=>PutInternal( key, enumerable );
        public virtual byte GetByte ( string key )=>GetInternal<byte>( key );
        public virtual List<byte> GetByteList ( string key )=>GetListInternal<byte>( key );
        public virtual void Put ( string key, byte value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<byte> enumerable )=>PutInternal( key, enumerable );
        public virtual sbyte GetSbyte ( string key )=>GetInternal<sbyte>( key );
        public virtual List<sbyte> GetSbyteList ( string key )=>GetListInternal<sbyte>( key );
        public virtual void Put ( string key, sbyte value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<sbyte> enumerable )=>PutInternal( key, enumerable );
        public virtual DateTime GetDateTime ( string key )=>GetInternal<DateTime>( key );
        public virtual List<DateTime> GetDateTimeList ( string key )=>GetListInternal<DateTime>( key );
        public virtual void Put ( string key, DateTime value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<DateTime> enumerable )=>PutInternal( key, enumerable );
        public virtual Bundle GetBundle ( string key )=>GetInternal<Bundle>( key );
        public virtual List<Bundle> GetBundleList ( string key )=>GetListInternal<Bundle>( key );
        public virtual void Put ( string key, Bundle value )=>PutInternal( key, value );
        public virtual void Put ( string key, IEnumerable<Bundle> enumerable )=>PutInternal( key, enumerable );
                
        protected abstract T GetInternal<T>(string key);
        protected abstract List<T> GetListInternal<T>(string key);
        protected abstract void PutInternal<T>(string key, T value);
        public abstract bool Contains(string key);

        protected abstract Bundle BundleFromBundlable(IBundlable bundlable);
        public virtual T Get<T>() where T : IBundlable
        {
            var className = GetString(TypeName);
            if (string.IsNullOrEmpty(className)) return default;

            var type = Type.GetType(className);

            if (type == null) return default;
            T obj;
            try
            {
                obj = (T)Activator.CreateInstance(type);
            }
            catch (MissingMethodException)
            {
                obj = (T) FormatterServices.GetUninitializedObject(type);
            }
            obj?.RestoreFromBundle(this);
            return  obj;
        }

        public virtual void Put(IBundlable value)
        {
            value.StoreInBundle(this);
            Put(TypeName, value.GetType().AssemblyQualifiedName);
        }

        public virtual T GetBundlable<T>(string key) where T : IBundlable
        {
            return GetBundle(key).Get<T>();
        }

        public virtual List<T> GetBundlableList<T>(string key)  where T : IBundlable
        {
            var bundles = GetBundleList(key);
            if (bundles == null) return null;
            var list = new List<T>();
            foreach (var t in bundles) list.Add(t.Get<T>());
            return list;
        }

        public virtual void Put(string key, IBundlable value)
        {
            Put(key,BundleFromBundlable(value));
        }

        public virtual void Put(string key, IEnumerable<IBundlable> enumerable)
        {
            var list = new List<Bundle>();
            foreach (var bundlable in enumerable)
            {
                list.Add(BundleFromBundlable(bundlable));
            }
            Put(key,list);
        }

        public virtual Type GetType(string key)
        {
            return Type.GetType(GetString(key));
        }

        public virtual List<Type> GetTypeList(string key)
        {
            var strings = GetStringList(key);
            if (strings == null) return null;
            var list = new List<Type>();
            foreach (var t in strings) list.Add(Type.GetType(key));
            return list;
        }

        public virtual void Put(string key, Type value)
        {
            Put(key, value.AssemblyQualifiedName);
        }

        public virtual void Put(string key, IEnumerable<Type> enumerable)
        {
            var list = new List<string>();
            foreach (var type in enumerable) list.Add(type.AssemblyQualifiedName);

            Put(key, list);
        }

        public abstract byte[] Serialize();
        public abstract void Deserialize(byte[] data);

        public abstract BundleFactory Factory();
    }

    public abstract class BundleFactory
    {
        public abstract Bundle CreateBundle();
    }
}