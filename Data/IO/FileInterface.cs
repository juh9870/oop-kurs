using System;
using System.IO;

namespace Data.IO
{
    public class FileInterface : IoInterface
    {
        public static readonly string DefaultPath = System.IO.Path.Combine(Environment.CurrentDirectory, "db.dat");

        public FileInterface(string path = null)
        {
            Path = path ?? DefaultPath;
        }

        private string Path { get; }

        public override void Write(byte[] data)
        {
            File.WriteAllBytes(Path, Compress(data));
        }

        public override byte[] Read()
        {
            if (!File.Exists(Path)) return null;
            return Decompress(File.ReadAllBytes(Path));
        }

        public override void Erase()
        {
            File.Delete(Path);
        }
    }
}