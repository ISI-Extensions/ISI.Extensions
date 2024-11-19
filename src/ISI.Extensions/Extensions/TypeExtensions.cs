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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Extensions
{
	public static class TypeExtensions
	{
		public static bool Implements<TValue>(this object @object)
		{
			return @object.Implements(typeof(TValue));
		}

		public static bool Implements(this object @object, Type interfaceType)
		{
			return @object.GetType().Implements(interfaceType);
		}

		public static bool Implements<TValue>(this Type objectType)
		{
			return objectType.Implements(typeof(TValue));
		}

		public static bool Implements(this Type objectType, Type interfaceType)
		{
			if (interfaceType.IsGenericTypeDefinition)
			{
				return objectType.ImplementsGeneric(interfaceType);
			}

			return interfaceType.IsAssignableFrom(objectType);
		}

		public static bool ImplementsGeneric(this Type objectType, Type interfaceType)
		{
			return objectType.ImplementsGeneric(interfaceType, out Type matchedType);
		}

		public static bool ImplementsGeneric(this Type objectType, Type interfaceType, out Type matchedType)
		{
			matchedType = null;

			if (interfaceType.IsInterface)
			{
				matchedType = objectType.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType);

				if (matchedType != null)
				{
					return true;
				}
			}

			if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == interfaceType)
			{
				matchedType = objectType;
				return true;
			}

			var baseType = objectType.BaseType;
			if (baseType == null)
			{
				return false;
			}

			return baseType.ImplementsGeneric(interfaceType, out matchedType);
		}

		public static string AssemblyQualifiedNameWithoutVersion(this Type type, bool stripVersion = true, bool stripCulture = true, bool stripPublicKeyToken = true)
		{
			if (type != null)
			{
				return StripQualifiersFromTypeName(type.AssemblyQualifiedName, stripVersion, stripCulture, stripPublicKeyToken);
			}

			return null;
		}

		public static string StripQualifiersFromTypeName(string typeName, bool stripVersion = true, bool stripCulture = true, bool stripPublicKeyToken = true)
		{
			const string versionToken = ", Version=";
			const string cultureToken = ", Culture=";
			const string publicKeyToken = ", PublicKeyToken=";

			var tokenValues = new System.Collections.Specialized.NameValueCollection();

			typeName = string.Format("[{0}]", typeName.Trim('[', ']'));

			foreach (var token in new[] { versionToken, cultureToken, publicKeyToken })
			{
				var index = typeName.IndexOf(token, StringComparison.InvariantCulture);
				while (index > 0)
				{
					var lastIndex = typeName.IndexOfAny([']', ','], index + 2);

					var tokenValue = typeName.Substring(index, lastIndex - index);

					tokenValues[token] = tokenValue;

					typeName = typeName.Replace(tokenValue, string.Empty);

					index = typeName.IndexOf(token, StringComparison.InvariantCulture);
				}
			}

			typeName = typeName.TrimStart('[').TrimEnd(']');

			if (!stripVersion)
			{
				typeName += tokenValues[versionToken];
			}
			if (!stripCulture)
			{
				typeName += tokenValues[cultureToken];
			}
			if (!stripPublicKeyToken)
			{
				typeName += tokenValues[publicKeyToken];
			}

			return typeName;
		}
	}
}
