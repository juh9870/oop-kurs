using Presentation.Interface;

namespace Presentation.Connectors
{
    public class RealtyOrClientTable : TableOutput<RealtyOrClient>
    {
        public RealtyOrClientTable(Charset charset, int rowsPerPage) : base(charset, rowsPerPage)
        {
            Col(5, "Index", (_, i) => i.ToString());
            Col(6, "Type", (combined, i) => combined.IsRealty ? "Realty" : "Client");
            Col(28, "Title", (combined, i) =>
                combined.IsRealty ? combined.Realty.Title : $"{combined.Client.FirstName} {combined.Client.LastName}");
        }

        public override TableOutput<RealtyOrClient> Clone()
        {
            return new RealtyOrClientTable(Chars, RowsPerPage);
        }
    }
}