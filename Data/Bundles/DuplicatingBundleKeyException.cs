using System;

namespace Data.Bundles
{
    public class DuplicatingBundleKeyException : Exception
    {
        public DuplicatingBundleKeyException(string key) : base($"Duplicating key \"{key}\"")
        {
        }
    }
}