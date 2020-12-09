using System;
using System.Collections.Generic;
using Data.Bundles;
using Logic;
using Logic.Model;
using Presentation.Interface;
using Presentation.Interface.Readers;
using Presentation.Interface.Readers.Validators;

namespace Presentation.Connectors
{
    public class ClientConsoleInterface : ConsoleListUtils<Client>
    {
        public static readonly ClientTable ClientTable = new ClientTable(Charset.SymbolicCharset, 5);
        public static readonly ClientTable ExtendedTable = new ExtendedClientTable(Charset.SymbolicCharset, 5);

        private static readonly ClassManipulator<Client> ClientManipulator = new ClassManipulator<Client>(
                () => new Client()
            ).AddReader(nameof(Client.FirstName), new StringReader("first name"))
            .AddReader(nameof(Client.LastName), new StringReader("last name"))
            .AddReader(nameof(Client.BankAccount),
                new StringReader("bank account number", new BankAccountValidator())
                    .Process(s => new BankAccount(s)).Default(acc => acc.Id));

        private static readonly ClassManipulator<Requirements> RequirementsManipulator =
            new ClassManipulator<Requirements>(
                    () => new Requirements())
                .AddReader(new FloatRangeReader("price range:", nameof(Requirements.MinPrice),
                    nameof(Requirements.MaxPrice), true, new GreaterThanValidator<float>(0, true)))
                .AddReader(new IntRangeReader("rooms number range:", nameof(Requirements.MinRoomsNumber),
                    nameof(Requirements.MaxRoomsNumber), true, new GreaterThanValidator<int>(0, true)))
                .AddReader(new FloatRangeReader("area range:", nameof(Requirements.MinArea),
                    nameof(Requirements.MaxArea), true, new GreaterThanValidator<float>(0, true)))
                .AddReader(nameof(Requirements.AcceptedTypes),
                    new ListReader<RealtyType>("accepted realty types", new EnumReader<RealtyType>("Realty type"))
                        .Process(list => new HashSet<RealtyType>(list)).Default(set => new List<RealtyType>(set)));

        private readonly Storage _storage;

        public ClientConsoleInterface(Storage storage) : base(ClientManipulator,
            new ClientTable(Charset.SymbolicCharset, 5, storage))
        {
            _storage = storage;
            AddAction(new ConsoleAction(EditRequirements, "Edit Client requirements", 0, ListNotEmpty), '5');
            AddAction(new ConsoleAction(ViewOffers, "View Client offers", 0, ListNotEmpty), '6');
            AddAction(new ConsoleAction(SearchForRealties, "Find suitable realty", 0, ListNotEmpty), '7');
        }

        protected override bool TryAdd(Client item)
        {
            _storage.AddClient(item);
            UpdateList();
            return true;
        }

        protected override bool TryRemove(int id)
        {
            var item = List[id];
            _storage.RemoveClient(item);
            return true;
        }

        protected override void ShowItemDetails(int id)
        {
            ExtendedTable.WriteOne(List[id], id);
        }

        protected override void UpdateList()
        {
            List.Clear();
            List.AddRange(_storage.Clients.Values);
            base.UpdateList();
        }

        private void EditRequirements()
        {
            var id = IdReader.Read();
            ExtendedTable.WriteOne(List[id], id);
            var values = RequirementsManipulator.InputValues(RequirementsManipulator.GetValues(List[id].Requirements));
            var empty = RequirementsManipulator.NewInstance();
            var clientClone = JsonBundle.CloneBundlable(List[id]);
            RequirementsManipulator.Assign(empty, values);
            clientClone.Requirements = empty;
            ExtendedTable.WriteOne(clientClone, id);

            if (new BooleanReader("").CustomMessage("Accept changes?").Read())
            {
                List[id].Requirements = empty;
                Console.WriteLine($"{ObjectName} modified.");
            }
            else
            {
                Console.WriteLine($"{ObjectName} modification canceled.");
            }

            Pause();
        }

        private void ViewOffers()
        {
            var id = IdReader.Read();
            var list = new OffersList(_storage, List[id]);
            list.Start();
        }

        private void SearchForRealties()
        {
            var id = IdReader.Read();
            var client = List[id];
            if (_storage.ClientOffers(client) >= Storage.OffersPerClient)
            {
                Console.WriteLine("This client already reached his offers limit");
                Pause();
                return;
            }
            var suitable = new List<Realty>();
            foreach (var realty in _storage.Realties.Values)
            {
                if (client.Requirements.IsAcceptable(realty) && !_storage.IsOfferExists(client,realty))
                {
                    suitable.Add(realty);
                }
            }

            if (suitable.Count == 0)
            {
                Console.WriteLine("No suitable offers exists");
                Pause();
                return;
            }

            var selected =
                new ElementReader<Realty>("realty to create offer", RealtyConsoleInterface.RealtyTable, suitable).Read();
            if (new BooleanReader("").CustomMessage($"Create offer with {selected.Title}?").Read())
            {
                Console.WriteLine(_storage.TryCreateOffer(client, selected)
                    ? "Offer created successfully."
                    : "Failed to create an offer.");
            }
            else
            {
                Console.WriteLine("Offer creation canceled.");
            }
            Pause();
        }


        private class BankAccountValidator : Validator<string>
        {
            public override bool Validate(string value, out string errorMessage)
            {
                errorMessage = "Account number should only contain 16 numeric digits that may be separated by spaces.";
                try
                {
                    BankAccount.CheckCodeValidity(value);
                    return true;
                }
                catch (BankAccount.AccountNumberFormatException)
                {
                    return false;
                }
            }
        }
    }
}