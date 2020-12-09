using System.Collections.Generic;
using System.Collections.ObjectModel;
using Data.Bundles;
using Data.IO;
using Logic.Model;

namespace Logic
{
    public class Storage
    {
        public const int OffersPerClient = 5;
        private readonly Bundle.Factory _bundleFactory;
        private readonly IoInterface _ioInterface;
        private Dictionary<string, int> _clientOffers;

        private Dictionary<string, Client> _clients;
        private Dictionary<string, Offer> _offers;
        private Dictionary<string, Realty> _realties;


        public Storage(Bundle.Factory bundleFactory, IoInterface ioInterface)
        {
            _bundleFactory = bundleFactory;
            _ioInterface = ioInterface;
            Reset();
        }

        public ReadOnlyDictionary<string, Client> Clients => new ReadOnlyDictionary<string, Client>(_clients);
        public ReadOnlyDictionary<string, Realty> Realties => new ReadOnlyDictionary<string, Realty>(_realties);
        public ReadOnlyDictionary<string, Offer> Offers => new ReadOnlyDictionary<string, Offer>(_offers);

        public void AddClient(Client client)
        {
            _clients[client.Id] = client;
        }

        public void RemoveClient(Client client)
        {
            _clients.Remove(client.Id);
            _clientOffers.Remove(client.Id);

            foreach (var offer in _offers.Values)
                if (offer.ClientId == client.Id)
                    _offers.Remove(offer.Id);
        }

        public void AddRealty(Realty realty)
        {
            _realties[realty.Id] = realty;
        }

        public void RemoveRealty(Realty realty)
        {
            _realties.Remove(realty.Id);

            foreach (var offer in _offers.Values)
                if (offer.RealtyId == realty.Id)
                {
                    _offers.Remove(offer.Id);
                    if (_clientOffers.ContainsKey(offer.ClientId)) _clientOffers[offer.ClientId]--;
                }
        }

        public int ClientOffers(Client client)
        {
            if (_clientOffers.TryGetValue(client.Id, out var result)) return result;

            var offersCount = 0;
            foreach (var of in _offers.Values)
                if (of.ClientId == client.Id)
                    offersCount++;
            _clientOffers[client.Id] = offersCount;
            return offersCount;
        }

        public bool IsOfferExists(Client client, Realty realty)
        {
            foreach (var offer in _offers.Values)
                if (offer.ClientId == client.Id && offer.RealtyId == realty.Id)
                    return true;

            return false;
        }

        public bool TryCreateOffer(Client client, Realty realty)
        {
            if (ClientOffers(client) >= OffersPerClient) return false;
            if (IsOfferExists(client, realty)) return false;
            var offer = new Offer(client.Id, realty.Id);
            _offers[offer.Id] = offer;
            _clientOffers[client.Id]++;
            return true;
        }

        public void CancelOffer(Offer offer)
        {
            if (_clientOffers.ContainsKey(offer.ClientId)) _clientOffers[offer.ClientId]--;
            _offers.Remove(offer.Id);
        }

        private void Reset()
        {
            _clients = new Dictionary<string, Client>();
            _realties = new Dictionary<string, Realty>();
            _offers = new Dictionary<string, Offer>();
            _clientOffers = new Dictionary<string, int>();
        }

        public void LoadData()
        {
            Reset();
            var bundle = _bundleFactory.CreateBundle();
            var bytes = _ioInterface.Read();
            if (bytes is null) return;
            bundle.Deserialize(bytes);
            foreach (var client in bundle.GetBundlableList<Client>(nameof(_clients))) _clients[client.Id] = client;
            foreach (var realty in bundle.GetBundlableList<Realty>(nameof(_realties))) _realties[realty.Id] = realty;
            foreach (var offer in bundle.GetBundlableList<Offer>(nameof(_offers))) _offers[offer.Id] = offer;
        }

        public void SaveData()
        {
            var bundle = _bundleFactory.CreateBundle();
            bundle.Put(nameof(_clients), _clients.Values);
            bundle.Put(nameof(_realties), _realties.Values);
            bundle.Put(nameof(_offers), _offers.Values);
            _ioInterface.Write(bundle.Serialize());
        }
    }
}