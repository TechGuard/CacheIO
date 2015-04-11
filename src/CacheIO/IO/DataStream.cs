using System;
using System.IO;

namespace CacheIO.IO
{
	public abstract class DataStream : Stream
	{
		/*
		 * Mersenne numbers
		 * for (int i = 0; i < 32; i++) BIT_MASK[i] = (1 << i) - 1;
		 */
		protected static readonly int[] BIT_MASK = { 0, 1, 3, 7, 15, 31, 63, 127, 255, 511, 1023, 2047, 4095, 8191, 16383, 32767, 65535, 131071, 262143, 524287, 1048575, 2097151, 4194303, 8388607, 16777215, 33554431, 67108863, 134217727, 268435455, 536870911, 1073741823, 2147483647, -1 };

		protected long _position = 0;
		//protected int bitPosition;

		protected long _length = 0;
		protected byte[] _buffer = null;

		public override long Length
		{
			get { return _length; }
		}

		public long Remaining
		{
			get { return _position < _length ? _length - _position : 0L; }
		}

		public void Skip(int length)
		{
			_position += length;
		}

		public byte[] Buffer
		{
			get { return _buffer; }
		}

		public override bool CanRead
		{
			get { return false; }
		}

		public override bool CanSeek
		{
			get { return true; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override void Flush() { }

		public override long Position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		public int readByte()
		{
			return Remaining > 0 ? _buffer[_position++] : 0;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			int read = 0;

			for (int i = offset; i < count + offset; i++)
			{
				buffer[i] = (byte) readByte();
				read++;
			}

			return read;
		}

		public int Read(byte[] buffer)
		{
			return Read(buffer, 0, buffer.Length);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			for (int i = offset; i < count + offset; i++)
			{
				_buffer[_position++] = buffer[i];
			}
		}

		public void Write(byte[] buffer)
		{
			Write(buffer, 0, buffer.Length);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch (origin)
			{
				case SeekOrigin.Begin:
					_position = offset;
					break;

				case SeekOrigin.Current:
					_position += offset;
					break;

				case SeekOrigin.End:
					_position = _length - offset;
					break;
			}

			return _position;
		}

		public long Seek(long offset)
		{
			return Seek(offset, SeekOrigin.Current);
		}

		public override void SetLength(long value)
		{
			_length = value;
		}
	}
}