using System;

namespace CacheIO
{
	public class ArchiveReference
	{
		private int _nameHash;
		private byte[] _whirpool;
		private int _crc;
		private int _revision;

		private int[] _validFileIds;
		private FileReference[] _fileList;

		public int NameHash
		{
			get { return _nameHash; }
			set { _nameHash = value; }
		}

		public byte[] Whirpool
		{
			get { return _whirpool; }
			set { _whirpool = value; }
		}

		public int CRC
		{
			get { return _crc; }
			set { _crc = value; }
		}

		public int Revision
		{
			get { return _revision; }
			set { _revision = value; }
		}

		public int[] ValidFileIds
		{
			get { return _validFileIds; }
			set { _validFileIds = value; }
		}

		public FileReference[] FileList
		{
			get { return _fileList; }
			set { _fileList = value; }
		}
	}
}
