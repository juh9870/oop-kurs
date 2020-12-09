using System.IO;
using System.IO.Compression;

namespace Data.IO
{
    public abstract class IoInterface
    {
        public abstract void Write(byte[] data);
        public abstract byte[] Read();

        public abstract void Erase();

        protected static byte[] Compress(byte[] data)
        {
            using var compressedStream = new MemoryStream();
            using var zipStream = new GZipStream(compressedStream, CompressionMode.Compress);
            zipStream.Write(data, 0, data.Length);
            zipStream.Close();
            return compressedStream.ToArray();
        }

        protected static byte[] Decompress(byte[] data)
        {
            using var compressedStream = new MemoryStream(data);
            using var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var resultStream = new MemoryStream();
            zipStream.CopyTo(resultStream);
            return resultStream.ToArray();
        }
    }
}