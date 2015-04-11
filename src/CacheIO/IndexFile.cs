using System;
using CacheIO.IO;

namespace CacheIO
{
	public class IndexFile
	{
		private int _id;
		private RandomAccessFile _data;
		private RandomAccessFile _index;
		private bool _newProtocol;
		private byte[] _readBuffer = new byte[520];


		public IndexFile(int id, RandomAccessFile data, RandomAccessFile index, bool newProtocol)
		{
			_id = id;
			_data = data;
			_index = index;
			_newProtocol = newProtocol;
		}

		public int getID()
		{
			return _id;
		}

		public int getArchivesCount()
		{
			return (int)(_index.getLength() / 6L);
		}
	}
}