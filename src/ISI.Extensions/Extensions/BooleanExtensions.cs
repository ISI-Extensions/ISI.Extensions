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
using System.Text;

namespace ISI.Extensions.Extensions
{
	public static class BooleanExtensions
	{
		public static bool ToBoolean(this string value, bool defaultValue = false)
		{
			switch ((value ?? string.Empty).ToLower().Trim())
			{
				case "on":
				case "t":
				case "true":
				case "yes":
				case "y":
					return true;

				case "off":
				case "f":
				case "false":
				case "no":
				case "n":
				case "0":
					return false;
			}

			var nullableBoolean = value.ToIntNullable();
			if (nullableBoolean.HasValue)
			{
				return nullableBoolean.Value != 0;
			}

			return defaultValue;
		}

		public static bool ToBoolean(this int value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(this long value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(this decimal value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(this double value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(this float value)
		{
			return (value != 0);
		}

		public static bool? ToBooleanNullable(this string value)
		{
			switch ((value ?? string.Empty).Trim().ToLower())
			{
				case "on":
				case "t":
				case "true":
				case "yes":
				case "y":
				case "1":
				case "-1":
					return true;

				case "off":
				case "f":
				case "false":
				case "no":
				case "n":
				case "0":
					return false;
			}

			return null;
		}

		private static object _boolDescriptionLock = new();
		private static IDictionary<Type, IBoolDescription> _boolDescriptions = new Dictionary<Type, IBoolDescription>();

		public interface IBoolDescription
		{
			string False { get; }
			string True { get; }
			string Null { get; }
		}

		private class TrueFalseNotKnownDescription : IBoolDescription
		{
			public string False => "False";
			public string True => "True";
			public string Null => "NotKnown";
		}

		private class TrueFalseDescription : IBoolDescription
		{
			public string False => "False";
			public string True => "True";
			public string Null => string.Empty;
		}

		private class YesNoDescription : IBoolDescription
		{
			public string False => "No";
			public string True => "Yes";
			public string Null => string.Empty;
		}

		private class OnOffDescription : IBoolDescription
		{
			public string False => "Off";
			public string True => "On";
			public string Null => string.Empty;
		}

		private class ZeroOneDescription : IBoolDescription
		{
			public string False => "0";
			public string True => "1";
			public string Null => string.Empty;
		}

		public enum TextCase
		{
			Title = 0,
			Upper = 1,
			Lower = 2
		}

		#region Boolean
		public static string Boolean<TBoolDescription>(this bool value, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
			where TBoolDescription : IBoolDescription, new()
		{
			return ((bool?)value).Boolean<TBoolDescription>(firstCharacterOnly, textCase);
		}

		public static string Boolean<TBoolDescription>(this bool? value, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
			where TBoolDescription : IBoolDescription, new()
		{
			var descriptionType = typeof(TBoolDescription);

			if (!_boolDescriptions.ContainsKey(descriptionType))
			{
				lock (_boolDescriptionLock)
				{
					if (!_boolDescriptions.ContainsKey(descriptionType))
					{
						_boolDescriptions.Add(descriptionType, new TBoolDescription());
					}
				}
			}

			var descriptions = _boolDescriptions[descriptionType];

			return ((bool?)value).Boolean(descriptions.False, descriptions.True, descriptions.Null, firstCharacterOnly, textCase);
		}

		public static string Boolean(this bool value, IBoolDescription descriptions, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return ((bool?)value).Boolean(descriptions.False, descriptions.True, string.Empty, firstCharacterOnly, textCase);
		}

		public static string Boolean(this bool? value, IBoolDescription descriptions, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return value.Boolean(descriptions.False, descriptions.True, descriptions.Null, firstCharacterOnly, textCase);
		}

		public static string Boolean(this bool value, string falseDescription, string trueDescription, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return ((bool?)value).Boolean(falseDescription, trueDescription, string.Empty, firstCharacterOnly, textCase);
		}

		public static string Boolean(this bool? value, string falseDescription, string trueDescription, string nullDescription, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			var result = (value.HasValue ? (value.GetValueOrDefault() ? trueDescription : falseDescription) : nullDescription);

			switch (textCase)
			{
				case TextCase.Upper:
					result = result.ToUpper();
					break;
				case TextCase.Lower:
					result = result.ToLower();
					break;
			}

			if (firstCharacterOnly && (result.Length > 1))
			{
				result = result.Substring(0, 1);
			}

			return result;
		}
		#endregion

		#region TrueFalse
		public static string TrueFalseNotKnown(this bool? value, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return value.Boolean<TrueFalseNotKnownDescription>(firstCharacterOnly, textCase);
		}
		#endregion

		#region TrueFalse
		public static string TrueFalse(this bool? value, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return value.Boolean<TrueFalseDescription>(firstCharacterOnly, textCase);
		}

		public static string TrueFalse(this bool value, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return value.Boolean<TrueFalseDescription>(firstCharacterOnly, textCase);
		}
		#endregion

		#region YesNo
		public static string YesNo(this bool? value, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return value.Boolean<YesNoDescription>(firstCharacterOnly, textCase);
		}

		public static string YesNo(this bool value, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return value.Boolean<YesNoDescription>(firstCharacterOnly, textCase);
		}
		#endregion

		#region OnOff
		public static string OnOff(this bool? value, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return value.Boolean<OnOffDescription>(firstCharacterOnly, textCase);
		}

		public static string OnOff(this bool value, bool firstCharacterOnly = false, TextCase textCase = TextCase.Title)
		{
			return value.Boolean<OnOffDescription>(firstCharacterOnly, textCase);
		}
		#endregion

		#region ZeroOne
		public static string ZeroOne(this bool? value)
		{
			return value.Boolean<ZeroOneDescription>(false, TextCase.Title);
		}

		public static string ZeroOne(this bool value)
		{
			return value.Boolean<ZeroOneDescription>(false, TextCase.Title);
		}
		#endregion
	}
}
