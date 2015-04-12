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
				throw new ArgumentOutOfRangeException("INVALID PROTOCOL");

			if (protocol >= 6)
				_revision = stream.readInt();

			int hash = stream.readUnsignedByte();
			_named = (0x1 & hash) != 0;
			_usingWhirpool = (0x2 & hash) != 0;

			int validArchivesCount = (protocol >= 7 ? stream.readBigSmart() : stream.readUnsignedShort());
			_validArchiveIds = new int[validArchivesCount];

			int lastArchiveId = 0;
			int biggestArchiveId = 0;
			for (int index = 0; index < validArchivesCount; index++)
			{
				int archiveId = (lastArchiveId = lastArchiveId + (protocol >= 7 ? stream.readBigSmart() : stream.readUnsignedShort()));
				if (archiveId > biggestArchiveId)
					biggestArchiveId = archiveId;
				_validArchiveIds[index] = archiveId;
			}

			_archiveList = new ArchiveReference[biggestArchiveId + 1];

			for (int index = 0; index < validArchivesCount; index++)
				_archiveList[_validArchiveIds[index]] = new ArchiveReference();

			if (_named)
			{
				for (int index = 0; index < validArchivesCount; index++)
					_archiveList[_validArchiveIds[index]].NameHash = (stream.readInt());
			}
			if (_usingWhirpool)
			{
				for (int index = 0; index < validArchivesCount; index++)
				{
					byte[] whirpool = new byte[64];
					stream.Read(whirpool, 0, 64);
					_archiveList[_validArchiveIds[index]].Whirpool = (whirpool);
				}
			}

			for (int index = 0; index < validArchivesCount; index++)
				_archiveList[_validArchiveIds[index]].CRC = (stream.readInt());
			for (int index = 0; index < validArchivesCount; index++)
				_archiveList[_validArchiveIds[index]].Revision = (stream.readInt());
			for (int index = 0; index < validArchivesCount; index++)
				_archiveList[_validArchiveIds[index]].ValidFileIds = (new int[(protocol >= 7 ? stream.readBigSmart() : stream.readUnsignedShort())]);

			for (int index = 0; index < validArchivesCount; index++)
			{
				int lastFileId = 0;
				int biggestFileId = 0;
				ArchiveReference archive = _archiveList[_validArchiveIds[index]];

				for (int index2 = 0; index2 < archive.ValidFileIds.Length; index2++)
				{
					int fileId = (lastFileId = lastFileId + (protocol >= 7 ? stream.readBigSmart() : stream.readUnsignedShort()));
					if (fileId > biggestFileId)
						biggestFileId = fileId;
					archive.ValidFileIds[index2] = fileId;
				}

				archive.FileList = (new FileReference[biggestFileId + 1]);

				for (int index2 = 0; index2 < archive.ValidFileIds.Length; index2++)
					archive.FileList[archive.ValidFileIds[index2]] = new FileReference();
			}
			if (_named)
			{
				for (int index = 0; index < validArchivesCount; index++)
				{
					ArchiveReference archive = _archiveList[_validArchiveIds[index]];
					for (int index2 = 0; index2 < archive.ValidFileIds.Length; index2++)
						archive.FileList[archive.ValidFileIds[index2]].NameHash = (stream.readInt());
				}
			}

			Console.WriteLine(validArchivesCount + " Archives\n");
		}
	}
}