using Logic.Model;

namespace Presentation.Connectors
{
    public class RealtyOrClient
    {
        public bool IsRealty => Realty!=null;
        public readonly Realty Realty;
        public readonly Client Client;

        public RealtyOrClient(Realty realty)
        {
            Realty = realty;
            Client = null;
        }
        public RealtyOrClient(Client client)
        {
            Client = client;
            Realty = null;
        }
    }
}