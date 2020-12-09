using Presentation.Interface.Readers.Validators;
namespace Presentation.Interface.Readers {
    public class ShortReader : NumericReader<short>
    {
        public ShortReader(string name, Validator<short> validator = null) : base(name, validator){}
        public ShortReader(string name, short min, short max) : base(name, min,max){}
        protected override bool TryParse(string text, out short result)=>short.TryParse(text, out result);
    }
    public class ShortRangeReader : RangeReader<short>
    {
        public ShortRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<short> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public ShortRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, short min, short max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<short> GetReader(string message, Validator<short> validator)=>new ShortReader(message,validator);
    }
    public class IntReader : NumericReader<int>
    {
        public IntReader(string name, Validator<int> validator = null) : base(name, validator){}
        public IntReader(string name, int min, int max) : base(name, min,max){}
        protected override bool TryParse(string text, out int result)=>int.TryParse(text, out result);
    }
    public class IntRangeReader : RangeReader<int>
    {
        public IntRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<int> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public IntRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, int min, int max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<int> GetReader(string message, Validator<int> validator)=>new IntReader(message,validator);
    }
    public class LongReader : NumericReader<long>
    {
        public LongReader(string name, Validator<long> validator = null) : base(name, validator){}
        public LongReader(string name, long min, long max) : base(name, min,max){}
        protected override bool TryParse(string text, out long result)=>long.TryParse(text, out result);
    }
    public class LongRangeReader : RangeReader<long>
    {
        public LongRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<long> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public LongRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, long min, long max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<long> GetReader(string message, Validator<long> validator)=>new LongReader(message,validator);
    }
    public class UshortReader : NumericReader<ushort>
    {
        public UshortReader(string name, Validator<ushort> validator = null) : base(name, validator){}
        public UshortReader(string name, ushort min, ushort max) : base(name, min,max){}
        protected override bool TryParse(string text, out ushort result)=>ushort.TryParse(text, out result);
    }
    public class UshortRangeReader : RangeReader<ushort>
    {
        public UshortRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<ushort> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public UshortRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, ushort min, ushort max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<ushort> GetReader(string message, Validator<ushort> validator)=>new UshortReader(message,validator);
    }
    public class UintReader : NumericReader<uint>
    {
        public UintReader(string name, Validator<uint> validator = null) : base(name, validator){}
        public UintReader(string name, uint min, uint max) : base(name, min,max){}
        protected override bool TryParse(string text, out uint result)=>uint.TryParse(text, out result);
    }
    public class UintRangeReader : RangeReader<uint>
    {
        public UintRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<uint> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public UintRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, uint min, uint max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<uint> GetReader(string message, Validator<uint> validator)=>new UintReader(message,validator);
    }
    public class UlongReader : NumericReader<ulong>
    {
        public UlongReader(string name, Validator<ulong> validator = null) : base(name, validator){}
        public UlongReader(string name, ulong min, ulong max) : base(name, min,max){}
        protected override bool TryParse(string text, out ulong result)=>ulong.TryParse(text, out result);
    }
    public class UlongRangeReader : RangeReader<ulong>
    {
        public UlongRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<ulong> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public UlongRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, ulong min, ulong max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<ulong> GetReader(string message, Validator<ulong> validator)=>new UlongReader(message,validator);
    }
    public class FloatReader : NumericReader<float>
    {
        public FloatReader(string name, Validator<float> validator = null) : base(name, validator){}
        public FloatReader(string name, float min, float max) : base(name, min,max){}
        protected override bool TryParse(string text, out float result)=>float.TryParse(text, out result);
    }
    public class FloatRangeReader : RangeReader<float>
    {
        public FloatRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<float> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public FloatRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, float min, float max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<float> GetReader(string message, Validator<float> validator)=>new FloatReader(message,validator);
    }
    public class DoubleReader : NumericReader<double>
    {
        public DoubleReader(string name, Validator<double> validator = null) : base(name, validator){}
        public DoubleReader(string name, double min, double max) : base(name, min,max){}
        protected override bool TryParse(string text, out double result)=>double.TryParse(text, out result);
    }
    public class DoubleRangeReader : RangeReader<double>
    {
        public DoubleRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<double> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public DoubleRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, double min, double max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<double> GetReader(string message, Validator<double> validator)=>new DoubleReader(message,validator);
    }
    public class DecimalReader : NumericReader<decimal>
    {
        public DecimalReader(string name, Validator<decimal> validator = null) : base(name, validator){}
        public DecimalReader(string name, decimal min, decimal max) : base(name, min,max){}
        protected override bool TryParse(string text, out decimal result)=>decimal.TryParse(text, out result);
    }
    public class DecimalRangeReader : RangeReader<decimal>
    {
        public DecimalRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<decimal> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public DecimalRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, decimal min, decimal max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<decimal> GetReader(string message, Validator<decimal> validator)=>new DecimalReader(message,validator);
    }
    public class ByteReader : NumericReader<byte>
    {
        public ByteReader(string name, Validator<byte> validator = null) : base(name, validator){}
        public ByteReader(string name, byte min, byte max) : base(name, min,max){}
        protected override bool TryParse(string text, out byte result)=>byte.TryParse(text, out result);
    }
    public class ByteRangeReader : RangeReader<byte>
    {
        public ByteRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<byte> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public ByteRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, byte min, byte max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<byte> GetReader(string message, Validator<byte> validator)=>new ByteReader(message,validator);
    }
    public class SbyteReader : NumericReader<sbyte>
    {
        public SbyteReader(string name, Validator<sbyte> validator = null) : base(name, validator){}
        public SbyteReader(string name, sbyte min, sbyte max) : base(name, min,max){}
        protected override bool TryParse(string text, out sbyte result)=>sbyte.TryParse(text, out result);
    }
    public class SbyteRangeReader : RangeReader<sbyte>
    {
        public SbyteRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange = true, Validator<sbyte> validator = null) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, validator){}
        public SbyteRangeReader(string name, string minFieldName, string maxFieldName, bool allowZeroLengthRange, sbyte min, sbyte max) : base(name, minFieldName, maxFieldName, allowZeroLengthRange, min, max){}
        protected override Reader<sbyte> GetReader(string message, Validator<sbyte> validator)=>new SbyteReader(message,validator);
    }
}