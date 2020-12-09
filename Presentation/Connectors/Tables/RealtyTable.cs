using System;
using Logic.Model;
using Presentation.Interface;

namespace Presentation.Connectors
{
    public class RealtyTable : TableOutput<Realty>
    {
        public RealtyTable(Charset charset, int rowsPerPage) : base(charset, rowsPerPage)
        {
            Col(5, "Index", (_, i) => i.ToString());
            Col(20, "Title", (realty, i) => realty.Title, realty => realty.Title);
            Col(15, "Price", (realty, i) => realty.Price.ToString("N") + "$", realty => realty.Price);
            Col(9, "Rooms Num", (realty, i) => realty.RoomsNumber.ToString(), realty => realty.RoomsNumber);
            Col(12, "Area (m²)", (realty, i) => realty.Area.ToString("N"), realty => realty.Area);
            Col(5, "Type", (realty, i) => Enum.GetName(typeof(RealtyType), realty.Type), realty => realty.Type);
            Col(19, "Added date", (realty, i) => realty.AddedDate.ToString("dd.MM.yy hh:mm:ss"), realty => realty.Area);
        }

        public override TableOutput<Realty> Clone()
        {
            return new RealtyTable(Chars, RowsPerPage);
        }
    }
}