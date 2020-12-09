using System;
using Data.Bundles;
using Data.IO;
using Logic;
using Presentation.Connectors;

namespace Presentation
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting up...");
            var storage = new Storage(new JsonBundle.Factory(), new FileInterface());
            storage.LoadData();
            new MainMenu(storage).Start();
            storage.SaveData();
        }
    }
}