#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Extensions
{
	public static class GuidExtensions
	{
		public enum GuidFormat
		{
			NoFormatting = 0,
			WithHyphens = 1,
			WithHyphensAndBrackets = 2,
			WithHyphensAndParentheses = 3,
			Base36 = 4
		}
		public static string Formatted(this Guid? value, GuidFormat format, string emptyValue = "", string nullValue = "")
		{
			return (value.HasValue ? Formatted(value.Value, format, emptyValue) : nullValue);
		}
		public static string Formatted(this Guid value, GuidFormat format, string emptyValue = "")
		{
			const string base36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

			var result = emptyValue;

			if (value != Guid.Empty) 
			{
				switch (format)
				{
					case GuidFormat.NoFormatting:
						result = value.ToString("N");
						break;
					
					case GuidFormat.WithHyphens:
						result = value.ToString("D");
						break;
					
					case GuidFormat.WithHyphensAndBrackets:
						result = value.ToString("B");
						break;
					
					case GuidFormat.WithHyphensAndParentheses:
						result = value.ToString("P");
						break;

					case GuidFormat.Base36:
						result = string.Empty;

						var bytes = value.ToByteArray();

						var byteValues = new[]
						{
							BitConverter.ToInt64(bytes, 0),
							BitConverter.ToInt64(bytes, 8)
						};

						for (var byteValuesIndex = 0; byteValuesIndex < byteValues.Length; byteValuesIndex++)
						{
							var byteValue = byteValues[byteValuesIndex];
							var byteBaseFormatted = string.Empty;

							while (byteValue > 0)
							{
								var remainder = byteValue % 36;
								byteBaseFormatted += base36[(int) remainder];
								byteValue = (byteValue - remainder) / 36;
							}

							result += byteBaseFormatted.TextJustify(StringExtensions.TextJustifyAlignment.Right, 11, '0');
						}

						break;
				}
			}

			return result;
		}

		public static Guid ToGuid(this string value)
		{
			return ToGuid(value, Guid.Empty);
		}

		public static Guid ToGuid(this string value, Guid defaultValue)
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}
			
			try
			{
				return new Guid(value ?? string.Empty);
			}
			catch
			{
				return defaultValue;
			}
		}

		public static Guid? ToGuidNullable(this string value, Guid? defaultValue = null)
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}
			
			try
			{
				return new Guid(value ?? string.Empty);
			}
			catch
			{
				return defaultValue;
			}
		}

		public static bool IsNullOrEmpty(this Guid? value)
		{
			if (value.HasValue && (value.Value != Guid.Empty))
			{
				return false;
			}

			return true;
		}
	}
}
