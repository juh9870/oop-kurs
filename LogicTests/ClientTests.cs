using Data.Bundles;
using Logic.Model;
using NUnit.Framework;

namespace LogicTests
{
    public class ClientTests
    {
        public static Bundle.Factory[] Factories = CommonData.BundleFactories;

        private Client _client;

        [SetUp]
        public void SetUp()
        {
            _client = new Client();
        }

        [Test]
        public void Client_Id_Should_Be_Guid()
        {
            CustomAssert.IsGuid(_client.Id);
        }

        [Test]
        public void Should_Be_Same_After_Storing_And_Restoring([ValueSource(nameof(Factories))] Bundle.Factory factory)
        {
            var b = factory.CreateBundle();
            b.Put(_client);
            b.Deserialize(b.Serialize());
            var newClient = b.Get<Client>();

            Assert.AreEqual(_client.Id, newClient.Id);
            Assert.AreEqual(_client.FirstName, newClient.FirstName);
            Assert.AreEqual(_client.LastName, newClient.LastName);
            Assert.AreEqual(_client.BankAccount, newClient.BankAccount);
        }
    }
}