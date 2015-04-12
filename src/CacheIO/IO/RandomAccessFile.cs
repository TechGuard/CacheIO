using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheIO.IO
{
	public class RandomAccessFile
	{
		private string _file;
		private int _offset = 0;


		public RandomAccessFile(string filename) {
			_file = filename;
		}

		public void Seek(int offset) {
			_offset = offset;
		}

		public long getLength() {
			FileInfo info = new FileInfo(_file);
			return info.Length;
		}

		public void Read(byte[] buffer, int offset, int length) {
			FileStream fin = new FileStream(_file, FileMode.Open, FileAccess.Read);

			fin.Position = _offset;
			fin.Read(buffer, offset, length);
			_offset += length;

			fin.Close();
		}

		public void Write(byte[] buffer, int offset, int length) {
			FileStream fout = new FileStream(_file, FileMode.Open, FileAccess.Write);

			fout.Position = _offset;
			fout.Write(buffer, offset, length);
			_offset += length;

			fout.Close();
		}
	}
}
