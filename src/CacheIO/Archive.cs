using System;
using CacheIO.IO;
using CacheIO.Util.GZip;
using CacheIO.Util.BZip2;

namespace CacheIO
{
	public enum CompressionType
	{
		RAW,
		BZIP,
		GZIP
	}

	public class Archive
	{
		private int _id;
		private int _revision;
		private CompressionType _compression;

		private byte[] _data;
		private int[] _keys;

		public int Id
		{
			get { return _id; }
		}

		public byte[] Data
		{
			get { return _data; }
		}


		public Archive(int id, byte[] data, int[] keys)
		{
			_id = id;
			_keys = keys;

			decompress(data);
		}

		public Archive(int id, int revision, CompressionType compression, byte[] data)
		{
			_id = id;
			_revision = revision;
			_compression = compression;
			_data = data;
		}

		public byte[] compress()
		{
			return null;
		}

		private void decompress(byte[] data)
		{
			DataInputStream stream = new DataInputStream(data);
			//if(_keys != null && _keys.length != 0) stream.decodeXTEA(_keys);

			int compression = stream.readUnsignedByte();
			_compression = (CompressionType)(compression > 2 ? 2 : compression);

			int compressedLength = stream.readInt();
			if (compressedLength < 0 || compressedLength > 1000000)
			{
				throw new InvalidOperationException("INVALID ARCHIVE HEADER");
			}

			int length;

			Console.WriteLine("Decompress #" + _id + ": " + _compression);

			switch (_compression)
			{
				case CompressionType.RAW:
					_data = new byte[compressedLength];
					checkRevision(stream, compressedLength);

					stream.Read(_data, 0, compressedLength);
					break;

				case CompressionType.BZIP:
					length = stream.readInt();
					
					if (length <= 0)
					{
						_data = null;
					}
					else
					{
						_data = new byte[length];
						checkRevision(stream, compressedLength);
						BZip2Decompressor.Decompress(_data, data);
					}
					break;

				default: // GZIP
					length = stream.readInt();

					if (length <= 0 || length > 1000000000)
					{
						_data = null;
					}
					else
					{
						_data = new byte[length];
						checkRevision(stream, compressedLength);
						GZipDecompressor.Decompress(_data, stream);
					}
					break;
			}
		}

		private void checkRevision(DataInputStream stream, int compressedLength)
		{
			long offset = stream.Position;

			if (stream.Length - (compressedLength + stream.Position) >= 2)
			{
				stream.Position = stream.Length - 2;
				_revision = stream.readUnsignedShort();
				stream.Position = offset;
			}
			else
			{
				_revision = -1;
			}
		}
	}
}