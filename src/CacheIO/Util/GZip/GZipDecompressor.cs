using ICSharpCode.SharpZipLib.GZip;

namespace CacheIO.Util.GZip
{
	public class GZipDecompressor
	{
		public static void Decompress(byte[] output, System.IO.Stream stream)
		{
			GZipInputStream gzip = new GZipInputStream(stream);
			gzip.Read(output, 0, output.Length);
			gzip.Close();
		}
	}
}
