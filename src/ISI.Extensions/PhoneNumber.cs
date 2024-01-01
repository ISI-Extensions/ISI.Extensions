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
using System.Text;

namespace ISI.Extensions
{
	public class PhoneNumber
	{
		public static readonly System.Text.RegularExpressions.Regex PhoneNumberRegex = new(@"^\s*(((\(\s*1\s*\))|1)\s*-?)?(?<AreaCode>\(?\s*\d{3}\s*\)?)\s*-?\s*(?<Exchange>\d{3})\s*-?\s*(?<Extension>\d{4})\s*$");

		#region GetPhoneNumber
		public static string GetPhoneNumber(string value)
		{
			var result = string.Empty;

			value ??= string.Empty;

			var match = PhoneNumberRegex.Match(value);

			if (match.Success)
			{
				result = string.Format("{0}{1}{2}", match.Groups["AreaCode"], match.Groups["Exchange"], match.Groups["Extension"]);
			}

			return result;
		}
		#endregion

		#region ConvertAlphaToNumeric
		public static string ConvertAlphaToNumeric(string value)
		{
			var result = value;

			const string phoneNumberAlphas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			const string phoneNumberAlphaNumerics = "22233344455566677778889999";

			if (!string.IsNullOrWhiteSpace(value))
			{
				var phoneNumber = new StringBuilder();

				foreach (var digit in value)
				{
					var keyIndex = phoneNumberAlphas.IndexOf(digit);

					phoneNumber.Append((keyIndex >= 0 ? phoneNumberAlphaNumerics[keyIndex] : digit));
				}

				result = phoneNumber.ToString();
			}

			return result;
		}
		#endregion

		#region Area Code
		public static string AreaCodeFormat(int value)
		{
			return AreaCodeFormat(value.ToString());
		}

		public static string AreaCodeFormat(string value)
		{
			value ??= string.Empty;

			if (value.Length > 0)
			{
				if (value.Length > 3) value = value.Substring(0, 3);
			}

			return value;
		}
		#endregion

		#region Phone Number
		public enum PhoneNumberFormat
		{
			None,
			Default,
			Dashes,
			Periods,
			e164
		}

		public static string Formatted(int value, PhoneNumberFormat phoneNumberFormat = PhoneNumberFormat.Default)
		{
			return Formatted(value.ToString(), phoneNumberFormat);
		}

		public static string Formatted(string value, PhoneNumberFormat phoneNumberFormat = PhoneNumberFormat.Default)
		{
			var result = string.Empty;

			value ??= string.Empty;

			if (value.StartsWith("+1"))
			{
				value = value.Substring(2);
			}

			var rawPhoneNumber = (value ?? string.Empty).Replace("+", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty).Replace(" ", string.Empty).Replace(".", string.Empty).Replace("x", string.Empty).Replace("X", string.Empty);

			if (long.TryParse(rawPhoneNumber, out var i))
			{
				#region check for international
				if (rawPhoneNumber.StartsWith("011"))
				{
					result += "+011 ";
					rawPhoneNumber = rawPhoneNumber.Remove(0, 3);
				}
				#endregion

				if (phoneNumberFormat == PhoneNumberFormat.e164)
				{
					result += "+1";
				}

				#region Area Code
				if (rawPhoneNumber.Length >= 10)
				{
					switch (phoneNumberFormat)
					{
						case PhoneNumberFormat.None:
							result += rawPhoneNumber.Substring(0, 3);
							break;
						case PhoneNumberFormat.Default:
							result += string.Format("({0}) ", rawPhoneNumber.Substring(0, 3));
							break;
						case PhoneNumberFormat.Dashes:
							result += string.Format("{0}-", rawPhoneNumber.Substring(0, 3));
							break;
						case PhoneNumberFormat.Periods:
							result += string.Format("{0}.", rawPhoneNumber.Substring(0, 3));
							break;
						case PhoneNumberFormat.e164:
							result += rawPhoneNumber.Substring(0, 3);
							break;
					}

					rawPhoneNumber = rawPhoneNumber.Remove(0, 3);
				}
				#endregion

				#region Phone Number
				if (rawPhoneNumber.Length >= 7)
				{
					switch (phoneNumberFormat)
					{
						case PhoneNumberFormat.None:
							result += string.Format("{0}{1}", rawPhoneNumber.Substring(0, 3), rawPhoneNumber.Substring(3, 4));
							break;
						case PhoneNumberFormat.Default:
							result += string.Format("{0}-{1}", rawPhoneNumber.Substring(0, 3), rawPhoneNumber.Substring(3, 4));
							break;
						case PhoneNumberFormat.Dashes:
							result += string.Format("{0}-{1}", rawPhoneNumber.Substring(0, 3), rawPhoneNumber.Substring(3, 4));
							break;
						case PhoneNumberFormat.Periods:
							result += string.Format("{0}.{1}", rawPhoneNumber.Substring(0, 3), rawPhoneNumber.Substring(3, 4));
							break;
						case PhoneNumberFormat.e164:
							result += rawPhoneNumber.Substring(0, 7);
							break;
					}
					rawPhoneNumber = rawPhoneNumber.Remove(0, 7);
				}
				#endregion

				#region Extension
				if ((phoneNumberFormat != PhoneNumberFormat.e164) && (rawPhoneNumber.Length > 0))
				{
					result += string.Format(" x{0}", rawPhoneNumber);
				}
				#endregion
			}
			else
			{
				result = value;
			}

			return result;
		}
		#endregion

		public static string SipAddressTo164(string sipAddress)
		{
			sipAddress ??= string.Empty;

			return Formatted(sipAddress.Split(new[] {":", "@"}, StringSplitOptions.RemoveEmptyEntries)[1], PhoneNumberFormat.e164);
		}
	}
}
