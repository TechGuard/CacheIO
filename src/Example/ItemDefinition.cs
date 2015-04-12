using System;
using CacheIO;
using CacheIO.IO;

namespace Example
{
	public class ItemDefinition
	{
		private const int ITEM_DEFINITIONS_INDEX = 19;

		public int Id;
		public int modelId;
		public String name;

		// model size information
		public int modelZoom;
		public int modelRotation1;
		public int modelRotation2;
		public int modelOffset1;
		public int modelOffset2;

		// extra information
		public int stackable;
		public int value;
		public bool membersOnly;

		// wearing model information
		public int maleEquip1;
		public int femaleEquip1;
		public int maleEquip2;
		public int femaleEquip2;

		// options
		public String[] groundOptions;
		public String[] inventoryOptions;

		// model information
		public int[] originalModelColors;
		public int[] modifiedModelColors;
		public short[] originalTextureColors;
		public short[] modifiedTextureColors;
		public byte[] unknownArray1;
		public int[] unknownArray2;
		public bool unnoted;

		public int maleEquipModelId3;
		public int femaleEquipModelId3;
		public int unknownInt1;
		public int unknownInt2;
		public int unknownInt3;
		public int unknownInt4;
		public int unknownInt5;
		public int unknownInt6;
		public int certId;
		public int certTemplateId;
		public int[] stackIds;
		public int[] stackAmounts;
		public int unknownInt7;
		public int unknownInt8;
		public int unknownInt9;
		public int unknownInt10;
		public int unknownInt11;
		public int teamId;
		public int lendId;
		public int lendTemplateId;
		public int unknownInt12;
		public int unknownInt13;
		public int unknownInt14;
		public int unknownInt15;
		public int unknownInt16;
		public int unknownInt17;
		public int unknownInt18;
		public int unknownInt19;
		public int unknownInt20;
		public int unknownInt21;
		public int unknownInt22;
		public int unknownInt23;
		public bool noted;
		public bool lended;

		public ItemDefinition(int id)
		{
			Id = id;

			name = "null";
			maleEquip1 = -1;
			maleEquip2 = -1;
			femaleEquip1 = -1;
			femaleEquip2 = -1;
			modelZoom = 2000;
			lendId = -1;
			lendTemplateId = -1;
			certId = -1;
			certTemplateId = -1;
			unknownInt9 = 128;
			value = 1;
			maleEquipModelId3 = -1;
			femaleEquipModelId3 = -1;
			unknownValue1 = -1;
			unknownValue2 = -1;

			groundOptions = new String[] { null, null, "take", null, null };
			inventoryOptions = new String[] { null, null, null, null, "drop" };
		}

		public void Load(Cache cache)
		{
			int archiveId = (int)((uint)Id >> 8);
			int itemId = 0xff & Id;

			byte[] data = cache.IndexList[ITEM_DEFINITIONS_INDEX].getFile(archiveId, itemId);
			if (data == null)
			{
				Console.Error.WriteLine("No data for item " + Id);
				return;
			}

			DataInputStream stream = new DataInputStream(data);

			while (true)
			{
				int opcode = stream.readUnsignedByte();
				if (opcode == 0)
				{
					break;
				}

				readOpcode(stream, opcode);
			}
		}

