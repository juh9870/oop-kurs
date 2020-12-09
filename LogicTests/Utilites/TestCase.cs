namespace LogicTests
{
    public class TestCase<T>
    {
        public readonly bool Result;
        public readonly T Value;

        public TestCase(T value, bool result)
        {
            Value = value;
            Result = result;
        }

        public void Assert(bool value)
        {
            NUnit.Framework.Assert.AreEqual(Result, value);
        }
    }
}