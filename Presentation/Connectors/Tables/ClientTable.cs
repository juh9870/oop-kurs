using Logic;
using Logic.Model;
using Presentation.Interface;

namespace Presentation.Connectors
{
    public class ClientTable : TableOutput<Client>
    {
        private readonly Storage _storage;

        public ClientTable(Charset charset, int rowsPerPage, Storage storage = null) : base(charset, rowsPerPage)
        {
            _storage = storage;
            Col(5, "Index", (source, index) => index.ToString());
            Col(10, "First Name", (source, index) => source.FirstName, client => client.FirstName);
            Col(10, "Last Name", (source, index) => source.LastName, client => client.LastName);
            Col(12, "Bank Account", (source, index) => FormatString(source.BankAccount.ToString(), 7),
                client => client.BankAccount.Id);
            if (!(storage is null))
                Col(6, "Offers",
                    (source, index) => storage.ClientOffers(source).ToString(), client => storage.ClientOffers(client));
        }

        public override TableOutput<Client> Clone()
        {
            return new ClientTable(Chars, RowsPerPage, _storage);
        }
    }
}