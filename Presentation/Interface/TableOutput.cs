using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Interface
{
    public abstract class TableOutput<T>
    {
        protected readonly Charset Chars;
        protected readonly List<TableColumn<T>> Columns;
        public readonly int RowsPerPage;

        private Predicate<T> _searchPredicate;

        public TableOutput(Charset charset, int rowsPerPage)
        {
            Chars = charset;
            RowsPerPage = rowsPerPage;
            Columns = new List<TableColumn<T>>();
        }

        public abstract TableOutput<T> Clone();

        public void ResetSearch()
        {
            _searchPredicate = null;
        }

        public void Search(Predicate<T> predicate)
        {
            _searchPredicate = predicate;
        }

        public IEnumerable<T> QueryFilter(IEnumerable<T> elements, string query)
        {
            foreach (var element in elements)
            {
                foreach (var column in Columns)
                {
                    if (!column.Value(element, 0).ToLowerInvariant().Contains(query)) continue;
                    yield return element;
                    break;
                }
            }
        }

        public IReadOnlyList<TableColumn<T>> GetColumns()
        {
            return Columns.AsReadOnly();
        }

        protected static string FormatString(string name, int length, bool right = false)
        {
            if (name.Length > length)
            {
                name = name.Substring(0, length - 3);
                name += "...";
            }
            else
            {
                name = right ? name.PadLeft(length) : name.PadRight(length);
            }

            return name;
        }

        private string SplitterLine(char[] line)
        {
            var builder = new StringBuilder();
            builder.Append(line[0]);
            var widths = ColWidths();
            for (var i = 0; i < widths.Count; i++)
            {
                if (i != 0) builder.Append(line[1]);
                builder.Append(new string(line[3], widths[i]));
            }

            builder.Append(line[2]);
            return builder.ToString();
        }

        private string EmptyLine(char filler = ' ')
        {
            var builder = new StringBuilder();
            builder.Append(Chars.LeftVertical);
            var widths = ColWidths();
            for (var i = 0; i < widths.Count; i++)
            {
                if (i != 0) builder.Append(Chars.CentralVertical);
                builder.Append(new string(filler, widths[i]));
            }

            builder.Append(Chars.RightVertical);

            return builder.ToString();
        }

        private string TextLine(IList<string> data)
        {
            var builder = new StringBuilder();
            builder.Append(Chars.LeftVertical);
            var widths = ColWidths();
            for (var i = 0; i < widths.Count; i++)
            {
                if (i != 0) builder.Append(Chars.CentralVertical);
                builder.Append(FormatString(data[i], widths[i]));
            }

            builder.Append(Chars.RightVertical);

            return builder.ToString();
        }

        private List<int> ColWidths()
        {
            var list = new List<int>();
            for (var i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];
                if (column.Visible()) list.Add(column.Width);
            }

            return list;
        }

        private List<string> GetHeader()
        {
            var list = new List<string>();
            for (var i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];
                if (column.Visible()) list.Add(column.Title);
            }

            return list;
        }

        private List<string> GetValues(T source, int index = 0)
        {
            var list = new List<string>();
            for (var i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];
                if (column.Visible()) list.Add(column.Value(source, index));
            }

            return list;
        }

        public List<TableColumn<T>> GetSortableColumns()
        {
            var list = new List<TableColumn<T>>();
            for (var i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];
                if (column.Visible() && column.SortSelector != null) list.Add(column);
            }

            return list;
        }

        public void WriteOne(T element, int index = -1)
        {
            WriteListContent(new[] {element}, forcedIndex: index, fillPage: false);
        }

        public void WriteListPaged(IEnumerable<T> list, int page)
        {
            var result = new List<T>();
            var skip = RowsPerPage * page;

            foreach (var element in list)
            {
                if (skip > 0)
                {
                    skip--;
                    continue;
                }

                if (skip-- <= -RowsPerPage) break;

                result.Add(element);
            }

            WriteListContent(result, page);
        }

        public void WriteListContent(IEnumerable<T> list, int page = 0, int forcedIndex = -1, bool fillPage = true,
            bool ignoreSorting = false)
        {
            var text = new StringBuilder();
            text.AppendLine(SplitterLine(Chars.TopRow()));
            text.AppendLine(TextLine(GetHeader()));
            text.AppendLine(SplitterLine(Chars.MiddleRow()));

            var index = forcedIndex >= 0 ? forcedIndex : page * RowsPerPage;
            var count = 0;
            foreach (var el in list)
            {
                if (!ignoreSorting && !(_searchPredicate is null) && !_searchPredicate(el))
                {
                    index++;
                    continue;
                }

                text.AppendLine(TextLine(GetValues(el, index)));
                index++;
                count++;
            }

            if (fillPage)
                for (var i = count; i < RowsPerPage; i++)
                    text.AppendLine(EmptyLine());

            text.AppendLine(SplitterLine(Chars.BottomRow()));

            Console.WriteLine(text.ToString());
        }

        protected void Col(int width, string title, Func<T, int, string> value,
            Func<T, IComparable> sortSelector = null, Func<bool> visible = null)
        {
            Columns.Add(new TableColumn<T>(width, title, value, sortSelector, visible));
        }
    }

    public readonly struct TableColumn<T>
    {
        public int Width { get; }
        public string Title { get; }
        public Func<T, int, string> Value { get; }
        public Func<T, IComparable> SortSelector { get; }
        public Func<bool> Visible { get; }

        public TableColumn(int width, string title, Func<T, int, string> value,
            Func<T, IComparable> sortSelector = null, Func<bool> visible = null)
        {
            Width = width;
            Title = title;
            Value = value;
            SortSelector = sortSelector;
            Visible = visible ?? (() => true);
        }
    }

    public readonly struct Charset
    {
        public Charset(char topHorizontal, char centralHorizontal, char bottomHorizontal,
            char leftVertical, char centralVertical, char rightVertical,
            char topLeftCorner, char topIntersection, char topRightCorner,
            char leftIntersection, char centralIntersection, char rightIntersection,
            char bottomLeftCorner, char bottomIntersection, char bottomRightCorner)
        {
            TopHorizontal = topHorizontal;
            CentralHorizontal = centralHorizontal;
            BottomHorizontal = bottomHorizontal;
            LeftVertical = leftVertical;
            CentralVertical = centralVertical;
            RightVertical = rightVertical;
            TopLeftCorner = topLeftCorner;
            TopIntersection = topIntersection;
            TopRightCorner = topRightCorner;
            LeftIntersection = leftIntersection;
            CentralIntersection = centralIntersection;
            RightIntersection = rightIntersection;
            BottomLeftCorner = bottomLeftCorner;
            BottomIntersection = bottomIntersection;
            BottomRightCorner = bottomRightCorner;
        }

        public Charset(char horizontal, char vertical, char topLeftCorner, char topIntersection, char topRightCorner,
            char leftIntersection, char centralIntersection, char rightIntersection, char bottomLeftCorner,
            char bottomIntersection, char bottomRightCorner) :
            this(horizontal, horizontal, horizontal,
                vertical, vertical, vertical,
                topLeftCorner, topIntersection, topRightCorner,
                leftIntersection, centralIntersection, rightIntersection,
                bottomLeftCorner, bottomIntersection, bottomRightCorner)
        {
        }

        public Charset(char horizontal, char vertical, char intersection) : this(horizontal, vertical,
            intersection, intersection, intersection,
            intersection, intersection, intersection,
            intersection, intersection, intersection)
        {
        }

        public Charset(char topHorizontal, char centralHorizontal, char bottomHorizontal,
            char leftVertical, char centralVertical, char rightVertical, char intersection) :
            this(topHorizontal, centralHorizontal, bottomHorizontal,
                leftVertical, centralVertical, rightVertical,
                intersection, intersection, intersection,
                intersection, intersection, intersection,
                intersection, intersection, intersection)
        {
        }

        public char[] TopRow()
        {
            return TableArray()[0];
        }

        public char[] MiddleRow()
        {
            return TableArray()[1];
        }

        public char[] BottomRow()
        {
            return TableArray()[2];
        }

        public char[][] TableArray()
        {
            return new[]
            {
                new[] {TopLeftCorner, TopIntersection, TopRightCorner, TopHorizontal},
                new[] {LeftIntersection, CentralIntersection, RightIntersection, CentralHorizontal},
                new[] {BottomLeftCorner, BottomIntersection, BottomRightCorner, BottomHorizontal}
            };
        }

        public char TopHorizontal { get; }
        public char CentralHorizontal { get; }
        public char BottomHorizontal { get; }
        public char LeftVertical { get; }
        public char CentralVertical { get; }
        public char RightVertical { get; }
        public char TopLeftCorner { get; }
        public char TopIntersection { get; }
        public char TopRightCorner { get; }
        public char LeftIntersection { get; }
        public char CentralIntersection { get; }
        public char RightIntersection { get; }
        public char BottomLeftCorner { get; }
        public char BottomIntersection { get; }
        public char BottomRightCorner { get; }

        public static Charset SymbolicCharset = new Charset('─', '│',
            '┌', '┬', '┐',
            '├', '┼', '┤',
            '└', '┴', '┘');

        public static Charset BorderlessSymbolicCharset = new Charset(' ', '─', ' ',
            ' ', '│', ' ',
            ' ', '│', ' ',
            '─', '┼', '─',
            ' ', '│', ' ');

        public static Charset AsciiCharset = new Charset('-', '|', '+');

        public static Charset BorderlessAsciiCharset = new Charset(' ', '-', ' ',
            ' ', '|', ' ',
            ' ', '|', ' ',
            '-', '+', '-',
            ' ', '|', ' ');
    }
}