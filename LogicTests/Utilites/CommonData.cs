using Data.Bundles;

namespace LogicTests
{
    public class CommonData
    {
        public static readonly Bundle.Factory[] BundleFactories =
        {
            new JsonBundle.Factory()
        };
    }
}