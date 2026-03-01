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
		public enum ZipStyle
		{
			ZipPlus4WithDashTruncateEmptyZip4,
			ZipPlus4WithoutDashTruncateEmptyZip4,
			Zip5Only,
		}

		public static string ZipFormat(int? zip, ZipStyle zipStyle = ZipStyle.ZipPlus4WithDashTruncateEmptyZip4, bool throwExceptionForBadZips = false)
		{
			return ZipFormat(zip.GetValueOrDefault(), zipStyle, throwExceptionForBadZips);
		}

		public static string ZipFormat(int zip, ZipStyle zipStyle = ZipStyle.ZipPlus4WithDashTruncateEmptyZip4, bool throwExceptionForBadZips = false)
		{
			return ZipFormat(string.Format((zip <= 99999 ? "{0:00000}" : "{0:000000000}"), zip), zipStyle, throwExceptionForBadZips);
		}

		public static string ZipFormat(string zip, string zip4, ZipStyle zipStyle = ZipStyle.ZipPlus4WithDashTruncateEmptyZip4, bool throwExceptionForBadZips = false)
		{
			return ZipFormat($"{zip}-{zip4}", zipStyle, throwExceptionForBadZips);
		}

		public static string ZipFormat(string zip, ZipStyle zipStyle = ZipStyle.ZipPlus4WithDashTruncateEmptyZip4, bool throwExceptionForBadZips = false)
		{
			zip = (zip ?? string.Empty).Replace("-", string.Empty);

			var value = ISI.Extensions.StringFormat.StringNumericOnly(zip);

			if (throwExceptionForBadZips && !string.Equals(zip, value, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new Exception($"Bad source zip \"{zip}\" invalid characters");
			}

			if (value.Length > 0)
			{
				if (value.Length > 9)
				{
					if (throwExceptionForBadZips)
					{
						throw new Exception($"Bad source zip \"{value}\" too many characters");
					}

					value = value.Substring(0, 9);
				}

				while (value.Length < 5)
				{
					value = "0" + value;
				}

				if (value.Length < 9)
				{
					value = value.Substring(0, 5);
				}

				if ((zipStyle == ZipStyle.ZipPlus4WithDashTruncateEmptyZip4) || (zipStyle == ZipStyle.ZipPlus4WithoutDashTruncateEmptyZip4))
				{
					if ((value.Length == 9) && (value.Substring(5, 4) == "0000"))
					{
						value = value.Substring(0, 5);
					}
				}

				if ((zipStyle == ZipStyle.ZipPlus4WithDashTruncateEmptyZip4) && (value.Length > 5))
				{
					value = value.Substring(0, 5) + "-" + value.Substring(5);
				}

				if (zipStyle == ZipStyle.Zip5Only)
				{
					value = value.Substring(0, 5);
				}
			}

			if (value == "00000")
			{
				value = string.Empty;
			}

			return value;
		}

		public static string Zip5(int value)
		{
			return Zip5($"{value}");
		}

		public static string Zip5(int? value)
		{
			return Zip5($"{value}");
		}

		public static string Zip5(string value)
		{
			return ZipFormat(value).Split(new char[] { '-' })[0];
		}

		public static string ZipPlus4(int value)
		{
			return ZipPlus4($"{value}");
		}

		public static string ZipPlus4(int? value)
		{
			return ZipPlus4($"{value}");
		}

		public static string ZipPlus4(string value)
		{
			return $"{ZipFormat(value)}-XXXX".Split(new char[] { '-' })[1].Trim('X');
		}
	}
}