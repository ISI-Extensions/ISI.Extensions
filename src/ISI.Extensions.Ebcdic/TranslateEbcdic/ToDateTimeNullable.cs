#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
		private static string ConvertMainframeDateFormat(string value)
		{
			value = value.PadLeft(7, '0');

			if (value.Length != 7)
			{
				return value;
			}

			if ((value == "0000000") || (value == "0999999") || (value == "9999999"))
			{
				return string.Empty;
			}

			var match = System.Text.RegularExpressions.Regex.Match(value, @"^(?<Year>\d{3})(?<Month>\d{2})(?<Day>\d{2})$"); //E.g.: 0801232 = 1980-12-31; 1811231 = 2080-12-31

			if (match.Success)
			{
				try
				{
					var year = match.Groups["Year"].Value.ToInt() + 1900; //013 = 1913, 113 = 2013...
					var month = match.Groups["Month"].Value.ToInt();
					var day = match.Groups["Day"].Value.ToInt();

					return (new DateTime(year, month, day)).Formatted(DateTimeExtensions.DateTimeFormat.DateTime);
				}
				catch { }
			}

			throw new("Bad format");
		}

		public static DateTime? ToDateTimeNullable(byte[] ebcdic)
		{
			var ascii = ToString(ebcdic);

			if (ascii == null)
			{
				return (DateTime?) null;
			}

			ascii = ascii.Trim();

			if (string.IsNullOrWhiteSpace(ascii) || System.Text.RegularExpressions.Regex.IsMatch(ascii, "^0+$") || System.Text.RegularExpressions.Regex.IsMatch(ascii, "^9+$"))
			{
				return (DateTime?)null;
			}

			if ((ascii.Length == 6) && System.Text.RegularExpressions.Regex.IsMatch(ascii, @"^\d{6}$"))
			{
				return ascii.ToDateTimeNullable();
			}
			if (ascii.Length == 7 && System.Text.RegularExpressions.Regex.IsMatch(ascii, @"^\d{7}$"))
			{
				//cyyMMdd (c = century)
				return ConvertMainframeDateFormat(ascii).ToDateTimeNullable();
			}
			if (ascii.Length == 8 && System.Text.RegularExpressions.Regex.IsMatch(ascii, @"^\d{8}$"))
			{
				return ascii.ToDateTimeNullable();
			}

			throw new("Bad format");
		}
	}
}