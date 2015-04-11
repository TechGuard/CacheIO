using System;

namespace CacheIO.IO
{
	public class DataOutputStream : DataStream
	{
		public DataOutputStream(int capacity)
		{
			_buffer = new byte[capacity];
		}

		public DataOutputStream(byte[] buffer)
		{
			_buffer = buffer;
			_length = buffer.Length;
			_position = buffer.Length;
		}

		public override bool CanWrite
		{
			get { return true; }
		}
	}
}