using System;
using Data.Bundles;
using Logic.Model;
using NUnit.Framework;

namespace LogicTests
{
    [TestFixture]
    public class OfferTests
    {
        [SetUp]
        public void SetUp()
        {
            _offer = new Offer(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        private Offer _offer;

        [Test]
        public void Realty_Id_Should_Be_Guid()
        {
            CustomAssert.IsGuid(_offer.Id);
        }

        [Test]
        public void Realty_ClientId_Should_Be_Guid()
        {
            CustomAssert.IsGuid(_offer.ClientId);
        }

        [Test]
        public void Realty_RealtyId_Should_Be_Guid()
        {
            CustomAssert.IsGuid(_offer.RealtyId);
        }

        [Test]
        public void Should_Be_Same_After_Storing_And_Restoring()
        {
            var b = new JsonBundle();


            b.Put(_offer);
            b.Deserialize(b.Serialize());
            var newOffer = b.Get<Offer>();

            Assert.AreEqual(_offer.Id, newOffer.Id);
            Assert.AreEqual(_offer.ClientId, newOffer.ClientId);
            Assert.AreEqual(_offer.RealtyId, newOffer.RealtyId);
        }
    }
}