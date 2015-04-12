using CacheIO.IO;

namespace CacheIO
{
	public class IndexFile
	{
		private int _id;
		private RandomAccessFile _data;
		private RandomAccessFile _index;
		private bool _newProtocol;
		private byte[] _readCacheBuffer;

		public int Id
		{
			get { return _id; }
		}

		public int ArchiveCount
		{
			get { return (int)(_index.getLength() / 6L); }
		}


		public IndexFile(int id, RandomAccessFile data, RandomAccessFile index, byte[] readCacheBuffer, bool newProtocol)
		{
			_id = id;
			_data = data;
			_index = index;
			_newProtocol = newProtocol;
			_readCacheBuffer = readCacheBuffer;
		}

		public Archive getArchive(int id)
		{
			return getArchive(id, null);
		}

		public Archive getArchive(int id, int[] keys)
		{
			byte[] data = getArchiveData(id);
			if (data == null)
			{
				return null;
			}

			return new Archive(id, data, keys);
		}

		public byte[] getArchiveData(int id)
		{
			if (_index.getLength() < 6 * id + 6)
			{
				return null;
			}
			
			_index.Seek(6 * id);
			_index.Read(_readCacheBuffer, 0, 6);

			int archiveLength = (_readCacheBuffer[2] & 0xFF) + (((0xFF & _readCacheBuffer[0]) << 16) + (_readCacheBuffer[1] << 8 & 0xFF00));
			if (archiveLength < 0 || archiveLength > 1000000)
			{
				return null;
			}

			int sector = ((_readCacheBuffer[3] & 0xFF) << 16) - (-(0xFF00 & _readCacheBuffer[4] << 8) - (_readCacheBuffer[5] & 0xFF));
			if (sector <= 0 || _data.getLength() / 520L < sector)
			{
				return null;
			}

			byte[] data = new byte[archiveLength];
			int readBytesCount = 0;
			int part = 0;

			while (archiveLength > readBytesCount)
			{
				if (sector == 0)
				{
					return null;
				}

				int dataBlockSize = archiveLength - readBytesCount;

				byte headerSize;
				int currentIndex;
				int currentArchive;
				int currentPart;
				int nextSector;

				_data.Seek(520 * sector);

				if (65535 < id && _newProtocol) // 2^16 - 1
				{
					headerSize = 10;

					if (dataBlockSize > 510)
					{
						dataBlockSize = 510;
					}

					_data.Read(_readCacheBuffer, 0, headerSize + dataBlockSize);

					currentIndex = _readCacheBuffer[9] & 0xFF;
					currentArchive = ((_readCacheBuffer[1] & 0xFF) << 16) + ((_readCacheBuffer[0] & 0xFF) << 24) + ((0xFF00 & _readCacheBuffer[2] << 8) - -(_readCacheBuffer[3] & 0xFF));
					currentPart = ((_readCacheBuffer[4] & 0xFF) << 8) + (0xFF & _readCacheBuffer[5]);
					nextSector = (_readCacheBuffer[8] & 0xFF) + (0xFF00 & _readCacheBuffer[7] << 8) + ((0xFF & _readCacheBuffer[6]) << 16);
				}
				else
				{
					headerSize = 8;

					if (dataBlockSize > 512)
					{
						dataBlockSize = 512;
					}

					_data.Read(_readCacheBuffer, 0, headerSize + dataBlockSize);

					currentIndex = _readCacheBuffer[7] & 0xFF;
					currentArchive = (0xFF & _readCacheBuffer[1]) + (0xFF00 & _readCacheBuffer[0] << 8);
					currentPart = ((_readCacheBuffer[2] & 0xFF) << 8) + (0xFF & _readCacheBuffer[3]);
					nextSector = (_readCacheBuffer[6] & 0xFF) + (0xFF00 & _readCacheBuffer[5] << 8) + ((0xFF & _readCacheBuffer[4]) << 16);
				}

				if ((_newProtocol && id != currentArchive) || currentPart != part || _id != currentIndex)
				{
					return null;
				}

				if (nextSector < 0 || _data.getLength() / 520L < nextSector)
				{
					return null;
				}

				for (int i = headerSize; dataBlockSize + headerSize > i; i++)
				{
					data[readBytesCount++] = _readCacheBuffer[i];
				}

				part++;
				sector = nextSector;
			}
			
			return data;
		}
	}
}