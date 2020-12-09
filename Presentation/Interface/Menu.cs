using System;

namespace Presentation.Interface
{
    public class Menu : ConsoleUtils
    {
        private bool _selfTerminating;

        public Menu(string title, bool selfTerminating = false)
        {
            Title = title;
            _selfTerminating = selfTerminating;
            AddAction(new ConsoleAction(Terminate, "Close menu", int.MaxValue), '0');
        }

        protected override void WriteControls()
        {
            Console.WriteLine(Title);
            base.WriteControls();
        }

        public new Menu AddAction(ConsoleAction action, params char[] chars)
        {
            base.AddAction(action, chars);
            return this;
        }

        protected override void ProcessKeyInput(char ch)
        {
            base.ProcessKeyInput(ch);
            if(_selfTerminating)Terminate();
        }
    }
}