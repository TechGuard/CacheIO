using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheIO.Util.Whirlpool
{
	public class Whirlpool
	{
		protected const int DIGESTBITS = 512;
		protected const int DIGESTBYTES = 64;
		protected const int R = 10;
		private const string sbox = "\u1823\uc6e8\u87b8\u014f\u36a6\ud2f5\u796f\u9152\u60bc\u9b8e\ua30c\u7b35\u1de0\ud7c2\u2e4b\ufe57\u1577\u37e5\u9ff0\u4ada\u58c9\u290a\ub1a0\u6b85\ubd5d\u10f4\ucb3e\u0567\ue427\u418b\ua77d\u95d8\ufbee\u7c66\udd17\u479e\uca2d\ubf07\uad5a\u8333\u6302\uaa71\uc819\u49d9\uf2e3\u5b88\u9a26\u32b0\ue90f\ud580\ubecd\u3448\uff7a\u905f\u2068\u1aae\ub454\u9322\u64f1\u7312\u4008\uc3ec\udba1\u8d3d\u9700\ucf2b\u7682\ud61b\ub5af\u6a50\u45f3\u30ef\u3f55\ua2ea\u65ba\u2fc0\ude1c\ufd4d\u9275\u068a\ub2e6\u0e1f\u62d4\ua896\uf9c5\u2559\u8472\u394c\u5e78\u388c\ud1a5\ue261\ub321\u9c1e\u43c7\ufc04\u5199\u6d0d\ufadf\u7e24\u3bab\uce11\u8f4e\ub7eb\u3c81\u94f7\ub913\u2cd3\ue76e\uc403\u5644\u7fa9\u2abb\uc153\udc0b\u9d6c\u3174\uf646\uac89\u14e1\u163a\u6909\u70b6\ud0ed\ucc42\u98a4\u285c\uf886";
		private static long[][] C = new long[8][];
		private static long[] rc = new long[11];
		protected byte[] bitLength = new byte[32];
		protected byte[] buffer = new byte[64];
		protected int bufferBits = 0;
		protected int bufferPos = 0;
		protected long[] hash = new long[8];
		protected long[] K = new long[8];
		protected long[] L = new long[8];
		protected long[] block = new long[8];
		protected long[] state = new long[8];

		static Whirlpool()
		{
			for (int i = 0; i < C.Length; i++) C[i] = new long[256];

			for (int x = 0; x < 256; x++)
			{
				char c =
					"\u1823\uc6e8\u87b8\u014f\u36a6\ud2f5\u796f\u9152\u60bc\u9b8e\ua30c\u7b35\u1de0\ud7c2\u2e4b\ufe57\u1577\u37e5\u9ff0\u4ada\u58c9\u290a\ub1a0\u6b85\ubd5d\u10f4\ucb3e\u0567\ue427\u418b\ua77d\u95d8\ufbee\u7c66\udd17\u479e\uca2d\ubf07\uad5a\u8333\u6302\uaa71\uc819\u49d9\uf2e3\u5b88\u9a26\u32b0\ue90f\ud580\ubecd\u3448\uff7a\u905f\u2068\u1aae\ub454\u9322\u64f1\u7312\u4008\uc3ec\udba1\u8d3d\u9700\ucf2b\u7682\ud61b\ub5af\u6a50\u45f3\u30ef\u3f55\ua2ea\u65ba\u2fc0\ude1c\ufd4d\u9275\u068a\ub2e6\u0e1f\u62d4\ua896\uf9c5\u2559\u8472\u394c\u5e78\u388c\ud1a5\ue261\ub321\u9c1e\u43c7\ufc04\u5199\u6d0d\ufadf\u7e24\u3bab\uce11\u8f4e\ub7eb\u3c81\u94f7\ub913\u2cd3\ue76e\uc403\u5644\u7fa9\u2abb\uc153\udc0b\u9d6c\u3174\uf646\uac89\u14e1\u163a\u6909\u70b6\ud0ed\ucc42\u98a4\u285c\uf886"
						.ToCharArray()[x / 2];
				long v1 = (long)((x & 0x1) == 0 ? (int)((uint)c >> 8) : c & 0xff);
				long v2 = v1 << 1;
				if (v2 >= 256L)
					v2 ^= 0x11dL;
				long v4 = v2 << 1;
				if (v4 >= 256L)
					v4 ^= 0x11dL;
				long v5 = v4 ^ v1;
				long v8 = v4 << 1;
				if (v8 >= 256L)
					v8 ^= 0x11dL;
				long v9 = v8 ^ v1;
				C[0][x] = (v1 << 56 | v1 << 48 | v4 << 40 | v1 << 32 | v8 << 24
					   | v5 << 16 | v2 << 8 | v9);
				for (int t = 1; t < 8; t++)
					C[t][x] = (int)((uint)(C[t - 1][x]) >> 8) | C[t - 1][x] << 56;
			}
			rc[0] = 0L;
			for (int r = 1; r <= 10; r++)
			{
				int i = 8 * (r - 1);
				rc[r] = (C[0][i] & ~0xffffffffffffffL
					 ^ C[1][i + 1] & 0xff000000000000L
					 ^ C[2][i + 2] & 0xff0000000000L
					 ^ C[3][i + 3] & 0xff00000000L ^ C[4][i + 4] & 0xff000000L
					 ^ C[5][i + 5] & 0xff0000L ^ C[6][i + 6] & 0xff00L
					 ^ C[7][i + 7] & 0xffL);
			}
		}

		public static byte[] GetHash(byte[] data)
		{
			return GetHash(data, 0, data.Length);	
		}

		public static byte[] GetHash(byte[] data, int off, int len)
		{
			byte[] source;
			if (off <= 0)
				source = data;
			else
			{
				source = new byte[len];
				for (int i = 0; i < len; i++)
					source[i] = data[off + i];
			}
			Whirlpool whirlpool = new Whirlpool();
			whirlpool.NESSIEinit();
			whirlpool.NESSIEadd(source, (long)(len * 8));
			byte[] digest = new byte[64];
			whirlpool.NESSIEfinalize(digest);
			return digest;
		}

		protected void processBuffer()
		{
			int i = 0;
			int j = 0;
			while (i < 8)
			{
				block[i] = ((long)buffer[j] << 56
					^ ((long)buffer[j + 1] & 0xffL) << 48
					^ ((long)buffer[j + 2] & 0xffL) << 40
					^ ((long)buffer[j + 3] & 0xffL) << 32
					^ ((long)buffer[j + 4] & 0xffL) << 24
					^ ((long)buffer[j + 5] & 0xffL) << 16
					^ ((long)buffer[j + 6] & 0xffL) << 8
					^ (long)buffer[j + 7] & 0xffL);
				i++;
				j += 8;
			}
			for (i = 0; i < 8; i++)
				state[i] = block[i] ^ (K[i] = hash[i]);
			for (int r = 1; r <= 10; r++)
			{
				for (int i_0_ = 0; i_0_ < 8; i_0_++)
				{
					L[i_0_] = 0L;
					int t = 0;
					int s = 56;
					while (t < 8)
					{
						L[i_0_] ^= C[t][(int)((int)((uint)K[i_0_ - t & 0x7] >> s)) & 0xff];
						t++;
						s -= 8;
					}
				}
				for (int i_1_ = 0; i_1_ < 8; i_1_++)
					K[i_1_] = L[i_1_];
				K[0] ^= rc[r];
				for (int i_2_ = 0; i_2_ < 8; i_2_++)
				{
					L[i_2_] = K[i_2_];
					int t = 0;
					int s = 56;
					while (t < 8)
					{
						L[i_2_]
						^= C[t][(int)((int)((uint)state[i_2_ - t & 0x7] >> s)) & 0xff];
						t++;
						s -= 8;
					}
				}
				for (int i_3_ = 0; i_3_ < 8; i_3_++)
					state[i_3_] = L[i_3_];
			}
			for (i = 0; i < 8; i++)
				hash[i] ^= state[i] ^ block[i];
		}

		protected void NESSIEinit()
		{
			for (int i = 0; i < bitLength.Length; i++) bitLength[i] = 0;
			bufferBits = bufferPos = 0;
			buffer[0] = 0;
			for (int i = 0; i < hash.Length; i++) hash[i] = 0L;
		}

		protected void NESSIEadd(byte[] source, long sourceBits)
		{
			int sourcePos = 0;
			int sourceGap = 8 - ((int)sourceBits & 0x7) & 0x7;
			int bufferRem = bufferBits & 0x7;
			long value = sourceBits;
			int i = 31;
			int carry = 0;
			for (/**/; i >= 0; i--)
			{
				carry += (bitLength[i] & 0xff) + ((int)value & 0xff);
				bitLength[i] = (byte)carry;
				carry = (int)((uint)carry >> 8);
				value = (int)((uint)value >> 8);
			}
			int b;
			while (sourceBits > 8L)
			{
				b = (source[sourcePos] << sourceGap & 0xff | (int)((uint)(source[sourcePos + 1] & 0xff) >> 8) - sourceGap);
				if (b < 0 || b >= 256)
					throw new ArgumentOutOfRangeException("LOGIC ERROR");
				buffer[bufferPos++] |= (byte)((sbyte)b >> bufferRem);
				bufferBits += 8 - bufferRem;
				if (bufferBits == 512)
				{
					processBuffer();
					bufferBits = bufferPos = 0;
				}
				buffer[bufferPos] = (byte)(b << 8 - bufferRem & 0xff);
				bufferBits += bufferRem;
				sourceBits -= 8L;
				sourcePos++;
			}
			if (sourceBits > 0L)
			{
				b = source[sourcePos] << sourceGap & 0xff;
				buffer[bufferPos] |= (byte)((sbyte)b >> bufferRem);
			}
			else
				b = 0;
			if ((long)bufferRem + sourceBits < 8L)
				bufferBits += (int)sourceBits;
			else
			{
				bufferPos++;
				bufferBits += 8 - bufferRem;
				sourceBits -= (long)(8 - bufferRem);
				if (bufferBits == 512)
				{
					processBuffer();
					bufferBits = bufferPos = 0;
				}
				buffer[bufferPos] = (byte)(b << 8 - bufferRem & 0xff);
				bufferBits += (int)sourceBits;
			}
		}

		protected void NESSIEfinalize(byte[] digest)
		{
			buffer[bufferPos] |= (byte)((uint)128 >> (bufferBits & 0x7));
			bufferPos++;
			if (bufferPos > 32)
			{
				while (bufferPos < 64)
					buffer[bufferPos++] = (byte)0;
				processBuffer();
				bufferPos = 0;
			}
			while (bufferPos < 32)
				buffer[bufferPos++] = (byte)0;
			Array.Copy(bitLength, 0, buffer, 32, 32);
			processBuffer();
			int i = 0;
			int j = 0;
			while (i < 8)
			{
				long h = hash[i];
				digest[j] = (byte)((sbyte)h >> 56);
				digest[j + 1] = (byte)((sbyte)h >> 48);
				digest[j + 2] = (byte)((sbyte)h >> 40);
				digest[j + 3] = (byte)((sbyte)h >> 32);
				digest[j + 4] = (byte)((sbyte)h >> 24);
				digest[j + 5] = (byte)((sbyte)h >> 16);
				digest[j + 6] = (byte)((sbyte)h >> 8);
				digest[j + 7] = (byte)h;
				i++;
				j += 8;
			}
		}

		protected void NESSIEadd(String source)
		{
			if (source.Length > 0)
			{
				byte[] data = new byte[source.Length];
				for (int i = 0; i < source.Length; i++)
					data[i] = (byte)source.ToCharArray()[i];
				NESSIEadd(data, (long)(8 * data.Length));
			}
		}
	}
}