using System;
using System.Collections;
using System.Collections.Generic;
using Data.Bundles;
using Data.IO;
using Logic;
using Logic.Model;
using NUnit.Framework;

namespace LogicTests
{
    [TestFixtureSource(typeof(StorageTestFixtureArgs))]
    public class StorageTests
    {
        [SetUp]
        public void SetUp()
        {
            _storage = new Storage(_factory, _ioInterface);
        }

        private Storage _storage;
        private readonly Bundle.Factory _factory;
        private readonly IoInterface _ioInterface;

        public StorageTests(Bundle.Factory factory, IoInterface ioInterface)
        {
            _factory = factory;
            _ioInterface = ioInterface;
        }

        private void PopulateClients(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var c = new Client(i.ToString(),
                    (Math.PI * i).ToString("N2"),
                    new BankAccount(new string(i.ToString().ToCharArray()[0], 16)));
                _storage.AddClient(c);
            }
        }

        private void PopulateRealties(int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                var r = new Realty(i.ToString(),
                    (10 * i).ToString());
                _storage.AddRealty(r);
            }
        }

        private void PopulateOffers(int amount)
        {
            PopulateClients(amount);
            PopulateRealties(amount);
            var clients = new List<Client>(_storage.Clients.Values);
            var realties = new List<Realty>(_storage.Realties.Values);

            for (var i = 0; i < amount; i++)
            {
                var c = clients[i];
                var r = realties[i];
                _storage.TryCreateOffer(c, r);
            }
        }

        [Test]
        public void Can_Add_150_Clients()
        {
            PopulateClients(150);
            Assert.AreEqual(_storage.Clients.Count, 150);
        }

        [Test]
        public void Cant_Add_Duplicating_Clients()
        {
            var client = new Client();
            _storage.AddClient(client);
            var oldLength = _storage.Clients.Count;
            _storage.AddClient(client);
            var newLength = _storage.Clients.Count;
            Assert.AreEqual(oldLength, newLength);
        }

        [Test]
        public void Can_Add_150_Realties()
        {
            PopulateRealties(150);
            Assert.AreEqual(_storage.Realties.Count, 150);
        }

        [Test]
        public void Cant_Add_Duplicating_Realties()
        {
            var realty = new Realty();
            _storage.AddRealty(realty);
            var oldLength = _storage.Clients.Count;
            _storage.AddRealty(realty);
            var newLength = _storage.Clients.Count;
            Assert.AreEqual(oldLength, newLength);
        }

        [Test]
        public void Can_Remove_10_Clients()
        {
            PopulateClients(10);
            foreach (var client in _storage.Clients.Values) _storage.RemoveClient(client);
        }

        [Test]
        public void Can_Remove_10_Realties()
        {
            PopulateRealties(10);
            foreach (var realty in _storage.Realties.Values) _storage.RemoveRealty(realty);
        }

        [Test]
        public void Can_Create_150_Different_Offers()
        {
            PopulateOffers(150);
        }

        [Test]
        public void Cant_Create_Duplicating_Offers()
        {
            var client = new Client();
            var realty = new Realty();
            _storage.AddClient(client);
            _storage.AddRealty(realty);
            Assert.IsTrue(_storage.TryCreateOffer(client, realty));
            Assert.IsFalse(_storage.TryCreateOffer(client, realty));
        }

        [Test]
        public void Can_Remove_10_Offers()
        {
            PopulateOffers(10);
            foreach (var offer in _storage.Offers.Values) _storage.CancelOffer(offer);
        }

        [Test]
        public void Only_5_Offers_Per_Client()
        {
            PopulateRealties(10);
            var client = new Client();
            _storage.AddClient(client);
            var count = 0;

            foreach (var realty in _storage.Realties.Values)
                if (_storage.TryCreateOffer(client, realty))
                    count++;

            Assert.AreEqual(10, _storage.Realties.Count);
            Assert.AreEqual(5, count);
            Assert.IsFalse(_storage.TryCreateOffer(client, new List<Realty>(_storage.Realties.Values)[0]));
        }

        [Test]
        public void Removing_Client_Should_Remove_Linked_Offers()
        {
            var realty = new Realty();
            var client = new Client();
            _storage.AddClient(client);
            _storage.AddRealty(realty);
            _storage.TryCreateOffer(client, realty);
            _storage.RemoveClient(client);
            CollectionAssert.IsEmpty(_storage.Offers);
        }

        [Test]
        public void Removing_Realty_Should_Remove_Linked_Offers()
        {
            var realty = new Realty();
            var client = new Client();
            _storage.AddClient(client);
            _storage.AddRealty(realty);
            _storage.TryCreateOffer(client, realty);
            _storage.RemoveRealty(realty);
            CollectionAssert.IsEmpty(_storage.Offers);
        }

        [Test]
        public void Should_Have_Same_Lists_Length_After_SaveLoad()
        {
            PopulateOffers(13);
            var oldClientsAmount = _storage.Clients.Count;
            var oldRealtiesAmount = _storage.Realties.Count;
            var oldOffersAmount = _storage.Offers.Count;

            _storage.SaveData();
            _storage.LoadData();

            Assert.AreEqual(oldClientsAmount, _storage.Clients.Count);
            Assert.AreEqual(oldRealtiesAmount, _storage.Realties.Count);
            Assert.AreEqual(oldOffersAmount, _storage.Offers.Count);

            Assert.AreEqual(1, _storage.ClientOffers(new List<Client>(_storage.Clients.Values)[0]));
        }

        [Test]
        public void Should_Be_Empty_After_Loading_Empty_File()
        {
            _ioInterface.Erase();
            _storage.LoadData();

            CollectionAssert.IsEmpty(_storage.Clients);
            CollectionAssert.IsEmpty(_storage.Realties);
            CollectionAssert.IsEmpty(_storage.Offers);
        }
    }

    public class StorageTestFixtureArgs : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            var list = new ArrayList();
            foreach (var factory in CommonData.BundleFactories)
                list.Add(new object[] {factory, new FileInterface()});
            return list.GetEnumerator();
        }
    }
}