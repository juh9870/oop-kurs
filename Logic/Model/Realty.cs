using System;
using Data.Bundles;

namespace Logic.Model
{
    public class Realty : IBundlable
    {
        public Realty(string title = "", string description = "", float price = 0, int roomsNumber = 0, float area = 0,
            RealtyType type = 0)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            Price = price;
            RoomsNumber = roomsNumber;
            Area = area;
            Type = type;
            AddedDate = DateTime.UtcNow;
        }

        public string Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int RoomsNumber { get; set; }
        public float Area { get; set; }
        public RealtyType Type { get; set; }
        public DateTime AddedDate { get; set; }

        public void StoreInBundle(Bundle bundle)
        {
            bundle.Put(nameof(Id), Id);
            bundle.Put(nameof(Title), Title);
            bundle.Put(nameof(Description), Description);
            bundle.Put(nameof(Price), Price);
            bundle.Put(nameof(RoomsNumber), RoomsNumber);
            bundle.Put(nameof(Area), Area);
            bundle.Put(nameof(Type), (int) Type);
            bundle.Put(nameof(AddedDate), AddedDate);
        }

        public void RestoreFromBundle(Bundle bundle)
        {
            Id = bundle.GetString(nameof(Id));
            Title = bundle.GetString(nameof(Title));
            Description = bundle.GetString(nameof(Description));
            Price = bundle.GetFloat(nameof(Price));
            RoomsNumber = bundle.GetInt(nameof(RoomsNumber));
            Area = bundle.GetFloat(nameof(Area));
            Type = (RealtyType) bundle.GetInt(nameof(Type));
            AddedDate = bundle.GetDateTime(nameof(AddedDate));
        }
    }
}