using System;
using CacheIO.IO;

namespace CacheIO
{
	public class ReferenceTable
	{
		private Archive _archive;

		private int _revision;
		private bool _named;
		private bool _usingWhirpool;

		private int[] _validArchiveIds;
		private ArchiveReference[] _archiveList;


		public ArchiveReference[] ArchiveList
		{
			get { return _archiveList; }
		}


		public ReferenceTable(Archive archive)
		{
			_archive = archive;
			
			decodeHeader();
		}

		private void decodeHeader()
		{
			DataInputStream stream = new DataInputStream(_archive.Data);
			int protocol = stream.readUnsignedByte();

			if (protocol < 5 || protocol > 7)
			{
				throw new ArgumentOutOfRangeException("INVALID PROTOCOL");
			}

			if (protocol >= 6)
			{
				_revision = stream.readInt();
			}

			int hash = stream.readUnsignedByte();
			_named = (0x1 & hash) != 0;
			_usingWhirpool = (0x2 & hash) != 0;

			int validArchivesCount = protocol >= 7 ? stream.readBigSmart() : stream.readUnsignedShort();
			_validArchiveIds = new int[validArchivesCount];

			int lastArchiveId = 0;
			int biggestArchiveId = 0;
			for (int i = 0; i < validArchivesCount; i++)
			{
				int archiveId = (lastArchiveId = lastArchiveId + (protocol >= 7 ? stream.readBigSmart() : stream.readUnsignedShort()));
				if (archiveId > biggestArchiveId)
				{
					biggestArchiveId = archiveId;
				}

				_validArchiveIds[i] = archiveId;
			}

			_archiveList = new ArchiveReference[biggestArchiveId + 1];

			for (int i = 0; i < validArchivesCount; i++)
			{
				_archiveList[_validArchiveIds[i]] = new ArchiveReference();
			}

			if (_named)
			{
				for (int i = 0; i < validArchivesCount; i++)
				{
					_archiveList[_validArchiveIds[i]].NameHash = stream.readInt();
				}
			}

			if (_usingWhirpool)
			{
				for (int i = 0; i < validArchivesCount; i++)
				{
					byte[] whirpool = new byte[64];
					stream.Read(whirpool, 0, 64);
					_archiveList[_validArchiveIds[i]].Whirpool = (whirpool);
				}
			}

			for (int i = 0; i < validArchivesCount; i++)
			{
				_archiveList[_validArchiveIds[i]].CRC = stream.readInt();
			}
			for (int i = 0; i < validArchivesCount; i++)
			{
				_archiveList[_validArchiveIds[i]].Revision = stream.readInt();
			}
			for (int i = 0; i < validArchivesCount; i++)
			{
				_archiveList[_validArchiveIds[i]].ValidFileIds = new int[(protocol >= 7 ? stream.readBigSmart() : stream.readUnsignedShort())];
			}

			for (int i = 0; i < validArchivesCount; i++)
			{
				int lastFileId = 0;
				int biggestFileId = 0;
				ArchiveReference archive = _archiveList[_validArchiveIds[i]];

				for (int j = 0; j < archive.ValidFileIds.Length; j++)
				{
					int fileId = (lastFileId = lastFileId + (protocol >= 7 ? stream.readBigSmart() : stream.readUnsignedShort()));
					if (fileId > biggestFileId)
					{
						biggestFileId = fileId;
					}

					archive.ValidFileIds[j] = fileId;
				}

				archive.FileList = (new FileReference[biggestFileId + 1]);

				for (int j = 0; j < archive.ValidFileIds.Length; j++)
				{
					archive.FileList[archive.ValidFileIds[j]] = new FileReference();
				}
			}

			if (_named)
			{
				for (int i = 0; i < validArchivesCount; i++)
				{
					ArchiveReference archive = _archiveList[_validArchiveIds[i]];
					for (int j = 0; j < archive.ValidFileIds.Length; j++)
					{
						archive.FileList[archive.ValidFileIds[j]].NameHash = (stream.readInt());
					}
				}
			}
		}
	}
}