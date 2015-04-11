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
			_offset = buffer.Length;
			_length = buffer.Length;
		}
	}
}