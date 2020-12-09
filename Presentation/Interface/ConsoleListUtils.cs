using System;
using System.Collections.Generic;
using Presentation.Interface.Readers;
using Presentation.Interface.Readers.Validators;

namespace Presentation.Interface
{
    public class ConsoleListUtils<T> : ConsoleUtils
    {
        public readonly List<T> List;
        protected readonly TableOutput<T> Table;

        protected Reader<int> IdReader;

        protected ClassManipulator<T> Manipulator;
        protected string ObjectName;
        protected int Page;

        protected IComparer<T> SortComparer;

        public ConsoleListUtils(ClassManipulator<T> manipulator, TableOutput<T> table, string exitText = "Close menu") :
            this(false, table, exitText)
        {
            Manipulator = manipulator;
        }

        public ConsoleListUtils(TableOutput<T> table, string exitText) : this(true, table, exitText)
        {
        }

        private ConsoleListUtils(bool isReadonly, TableOutput<T> table, string exitText)
        {
            List = new List<T>();
            ObjectName = typeof(T).Name;
            Table = table;

            IdReader = new IntReader($"{ObjectName} index", new IndexValidator(List));

            AddAction(new ConsoleAction(ViewItem, $"View {ObjectName}", 0, ListNotEmpty), '1');
            if (!isReadonly)
            {
                AddAction(new ConsoleAction(AddItem, $"Create new {ObjectName}"), '2');
                AddAction(new ConsoleAction(RemoveItem, $"Remove {ObjectName}", 0, ListNotEmpty), '3');
                AddAction(new ConsoleAction(EditItem, $"Edit {ObjectName}", 0, ListNotEmpty), '4');
            }

            AddAction(
                new ConsoleAction(Sort, "Sort list", 1, () => !isReadonly && Table.GetSortableColumns().Count > 0),
                's');
            AddAction(new ConsoleAction(new Menu("Search type", true)
                    .AddAction(new ConsoleAction(Search, "Normal search"), '1')
                    .AddAction(new ConsoleAction(AdvancedSearch, "Advanced search"), '2'),
                "Search", 1, () => !isReadonly), 'f');
            AddAction(new ConsoleAction(() => Page--, "Previous page", 2, () => Page > 0), '<', ',');
            AddAction(new ConsoleAction(() => Page++, "Next page", 2, () => Page < PagesNum - 1), '>', '.');
            if (exitText != null)
                AddAction(new ConsoleAction(Terminate, exitText, int.MaxValue), '0');
        }

        protected int PagesNum => (List.Count-1) / Table.RowsPerPage + 1;

        public override void Start()
        {
            UpdateList();
            base.Start();
        }

        protected virtual void AddItem()
        {
            var instance = Manipulator.NewInstance();
            Manipulator.Modify(instance);
            Table.WriteOne(instance, List.Count);
            if (new BooleanReader("").CustomMessage($"Confirm adding this {ObjectName}?").Read())
            {
                if (TryAdd(instance))
                    Console.WriteLine($"{ObjectName} added.");
                else
                    Console.WriteLine("Couldn't add object");
            }
            else
            {
                Console.WriteLine("Adding canceled");
            }

            Pause();
        }

        protected virtual bool TryAdd(T item)
        {
            List.Add(item);
            if (SortComparer != null) List.Sort(SortComparer);
            return true;
        }

        protected virtual void RemoveItem()
        {
            var id = IdReader.Read();
            Table.WriteOne(List[id], id);

            if (new BooleanReader("").CustomMessage($"Confirm deleting this {ObjectName}?").Read())
            {
                if (TryRemove(id))
                    Console.WriteLine($"{ObjectName} deleted");
                else
                    Console.WriteLine("Deletion failed");
            }
            else
            {
                Console.WriteLine("Deletion canceled");
            }

            UpdateList();
            Pause();
        }

        protected virtual bool TryRemove(int id)
        {
            List.RemoveAt(id);
            return true;
        }

