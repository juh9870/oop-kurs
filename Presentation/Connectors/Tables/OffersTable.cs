using Logic;
using Logic.Model;
using Presentation.Interface;

namespace Presentation.Connectors
{
    public class OffersTable : TableOutput<Offer>
    {
        private readonly Storage _storage;

        public OffersTable(Charset charset, int rowsPerPage, Storage storage) : base(charset, rowsPerPage)
        {
            _storage = storage;
            Col(5, "Index", (source, index) => index.ToString());
            Col(25, "Client", (source, index) =>
            {
                var client = _storage.Clients[source.ClientId];
                return client.FirstName + " " + client.LastName;
            });
            Col(25, "Realty", (source, index) => _storage.Realties[source.RealtyId].Title);
        }

        public override TableOutput<Offer> Clone()
        {
            return new OffersTable(Chars, RowsPerPage,_storage);
        }
    }
}