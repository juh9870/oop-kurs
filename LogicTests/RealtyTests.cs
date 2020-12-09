using Data.Bundles;
using Logic.Model;
using NUnit.Framework;

namespace LogicTests
{
    public class RealtyTests
    {
        public static Bundle.Factory[] Factories = CommonData.BundleFactories;

        private Realty _realty;

        [SetUp]
        public void SetUp()
        {
            _realty = new Realty();
        }

        [Test]
        public void Realty_Id_Should_Be_Guid()
        {
            CustomAssert.IsGuid(_realty.Id);
        }

        [Test]
        public void Should_Be_Same_After_Storing_And_Restoring([ValueSource(nameof(Factories))] Bundle.Factory factory)
        {
            var b = factory.CreateBundle();
            b.Put(_realty);
            b.Deserialize(b.Serialize());
            var newRealty = b.Get<Realty>();

            Assert.AreEqual(_realty.Id, newRealty.Id);
            Assert.AreEqual(_realty.Title, newRealty.Title);
            Assert.AreEqual(_realty.Description, newRealty.Description);
            Assert.AreEqual(_realty.Price, newRealty.Price);
            Assert.AreEqual(_realty.RoomsNumber, newRealty.RoomsNumber);
            Assert.AreEqual(_realty.Area, newRealty.Area);
            Assert.AreEqual(_realty.Type, newRealty.Type);
            Assert.AreEqual(_realty.AddedDate, newRealty.AddedDate);
        }
    }
}