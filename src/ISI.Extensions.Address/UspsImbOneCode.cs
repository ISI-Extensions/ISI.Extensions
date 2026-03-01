#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class Address
	{
		public class UspsImbOneCode
		{
			public int? BarCodeId { get; set; }
			public int SpecialServices { get; set; }
			public int MailerId { get; set; }
			public long SequenceNumber { get; set; }
			public string ZipCode { get; set; }

			public UspsImbOneCode()
			{

			}
			public UspsImbOneCode(string value)
			{
				Value = (value.IndexOf(" ") > 0 ? value : Decode(value));
			}

			public string Value
			{
				get
				{
					var specialServices = $"{SpecialServices}";
					specialServices = specialServices.TextJustify(StringExtensions.TextJustifyAlignment.Right, 3, '0');

					var mailerId = $"{MailerId}";
					mailerId = (mailerId.Length <= 6 ? mailerId.TextJustify(StringExtensions.TextJustifyAlignment.Left, 6, '0') : mailerId.TextJustify(StringExtensions.TextJustifyAlignment.Left, 9, '0'));

					var sequenceNumberLow = SequenceNumber % (mailerId.Length <= 6 ? 1000000000 : 1000000);
					var sequenceNumberHigh = (SequenceNumber - sequenceNumberLow) / (mailerId.Length <= 6 ? 1000000000 : 1000000);

					var sequenceNumber = $"{sequenceNumberLow}";
					sequenceNumber = (mailerId.Length <= 6 ? sequenceNumber.TextJustify(StringExtensions.TextJustifyAlignment.Right, 9, '0') : sequenceNumber.TextJustify(StringExtensions.TextJustifyAlignment.Right, 6, '0'));

					var barCodeId = $"{BarCodeId}";

					if (!BarCodeId.HasValue)
					{
						var barcodeLow = sequenceNumberHigh % 4;
						var barcodeHigh = ((sequenceNumberHigh - barcodeLow) / 4) % 10;

						barCodeId = $"{barcodeHigh}{barcodeLow}";
					}

					barCodeId = barCodeId.TextJustify(StringExtensions.TextJustifyAlignment.Right, 2, '0');

					var zipCode = ISI.Extensions.Address.ZipFormat(ZipCode, ZipStyle.ZipPlus4WithoutDashTruncateEmptyZip4);

					return $"{barCodeId} {specialServices} {mailerId} {sequenceNumber} {zipCode}";
				}
				set
				{
					var pieces = $"{value}    ".Split(' ');
					BarCodeId = pieces[0].ToInt();
					SpecialServices = pieces[1].ToInt();
					MailerId = pieces[2].ToInt();
					SequenceNumber = pieces[3].ToInt();
					ZipCode = pieces[0];
				}
			}

			public string GetEncoded() => Encode(Value);


			// for more information and specs check
			// http://ribbs.usps.gov/onecodesolution/USPS-B-3200D001.pdf
			private static int table2of13Size = 78;
			private static int table5of13Size = 1287;
			private static long entries2of13 = table5of13Size;
			private static long entries5of13 = table2of13Size;
			private static int[] table2of13 = OneCodeInfo(1);
			private static int[] table5of13 = OneCodeInfo(2);
			private static int[] table2of13ArrayPtr = table2of13;
			private static int[] table5of13ArrayPtr = table5of13;
			private static decimal[][] codewordArray = OneCodeInfo();
			private static int[] BarTopCharacterIndexArray = [4, 0, 2, 6, 3, 5, 1, 9, 8, 7, 1, 2, 0, 6, 4, 8, 2, 9, 5, 3, 0, 1, 3, 7, 4, 6, 8, 9, 2, 0, 5, 1, 9, 4, 3, 8, 6, 7, 1, 2, 4, 3, 9, 5, 7, 8, 3, 0, 2, 1, 4, 0, 9, 1, 7, 0, 2, 4, 6, 3, 7, 1, 9, 5, 8];
			private static int[] BarBottomCharacterIndexArray = [7, 1, 9, 5, 8, 0, 2, 4, 6, 3, 5, 8, 9, 7, 3, 0, 6, 1, 7, 4, 6, 8, 9, 2, 5, 1, 7, 5, 4, 3, 8, 7, 6, 0, 2, 5, 4, 9, 3, 0, 1, 6, 8, 2, 0, 4, 5, 9, 6, 7, 5, 2, 6, 3, 8, 5, 1, 9, 8, 7, 4, 0, 2, 6, 3];
			private static int[] BarTopCharacterShiftArray = [3, 0, 8, 11, 1, 12, 8, 11, 10, 6, 4, 12, 2, 7, 9, 6, 7, 9, 2, 8, 4, 0, 12, 7, 10, 9, 0, 7, 10, 5, 7, 9, 6, 8, 2, 12, 1, 4, 2, 0, 1, 5, 4, 6, 12, 1, 0, 9, 4, 7, 5, 10, 2, 6, 9, 11, 2, 12, 6, 7, 5, 11, 0, 3, 2];
			private static int[] BarBottomCharacterShiftArray = [2, 10, 12, 5, 9, 1, 5, 4, 3, 9, 11, 5, 10, 1, 6, 3, 4, 1, 10, 0, 2, 11, 8, 6, 1, 12, 3, 8, 6, 4, 4, 11, 0, 6, 1, 9, 11, 5, 3, 7, 3, 10, 7, 11, 8, 2, 10, 3, 5, 8, 0, 3, 12, 11, 8, 4, 5, 1, 3, 0, 7, 12, 9, 8, 10];

			public static string Encode(string source)
			{
				if (string.IsNullOrEmpty(source)) return null;
				source = TrimOff(source, " -.");
				if (!System.Text.RegularExpressions.Regex.IsMatch(source, "^[0-9][0-4](([0-9]{18})|([0-9]{23})|([0-9]{27})|([0-9]{29}))$")) return string.Empty;
				var encoded = string.Empty;
				var l = 0L;
				var zip = source.Substring(20);
				switch (zip.Length)
				{
					case 5:
						l = long.Parse(zip) + 1;
						break;
					case 9:
						l = long.Parse(zip) + 100001;
						break;
					case 11:
						l = long.Parse(zip) + 1000100001;
						break;
				}
				decimal v = l;
				v = v * 10 + int.Parse(source.Substring(0, 1));
				v = v * 5 + int.Parse(source.Substring(1, 1));
				var ds = v.ToString() + source.Substring(2, 18);
				var byteArray = new int[13];
				byteArray[12] = (int)(l & 255);
				byteArray[11] = (int)(l >> 8 & 255);
				byteArray[10] = (int)(l >> 16 & 255);
				byteArray[9] = (int)(l >> 24 & 255);
				byteArray[8] = (int)(l >> 32 & 255);
				OneCodeMathMultiply(ref byteArray, 13, 10);
				OneCodeMathAdd(ref byteArray, 13, int.Parse(source.Substring(0, 1)));
				OneCodeMathMultiply(ref byteArray, 13, 5);
				OneCodeMathAdd(ref byteArray, 13, int.Parse(source.Substring(1, 1)));
				for (var i = 2; i <= 19; i++)
				{
					OneCodeMathMultiply(ref byteArray, 13, 10);
					OneCodeMathAdd(ref byteArray, 13, int.Parse(source.Substring(i, 1)));
				}
				var fcs = OneCodeMathFcs(byteArray);
				for (var i = 0; i <= 9; i++)
				{
					codewordArray[i][0] = entries2of13 + entries5of13;
					codewordArray[i][1] = 0;
				}
				codewordArray[0][0] = 659;
				codewordArray[9][0] = 636;
				OneCodeMathDivide(ds);
				codewordArray[9][1] *= 2;
				if (fcs >> 10 != 0) codewordArray[0][1] += 659;
				int[] ai = new int[65], ai1 = new int[65];
				var ad = new decimal[11][];
				for (var i = 0; i <= 9; i++) ad[i] = new decimal[2];
				for (var i = 0; i <= 9; i++)
				{
					if (codewordArray[i][1] >= (decimal)(entries2of13 + entries5of13)) return string.Empty;
					ad[i][0] = 8192;
					if (codewordArray[i][1] >= (decimal)entries2of13) ad[i][1] = table2of13[(int)(codewordArray[i][1] - entries2of13)];
					else ad[i][1] = table5of13[(int)codewordArray[i][1]];
				}
				for (var i = 0; i <= 9; i++) if ((fcs & 1 << i) != 0) ad[i][1] = ~(int)ad[i][1] & 8191;
				for (var i = 0; i <= 64; i++)
				{
					ai[i] = (int)ad[BarTopCharacterIndexArray[i]][1] >> BarTopCharacterShiftArray[i] & 1;
					ai1[i] = (int)ad[BarBottomCharacterIndexArray[i]][1] >> BarBottomCharacterShiftArray[i] & 1;
				}
				encoded = "";
				// T: track, D: descender, A: ascender, F: full bar
				for (var i = 0; i <= 64; i++)
				{
					if (ai[i] == 0) encoded += (ai1[i] == 0) ? "T" : "D";
					else encoded += (ai1[i] == 0) ? "A" : "F";
				}
				return encoded;
			}

			public static string Decode(string source)
			{
				if (!System.Text.RegularExpressions.Regex.IsMatch(source, "^[ADFT]{65}$")) return string.Empty;
				int[] ad = new int[10], byteArray = new int[13];
				var r = 0;
				var bin = new System.Text.StringBuilder();
				var result = string.Empty;
				for (var i = 0; i <= 64; i++)
				{
					if (source[i] == 'T') bin.Append("00");
					else if (source[i] == 'D') bin.Append("01");
					else if (source[i] == 'A') bin.Append("10");
					else bin.Append("11");
				}
				var bits = bin.ToString();
				for (var i = 0; i <= 128; i += 2)
				{
					int v = Convert.ToInt32(bits.Substring(i, 2), 2), k = i / 2;
					if ((v > 1)) ad[BarTopCharacterIndexArray[k]] += 1 << BarTopCharacterShiftArray[k];
					if ((v % 2 == 1)) ad[BarBottomCharacterIndexArray[k]] += 1 << BarBottomCharacterShiftArray[k];
				}
				for (var i = 0; i <= 9; i++)
				{
					int test = ad[i], index = Array.IndexOf(table5of13, test);
					if ((index < 0))
					{
						test = ~test & 8191;
						index = Array.IndexOf(table5of13, test);
						if ((index < 0))
						{
							index = Array.IndexOf(table2of13, test);
							index += 1287;
						}
					}
					ad[i] = index;
				}
				ad[9] = (int)ad[9] / 2;
				if ((ad[0] > 658)) ad[0] -= 659;
				OneCodeMathAdd(ref byteArray, 13, ad[0]);
				for (var i = 1; i <= 8; i++)
				{
					OneCodeMathMultiply(ref byteArray, 13, 1365);
					OneCodeMathAdd(ref byteArray, 13, ad[i]);
				}
				OneCodeMathMultiply(ref byteArray, 13, 636);
				OneCodeMathAdd(ref byteArray, 13, ad[9]);
				r = OneCodeMathMod(byteArray, 10);
				result = r.ToString() + result;
				for (var i = 2; i <= 19; i++)
				{
					OneCodeMathAdd(ref byteArray, 13, -r);
					OneCodeMathDivide(ref byteArray, 10);
					r = OneCodeMathMod(byteArray, 10);
					result = r.ToString() + result;
				}
				OneCodeMathAdd(ref byteArray, 13, -r);
				OneCodeMathDivide(ref byteArray, 5);
				r = OneCodeMathMod(byteArray, 5);
				result = r.ToString() + result;
				OneCodeMathAdd(ref byteArray, 13, -r);
				OneCodeMathDivide(ref byteArray, 10);
				var restBytes = new byte[8];
				for (var i = 12; i >= 5; i += -1) restBytes[12 - i] = (byte)byteArray[i];
				var rest = BitConverter.ToInt64(restBytes, 0);
				if (rest > 1000100001) result += (rest - 1000100001).ToString().PadLeft(11, '0');
				else if (rest > 100001) result += (rest - 100001).ToString().PadLeft(9, '0');
				else if (rest > 0) result += (rest - 1).ToString().PadLeft(5, '0');
				return result;
			}

			private static int[] OneCodeInfo(int topic)
			{
				switch (topic)
				{
					case 1:
						var a = new int[table2of13Size + 2];
						OneCodeInitializeNof13Table(ref a, 2, table2of13Size);
						entries5of13 = table2of13Size;
						return a;
					case 2:
						var b = new int[table5of13Size + 2];
						OneCodeInitializeNof13Table(ref b, 5, table5of13Size);
						entries2of13 = table5of13Size;
						return b;
				}
				return new int[2];
			}

			private static decimal[][] OneCodeInfo()
			{
				var da = new decimal[11][];
				try
				{
					for (var i = 0; i <= 9; i++) da[i] = new decimal[2];
					return da;
				}
				finally
				{
					da = null;
				}
			}

			private static bool OneCodeInitializeNof13Table(ref int[] ai, int i, int j)
			{
				int i1 = 0, j1 = j - 1;
				for (var k = 0; k <= 8191; k++)
				{
					var k1 = 0;
					for (var l1 = 0; l1 <= 12; l1++) if ((k & 1 << l1) != 0) k1 += 1;
					if (k1 == i)
					{
						var l = OneCodeMathReverse(k) >> 3;
						var flag = (k == l);
						if (l >= k)
						{
							if (flag)
							{
								ai[j1] = k;
								j1 -= 1;
							}
							else
							{
								ai[i1] = k;
								i1 += 1;
								ai[i1] = l;
								i1 += 1;
							}
						}
					}
				}
				return i1 == j1 + 1;
			}

			private static bool OneCodeMathAdd(ref int[] bytearray, int i, int j)
			{
				if (j == 0) return true;
				if (bytearray == null) return false;
				if (i < 1) return false;
				i -= 1;
				bytearray[i] += j;
				var carry = 0;
				if (j > 0)
				{
					while (i > 0 & bytearray[i] > 255)
					{
						carry = (bytearray[i] >> 8);
						bytearray[i] = bytearray[i] % 256;
						i -= 1;
						bytearray[i] += carry;
					}
				}
				else
				{
					while (i > 0 & bytearray[i] < 0)
					{
						carry = 1;
						bytearray[i] += 256;
						i -= 1;
						bytearray[i] -= carry;
					}
				}
				return true;
			}

			private static int OneCodeMathMod(int[] byteArray, int d)
			{
				int i = 0, r = 0, l = byteArray.Length;
				while ((i < 13))
				{
					r <<= 8;
					r |= byteArray[i];
					r %= d;
					i += 1;
				}
				return r;
			}

			private static void OneCodeMathDivide(ref int[] byteArray, int d)
			{
				int i = 0, r = 0, l = byteArray.Length;
				while ((i < l))
				{
					r <<= 8;
					r |= byteArray[i];
					byteArray[i] = (int)r / d;
					r %= d;
					i += 1;
				}
			}

			private static void OneCodeMathDivide(string v)
			{
				// back to school - you may change it to use shifting
				var j = 10;
				var n = v;
				for (var k = j - 1; k >= 1; k += -1)
				{
					string r = string.Empty, copy = n, left = "0";
					int divider = (int)codewordArray[k][0], l = copy.Length;
					for (var i = 1; i <= l; i++)
					{
						var divident = int.Parse(copy.Substring(0, i));
						while (divident < divider & i < l - 1)
						{
							r += "0";
							i += 1;
							divident = int.Parse(copy.Substring(0, i));
						}
						r += (divident / divider).ToString();
						left = (divident % divider).ToString().PadLeft(i, '0');
						copy = left + copy.Substring(i);
					}
					n = r.TrimStart('0');
					if (string.IsNullOrEmpty(n)) n = "0";
					codewordArray[k][1] = int.Parse(left);
					if (k == 1) codewordArray[0][1] = int.Parse(r);
				}
			}

			private static int OneCodeMathFcs(int[] bytearray)
			{
				int c = 3893, i = 2047, j = bytearray[0] << 5;
				for (var b = 2; b <= 7; b++)
				{
					if (((i ^ j) & 1024) != 0) i = (i << 1) ^ c;
					else i <<= 1;
					i &= 2047;
					j <<= 1;
				}
				for (var l = 1; l <= 12; l++)
				{
					var k = bytearray[l] << 3;
					for (var b = 0; b <= 7; b++)
					{
						if (((i ^ k) & 1024) != 0) i = (i << 1) ^ c;
						else i <<= 1;
						i &= 2047;
						k <<= 1;
					}
				}
				return i;
			}

			private static bool OneCodeMathMultiply(ref int[] bytearray, int i, int j)
			{
				if (bytearray == null) return false;
				if (i < 1) return false;
				int l = 0, k = 0;
				for (k = i - 1; k >= 1; k += -2)
				{
					var x = (bytearray[k] | (bytearray[k - 1] << 8)) * j + l;
					bytearray[k] = x & 255;
					bytearray[k - 1] = x >> 8 & 255;
					l = x >> 16;
				}
				if (k == 0) bytearray[0] = (bytearray[0] * j + l) & 255;
				return true;
			}

			private static int OneCodeMathReverse(int i)
			{
				var j = 0;
				for (var k = 0; k <= 15; k++)
				{
					j <<= 1;
					j |= i & 1;
					i >>= 1;
				}
				return j;
			}

			private static string TrimOff(string source, string bad)
			{
				var l = bad.Length - 1;
				for (var i = 0; i <= l; i++) source = source.Replace(bad.Substring(i, 1), string.Empty);
				return source;
			}
		}
	}
}