using Data.Bundles;

namespace LogicTests
{
    public class CommonData
    {
        public static readonly BundleFactory[] BundleFactories =
        {
            JsonBundleFactory.Instance,
            BinaryBundleFactory.Instance,
            XmlBundleFactory.Instance, 
        };
    }
}