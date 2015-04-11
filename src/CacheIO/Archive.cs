using System;

namespace CacheIO
{
	public class Archive
	{
		private int _id;
		private int _revision;
		private int _compression;

		private byte[] _data;
		private int[] _keys;


		public Archive(int id, byte[] data, int[] keys)
		{
			_id = id;
			_keys = keys;

			decompress(data);
		}

		public Archive(int id, int revision, int compression, byte[] data)
		{
			_id = id;
			_revision = revision;
			_compression = compression;
			_data = data;
		}

		public byte[] compress()
		{
			return null;
		}

		private void decompress(byte[] data)
		{

		}
	}
}