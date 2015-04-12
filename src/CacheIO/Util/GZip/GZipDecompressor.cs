using System;
using System.IO.Compression;

namespace CacheIO.Util.GZip
{
	public class GZipDecompressor
	{
		public static void Decompress(byte[] output, System.IO.Stream stream)
		{
			GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress);
			gzip.Read(output, 0, output.Length);
			gzip.Close();
		}
	}
}
