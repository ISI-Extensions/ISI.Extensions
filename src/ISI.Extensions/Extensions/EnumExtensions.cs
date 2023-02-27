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
	public static class EnumExtensions
	{
		public static int? GetIndex<TEnum>(this TEnum value)
			where TEnum : System.Enum
		{
			return ISI.Extensions.Enum<TEnum>.GetIndex(value);
		}

		public static int? GetIndex<TEnum>(this TEnum? value)
			where TEnum : struct, System.Enum
		{
			return ISI.Extensions.Enum<TEnum?>.GetIndex(value);
		}

		public static string GetKey<TEnum>(this TEnum value)
			where TEnum : System.Enum
		{
			return ISI.Extensions.Enum<TEnum>.GetKey(value);
		}

		public static string GetKey<TEnum>(this TEnum? value)
			where TEnum : struct, System.Enum
		{
			return ISI.Extensions.Enum<TEnum?>.GetKey(value);
		}

		public static string GetAbbreviation<TEnum>(this TEnum value)
			where TEnum : System.Enum
		{
			return ISI.Extensions.Enum<TEnum>.GetAbbreviation(value);
		}

		public static string GetAbbreviation<TEnum>(this TEnum? value)
			where TEnum : struct, System.Enum
		{
			return ISI.Extensions.Enum<TEnum?>.GetAbbreviation(value);
		}

		public static string GetDescription<TEnum>(this TEnum value, bool addSpaceBetweenWords = true)
			where TEnum : System.Enum
		{
			return ISI.Extensions.Enum<TEnum>.GetDescription(value, addSpaceBetweenWords);
		}

		public static string GetDescription<TEnum>(this TEnum? value, bool addSpaceBetweenWords = true)
			where TEnum : struct, System.Enum
		{
			return ISI.Extensions.Enum<TEnum?>.GetDescription(value, addSpaceBetweenWords);
		}

		public static Guid GetUuid<TEnum>(this TEnum value)
			where TEnum : System.Enum
		{
			return ISI.Extensions.Enum<TEnum>.GetUuid(value).GetValueOrDefault();
		}
		
		public static Guid? GetUuid<TEnum>(this TEnum? value)
			where TEnum : struct, System.Enum
		{
			return ISI.Extensions.Enum<TEnum?>.GetUuid(value);
		}

		public static string GetValueSource<TEnum>(this TEnum value, Enum.ValueSource valueSource)
			where TEnum : System.Enum
		{
			return ISI.Extensions.Enum<TEnum>.GetValueSource(value, valueSource);
		}
		
		public static string GetValueSource<TEnum>(this TEnum? value, Enum.ValueSource valueSource)
			where TEnum : struct, System.Enum
		{
			return ISI.Extensions.Enum<TEnum?>.GetValueSource(value, valueSource);
		}
		
		public static TEnumTo Convert<TEnumFrom, TEnumTo>(this TEnumFrom value, TEnumTo defaultValue = default)
			where TEnumFrom : System.Enum
			where TEnumTo : System.Enum
		{
			return ISI.Extensions.Enum<TEnumTo>.Convert(value, defaultValue);
		}

		public static TEnumTo? Convert<TEnumFrom, TEnumTo>(this TEnumFrom? value, TEnumTo? defaultValue = null)
			where TEnumFrom : struct, System.Enum
			where TEnumTo : struct, System.Enum
		{
			return ISI.Extensions.Enum<TEnumTo?>.Convert(value, defaultValue);
		}

		public static bool TryConvert<TEnumFrom, TEnumTo>(this TEnumFrom value, out TEnumTo @enum)
			where TEnumFrom : System.Enum
			where TEnumTo : System.Enum
		{
			return ISI.Extensions.Enum<TEnumTo>.TryConvert(value, out @enum);
		}

		public static bool TryConvert<TEnumFrom, TEnumTo>(this TEnumFrom? value, out TEnumTo? @enum)
			where TEnumFrom : struct, System.Enum
			where TEnumTo : struct, System.Enum
		{
			return ISI.Extensions.Enum<TEnumTo?>.TryConvert(value, out @enum);
		}
	}
}
