using System;

namespace CacheIO
{
	public class Index
	{
		private IndexFile _index;
		private IndexFile _index255;
		private ReferenceTable _table;

		private byte[][][] _cachedFiles;

		public ReferenceTable Table
		{
			get { return _table; }
		}

		public int Id
		{
			get { return _index.Id; }
		}


		public Index(IndexFile index, IndexFile index255)
		{
			_index = index;
			_index255 = index255;

			byte[] archiveData = index255.getArchiveData(Id);
			if (archiveData == null)
			{
				return;
			}

			//crc = CRC32HGenerator.getHash(archiveData);
			//whirlpool = Whirlpool.getHash(archiveData, 0, archiveData.length);

			Archive archive = new Archive(Id, archiveData, null);
			if (archive.Data == null) return;
			_table = new ReferenceTable(archive);

			resetCachedFiles();
		}

		private void resetCachedFiles()
		{
			_cachedFiles = new byte[getLastArchiveId() + 1][][];
		}

		public int getLastArchiveId()
		{
			return _table.ArchiveList.Length - 1;
		}
	}
}