        protected virtual void ViewItem()
        {
            var id = IdReader.Read();
            ShowItemDetails(id);
            Pause();
        }

        protected virtual void ShowItemDetails(int id)
        {
            Table.WriteOne(List[id], id);
        }

        protected virtual void EditItem()
        {
            var id = IdReader.Read();
            Table.WriteOne(List[id], id);
            var values = Manipulator.InputValues(Manipulator.GetValues(List[id]));
            var empty = Manipulator.NewInstance();
            Manipulator.Assign(empty, values);
            Table.WriteOne(empty, id);

            if (new BooleanReader("").CustomMessage("Accept changes?").Read())
            {
                if (TryModify(List[id], values))
                    Console.WriteLine($"{ObjectName} modified.");
                else
                    Console.WriteLine($"{ObjectName} modification failed.");
            }
            else
            {
                Console.WriteLine($"{ObjectName} modification canceled.");
            }

            UpdateList();
            Pause();
        }

        protected virtual bool TryModify(T target, Dictionary<string, object> values)
        {
            Manipulator.Assign(target, values);
            return true;
        }

        protected virtual void Sort()
        {
            var cols = Table.GetSortableColumns();
            for (var i = 0; i < cols.Count; i++)
            {
                var column = cols[i];
                Console.WriteLine($"{i}: {column.Title}");
            }

            var index = new IntReader("", 0, cols.Count - 1).CustomMessage("Select column to sort by.").Read();

            var col = cols[index];

            var ascending = new BooleanReader("Sort ascending?").Read();

            SortComparer = new DelegateComparer<T>(col.SortSelector, !ascending);

            Console.WriteLine("Sorting settings applied.");
            UpdateList();
            Pause();
        }

        protected virtual void AdvancedSearch()
        {
            Console.WriteLine("Input values to filter by");
            var values = Manipulator.InputValues(null, true);
            ShowSearchWindow(obj => Manipulator.Match(obj, values), "Advanced Search");
        }

        protected virtual void Search()
        {
            var query = new StringReader("search query").Read().ToLowerInvariant();
            var filtered = new HashSet<T>(Table.QueryFilter(List, query));
            ShowSearchWindow(obj => filtered.Contains(obj),$"Searching for \"{query}\" (Case insensitive)");
        }

        protected virtual void ShowSearchWindow(Predicate<T> predicate,string title=null)
        {
            var newTable = Table.Clone();
            newTable.Search(predicate);
            var console = new ConsoleListUtils<T>(newTable, "Close search window");
            console.List.AddRange(List);
            console.Title = title;
            console.Start();
        }

        protected virtual void UpdateList()
        {
            if (SortComparer != null) List.Sort(SortComparer);
        }

        protected virtual bool ListNotEmpty()
        {
            return List.Count > 0;
        }

        protected override void WriteControls()
        {
            if (!string.IsNullOrEmpty(Title)) Console.WriteLine(Title);
            Table.WriteListPaged(List, Page);
            base.WriteControls();
        }

        private class IndexValidator : Validator<int>
        {
            public readonly List<T> TargetList;

            public IndexValidator(List<T> targetList)
            {
                TargetList = targetList;
            }

            public override bool Validate(int value, out string errorMessage)
            {
                errorMessage = $"Index {value} is out of [0;{TargetList.Count - 1}] range";
                return value >= 0 && value < TargetList.Count;
            }
        }

        private class DelegateComparer<T> : IComparer<T>
        {
            private readonly Func<T, IComparable> _compareValue;
            private readonly bool _descending;

            public DelegateComparer(Func<T, IComparable> compareValue, bool descending)
            {
                _compareValue = compareValue;
                _descending = descending;
            }


            public int Compare(T x, T y)
            {
                var result = _compareValue(x).CompareTo(_compareValue(y));
                if (_descending) result *= -1;
                return result;
            }
        }
    }
}