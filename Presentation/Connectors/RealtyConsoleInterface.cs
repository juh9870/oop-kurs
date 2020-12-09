using System;
using System.Collections.Generic;
using Logic;
using Logic.Model;
using Presentation.Interface;
using Presentation.Interface.Readers;

namespace Presentation.Connectors
{
    public class RealtyConsoleInterface : ConsoleListUtils<Realty>
    {
        public static readonly RealtyTable RealtyTable = new RealtyTable(Charset.SymbolicCharset, 5);

        private static readonly ClassManipulator<Realty> RealtyManipulator = new ClassManipulator<Realty>(
                () => new Realty())
            .AddReader(nameof(Realty.Title), new StringReader("title"))
            .AddReader(nameof(Realty.Description), new StringReader("description"))
            .AddReader(nameof(Realty.Price), new FloatReader("price", 0, float.MaxValue))
            .AddReader(nameof(Realty.RoomsNumber), new IntReader("rooms number", 1, int.MaxValue))
            .AddReader(nameof(Realty.Area), new FloatReader("area", 0, float.MaxValue))
            .AddReader(nameof(Realty.Type), new EnumReader<RealtyType>("Realty type"));

        private readonly Storage _storage;

        public RealtyConsoleInterface(Storage storage) : base(RealtyManipulator, RealtyTable)
        {
            _storage = storage;
            AddAction(new ConsoleAction(CreateOffer, "Offer Realty to client", 0, ListNotEmpty), '5');
        }

        protected override bool TryAdd(Realty item)
        {
            _storage.AddRealty(item);
            UpdateList();
            return true;
        }

        protected override bool TryRemove(int id)
        {
            var item = List[id];
            _storage.RemoveRealty(item);
            return true;
        }

        protected override void ShowItemDetails(int id)
        {
            RealtyTable.WriteOne(List[id], id);
            Console.WriteLine($"Description: {List[id].Description}");
        }

        protected override void UpdateList()
        {
            List.Clear();
            List.AddRange(_storage.Realties.Values);
            base.UpdateList();
        }

        private void CreateOffer()
        {
            var id = IdReader.Read();
            var realty = List[id];
            var list = new List<Client>();
            var overloadedFound = false;
            foreach (var cl in _storage.Clients.Values)
                if (cl.Requirements.IsAcceptable(realty))
                {
                    if (_storage.IsOfferExists(cl, realty)) continue;
                    if (_storage.ClientOffers(cl) < 5) list.Add(cl);
                    else overloadedFound = true;
                }

            if (list.Count == 0)
            {
                Console.WriteLine(overloadedFound
                    ? "All suitable clients already have offers limit reached."
                    : "No client will accept this offer.");
                Pause();
                return;
            }

            var client = new ElementReader<Client>("client", ClientConsoleInterface.ClientTable, list).Read();

            Console.WriteLine(_storage.TryCreateOffer(client, realty)
                ? "Offer created successfully."
                : "Failed to create offer.");

            Pause();
        }
    }
}