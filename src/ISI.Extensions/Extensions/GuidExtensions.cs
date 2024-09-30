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

namespace ISI.Extensions.Extensions
{
	public static class GuidExtensions
	{
		private const string GuidBase36 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		public enum GuidFormat
		{
			NoFormatting = 0,
			WithHyphens = 1,
			WithHyphensAndBrackets = 2,
			WithHyphensAndParentheses = 3,
			Base36 = 4,
		}
		public static string Formatted(this Guid? value, GuidFormat format, string emptyValue = "", string nullValue = "")
		{
			return (value.HasValue ? Formatted(value.Value, format, emptyValue) : nullValue);
		}
		public static string Formatted(this Guid value, GuidFormat format, string emptyValue = "")
		{
			var formattedValue = emptyValue;

			if (value != Guid.Empty)
			{
				switch (format)
				{
					case GuidFormat.NoFormatting:
						formattedValue = value.ToString("N");
						break;

					case GuidFormat.WithHyphens:
						formattedValue = value.ToString("D");
						break;

					case GuidFormat.WithHyphensAndBrackets:
						formattedValue = value.ToString("B");
						break;

					case GuidFormat.WithHyphensAndParentheses:
						formattedValue = value.ToString("P");
						break;

					case GuidFormat.Base36:
						{
							formattedValue = string.Empty;

							var bytes = value.ToByteArray();

							var longValues = new[]
							{
								BitConverter.ToUInt64(bytes, 0),
								BitConverter.ToUInt64(bytes, 8)
							};

							for (var longValuesIndex = 0; longValuesIndex < longValues.Length; longValuesIndex++)
							{
								var longValue = longValues[longValuesIndex];
								var base36Formatted = string.Empty;

								while (longValue > 0)
								{
									var remainder = longValue % 36;
									base36Formatted = $"{GuidBase36[(int)remainder]}{base36Formatted}";
									longValue = (longValue - remainder) / 36;
								}

								formattedValue += base36Formatted.TextJustify(StringExtensions.TextJustifyAlignment.Right, 13, '0');
							}
						}
						break;
				}
			}

			return formattedValue;
		}

		private static bool TryParseGuidBase36(string value, out Guid result)
		{
			if (!string.IsNullOrWhiteSpace(value) && (value.Length == 26))
			{
				var base36Lookup = new Dictionary<char, ulong>();
				for (var base36Index = 0; base36Index < GuidBase36.Length; base36Index++)
				{
					base36Lookup.Add(GuidBase36[base36Index], (ulong)base36Index);
				}

				var longValues = new ulong[] { 0, 0, };

				var segments = new[]
				{
					value.Substring(0, 13),
					value.Substring(13, 13),
				};

				for (var segmentIndex = 0; segmentIndex < segments.Length; segmentIndex++)
				{
					var segment = segments[segmentIndex];
					for (var index = 0; index < segment.Length; index++)
					{
						if (base36Lookup.TryGetValue(segment[index], out var pieceValue))
						{
							longValues[segmentIndex] = (longValues[segmentIndex] * 36) + pieceValue;
						}
						else
						{
							return false;
						}
					}
				}

				var bytes = longValues.SelectMany(BitConverter.GetBytes).ToArray();

				result = new Guid(bytes);

				return true;
			}

			return false;
		}

		public static Guid ToGuid(this string value)
		{
			return ToGuid(value, Guid.Empty);
		}

		public static Guid ToGuid(this string value, Guid defaultValue)
		{
			if (!string.IsNullOrWhiteSpace(value) && (Guid.TryParse(value, out var parsedValue) || TryParseGuidBase36(value, out parsedValue)))
			{
				return parsedValue;
			}

			return defaultValue;
		}

		public static Guid? ToGuidNullable(this string value, Guid? defaultValue = null)
		{
			if (!string.IsNullOrWhiteSpace(value) && (Guid.TryParse(value, out var parsedValue) || TryParseGuidBase36(value, out parsedValue)))
			{
				return parsedValue;
			}

			return defaultValue;
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
