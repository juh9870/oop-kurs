using System;
using System.Collections.Generic;
using Data.Bundles;

namespace Logic.Model
{
    public class Requirements : IBundlable
    {
        public Requirements() : this(0)
        {
        }

        public Requirements(float minPrice = 0, float maxPrice = 0, int minRoomsNumber = 0, int maxRoomsNumber = 0,
            float minArea = 0, float maxArea = 0, IEnumerable<RealtyType> realtyTypes = null)
        {
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            MinRoomsNumber = minRoomsNumber;
            MaxRoomsNumber = maxRoomsNumber;
            MinArea = minArea;
            MaxArea = maxArea;
            AcceptedTypes = new HashSet<RealtyType>(realtyTypes ?? Array.Empty<RealtyType>());
        }

        public float MinPrice { get; set; }
        public float MaxPrice { get; set; }
        public int MinRoomsNumber { get; set; }
        public int MaxRoomsNumber { get; set; }
        public float MinArea { get; set; }
        public float MaxArea { get; set; }
        public HashSet<RealtyType> AcceptedTypes { get; set; }

        public void StoreInBundle(Bundle bundle)
        {
            bundle.Put(nameof(MinPrice), MinPrice);
            bundle.Put(nameof(MaxPrice), MaxPrice);
            bundle.Put(nameof(MinRoomsNumber), MinRoomsNumber);
            bundle.Put(nameof(MaxRoomsNumber), MaxRoomsNumber);
            bundle.Put(nameof(MinArea), MinArea);
            bundle.Put(nameof(MaxArea), MaxArea);
            var types = new List<int>();
            foreach (var type in AcceptedTypes)
                types.Add((int) type);

            bundle.Put(nameof(AcceptedTypes), types);
        }

        public void RestoreFromBundle(Bundle bundle)
        {
            MinPrice = bundle.GetFloat(nameof(MinPrice));
            MaxPrice = bundle.GetFloat(nameof(MaxPrice));
            MinRoomsNumber = bundle.GetInt(nameof(MinRoomsNumber));
            MaxRoomsNumber = bundle.GetInt(nameof(MaxRoomsNumber));
            MinArea = bundle.GetFloat(nameof(MinArea));
            MaxArea = bundle.GetFloat(nameof(MaxArea));
            AcceptedTypes = new HashSet<RealtyType>();
            foreach (var type in bundle.GetIntList(nameof(AcceptedTypes)))
                AcceptedTypes.Add((RealtyType) type);
        }

        public bool IsAcceptable(Realty realty)
        {
            if (MinPrice != 0 && realty.Price < MinPrice) return false;
            if (MaxPrice != 0 && realty.Price > MaxPrice) return false;
            if (MinRoomsNumber != 0 && realty.RoomsNumber < MinRoomsNumber) return false;
            if (MaxRoomsNumber != 0 && realty.RoomsNumber > MaxRoomsNumber) return false;
            if (MinArea != 0 && realty.Area < MinArea) return false;
            if (MaxArea != 0 && realty.Area > MaxArea) return false;
            if (AcceptedTypes.Count != 0 && !AcceptedTypes.Contains(realty.Type)) return false;
            return true;
        }
    }
}