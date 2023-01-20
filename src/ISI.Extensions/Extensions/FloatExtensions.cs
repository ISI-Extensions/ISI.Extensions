#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
	public static class FloatExtensions
	{
		public static float ToFloat(this string value, float defaultValue = 0)
		{
			var result = defaultValue;
			var isNegative = false;
			var isPercent = false;

			if (value == null)
			{
				value = string.Empty;
			}

			value = value.Replace(" ", string.Empty);
			value = value.Replace(",", string.Empty);

			while (value.StartsWith("$"))
			{
				value = value.Substring(1);
			}

			if (value.StartsWith("("))
			{
				isNegative = true;
				while (value.StartsWith("("))
				{
					value = value.Substring(1);
				}

				while (value.EndsWith(")"))
				{
					value = value.Substring(0, value.Length - 1);
				}
			}

			while (value.StartsWith("$"))
			{
				value = value.Substring(1);
			}

			if (value.StartsWith("-"))
			{
				isNegative = true;
				while (value.StartsWith("-"))
				{
					value = value.Substring(1);
				}
			}

			if (value.StartsWith("%") || value.EndsWith("%"))
			{
				isPercent = true;
				while (value.StartsWith("%"))
				{
					value = value.Substring(1);
				}

				while (value.EndsWith("%"))
				{
					value = value.Substring(0, value.Length - 1);
				}
			}

			if (!float.TryParse(value, out result))
			{
				if (!float.TryParse(value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.CurrentCulture, out result))
				{
					result = defaultValue;
				}
			}

			return result * (isNegative ? -1 : 1) / (isPercent ? 100 : 1);
		}

		public static float? ToFloatNullable(this string value, float? defaultValue = null)
		{
			if (float.TryParse(value ?? string.Empty, out var parsedValue))
			{
				return parsedValue;
			}

			return defaultValue;
		}
	}
}
