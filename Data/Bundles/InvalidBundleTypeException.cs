using System;

namespace Data.Bundles
{
    public class InvalidBundleTypeException : Exception
    {
        public InvalidBundleTypeException(Type expected, Type actual) : base($"Bundle of type {actual} can't be stored in {expected} bundle.")
        {
        }
    }
}