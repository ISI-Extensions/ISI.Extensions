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
	public static class StringExtensions
	{
		public static bool Equals(this IEnumerable<string> x, IEnumerable<string> y, StringComparer stringComparer, bool resortFirst)
		{
			if (x?.Count() != y?.Count())
			{
				return false;
			}

			if (resortFirst)
			{
				x = x.ToList();
				((List<string>)x).Sort(stringComparer);

				y = y.ToList();
				((List<string>)y).Sort(stringComparer);
			}

			x = x.ToArray();
			y = y.ToArray();

			var indexCount = x.Count();
			for (var index = 0; index < indexCount; index++)
			{
				if (!stringComparer.Equals(((string[]) x)[index], ((string[]) y)[index]))
				{
					return false;
				}
			}

			return true;
		}

		public static int IndexOf(this string source, string value, int startIndex, IEqualityComparer<string> stringComparer = null)
		{
			stringComparer ??= System.StringComparer.InvariantCulture;

			var valueLength = value.Length;
			var sourceLength = source.Length - valueLength + 1;

			while (startIndex < sourceLength)
			{
				if (stringComparer.Equals(source.Substring(startIndex, valueLength), value))
				{
					return startIndex;
				}
				startIndex++;
			}

			return -1;
		}

		public static string Replace(this string value, string oldValue, string newValue, IEqualityComparer<string> stringComparer = null)
		{
			var result = new StringBuilder();

			stringComparer ??= System.StringComparer.InvariantCulture;

			var foundIndex = value.IndexOf(oldValue, 0, stringComparer);
			if (foundIndex < 0)
			{
				result.Append(value);
			}
			else
			{
				var offset = 0;
				while (foundIndex >= 0)
				{
					var length = foundIndex - offset;
					if (length > 0)
					{
						result.Append(value.Substring(offset, length));
					}
					result.Append(newValue);
					offset = foundIndex + oldValue.Length;
					foundIndex = value.IndexOf(oldValue, offset, stringComparer);
				}
				if (offset < value.Length)
				{
					result.Append(value.Substring(offset));
				}
			}

			return result.ToString();
		}

		public static string Replace(this string value, IDictionary<string, string> replacementValues, IEqualityComparer<string> stringComparer = null)
		{
			var result = value;

			if (replacementValues != null)
			{
				if ((stringComparer == null) && (replacementValues is Dictionary<string, string>))
				{
					stringComparer = ((Dictionary<string, string>) replacementValues).Comparer;
				}
				stringComparer ??= System.StringComparer.InvariantCulture;

				if (!string.IsNullOrWhiteSpace(value) && (replacementValues != null) && replacementValues.Any())
				{
					foreach (var replacementValue in replacementValues)
					{
						result = result.Replace(replacementValue.Key, replacementValue.Value, stringComparer);
					}
				}
			}

			return result;
		}

		public static string TrimStart(this string value, string startsWith)
		{
			return value.TrimStart(startsWith, StringComparison.CurrentCulture);
		}

		public static string TrimStart(this string value, params string[] startsWithSet)
		{
			return value.TrimStart(startsWithSet, StringComparison.CurrentCulture);
		}

		public static string TrimStart(this string value, string startsWith, StringComparison stringComparison)
		{
			return value.TrimStart([startsWith], stringComparison);
		}

		public static string TrimStart(this string value, string[] startsWithSet, StringComparison stringComparison)
		{
			foreach (var startsWith in startsWithSet)
			{
				if (value.StartsWith(startsWith, stringComparison))
				{
					value = value.Substring(startsWith.Length);
				}
			}

			return value;
		}

		public static string TrimEnd(this string value, string endsWith)
		{
			return value.TrimEnd(endsWith, StringComparison.CurrentCulture);
		}

		public static string TrimEnd(this string value, params string[] endsWithSet)
		{
			return value.TrimEnd(endsWithSet, StringComparison.CurrentCulture);
		}

		public static string TrimEnd(this string value, string endsWith, StringComparison stringComparison)
		{
			return value.TrimEnd([endsWith], stringComparison);
		}

		public static string TrimEnd(this string value, string[] endsWithSet, StringComparison stringComparison)
		{
			foreach (var endsWith in endsWithSet)
			{
				if (value.EndsWith(endsWith, stringComparison))
				{
					value = value.Substring(0, value.Length - endsWith.Length);
				}
			}

			return value;
		}













		public static string NullCheckedTrim(this string value, char trimChar, NullCheckResult nullCheckResult)
		{
			return value.NullCheckedTrim([trimChar], NullCheckResult.ReturnDefault);
		}

		public static string NullCheckedTrim(this string value, params char[] trimChars)
		{
			return value.NullCheckedTrim(trimChars, NullCheckResult.ReturnDefault);
		}

		public static string NullCheckedTrim(this string value, char[] trimChars, NullCheckResult nullCheckResult)
		{
			if (value == null)
			{
				switch (nullCheckResult)
				{
					case NullCheckResult.ReturnDefault:
						return string.Empty;
					case NullCheckResult.ReturnNull:
						return null;
					default:
						throw new ArgumentOutOfRangeException(nameof(nullCheckResult), nullCheckResult, null);
				}
			}

			return value.Trim(trimChars);
		}

		public static string NullCheckedTrimStart(this string value, char trimChar, NullCheckResult nullCheckResult)
		{
			return value.NullCheckedTrimStart([trimChar], NullCheckResult.ReturnDefault);
		}

		public static string NullCheckedTrimStart(this string value, params char[] trimChars)
		{
			return value.NullCheckedTrimStart(trimChars, NullCheckResult.ReturnDefault);
		}

		public static string NullCheckedTrimStart(this string value, char[] trimChars, NullCheckResult nullCheckResult)
		{
			if (value == null)
			{
				switch (nullCheckResult)
				{
					case NullCheckResult.ReturnDefault:
						return string.Empty;
					case NullCheckResult.ReturnNull:
						return null;
					default:
						throw new ArgumentOutOfRangeException(nameof(nullCheckResult), nullCheckResult, null);
				}
			}

			return value.TrimStart(trimChars);
		}

		public static string NullCheckedTrimStart(this string value, string startsWith)
		{
			return value.NullCheckedTrimStart(startsWith, StringComparison.CurrentCulture);
		}

		public static string NullCheckedTrimStart(this string value, params string[] startsWiths)
		{
			return value.NullCheckedTrimStart(startsWiths, StringComparison.CurrentCulture);
		}

		public static string NullCheckedTrimStart(this string value, string startsWith, StringComparison stringComparison)
		{
			return value.NullCheckedTrimStart([startsWith], stringComparison);
		}

		public static string NullCheckedTrimStart(this string value, string[] startsWiths, StringComparison stringComparison)
		{
			foreach (var startsWith in startsWiths)
			{
				if (value.StartsWith(startsWith, stringComparison))
				{
					value = value.Substring(startsWith.Length);
				}
			}

			return value;
		}

		public static string NullCheckedTrimEnd(this string value, char trimChar, NullCheckResult nullCheckResult)
		{
			return value.NullCheckedTrimEnd([trimChar], NullCheckResult.ReturnDefault);
		}

		public static string NullCheckedTrimEnd(this string value, params char[] trimChars)
		{
			return value.NullCheckedTrimEnd(trimChars, NullCheckResult.ReturnDefault);
		}

		public static string NullCheckedTrimEnd(this string value, char[] trimChars, NullCheckResult nullCheckResult)
		{
			if (value == null)
			{
				switch (nullCheckResult)
				{
					case NullCheckResult.ReturnDefault:
						return string.Empty;
					case NullCheckResult.ReturnNull:
						return null;
					default:
						throw new ArgumentOutOfRangeException(nameof(nullCheckResult), nullCheckResult, null);
				}
			}

			return value.TrimEnd(trimChars);
		}

		public static string NullCheckedTrimEnd(this string value, string endsWith)
		{
			return value.NullCheckedTrimEnd(endsWith, StringComparison.CurrentCulture);
		}

		public static string NullCheckedTrimEnd(this string value, params string[] endsWiths)
		{
			return value.NullCheckedTrimEnd(endsWiths, StringComparison.CurrentCulture);
		}

		public static string NullCheckedTrimEnd(this string value, string endsWith, StringComparison stringComparison)
		{
			return value.NullCheckedTrimEnd([endsWith], stringComparison);
		}

		public static string NullCheckedTrimEnd(this string value, string[] endsWiths, StringComparison stringComparison)
		{
			foreach (var endsWith in endsWiths)
			{
				if (value.EndsWith(endsWith, stringComparison))
				{
					value = value.Substring(0, value.Length - endsWith.Length);
				}
			}

			return value;
		}




		public static string NullCheckedTrimmedUpper(this string value, params char[] trimChars)
		{
			return value.NullCheckedTrim(trimChars, NullCheckResult.ReturnDefault).ToUpper();
		}
		public static string NullCheckedTrimmedUpper(this string value, char[] trimChars, NullCheckResult nullCheckResult)
		{
			return value.NullCheckedTrim(trimChars, nullCheckResult)?.ToUpper();
		}


		public static string NullCheckedTrimmedLower(this string value, params char[] trimChars)
		{
			return value.NullCheckedTrim(trimChars, NullCheckResult.ReturnDefault).ToLower();
		}
		public static string NullCheckedTrimmedLower(this string value, char[] trimChars, NullCheckResult nullCheckResult)
		{
			return value.NullCheckedTrim(trimChars, nullCheckResult)?.ToLower();
		}



		public static IEnumerable<string> SplitIntoChunks(this string value, int maxChunkSize, int maxChunks = int.MaxValue, bool appendRemainder = false)
		{
			var chunkIndex = 0;

			var valueLength = value.Length;

			for (var offset = 0; offset < valueLength; offset += maxChunkSize)
			{
				if (chunkIndex++ < maxChunks)
				{
					yield return value.Substring(offset, Math.Min(maxChunkSize, valueLength - offset));
				}
				else
				{
					valueLength = 0;

					if (appendRemainder)
					{
						yield return value.Substring(offset);
					}
				}
			}
		}

		public static bool NullCheckedStartsWith(this string value, string searchFor)
		{
			return (value ?? string.Empty).StartsWith(searchFor);
		}
		public static bool NullCheckedStartsWith(this string value, string searchFor, StringComparison stringComparison)
		{
			return (value ?? string.Empty).StartsWith(searchFor, stringComparison);
		}
		public static bool NullCheckedStartsWith(this string value, string searchFor, bool ignoreCase, System.Globalization.CultureInfo cultureInfo)
		{
			return (value ?? string.Empty).StartsWith(searchFor, ignoreCase, cultureInfo);
		}

		public static bool NullCheckedEndsWith(this string value, string searchFor)
		{
			return (value ?? string.Empty).EndsWith(searchFor);
		}
		public static bool NullCheckedEndsWith(this string value, string searchFor, StringComparison stringComparison)
		{
			return (value ?? string.Empty).EndsWith(searchFor, stringComparison);
		}
		public static bool NullCheckedEndsWith(this string value, string searchFor, bool ignoreCase, System.Globalization.CultureInfo cultureInfo)
		{
			return (value ?? string.Empty).EndsWith(searchFor, ignoreCase, cultureInfo);
		}


		public static byte[] ToBytes(this string value) => value.ToBytes(Encoding.UTF8);
		public static byte[] ToBytes(this string value, Encoding encoding) => encoding.GetBytes(value);
		public static byte[][] ToBytes(this IEnumerable<string> values) => values.ToBytes(Encoding.UTF8);
		public static byte[][] ToBytes(this IEnumerable<string> values, Encoding encoding) => values.ToNullCheckedArray(value => value.ToBytes(encoding));

		public static string ToString(this IEnumerable<byte> value) => value.ToString(Encoding.UTF8);
		public static string ToString(this IEnumerable<byte> value, Encoding encoding) => encoding.GetString(value.ToNullCheckedArray(NullCheckCollectionResult.Empty));
		public static string[] ToStrings(this IEnumerable<IEnumerable<byte>> values) => values.ToStrings(Encoding.UTF8);
		public static string[] ToStrings(this IEnumerable<IEnumerable<byte>> values, Encoding encoding) => values.ToNullCheckedArray(value => value.ToString(encoding));


		public static string RemoveTitleCase(this string value, string separator = " ", string deliminator = null)
		{
			var response = System.Text.RegularExpressions.Regex.Replace(value, @"(?<begin>(\w*?))(?<end>[A-Z]+)", string.Format(@"${{begin}}{0}${{end}}", separator)).Trim();

			if (!string.IsNullOrEmpty(deliminator))
			{
				response = response.Replace(deliminator, string.Empty);
			}

			return response;
		}

		public enum TextJustifyAlignment
		{
			Center,
			Left,
			Right
		}

		public static string TextJustify(this string value, TextJustifyAlignment justify, int length, char fill = ' ')
		{
			var result = new StringBuilder();

			if (value != null) result.Append(value);

			#region Shorten if necessary
			if (result.Length > length)
			{
				switch (justify)
				{
					case TextJustifyAlignment.Center:
						break;
					case TextJustifyAlignment.Left:
						result.Remove(length, result.Length - length);
						break;
					case TextJustifyAlignment.Right:
						result.Length = length;
						break;
				}
			}
			#endregion

			if (result.Length < length)
			{
				switch (justify)
				{
					case TextJustifyAlignment.Center:
						result.Insert(0, string.Format("{0}", fill), (length - result.Length) / 2);
						if (result.Length < length) result.Append(fill, length - result.Length);
						break;
					case TextJustifyAlignment.Left:
						result.Append(fill, length - result.Length);
						break;
					case TextJustifyAlignment.Right:
						result.Insert(0, string.Format("{0}", fill), length - result.Length);
						break;
				}
			}

			return result.ToString();
		}

		public static string ToCamelCase(this string value)
		{
			value = (value ?? string.Empty).Replace(" ", string.Empty);

			return ((string.IsNullOrEmpty(value) || (value.Length < 2)) ? value : char.ToLowerInvariant(value[0]) + value.Substring(1));
		}

		public static UriBuilder ToUriBuilder(this string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return null;
			}

			return new(url);
		}

		public static Uri ToUri(this string url)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return null;
			}

			return new UriBuilder(url).Uri;
		}
	}
}