		private void readOpcode(DataInputStream stream, int opcode)
		{
			if (opcode == 1)
				modelId = stream.readBigSmart();
			else if (opcode == 2)
				name = stream.readString();
			else if (opcode == 4)
				modelZoom = stream.readUnsignedShort();
			else if (opcode == 5)
				modelRotation1 = stream.readUnsignedShort();
			else if (opcode == 6)
				modelRotation2 = stream.readUnsignedShort();
			else if (opcode == 7)
			{
				modelOffset1 = stream.readUnsignedShort();
				if (modelOffset1 > 32767)
					modelOffset1 -= 65536;
				modelOffset1 <<= 0;
			}
			else if (opcode == 8)
			{
				modelOffset2 = stream.readUnsignedShort();
				if (modelOffset2 > 32767)
					modelOffset2 -= 65536;
				modelOffset2 <<= 0;
			}
			else if (opcode == 11)
				stackable = 1;
			else if (opcode == 12)
				value = stream.readInt();
			else if (opcode == 117)
				opcode117 = stream.readUnsignedByte();
			else if (opcode == 211)
				opcode211 = stream.readUnsignedByte();
			else if (opcode == 255)
				opcode255 = stream.readUnsignedByte();
			else if (opcode == 75)
				opcode75 = stream.readUnsignedByte();
			else if (opcode == 87)
				opcode87 = stream.readUnsignedByte();
			else if (opcode == 68)
				opcode68 = stream.readUnsignedByte();
			else if (opcode == 118)
				opcode118 = stream.readUnsignedByte();
			else if (opcode == 83)
				opcode83 = stream.readUnsignedByte();
			else if (opcode == 254)
				opcode254 = stream.readUnsignedByte();
			else if (opcode == 156)
				opcode156 = stream.readUnsignedByte();
			else if (opcode == 232)
				opcode232 = stream.readUnsignedByte();
			else if (opcode == 199)
				opcode199 = stream.readUnsignedByte();
			else if (opcode == 253)
				opcode253 = stream.readUnsignedByte();
			else if (opcode == 223)
				opcode223 = stream.readUnsignedByte();
			else if (opcode == 198)
				opcode198 = stream.readUnsignedByte();
			else if (opcode == 186)
				opcode186 = stream.readUnsignedByte();
			else if (opcode == 29)
				opcode29 = stream.readUnsignedByte();
			else if (opcode == 238)
				opcode238 = stream.readUnsignedByte();
			else if (opcode == 153)
				opcode153 = stream.readUnsignedByte();
			else if (opcode == 155)
				opcode155 = stream.readUnsignedByte();
			else if (opcode == 99)
				opcode99 = stream.readUnsignedByte();
			else if (opcode == 251)
				opcode251 = stream.readUnsignedByte();
			else if (opcode == 22)
				opcode22 = stream.readUnsignedByte();
			else if (opcode == 192)
				opcode192 = stream.readUnsignedByte();
			else if (opcode == 245)
				opcode245 = stream.readUnsignedByte();
			else if (opcode == 45)
				opcode45 = stream.readUnsignedByte();
			else if (opcode == 56)
				opcode56 = stream.readUnsignedByte();
			else if (opcode == 248)
				opcode248 = stream.readUnsignedByte();
			else if (opcode == 237)
				opcode237 = stream.readUnsignedByte();
			else if (opcode == 243)
				opcode243 = stream.readUnsignedByte();
			else if (opcode == 185)
				opcode185 = stream.readUnsignedByte();
			else if (opcode == 221)
				opcode221 = stream.readUnsignedByte();
			else if (opcode == 240)
				opcode240 = stream.readUnsignedByte();
			else if (opcode == 154)
				opcode154 = stream.readUnsignedByte();
			else if (opcode == 158)
				opcode158 = stream.readUnsignedByte();
			else if (opcode == 137)
				opcode137 = stream.readUnsignedByte();
			else if (opcode == 143)
				opcode143 = stream.readUnsignedByte();
			else if (opcode == 61)
				opcode61 = stream.readUnsignedByte();
			else if (opcode == 80)
				opcode80 = stream.readUnsignedByte();
			else if (opcode == 196)
				opcode196 = stream.readUnsignedByte();
			else if (opcode == 85)
				opcode85 = stream.readUnsignedByte();
			else if (opcode == 239)
				opcode239 = stream.readUnsignedByte();
			else if (opcode == 177)
				opcode177 = stream.readUnsignedByte();
			else if (opcode == 163)
				opcode163 = stream.readUnsignedByte();
			else if (opcode == 150)
				opcode150 = stream.readUnsignedByte();
			else if (opcode == 152)
				opcode152 = stream.readUnsignedByte();
			else if (opcode == 135)
				opcode135 = stream.readUnsignedByte();
			else if (opcode == 120)
				opcode120 = stream.readUnsignedByte();
			else if (opcode == 204)
				opcode204 = stream.readUnsignedByte();
			else if (opcode == 81)
				opcode81 = stream.readUnsignedByte();
			else if (opcode == 208)
				opcode208 = stream.readUnsignedByte();
			else if (opcode == 242)
				opcode242 = stream.readUnsignedByte();
			else if (opcode == 15)
				opcode15 = stream.readUnsignedByte();
			else if (opcode == 233)
				opcode233 = stream.readUnsignedByte();
			else if (opcode == 213)
				opcode213 = stream.readUnsignedByte();
			else if (opcode == 207)
				opcode207 = stream.readUnsignedByte();
			else if (opcode == 216)
				opcode216 = stream.readUnsignedByte();
			else if (opcode == 206)
				opcode206 = stream.readUnsignedByte();
			else if (opcode == 50)
				opcode50 = stream.readUnsignedByte();
			else if (opcode == 193)
				opcode193 = stream.readUnsignedByte();
			else if (opcode == 71)
				opcode71 = stream.readUnsignedByte();
			else if (opcode == 10)
				opcode10 = stream.readUnsignedByte();
			else if (opcode == 55)
				opcode55 = stream.readUnsignedByte();
			else if (opcode == 144)
				opcode144 = stream.readUnsignedByte();
			else if (opcode == 235)
				opcode235 = stream.readUnsignedByte();
			else if (opcode == 188)
				opcode188 = stream.readUnsignedByte();
			else if (opcode == 241)
				opcode241 = stream.readUnsignedByte();
			else if (opcode == 236)
				opcode236 = stream.readUnsignedByte();
			else if (opcode == 182)
				opcode182 = stream.readUnsignedByte();
			else if (opcode == 169)
				opcode169 = stream.readUnsignedByte();
			else if (opcode == 190)
				opcode190 = stream.readUnsignedByte();
			else if (opcode == 178)
				opcode178 = stream.readUnsignedByte();
			else if (opcode == 88)
				opcode88 = stream.readUnsignedByte();
			else if (opcode == 200)
				opcode200 = stream.readUnsignedByte();
			else if (opcode == 184)
				opcode184 = stream.readUnsignedByte();
			else if (opcode == 176)
				opcode176 = stream.readUnsignedByte();
			else if (opcode == 197)
				opcode197 = stream.readUnsignedByte();
			else if (opcode == 247)
				opcode247 = stream.readUnsignedByte();
			else if (opcode == 218)
				opcode218 = stream.readUnsignedByte();
			else if (opcode == 250)
				opcode250 = stream.readUnsignedByte();
			else if (opcode == 174)
				opcode174 = stream.readUnsignedByte();
			else if (opcode == 210)
				opcode210 = stream.readUnsignedByte();
			else if (opcode == 164)
				opcode164 = stream.readUnsignedByte();
			else if (opcode == 142)
				opcode142 = stream.readUnsignedByte();
			else if (opcode == 148)
				opcode148 = stream.readUnsignedByte();
			else if (opcode == 133)
				opcode133 = stream.readUnsignedByte();
			else if (opcode == 222)
				opcode222 = stream.readUnsignedByte();
			else if (opcode == 138)
				opcode138 = stream.readUnsignedByte();
			else if (opcode == 194)
				opcode194 = stream.readUnsignedByte();
			else if (opcode == 119)
				opcode119 = stream.readUnsignedByte();
			else if (opcode == 202)
				opcode202 = stream.readUnsignedByte();
			else if (opcode == 149)
				opcode149 = stream.readUnsignedByte();
			else if (opcode == 64)
				opcode64 = stream.readUnsignedByte();
			else if (opcode == 147)
				opcode147 = stream.readUnsignedByte();
			else if (opcode == 214)
				opcode214 = stream.readUnsignedByte();
			else if (opcode == 74)
				opcode74 = stream.readUnsignedByte();
			else if (opcode == 86)
				opcode86 = stream.readUnsignedByte();
			else if (opcode == 167)
				opcode167 = stream.readUnsignedByte();
			else if (opcode == 161)
				opcode161 = stream.readUnsignedByte();
			else if (opcode == 58)
				opcode58 = stream.readUnsignedByte();
			else if (opcode == 59)
				opcode59 = stream.readUnsignedByte();
			else if (opcode == 187)
				opcode187 = stream.readUnsignedByte();
			else if (opcode == 77)
				opcode77 = stream.readUnsignedByte();
			else if (opcode == 229)
				opcode229 = stream.readUnsignedByte();
			else if (opcode == 230)
				opcode230 = stream.readUnsignedByte();
			else if (opcode == 17)
				opcode17 = stream.readUnsignedByte();
			else if (opcode == 67)
				opcode67 = stream.readUnsignedByte();
			else if (opcode == 131)
				opcode131 = stream.readUnsignedByte();
			else if (opcode == 225)
				opcode225 = stream.readUnsignedByte();
			else if (opcode == 203)
				opcode203 = stream.readUnsignedByte();
			else if (opcode == 19)
				opcode19 = stream.readUnsignedByte();
			else if (opcode == 43)
				opcode43 = stream.readUnsignedByte();
			else if (opcode == 168)
				opcode168 = stream.readUnsignedByte();
			else if (opcode == 46)
				opcode46 = stream.readUnsignedByte();
			else if (opcode == 209)
				opcode209 = stream.readUnsignedByte();
			else if (opcode == 166)
				opcode166 = stream.readUnsignedByte();
			else if (opcode == 54)
				opcode54 = stream.readUnsignedByte();
			else if (opcode == 21)
				opcode21 = stream.readUnsignedByte();
			else if (opcode == 73)
				opcode73 = stream.readUnsignedByte();
			else if (opcode == 159)
				opcode159 = stream.readUnsignedByte();
			else if (opcode == 123)
				opcode123 = stream.readUnsignedByte();
			else if (opcode == 146)
				opcode146 = stream.readUnsignedByte();
			else if (opcode == 180)
				opcode180 = stream.readUnsignedByte();
			else if (opcode == 20)
				opcode20 = stream.readUnsignedByte();
			else if (opcode == 165)
				opcode165 = stream.readUnsignedByte();
			else if (opcode == 84)
				opcode84 = stream.readUnsignedByte();
			else if (opcode == 28)
				opcode28 = stream.readUnsignedByte();
			else if (opcode == 175)
				opcode175 = stream.readUnsignedByte();
			else if (opcode == 141)
				opcode141 = stream.readUnsignedByte();
			else if (opcode == 205)
				opcode205 = stream.readUnsignedByte();
			else if (opcode == 220)
				opcode220 = stream.readUnsignedByte();
			else if (opcode == 136)
				opcode136 = stream.readUnsignedByte();
			else if (opcode == 212)
				opcode212 = stream.readUnsignedByte();
			else if (opcode == 49)
				opcode49 = stream.readUnsignedByte();
			else if (opcode == 69)
				opcode69 = stream.readUnsignedByte();
			else if (opcode == 72)
				opcode72 = stream.readUnsignedByte();
			else if (opcode == 60)
				opcode60 = stream.readUnsignedByte();
			else if (opcode == 62)
				opcode62 = stream.readUnsignedByte();
			else if (opcode == 219)
				opcode219 = stream.readUnsignedByte();
			else if (opcode == 44)
				opcode44 = stream.readUnsignedByte();
			else if (opcode == 227)
				opcode227 = stream.readUnsignedByte();
			else if (opcode == 76)
				opcode76 = stream.readUnsignedByte();
			else if (opcode == 234)
				opcode234 = stream.readUnsignedByte();
			else if (opcode == 57)
				opcode57 = stream.readUnsignedByte();
			else if (opcode == 51)
				opcode51 = stream.readUnsignedByte();
			else if (opcode == 124)
				opcode124 = stream.readUnsignedByte();
			else if (opcode == 70)
				opcode70 = stream.readUnsignedByte();
			else if (opcode == 231)
				opcode231 = stream.readUnsignedByte();
			else if (opcode == 162)
				opcode162 = stream.readUnsignedByte();
			else if (opcode == 160)
				opcode160 = stream.readUnsignedByte();
			else if (opcode == 181)
				opcode181 = stream.readUnsignedByte();
			else if (opcode == 183)
				opcode183 = stream.readUnsignedByte();
			else if (opcode == 191)
				opcode191 = stream.readUnsignedByte();
			else if (opcode == 189)
				opcode189 = stream.readUnsignedByte();
			else if (opcode == 179)
				opcode179 = stream.readUnsignedByte();
			else if (opcode == 173)
				opcode173 = stream.readUnsignedByte();
			else if (opcode == 48)
				opcode48 = stream.readUnsignedByte();
			else if (opcode == 172)
				opcode172 = stream.readUnsignedByte();
			else if (opcode == 42)
				opcode42 = stream.readUnsignedByte();
			else if (opcode == 47)
				opcode47 = stream.readUnsignedByte();
			else if (opcode == 246)
				opcode246 = stream.readUnsignedByte();
			else if (opcode == 89)
				opcode89 = stream.readUnsignedByte();
			else if (opcode == 195)
				opcode195 = stream.readUnsignedByte();
			else if (opcode == 145)
				opcode145 = stream.readUnsignedByte();
			else if (opcode == 224)
				opcode224 = stream.readUnsignedByte();
			else if (opcode == 63)
				opcode63 = stream.readUnsignedByte();
			else if (opcode == 94)
				opcode94 = stream.readUnsignedByte();
			else if (opcode == 201)
				opcode201 = stream.readUnsignedByte();
			else if (opcode == 217)
				opcode217 = stream.readUnsignedByte();
			else if (opcode == 252)
				opcode252 = stream.readUnsignedByte();
			else if (opcode == 228)
				opcode228 = stream.readUnsignedByte();
			else if (opcode == 82)
				opcode82 = stream.readUnsignedByte();
			else if (opcode == 13)
				opcode13 = stream.readUnsignedByte();
			else if (opcode == 14)
				opcode14 = stream.readUnsignedByte();
			else if (opcode == 9)
				opcode9 = stream.readUnsignedByte();
			else if (opcode == 27)
				opcode27 = stream.readUnsignedByte();
			else if (opcode == 66)
				opcode66 = stream.readUnsignedByte();
			else if (opcode == 116)
				opcode116 = stream.readUnsignedByte();
			else if (opcode == 157)
				opcode157 = stream.readUnsignedByte();
			else if (opcode == 244)
				opcode244 = stream.readUnsignedByte();
			else if (opcode == 53)
				opcode53 = stream.readUnsignedByte();
			else if (opcode == 215)
				opcode215 = stream.readUnsignedByte();
			else if (opcode == 171)
				opcode171 = stream.readUnsignedByte();
			else if (opcode == 3)
				opcode3 = stream.readUnsignedByte();
			else if (opcode == 170)
				opcode170 = stream.readUnsignedByte();
			else if (opcode == 226)
				opcode226 = stream.readUnsignedByte();
			else if (opcode == 52)
				opcode52 = stream.readUnsignedByte();
			else if (opcode == 151)
				opcode151 = stream.readUnsignedByte();//		14 66 116 157 244 170 151 9 27
			else if (opcode == 16)
				membersOnly = true;
			else if (opcode == 18) // added
				stream.readUnsignedShort();
			else if (opcode == 23)
				maleEquip1 = stream.readBigSmart();
			else if (opcode == 24)
				maleEquip2 = stream.readBigSmart();
			else if (opcode == 25)
				femaleEquip1 = stream.readBigSmart();
			else if (opcode == 26)
				femaleEquip2 = stream.readBigSmart();
			else if (opcode >= 30 && opcode < 35)
				groundOptions[opcode - 30] = stream.readString();
			else if (opcode >= 35 && opcode < 40)
				inventoryOptions[opcode - 35] = stream.readString();
			else if (opcode == 40)
			{
				int length = stream.readUnsignedByte();
				originalModelColors = new int[length];
				modifiedModelColors = new int[length];
				for (int index = 0; index < length; index++)
				{
					originalModelColors[index] = stream.readUnsignedShort();
					modifiedModelColors[index] = stream.readUnsignedShort();
				}
			}
			else if (opcode == 41)
			{
				int length = stream.readUnsignedByte();
				originalTextureColors = new short[length];
				modifiedTextureColors = new short[length];
				for (int index = 0; index < length; index++)
				{
					originalTextureColors[index] = (short)stream
							.readUnsignedShort();
					modifiedTextureColors[index] = (short)stream
							.readUnsignedShort();
				}
			}
			else if (opcode == 42)
			{
				int length = stream.readUnsignedByte();
				unknownArray1 = new byte[length];
				for (int index = 0; index < length; index++)
					unknownArray1[index] = (byte)stream.readByte();
			}
			else if (opcode == 65)
				unnoted = true;
			else if (opcode == 78)
				maleEquipModelId3 = stream.readBigSmart();
			else if (opcode == 79)
				femaleEquipModelId3 = stream.readBigSmart();
			else if (opcode == 90)
				unknownInt1 = stream.readBigSmart();
			else if (opcode == 91)
				unknownInt2 = stream.readBigSmart();
			else if (opcode == 92)
				unknownInt3 = stream.readBigSmart();
			else if (opcode == 93)
				unknownInt4 = stream.readBigSmart();
			else if (opcode == 95)
				unknownInt5 = stream.readUnsignedShort();
			else if (opcode == 96)
				unknownInt6 = stream.readUnsignedByte();
			else if (opcode == 97)
				certId = stream.readUnsignedShort();
			else if (opcode == 98)
				certTemplateId = stream.readUnsignedShort();
			else if (opcode >= 100 && opcode < 110)
			{
				if (stackIds == null)
				{
					stackIds = new int[10];
					stackAmounts = new int[10];
				}
				stackIds[opcode - 100] = stream.readUnsignedShort();
				stackAmounts[opcode - 100] = stream.readUnsignedShort();
			}
			else if (opcode == 110)
				unknownInt7 = stream.readUnsignedShort();
			else if (opcode == 111)
				unknownInt8 = stream.readUnsignedShort();
			else if (opcode == 112)
				unknownInt9 = stream.readUnsignedShort();
			else if (opcode == 113)
				unknownInt10 = stream.readByte();
			else if (opcode == 114)
				unknownInt11 = stream.readByte() * 5;
			else if (opcode == 115)
				teamId = stream.readUnsignedByte();
			else if (opcode == 121)
				lendId = stream.readUnsignedShort();
			else if (opcode == 122)
				lendTemplateId = stream.readUnsignedShort();
			else if (opcode == 125)
			{
				unknownInt12 = stream.readByte() << 0;
				unknownInt13 = stream.readByte() << 0;
				unknownInt14 = stream.readByte() << 0;
			}
			else if (opcode == 126)
			{
				unknownInt15 = stream.readByte() << 0;
				unknownInt16 = stream.readByte() << 0;
				unknownInt17 = stream.readByte() << 0;
			}
			else if (opcode == 127)
			{
				unknownInt18 = stream.readUnsignedByte();
				unknownInt19 = stream.readUnsignedShort();
			}
			else if (opcode == 128)
			{
				unknownInt20 = stream.readUnsignedByte();
				unknownInt21 = stream.readUnsignedShort();
			}
			else if (opcode == 129)
			{
				unknownInt20 = stream.readUnsignedByte();
				unknownInt21 = stream.readUnsignedShort();
			}
			else if (opcode == 130)
			{
				unknownInt22 = stream.readUnsignedByte();
				unknownInt23 = stream.readUnsignedShort();
			}
			else if (opcode == 132)
			{
				int length = stream.readUnsignedByte();
				unknownArray2 = new int[length];
				for (int index = 0; index < length; index++)
					unknownArray2[index] = stream.readUnsignedShort();
			}
			else if (opcode == 134)
			{
				int unknownValue = stream.readUnsignedByte();
			}
			else if (opcode == 139)
			{
				unknownValue2 = stream.readUnsignedShort();
			}
			else if (opcode == 140)
			{
				unknownValue1 = stream.readUnsignedShort();
			}
			else if (opcode == 249)
			{
				int length = stream.readUnsignedByte();
				for (int index = 0; index < length; index++)
				{
					bool stringInstance = stream.readUnsignedByte() == 1;
					int key = stream.read24BitInt();
					if (stringInstance)
					{
						stream.readString();
					}
					else
					{
						stream.readInt();
					}
				}
			}
			else
			{
				throw new ArgumentOutOfRangeException("MISSING OPCODE " + opcode + " FOR ITEM " + Id);
			}
		}

