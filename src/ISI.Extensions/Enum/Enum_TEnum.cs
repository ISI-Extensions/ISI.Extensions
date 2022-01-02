#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	public partial class Enum<TEnum>
	{
		private static readonly List<EnumInformation> _EnumInformations = null;
		private static readonly Dictionary<TEnum, EnumInformation> _EnumInformationLookUp = null;
		private static readonly Dictionary<string, TEnum> _ValueLookup = null;
		private static readonly Dictionary<int, TEnum> _IndexLookup = null;
		private static readonly Dictionary<string, TEnum> _KeyLookup = null;
		private static readonly Dictionary<string, TEnum> _AbbreviationLookup = null;
		private static readonly Dictionary<string, TEnum> _DescriptionLookup = null;
		private static readonly Dictionary<Guid, TEnum> _UuidLookup = null;

		private static Dictionary<TEnum, System.Runtime.Serialization.EnumMemberAttribute> _EnumMemberLookUp = null;
		
		private static bool _IsNullable = false;

		static Enum()
		{
			try
			{
				var type = typeof(TEnum);

				_EnumInformations = new List<EnumInformation>();
				_EnumInformationLookUp = new Dictionary<TEnum, EnumInformation>();
				_ValueLookup = new Dictionary<string, TEnum>(StringComparer.InvariantCultureIgnoreCase);
				_IndexLookup = new Dictionary<int, TEnum>();
				_KeyLookup = new Dictionary<string, TEnum>(StringComparer.InvariantCultureIgnoreCase);
				_AbbreviationLookup = new Dictionary<string, TEnum>(StringComparer.InvariantCultureIgnoreCase);
				_DescriptionLookup = new Dictionary<string, TEnum>(StringComparer.InvariantCultureIgnoreCase);
				_UuidLookup = new Dictionary<Guid, TEnum>();

				_EnumMemberLookUp = new Dictionary<TEnum, System.Runtime.Serialization.EnumMemberAttribute>();

				_IsNullable = (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));

				if (_IsNullable)
				{
					type = (new System.ComponentModel.NullableConverter(type)).UnderlyingType;
				}
				
				if (!type.IsEnum)
				{
					throw new Exception(string.Format("Cannot create Enum<T> when T is not a Enum, T is \"{0}\"", type.AssemblyQualifiedName));
				}

				var knownAliases = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

				var index = 0;
				foreach (TEnum value in System.Enum.GetValues(type))
				{
					var enumInformation = new EnumInformation(index++, value);
					var enumAttributes = (EnumAttribute[])(type.GetField(value.ToString()).GetCustomAttributes(typeof(EnumAttribute), false));
					
					if (enumAttributes.Length > 0)
					{
						var enumAttribute = enumAttributes.First();

						if (!string.IsNullOrWhiteSpace(enumAttribute.Description))
						{
							enumInformation.DefaultDescription = enumAttribute.Description;
							enumInformation.WithSpaces = enumAttribute.Description;
						}

						if (!string.IsNullOrWhiteSpace(enumAttribute.Abbreviation))
						{
							enumInformation.Abbreviation = enumAttribute.Abbreviation;
						}

						enumInformation.Active = enumAttribute.Active;
						enumInformation.Order = enumAttribute.Order ?? enumInformation.Order;
						enumInformation.Aliases = enumAttribute.Aliases;

						if (!string.IsNullOrWhiteSpace(enumAttribute.Uuid) && Guid.TryParse(enumAttribute.Uuid, out var uuid))
						{
							enumInformation.Uuid = uuid;
						}
					}

					_EnumInformations.Add(enumInformation);
					_EnumInformationLookUp.Add(value, enumInformation);

					void AddToLookUpValuesIfNotKnownAlias(string key)
					{
						if (!string.IsNullOrEmpty(key) && !_ValueLookup.ContainsKey(key) && !knownAliases.Contains(key))
						{
							_ValueLookup.Add(key, value);
						}
					}

					void AddToLookUpValues(string key)
					{
						if (!string.IsNullOrEmpty(key) && !_ValueLookup.ContainsKey(key))
						{
							_ValueLookup.Add(key, value);
						}
					}

					AddToLookUpValuesIfNotKnownAlias(System.Convert.ToInt32(value).ToString());
					AddToLookUpValuesIfNotKnownAlias(enumInformation.Value.ToString());
					AddToLookUpValuesIfNotKnownAlias(enumInformation.Abbreviation ?? string.Empty);
					AddToLookUpValuesIfNotKnownAlias(enumInformation.DefaultDescription);
					AddToLookUpValuesIfNotKnownAlias(enumInformation.WithSpaces);

					_IndexLookup.Add(System.Convert.ToInt32(value), value);

					{
						var key = enumInformation.Value.ToString();
						if (!string.IsNullOrEmpty(key) && !_KeyLookup.ContainsKey(key))
						{
							_KeyLookup.Add(key, value);
						}
					}

					{
						var key = (enumInformation.Abbreviation ?? string.Empty);
						if (!string.IsNullOrEmpty(key) && !_AbbreviationLookup.ContainsKey(key))
						{
							_AbbreviationLookup.Add(key, value);
						}
					}

					{
						var key = enumInformation.DefaultDescription;
						if (!string.IsNullOrEmpty(key) && !_DescriptionLookup.ContainsKey(key))
						{
							_DescriptionLookup.Add(key, value);
						}
					}

					if (enumInformation.Aliases != null)
					{
						foreach (var alias in enumInformation.Aliases)
						{
							AddToLookUpValues(alias);
						}
					}

					if (enumInformation.Uuid.HasValue)
					{
						_UuidLookup.Add(enumInformation.Uuid.Value, value);

						foreach (GuidExtensions.GuidFormat guidFormat in System.Enum.GetValues(typeof(GuidExtensions.GuidFormat)))
						{
							AddToLookUpValues(enumInformation.Uuid.Formatted(guidFormat));
						}
					}

					AddToLookUpValues(enumInformation.DefaultDescription);
					AddToLookUpValues(enumInformation.Abbreviation ?? string.Empty);

					enumInformation.SerializationValue = enumInformation.Value.ToString();

					var enumMemberAttributes = (System.Runtime.Serialization.EnumMemberAttribute[])(type.GetField(value.ToString()).GetCustomAttributes(typeof(System.Runtime.Serialization.EnumMemberAttribute), false));

					if (enumMemberAttributes.Length > 0)
					{
						var enumMemberAttribute = enumMemberAttributes.First();

						_EnumMemberLookUp.Add(value, enumMemberAttribute);

						if (!string.IsNullOrEmpty(enumMemberAttribute.Value))
						{
							enumInformation.SerializationValue = enumMemberAttribute.Value;

							AddToLookUpValuesIfNotKnownAlias(enumInformation.SerializationValue);
						}
					}
				}

				_EnumInformations.Sort((x, y) => x.Order.CompareTo(y.Order));

			}
			catch (Exception exception)
			{
#if DEBUG
				if (string.Equals(System.Environment.MachineName, "ronmuth", StringComparison.InvariantCultureIgnoreCase))
				{
					System.IO.File.AppendAllText(@"C:\Temp\ISI.Extensions.Enum.Error.txt", exception.ErrorMessageFormatted());

					Console.WriteLine(exception);
				}
#endif
				throw;
			}
		}
	}
}