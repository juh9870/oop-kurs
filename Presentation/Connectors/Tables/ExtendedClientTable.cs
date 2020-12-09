using Logic.Model;
using Presentation.Interface;

namespace Presentation.Connectors
{
    public class ExtendedClientTable : ClientTable
    {
        public ExtendedClientTable(Charset charset, int rowsPerPage) : base(charset, rowsPerPage)
        {
            Col(16, "Min Price", (source, index) => $"{source.Requirements.MinPrice:N}$");
            Col(16, "Max Price", (source, index) => $"{source.Requirements.MaxPrice:N}$");
            Col(9, "Rooms Num",
                (source, index) => $"{source.Requirements.MinRoomsNumber}-{source.Requirements.MaxRoomsNumber}");
            Col(18, "Area (m²)", (source, index) => $"{source.Requirements.MinArea:N}-{source.Requirements.MaxArea:N}");
            Col(16, "Accepted types", (source, index) => string.Join(", ", source.Requirements.AcceptedTypes));
        }

        public override TableOutput<Client> Clone()
        {
            return new ExtendedClientTable(Chars, RowsPerPage);
        }
    }
}