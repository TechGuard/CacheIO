using System;

namespace CacheIO.IO
{
	public class DataInputStream : DataStream
	{
		public DataInputStream(int capacity)
		{
			_buffer = new byte[capacity];
		}

		public DataInputStream(byte[] buffer)
		{
			_buffer = buffer;
			_length = buffer.Length;
		}

		public override bool CanRead
		{
			get { return true; }
		}


		public int readUnsignedByte()
		{
			return readByte() & 0xFF;
		}

		public int readByte128()
		{
			return readByte() - 128;
		}

		public int readByteC()
		{
			return -readByte();
		}

		public int read128Byte()
		{
			return 128 - readByte();
		}

		public int readUnsignedByte128()
		{
			return readUnsignedByte() - 128 & 0xFF;
		}

		public int readUnsignedByteC()
		{
			return -readUnsignedByte() & 0xFF;
		}

		public int readUnsigned128Byte()
		{
			return 128 - readUnsignedByte() & 0xFF;
		}

		public int readShortLE()
		{
			int result = readUnsignedByte() + (readUnsignedByte() << 8);

			if (result > 32767) // 2^15 - 1
			{
				return result - 65536; // 2^16
			}

			return result;
		}

		public int readShort128()
		{
			int result = (readUnsignedByte() << 8) + (readByte() - 128 & 0xFF);

			if (result > 32767) // 2^15 - 1
			{
				return result - 65536; // 2^16
			}

			return result;
		}

		public int readShortLE128()
		{
			int result = (readByte() - 128 & 0xFF) + (readUnsignedByte() << 8);

			if (result > 32767) // 2^15 - 1
			{
				return result - 65536; // 2^16
			}

			return result;
		}

		public int read128ShortLE()
		{
			int result = (128 - readByte() & 0xFF) + (readUnsignedByte() << 8);

			if (result > 32767) // 2^15 - 1
			{
				return result - 65536; // 2^16
			}

			return result;
		}

		public int readShort()
		{
			int result = (readUnsignedByte() << 8) + readUnsignedByte();

			if (result > 32767) // 2^15 - 1
			{
				return result - 65536; // 2^16
			}

			return result;
		}

		public int readUnsignedShortLE()
		{
			return readUnsignedByte() + (readUnsignedByte() << 8);
		}

		public int readUnsignedShort()
		{
			return (readUnsignedByte() << 8) + readUnsignedByte();
		}

		public int readUnsignedShort128()
		{
			return (readUnsignedByte() << 8) + (readByte() - 128 & 0xFF);
		}

		public int readUnsignedShortLE128()
		{
			return (readByte() - 128 & 0xFF) + (readUnsignedByte() << 8);
		}

		public int readInt()
		{
			return (readUnsignedByte() << 24) + (readUnsignedByte() << 16) + (readUnsignedByte() << 8) + readUnsignedByte();
		}

		public int readIntV1()
		{
			return (readUnsignedByte() << 8) + readUnsignedByte() + (readUnsignedByte() << 24) + (readUnsignedByte() << 16);
		}

		public int readIntV2()
		{
			return (readUnsignedByte() << 16) + (readUnsignedByte() << 24) + readUnsignedByte() + (readUnsignedByte() << 8);
		}

		public int readIntLE()
		{
			return readUnsignedByte() + (readUnsignedByte() << 8) + (readUnsignedByte() << 16) + (readUnsignedByte() << 24);
		}

		public long readLong()
		{
			long p1 = readInt() & 0xFFFFFFFF;
			long p2 = readInt() & 0xFFFFFFFF;

			return (p1 << 32) + p2;
		}

		public string readString()
		{
			string result = "";
			char char0;

			while ((char0 = (char)readByte()) != 0)
			{
				result += char0;
			}

			return result;
		}

		public string readJagString()
		{
			readByte(); // unused

			return readString();
		}

		public int readBigSmart()
		{
			if (_buffer[_position] >= 0)
			{
				return readUnsignedShort();
			}

			return readInt() & 0x7FFFFFFF;
		}

		public int readSmart()
		{
			int result = 0;
			int smart = readUnsignedSmart();

			while ((smart ^ 0xFFFFFFFF) == -32768) // 2^15
			{
				smart = readUnsignedSmart();
				result += 32767; // 2^15 - 1
			}

			return result + smart;
		}

		public int readUnsignedSmart()
		{
			int smart = readUnsignedByte();

			if (smart >= 128)
			{
				return ((smart << 8) + readUnsignedByte()) - 32768; // 2^15
			}

			return smart;
		}
	}
}