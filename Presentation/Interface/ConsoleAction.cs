using System;

namespace Presentation.Interface
{
    public class ConsoleAction
    {
        public ConsoleAction(Action action, string title, int order = 0, Func<bool> visibilityFunction = null)
        {
            Action = action;
            Title = title;
            Order = order;
            Visible = visibilityFunction ?? (() => true);
        }

        public ConsoleAction(ConsoleUtils menu, string title, int order = 0, Func<bool> visibilityFunction = null) :
            this(menu.Start, title, order, visibilityFunction)
        {
        }

        public Action Action { get; }

        public Func<bool> Visible { get; }
        public string Title { get; }

        public int Order { get; }
    }
}