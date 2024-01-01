#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Ebcdic
{
	public partial class TranslateEbcdic
	{
		public static decimal? ToDecimalNullableFromPacked(byte[] packedEbcdic, int scale)
		{
			if (IsNull(packedEbcdic))
			{
				return (decimal?) null;
			}

			long lo = 0;
			long mid = 0;
			long hi = 0;
			bool isNegative;

			// this nybble stores only the sign, not a digit.  
			// "0x0C" hex is positive, "0x0D" hex is negative, and "0x0F" hex is unsigned. 
			switch (Nibble(packedEbcdic, 0))
			{
				case 0x0D:
					isNegative = true;
					break;
				case 0x0F:
				case 0x0C:
					isNegative = false;
					break;
				default:
					throw new("Format missing sign");
			}

			for (int byteIndex = packedEbcdic.Length * 2 - 1; byteIndex > 0; byteIndex--)
			{
				// multiply by 10
				var intermediate = lo * 10;
				lo = intermediate & 0xffffffff;
				var carry = intermediate >> 32;
				intermediate = mid * 10 + carry;
				mid = intermediate & 0xffffffff;
				carry = intermediate >> 32;
				intermediate = hi * 10 + carry;
				hi = intermediate & 0xffffffff;
				carry = intermediate >> 32;
				// By limiting input length to 14, we ensure overflow will never occur

				long digit = Nibble(packedEbcdic, byteIndex);
				if (digit > 9)
				{
					throw new("Format with bad (high) digit");
				}
				intermediate = lo + digit;
				lo = intermediate & 0xffffffff;
				carry = intermediate >> 32;
				if (carry > 0)
				{
					intermediate = mid + carry;
					mid = intermediate & 0xffffffff;
					carry = intermediate >> 32;
					if (carry > 0)
					{
						intermediate = hi + carry;
						hi = intermediate & 0xffffffff;
						carry = intermediate >> 32;
						// carry should never be non-zero. Back up with validation
					}
				}
			}

			return new decimal((int)lo, (int)mid, (int)hi, isNegative, (byte)scale);
		}
	}
}