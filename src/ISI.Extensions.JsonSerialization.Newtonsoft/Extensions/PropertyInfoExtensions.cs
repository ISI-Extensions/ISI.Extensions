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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.JsonSerialization.Newtonsoft.Extensions
{
	internal static class PropertyInfoExtensions
	{
		private static  HashSet<Type> _ignorePropertyInfoAttributeTypes = null;
		private static HashSet<Type> IgnorePropertyInfoAttributeTypes => _ignorePropertyInfoAttributeTypes ??= GetIgnorePropertyInfoAttributeTypes();

		private static HashSet<Type> GetIgnorePropertyInfoAttributeTypes()
		{
			var ignorePropertyInfoAttributeTypes = new HashSet<Type>();

			foreach (var ignorePropertyInfoAttributeType in new [] { Type.GetType("System.Text.Json.Serialization.JsonIgnoreAttribute, System.Text.Json"), typeof(global::Newtonsoft.Json.JsonIgnoreAttribute), typeof(System.Runtime.Serialization.IgnoreDataMemberAttribute) })
			{
				if (ignorePropertyInfoAttributeType != null)
				{
					ignorePropertyInfoAttributeTypes.Add(ignorePropertyInfoAttributeType);
				}
			}

			return ignorePropertyInfoAttributeTypes;
		}

		internal static bool IgnorePropertyInfo(this System.Reflection.PropertyInfo propertyInfo)
		{
			return propertyInfo.GetCustomAttributes(true).NullCheckedAny(attribute => IgnorePropertyInfoAttributeTypes.Contains(attribute.GetType()));
		}
	}
}
