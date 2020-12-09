using System;

namespace Presentation.Interface
{
    public class ReflectionTableOutput<T> : TableOutput<T>
    {
        private readonly bool _indexed;

        public ReflectionTableOutput(Charset charset, int rowsPerPage, bool indexed) : base(charset, rowsPerPage)
        {
            _indexed = indexed;
            _indexed = indexed;
            if (indexed) Col(5, "Index", (_, i) => i.ToString());

            foreach (var property in typeof(T).GetProperties())
                if (!(property.GetMethod is null) && property.GetMethod.IsPublic)
                    Col(Math.Max(property.Name.Length, 8),
                        property.Name,
                        (obj, i) => property.GetValue(obj)?.ToString());
        }

        public override TableOutput<T> Clone()
        {
            return new ReflectionTableOutput<T>(Chars, RowsPerPage,_indexed);
        }
    }
}