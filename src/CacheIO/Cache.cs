using System.IO;
using CacheIO.IO;

namespace CacheIO
{
	public class Cache
	{
		private string _folder;
		private bool _newProtocol;

		private Index[] _indexList;
		private IndexFile _index255;

		private RandomAccessFile _data;
		private byte[] _readCacheBuffer;

		public Index[] IndexList
		{
			get { return _indexList; }
		}


		public Cache(string folder) : this(folder, true) { }

		public Cache(string folder, bool newProtocol)
		{
			_folder = Path.GetFullPath(folder).TrimEnd('\\') + '\\';
			_newProtocol = newProtocol;

			_readCacheBuffer = new byte[520];

			_data = new RandomAccessFile(_folder + "main_file_cache.dat2");
			_index255 = new IndexFile(255, _data, new RandomAccessFile(_folder + "main_file_cache.idx255"), _readCacheBuffer, newProtocol);

			int indexCount = _index255.ArchiveCount;
			_indexList = new Index[indexCount];

			for (int i = 0; i < indexCount; i++)
			{
				Index index = new Index(new IndexFile(i, _data, new RandomAccessFile(_folder + "main_file_cache.idx" + i), _readCacheBuffer, newProtocol), _index255);
				if (index.Table != null)
				{
					_indexList[i] = index;
				}
			}
		}
	}
}