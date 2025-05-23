﻿#region Copyright & License
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

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static class SelectListExtensions
	{
		public static Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] ToSelectListItems<TEnum>(this TEnum selectedValue, ISI.Extensions.Enum.ValueSource keySource = Enum.ValueSource.Key, ISI.Extensions.Enum.ValueSource valueSource = Enum.ValueSource.Description)
			where TEnum : System.Enum
		{
			return ISI.Extensions.Enum<TEnum>.ToArray().ToNullCheckedArray(value => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(value.GetValueSource(valueSource), value.GetValueSource(keySource), value.Equals(selectedValue)));
		}

		public static Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] ToSelectListItems<TEnum>(this IEnumerable<TEnum> selectedValues, ISI.Extensions.Enum.ValueSource keySource = Enum.ValueSource.Key, ISI.Extensions.Enum.ValueSource valueSource = Enum.ValueSource.Description)
			where TEnum : System.Enum
		{
			var selected = new HashSet<TEnum>(selectedValues ?? []);

			return ISI.Extensions.Enum<TEnum>.ToArray().ToNullCheckedArray(value => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(value.GetValueSource(valueSource), value.GetValueSource(keySource), selected.Contains(value)));
		}

		public static Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] ToSelectListItems<TEnum>(this string selectedValue, ISI.Extensions.Enum.ValueSource keySource = Enum.ValueSource.Key, ISI.Extensions.Enum.ValueSource valueSource = Enum.ValueSource.Description)
			where TEnum : System.Enum
		{
			var selected = ISI.Extensions.Enum<TEnum>.Parse(selectedValue);

			return ISI.Extensions.Enum<TEnum>.ToArray().ToNullCheckedArray(value => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(value.GetValueSource(valueSource), value.GetValueSource(keySource), value.Equals(selected)));
		}

		public static Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] ToSelectListItems<TEnum>(this IEnumerable<string> selectedValues, ISI.Extensions.Enum.ValueSource keySource = Enum.ValueSource.Key, ISI.Extensions.Enum.ValueSource valueSource = Enum.ValueSource.Description)
			where TEnum : System.Enum
		{
			var selected = new HashSet<TEnum>(selectedValues.ToNullCheckedArray(selectedValue => ISI.Extensions.Enum<TEnum>.Parse(selectedValue)));

			return ISI.Extensions.Enum<TEnum>.ToArray().ToNullCheckedArray(value => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(value.GetValueSource(valueSource), value.GetValueSource(keySource), selected.Contains(value)));
		}

		public static Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] ToSelectListItems<TEnum>(this TEnum? selectedValue, ISI.Extensions.Enum.ValueSource keySource = Enum.ValueSource.Key, ISI.Extensions.Enum.ValueSource valueSource = Enum.ValueSource.Description)
			where TEnum : struct, System.Enum
		{
			return ISI.Extensions.Enum<TEnum>.ToArray().ToNullCheckedArray(value => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(value.GetValueSource(valueSource), value.GetValueSource(keySource), value.Equals(selectedValue)));
		}

		public static Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] ToSelectListItems<TValue>(this IEnumerable<TValue> values, Func<TValue, string> keySource, Func<TValue, string> valueSource, string selectedValue = null)
		{
			return values.ToNullCheckedArray(value => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(valueSource(value), keySource(value), (keySource(value) ?? string.Empty).Equals(selectedValue ?? string.Empty)));
		}

		public static Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] ToSelectListItems(this IEnumerable<string> values, string selectedValue = null)
		{
			return values.ToNullCheckedArray(value => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(value, value, (value ?? string.Empty).Equals(selectedValue ?? string.Empty)));
		}

		public static Microsoft.AspNetCore.Mvc.Rendering.SelectListItem[] ToSelectListItems(this IEnumerable<string> values, IEnumerable<string> selectedValues, StringComparer stringComparer = null)
		{
			var selected = new HashSet<string>(selectedValues ?? [], stringComparer ?? StringComparer.InvariantCultureIgnoreCase);

			return values.ToNullCheckedArray(value => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem(value, value,selected.Contains(value ?? string.Empty)));
		}
	}
}
