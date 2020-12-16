using System.Collections.Generic;
using Logic.Model;
using Presentation.Interface;
using Presentation.Interface.Readers;

namespace Presentation.Connectors
{
    public class RealtyOrClientList : ConsoleListUtils<RealtyOrClient>
    {
        public static RealtyOrClientTable RealtyOrClientTable = new RealtyOrClientTable(Charset.SymbolicCharset, 5);
        public RealtyOrClientList(TableOutput<RealtyOrClient> table, string exitText) : base(table, exitText)
        {
        }

        protected override string ObjectName => "Realty or Client";

        protected override void ShowItemDetails(int id)
        {
            var item = List[id];
            if(item.IsRealty) RealtyConsoleInterface.RealtyTable.WriteOne(item.Realty);
            else ClientConsoleInterface.ExtendedTable.WriteOne(item.Client);
        }

        public static void Search(IEnumerable<Client> clients, IEnumerable<Realty> realties)
        {
            var query = new StringReader("search query").Read().ToLowerInvariant();
            
            var list = new RealtyOrClientList(RealtyOrClientTable, "Close search window");

            list.Title = $"Searching for \"{query}\" (Case insensitive)";

            foreach (var client in ClientConsoleInterface.ClientTable.QueryFilter(clients,query))
            {
                list.List.Add(new RealtyOrClient(client));
            }
            foreach (var realty in RealtyConsoleInterface.RealtyTable.QueryFilter(realties,query))
            {
                list.List.Add(new RealtyOrClient(realty));
            }
            
            list.Start();
        }
    }
}