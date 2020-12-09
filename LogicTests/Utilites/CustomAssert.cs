using System;
using NUnit.Framework;

namespace LogicTests
{
    public static class CustomAssert
    {
        public static void IsGuid(string value)
        {
            Guid.TryParse(value, out var guid);
            Assert.NotNull(guid);
        }
    }
}