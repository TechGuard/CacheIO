using System;

namespace CacheIO
{
	public class Index
	{
		private IndexFile _index;
		private IndexFile _index255;
		private ReferenceTable _table;


		public Index(IndexFile index, IndexFile index255)
		{
			_index = index;
			_index255 = index255;
			_table = null;
		}

		public ReferenceTable getTable()
		{
			return _table;
		}
	}
}