		private int opcode3;
		private int opcode199;
		private int opcode223;
		private int opcode198;
		private int opcode186;
		private int opcode29;
		private int opcode238;
		private int opcode153;
		private int opcode155;
		private int opcode99;
		private int opcode251;
		private int opcode22;
		private int opcode192;
		private int opcode117;
		private int opcode66;
		private int opcode245;
		private int opcode45;
		private int opcode56;
		private int opcode248;
		private int opcode237;
		private int opcode243;
		private int opcode185;
		private int opcode221;
		private int opcode240;
		private int opcode154;
		private int opcode158;
		private int opcode137;
		private int opcode143;
		private int opcode61;
		private int opcode80;
		private int opcode196;
		private int opcode85;
		private int opcode239;
		private int opcode177;
		private int opcode163;
		private int opcode150;
		private int opcode152;
		private int opcode135;
		private int opcode120;
		private int opcode204;
		private int opcode81;
		private int opcode208;
		private int opcode242;
		private int opcode15;
		private int opcode233;
		private int opcode213;
		private int opcode207;
		private int opcode216;
		private int opcode206;
		private int opcode50;
		private int opcode193;
		private int opcode71;
		private int opcode10;
		private int opcode55;
		private int opcode144;
		private int opcode235;
		private int opcode188;
		private int opcode241;
		private int opcode236;
		private int opcode182;
		private int opcode169;
		private int opcode190;
		private int opcode178;
		private int opcode88;
		private int opcode200;
		private int opcode184;
		private int opcode176;
		private int opcode197;
		private int opcode247;
		private int opcode218;
		private int opcode250;
		private int opcode174;
		private int opcode210;
		private int opcode164;
		private int opcode142;
		private int opcode148;
		private int opcode133;
		private int opcode222;
		private int opcode138;
		private int opcode194;
		private int opcode119;
		private int opcode202;
		private int opcode149;
		private int opcode64;
		private int opcode147;
		private int opcode214;
		private int opcode74;
		private int opcode86;
		private int opcode167;
		private int opcode161;
		private int opcode58;
		private int opcode59;
		private int opcode187;
		private int opcode77;
		private int opcode229;
		private int opcode230;
		private int opcode17;
		private int opcode67;
		private int opcode131;
		private int opcode225;
		private int opcode203;
		private int opcode19;
		private int opcode43;
		private int opcode168;
		private int opcode46;
		private int opcode209;
		private int opcode166;
		private int opcode54;
		private int opcode21;
		private int opcode73;
		private int opcode159;
		private int opcode123;
		private int opcode146;
		private int opcode180;
		private int opcode20;
		private int opcode165;
		private int opcode84;
		private int opcode28;
		private int opcode175;
		private int opcode141;
		private int opcode205;
		private int opcode220;
		private int opcode136;
		private int opcode212;
		private int opcode49;
		private int opcode69;
		private int opcode72;
		private int opcode60;
		private int opcode62;
		private int opcode219;
		private int opcode44;
		private int opcode227;
		private int opcode76;
		private int opcode234;
		private int opcode57;
		private int opcode51;
		private int opcode124;
		private int opcode70;
		private int opcode231;
		private int opcode162;
		private int opcode160;
		private int opcode181;
		private int opcode183;
		private int opcode191;
		private int opcode189;
		private int opcode179;
		private int opcode173;
		private int opcode48;
		private int opcode172;
		private int opcode42;
		private int opcode47;
		private int opcode246;
		private int opcode89;
		private int opcode195;
		private int opcode145;
		private int opcode211;
		private int opcode255;
		private int opcode75;
		private int opcode87;
		private int opcode68;
		private int opcode118;
		private int opcode83;
		private int opcode254;
		private int opcode156;
		private int opcode232;
		private int opcode253;
		private int opcode224;
		private int opcode63;
		private int opcode94;
		private int opcode201;
		private int opcode217;
		private int opcode252;
		private int opcode228;
		private int opcode82;
		private int opcode13;
		private int opcode14;
		private int opcode9;
		private int opcode27;
		private int opcode116;
		private int opcode157;
		private int opcode244;
		private int opcode53;
		private int opcode215;
		private int opcode171;
		private int opcode170;
		private int opcode226;
		private int opcode52;
		private int opcode151;
		private int unknownValue1;
		private int unknownValue2;
	}
}
