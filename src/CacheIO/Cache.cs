using System;
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


		public Cache(string folder) : this(folder, true) { }

		public Cache(string folder, bool newProtocol)
		{
			_folder = Path.GetFullPath(folder).TrimEnd('\\') + '\\';
			_newProtocol = newProtocol;

			_data = new RandomAccessFile(_folder + "main_file_cache.dat2");
			_index255 = new IndexFile(255, _data, new RandomAccessFile(_folder + "main_file_cache.idx255"), newProtocol);

			int indexCount = _index255.getArchivesCount();
			_indexList = new Index[indexCount];

			for (int i = 0; i < indexCount; i++)
			{
				Index index = new Index(new IndexFile(i, _data, new RandomAccessFile(_folder + "main_file_cache.idx" + i), newProtocol), _index255);
				if (index.getTable() != null)
				{
					_indexList[i] = index;
				}
			}
		}

		public Index[] getIndexList()
		{
			return _indexList;
		}
	}
}