using System;
using Logic;
using Presentation.Interface;

namespace Presentation.Connectors
{
    public class MainMenu : Menu
    {
        public MainMenu(Storage storage) : base("")
        {
            AddAction(new ConsoleAction(new ClientConsoleInterface(storage), "Clients management"), '1');
            AddAction(new ConsoleAction(new RealtyConsoleInterface(storage), "Realties management"), '2');
            AddAction(new ConsoleAction(() =>
            {
                RealtyOrClientList.Search(storage.Clients.Values, storage.Realties.Values);
            }, "Search in clients and realties"), 'f');
            AddAction(new ConsoleAction(storage.SaveData, "Save database"), 's');
            AddAction(
                new ConsoleAction(storage.LoadData, "Reload database from file (discard changes since last save)"),
                'l');
            AddAction(new ConsoleAction(Terminate, "Exit", int.MaxValue), true, '0');
        }

        public override void Start()
        {
            Console.SetWindowSize(128, 26);
            base.Start();
        }
    }
}