using System;
using CacheIO.IO;
using CacheIO.Util.CRC32;
using CacheIO.Util.NameHash;

namespace CacheIO
{
	public class Index
	{
		private IndexFile _index;
		private IndexFile _index255;
		private ReferenceTable _table;

		private int _crc;
		private byte[] _whirlpool;

		private byte[][][] _cachedFiles;

		public ReferenceTable Table
		{
			get { return _table; }
		}

		public int Id
		{
			get { return _index.Id; }
		}

		public int CRC
		{
			get { return _crc; }
		}

		public byte[] Whirlpool
		{
			get { return _whirlpool; }
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

			_crc = CRC32Generator.GetHash(archiveData);
			_whirlpool = Util.Whirlpool.Whirlpool.GetHash(archiveData);

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

		public int getLastFileId(int archiveId)
		{
			if (!archiveExists(archiveId))
			{
				return -1;
			}

			return _table.ArchiveList[archiveId].FileList.Length - 1;
		}

		public int getValidArchivesCount()
		{
			return _table.ValidArchiveIds.Length;
		}

		public int getValidFilesCount(int archiveId)
		{
			if (!archiveExists(archiveId))
			{
				return -1;
			}

			return _table.ArchiveList[archiveId].ValidFileIds.Length;
		}

		public bool archiveExists(int archiveId)
		{
			ArchiveReference[] archives = _table.ArchiveList;
			return archives.Length > archiveId && archives[archiveId] != null;
		}

		public bool fileExists(int archiveId, int fileId)
		{
			if (!archiveExists(archiveId))
			{
				return false;
			}

			FileReference[] files = _table.ArchiveList[archiveId].FileList;
			return files.Length > fileId && files[fileId] != null;
		}

		public int getArchiveId(string name)
		{
			int nameHash = NameHasher.getNameHash(name);
			ArchiveReference[] archives = _table.ArchiveList;
			int[] validArchiveIds = _table.ValidArchiveIds;

			for (int i = 0; i < validArchiveIds.Length; i++)
			{
				int archiveId = validArchiveIds[i];
				if (archives[archiveId].NameHash == nameHash)
				{
					return archiveId;
				}
			}

			return -1;
		}

		public int getFileId(int archiveId, string name)
		{
			if (!archiveExists(archiveId))
			{
				return -1;
			}

			int nameHash = NameHasher.getNameHash(name);
			FileReference[] files = _table.ArchiveList[archiveId].FileList;
			int[] validFileIds = _table.ArchiveList[archiveId].ValidFileIds;

			for (int index = 0; index < validFileIds.Length; index++)
			{
				int fileId = validFileIds[index];
				if (files[fileId].NameHash == nameHash)
					return fileId;
			}

			return -1;
		}

		public Archive getArchive(int id)
		{
			return _index.getArchive(id, null);
		}

		public Archive getArchive(int id, int[] keys)
		{
			return _index.getArchive(id, keys);
		}

		public byte[] getFile(int archiveId)
		{
			if (!archiveExists(archiveId))
			{
				return null;
			}

			return getFile(archiveId, _table.ArchiveList[archiveId].ValidFileIds[0]);
		}

		public byte[] getFile(int archiveId, int fileId)
		{
			return getFile(archiveId, fileId, null);
		}

		public byte[] getFile(int archiveId, int fileId, int[] keys)
		{
			if (!fileExists(archiveId, fileId))
			{
				return null;
			}

			if (_cachedFiles[archiveId] == null || _cachedFiles[archiveId][fileId] == null)
			{
				cacheArchiveFiles(archiveId, keys);
			}

			byte[] file = _cachedFiles[archiveId][fileId];
			_cachedFiles[archiveId][fileId] = null;
			return file;
		}

		private void cacheArchiveFiles(int archiveId, int[] keys)
		{
			Archive archive = getArchive(archiveId, keys);

			int lastFileId = getLastFileId(archiveId);
			_cachedFiles[archiveId] = new byte[lastFileId + 1][];

			if (archive == null)
			{
				return;
			}

			byte[] data = archive.Data;
			if (data == null)
			{
				return;
			}

			int filesCount = getValidFilesCount(archiveId);
			if (filesCount == 1)
			{
				_cachedFiles[archiveId][lastFileId] = data;
			}
			else
			{
				int readPosition = data.Length;
				int amtOfLoops = data[(--readPosition)] & 0xFF;
				readPosition -= amtOfLoops * (filesCount * 4);

				DataInputStream stream = new DataInputStream(data);
				stream.Position = readPosition;

				int[] filesSize = new int[filesCount];
				for (int i = 0; i < amtOfLoops; i++)
				{
					int offset = 0;

					for (int j = 0; j < filesCount; j++)
					{
						int size = offset + stream.readInt();
						offset = size;
						filesSize[j] += size;
					}
				}

				byte[][] filesData = new byte[filesCount][];
				for (int i = 0; i < filesCount; i++)
				{
					filesData[i] = new byte[filesSize[i]];
					filesSize[i] = 0;
				}

				stream.Position = readPosition;

				int sourceOffset = 0;
				for (int i = 0; i < amtOfLoops; i++)
				{
					int dataRead = 0;

					for (int j = 0; j < filesCount; j++)
					{
						dataRead += stream.readInt();

						Array.Copy(data, sourceOffset, filesData[j], filesSize[j], dataRead);

						sourceOffset += dataRead;
						filesSize[j] += dataRead;
					}
				}

				int[] validFileIds = _table.ArchiveList[archiveId].ValidFileIds;
				for (int i = 0; i < validFileIds.Length; i++)
				{
					_cachedFiles[archiveId][validFileIds[i]] = filesData[i];
				}
			}
		}
	}
}