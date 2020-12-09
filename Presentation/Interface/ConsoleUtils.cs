using System;
using System.Collections.Generic;
using System.Linq;

namespace Presentation.Interface
{
    public abstract class ConsoleUtils
    {
        private readonly Dictionary<char, ConsoleAction> Actions = new Dictionary<char, ConsoleAction>();
        private readonly Dictionary<char, char> Aliases = new Dictionary<char, char>();
        private bool _terminated;
        protected string Title;

        protected virtual void WriteControls()
        {
            Console.WriteLine("Possible actions:");
            foreach (var (key, value) in Actions.OrderBy(pair => pair.Value.Order).ThenBy(pair => pair.Key))
            {
                if (!value.Visible()) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"{key}: {value.Title}");
                Console.ResetColor();
            }
        }

        protected virtual void ProcessKeyInput(char ch)
        {
            foreach (var (key, value) in Actions)
            {
                if (key != ch) continue;
                value.Action.Invoke();
                return;
            }
        }

        protected virtual bool ValidateInputChar(char ch)
        {
            foreach (var (key, value) in Actions)
                if (key == ch && value.Visible())
                    return true;

            return false;
        }

        protected void WaitForInput()
        {
            Console.Clear();
            WriteControls();

            Console.WriteLine();
            var ch = '\u0000';
            do
            {
                ClearLine();
                var key = Console.ReadKey();
                ch = Aliases.ContainsKey(key.KeyChar) ? Aliases[key.KeyChar] : key.KeyChar;
            } while (!ValidateInputChar(ch));

            Console.WriteLine();

            ProcessKeyInput(ch);
        }

        public virtual void Start()
        {
            _terminated = false;
            while (!_terminated) WaitForInput();
        }

        protected void Terminate()
        {
            _terminated = true;
        }

        protected static void Pause()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            ClearLine();
        }

        protected void AddAction(ConsoleAction action, params char[] chars)
        {
            AddAction(action, false, chars);
        }

        protected void AddAction(ConsoleAction action, bool force, params char[] chars)
        {
            if (!force && HasAction(chars[0]))
                throw new ConflictingActionKeyException(chars[0]);
            Actions[chars[0]] = action;
            for (var i = 1; i < chars.Length; i++)
            {
                if (!force && HasAction(chars[i]))
                    throw new ConflictingActionKeyException(chars[i]);
                Aliases.Add(chars[i], chars[0]);
            }
        }

        protected bool HasAction(char key)
        {
            return Actions.ContainsKey(key) || Aliases.ContainsKey(key);
        }

        public static void ClearLine()
        {
            var curLine = Console.CursorTop;
            Console.CursorLeft = 0;
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, curLine);
        }

        public class ConflictingActionKeyException : Exception
        {
            public ConflictingActionKeyException(char key) : base(
                $"Character '{key}' already assigned to an action. Invoke AddAction with force: true if overwriting is intended.")
            {
            }
        }
    }
}