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
		private static IDictionary<char, Tuple<string, long>> _signedTranlationHelpers = new Dictionary<char, Tuple<string, long>>()
		{
			{'{', new Tuple<string, long>("0", 1)},
			{'A', new Tuple<string, long>("1", 1)},
			{'B', new Tuple<string, long>("2", 1)},
			{'C', new Tuple<string, long>("3", 1)},
			{'D', new Tuple<string, long>("4", 1)},
			{'E', new Tuple<string, long>("5", 1)},
			{'F', new Tuple<string, long>("6", 1)},
			{'G', new Tuple<string, long>("7", 1)},
			{'H', new Tuple<string, long>("8", 1)},
			{'I', new Tuple<string, long>("9", 1)},
			{'}', new Tuple<string, long>("0", -1)},
			{'J', new Tuple<string, long>("1", -1)},
			{'K', new Tuple<string, long>("2", -1)},
			{'L', new Tuple<string, long>("3", -1)},
			{'M', new Tuple<string, long>("4", -1)},
			{'N', new Tuple<string, long>("5", -1)},
			{'O', new Tuple<string, long>("6", -1)},
			{'P', new Tuple<string, long>("7", -1)},
			{'Q', new Tuple<string, long>("8", -1)},
			{'R', new Tuple<string, long>("9", -1)}
		};

		public static long? ToLongNullableFromSigned(byte[] signedEbcdic)
		{
			var ascii = ToString(signedEbcdic);

			if (ascii == null)
			{
				return (long?) null;
			}

			ascii = ascii.Trim();

			if (string.IsNullOrWhiteSpace(ascii))
			{
				return (long?) null;
			}

			if (System.Text.RegularExpressions.Regex.IsMatch(ascii, @"^\d+$")) //Unsigned integer
			{
				return ascii.ToLongNullable();
			}
			else if (System.Text.RegularExpressions.Regex.IsMatch(ascii, @"^\d+[A-R}{]$")) //Signed integer. Last characrer is A-R, or curly braces
			{
				var lastChar = ascii.Last();

				Tuple<string, long> signedTranlationHelper;
				if (_signedTranlationHelpers.TryGetValue(lastChar, out signedTranlationHelper))
				{
					ascii = ascii.Replace(new([lastChar]), signedTranlationHelper.Item1);
					var result = ascii.ToLongNullable();
					return (result.HasValue ? result.Value*signedTranlationHelper.Item2 : result);

				}
			}

			throw new("Bad packed format");
		}
	}
}