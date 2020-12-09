using Logic;
using Logic.Model;
using Presentation.Interface;

namespace Presentation.Connectors
{
    public class OffersList : ConsoleListUtils<Offer>
    {
        private readonly Client _client;
        private readonly Storage _storage;

        public OffersList(Storage storage, Client client) : base(new OffersTable(Charset.SymbolicCharset, 5, storage),
            "Close menu")
        {
            _storage = storage;
            AddAction(new ConsoleAction(RemoveItem, $"Remove {ObjectName}", 0, ListNotEmpty), '2');
            _client = client;
        }

        protected override void UpdateList()
        {
            List.Clear();
            if (!(_client is null))
            {
                foreach (var offer in _storage.Offers.Values)
                    if (offer.ClientId == _client.Id)
                        List.Add(offer);
            }
            else
            {
                List.AddRange(_storage.Offers.Values);
            }

            if (SortComparer != null) List.Sort(SortComparer);
        }

        protected override bool TryRemove(int id)
        {
            _storage.CancelOffer(List[id]);
            UpdateList();
            return true;
        }

        public override void Start()
        {
            UpdateList();
            base.Start();
        }

        protected override void ShowItemDetails(int id)
        {
            var realty = _storage.Realties[List[id].RealtyId];
            RealtyConsoleInterface.RealtyTable.WriteOne(realty);
        }
    }
}