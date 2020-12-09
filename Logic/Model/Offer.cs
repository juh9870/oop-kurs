using System;
using Data.Bundles;

namespace Logic.Model
{
    public class Offer : IBundlable
    {
        public string ClientId;
        public string RealtyId;

        public Offer(string clientId, string realtyId)
        {
            Id = Guid.NewGuid().ToString();
            ClientId = clientId;
            RealtyId = realtyId;
        }

        public string Id { get; private set; }


        public void StoreInBundle(Bundle bundle)
        {
            bundle.Put(nameof(Id), Id);
            bundle.Put(nameof(ClientId), ClientId);
            bundle.Put(nameof(RealtyId), RealtyId);
        }

        public void RestoreFromBundle(Bundle bundle)
        {
            Id = bundle.GetString(nameof(Id));
            ClientId = bundle.GetString(nameof(ClientId));
            RealtyId = bundle.GetString(nameof(RealtyId));
        }
    }
}