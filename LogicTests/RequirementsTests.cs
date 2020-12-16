using Data.Bundles;
using Logic.Model;
using NUnit.Framework;

namespace LogicTests
{
    [TestFixture]
    public class RequirementsTests
    {
        [SetUp]
        public void SetUp()
        {
            _requirements = new Requirements();
            _comparisonRealty = new Realty();
        }

        private Requirements _requirements;
        private Realty _comparisonRealty;


        public static TestCase<float>[] PriceTestCases =
        {
            new TestCase<float>(1000, false),
            new TestCase<float>(2000, true),
            new TestCase<float>(3000, false)
        };

        [Test]
        public void Match_By_Price([ValueSource(nameof(PriceTestCases))] TestCase<float> testCase)
        {
            _requirements.MinPrice = 1500;
            _requirements.MaxPrice = 2500;
            _comparisonRealty.Price = testCase.Value;
            testCase.Assert(_requirements.IsAcceptable(_comparisonRealty));
        }

        public static TestCase<int>[] RoomsNumberCases =
        {
            new TestCase<int>(1, false),
            new TestCase<int>(2, true),
            new TestCase<int>(3, true),
            new TestCase<int>(4, false)
        };

        [Test]
        public void Match_By_RoomsNumber([ValueSource(nameof(RoomsNumberCases))]
            TestCase<int> testCase)
        {
            _requirements.MinRoomsNumber = 2;
            _requirements.MaxRoomsNumber = 3;
            _comparisonRealty.RoomsNumber = testCase.Value;
            testCase.Assert(_requirements.IsAcceptable(_comparisonRealty));
        }

        public static TestCase<float>[] AreaTestCases =
        {
            new TestCase<float>(100, false),
            new TestCase<float>(200, true),
            new TestCase<float>(300, false)
        };

        [Test]
        public void Match_By_Area([ValueSource(nameof(AreaTestCases))] TestCase<float> testCase)
        {
            _requirements.MinArea = 150;
            _requirements.MaxArea = 250;
            _comparisonRealty.Area = testCase.Value;
            testCase.Assert(_requirements.IsAcceptable(_comparisonRealty));
        }

        public static TestCase<RealtyType>[] TypeCases =
        {
            new TestCase<RealtyType>(RealtyType.Flat, true),
            new TestCase<RealtyType>(RealtyType.House, true),
            new TestCase<RealtyType>(RealtyType.Room, false)
        };

        [Test]
        public void Match_By_Type([ValueSource(nameof(TypeCases))] TestCase<RealtyType> testCase)
        {
            _requirements.AcceptedTypes.Add(RealtyType.Flat);
            _requirements.AcceptedTypes.Add(RealtyType.House);
            _comparisonRealty.Type = testCase.Value;
            testCase.Assert(_requirements.IsAcceptable(_comparisonRealty));
        }

        public static BundleFactory[] Factories = CommonData.BundleFactories;

        [Test]
        public void Should_Be_Same_After_Storing_And_Restoring([ValueSource(nameof(Factories))] BundleFactory factory)
        {
            _requirements.AcceptedTypes.Add(RealtyType.Flat);
            _requirements.AcceptedTypes.Add(RealtyType.Room);

            var b = factory.CreateBundle();
            b.Put(_requirements);
            b.Deserialize(b.Serialize());
            var newRequirements = b.Get<Requirements>();

            Assert.AreEqual(_requirements.MinPrice, newRequirements.MinPrice);
            Assert.AreEqual(_requirements.MaxPrice, newRequirements.MaxPrice);
            Assert.AreEqual(_requirements.MinRoomsNumber, newRequirements.MinRoomsNumber);
            Assert.AreEqual(_requirements.MaxRoomsNumber, newRequirements.MaxRoomsNumber);
            Assert.AreEqual(_requirements.MinArea, newRequirements.MinArea);
            Assert.AreEqual(_requirements.MaxArea, newRequirements.MaxArea);
            CollectionAssert.AreEquivalent(newRequirements.AcceptedTypes, _requirements.AcceptedTypes);
        }
    }
}