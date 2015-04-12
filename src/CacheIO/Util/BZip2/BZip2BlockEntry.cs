namespace CacheIO.Util.BZip2
{
	public class BZip2BlockEntry
	{
		public bool[] aBooleanArray2205;
		public bool[] aBooleanArray2213;
		public byte aByte2201;
		public byte[] aByteArray2204;
		public byte[] aByteArray2211;
		public byte[] aByteArray2212;
		public byte[] aByteArray2214;
		public byte[] aByteArray2219;
		public byte[] aByteArray2224;
		public byte[][] aByteArrayArray2229;
		public int anInt2202;
		public int anInt2203;
		public int anInt2206;
		public int anInt2207;
		public int anInt2208;
		public int anInt2209;
		public int anInt2215;
		public int anInt2216;
		public int anInt2217;
		public int anInt2221;
		public int anInt2222;
		public int anInt2223;
		public int anInt2225;
		public int anInt2227;
		public int anInt2232;
		public int[] anIntArray2200;
		public int[] anIntArray2220;
		public int[] anIntArray2226;
		public int[] anIntArray2228;
		public int[][] anIntArrayArray2210;
		public int[][] anIntArrayArray2218;
		public int[][] anIntArrayArray2230;

		public BZip2BlockEntry()
		{
			anIntArray2200 = new int[6];
			anInt2203 = 0;
			aByteArray2204 = new byte[4096];
			aByteArray2211 = new byte[256];
			aByteArray2214 = new byte[18002];
			aByteArray2219 = new byte[18002];
			anIntArray2220 = new int[257];
			anIntArrayArray2218 = new int[6][];
			for (int i = 0; i < anIntArrayArray2218.Length; i++) anIntArrayArray2218[i] = new int[258];
			aBooleanArray2205 = new bool[16];
			aBooleanArray2213 = new bool[256];
			anInt2209 = 0;
			anIntArray2226 = new int[16];
			anIntArrayArray2210 = new int[6][];
			for (int i = 0; i < anIntArrayArray2210.Length; i++) anIntArrayArray2210[i] = new int[258];
			aByteArrayArray2229 = new byte[6][];
			for (int i = 0; i < aByteArrayArray2229.Length; i++) aByteArrayArray2229[i] = new byte[258];
			anIntArrayArray2230 = new int[6][];
			for (int i = 0; i < anIntArrayArray2230.Length; i++) anIntArrayArray2230[i] = new int[258];
			anIntArray2228 = new int[256];
		}
	}
}
