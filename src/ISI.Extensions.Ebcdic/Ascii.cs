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

namespace ISI.Extensions.Ebcdic
{
	public class Ascii
	{
		public static string AsciiToEbcdic(string ascii)
		{
			var length = ascii.Length;

			var result = new char[length];

			var chars = ascii.ToCharArray();

			for (var index = 0; index < length; index++)
			{
				result[index] = Convert.ToChar(Translations.AsciiToEbcdic[Convert.ToInt32(chars[index])]);
			}

			return new(result);
		}

		public static byte[] AsciiToEbcdic(byte[] ascii)
		{
			var length = ascii.Length;

			var result = new byte[length];

			for (var index = 0; index < length; index++)
			{
				result[index] = Convert.ToByte(Translations.AsciiToEbcdic[Convert.ToInt32(ascii[index])]);
			}

			return result;
		}

		public static void AsciiToEbcdic(System.IO.Stream ascii, System.IO.Stream result)
		{
			int value;
			while ((value = ascii.ReadByte()) >= 0)
			{
				result.WriteByte(Convert.ToByte(Translations.AsciiToEbcdic[value]));
			}
		}

		public static TResult AsciiToEbcdic<TResult>(System.IO.Stream ascii)
			where TResult : System.IO.Stream, new()
		{
			var result = new TResult();

			AsciiToEbcdic(ascii, result);

			return result;
		}
	}
}
