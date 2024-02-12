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
using ISI.Extensions.JsonSerialization.Extensions;

namespace ISI.Extensions.JsonJwt.Extensions
{
	public static class JwtExtensions
	{
		public static void AddToPayload<TValue>(this Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor securityTokenDescriptor, TValue value)
			where TValue : class, new()
		{
			var columns = ISI.Extensions.Columns.ColumnCollection<TValue>.GetDefault();

			foreach (var column in columns)
			{
				var columnValue = column.GetValue(value);

				if (columnValue != null)
				{
					if (column.PropertyType.IsPrimitive)
					{
						securityTokenDescriptor.Claims.Add(column.ColumnName, columnValue);
					}
					else if (column.PropertyType.IsGenericType && (column.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) && (new System.ComponentModel.NullableConverter(column.PropertyType)).UnderlyingType.IsPrimitive)
					{
						securityTokenDescriptor.Claims.Add(column.ColumnName, columnValue);
					}
					else if (columnValue is string stringValue)
					{
						if (!string.IsNullOrWhiteSpace(stringValue))
						{
							securityTokenDescriptor.Claims.Add(column.ColumnName, stringValue);
						}
					}
					else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(column.PropertyType))
					{
						var elementType = column.PropertyType.GetInterfaces()
							.Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
							.Select(t => t.GetGenericArguments().NullCheckedFirstOrDefault()).NullCheckedFirstOrDefault();

						var columnValues = columnValue as System.Collections.IEnumerable;

						if (elementType.IsPrimitive || (elementType == typeof(string)))
						{
							securityTokenDescriptor.Claims.Add(column.ColumnName, columnValue);
						}
						else
						{
							var dictionaries = new List<Dictionary<string, object>>();

							foreach (var columnValuesValue in columnValues)
							{
								dictionaries.Add(getPayloadDictionary(columnValuesValue));
							}

							securityTokenDescriptor.Claims.Add(column.ColumnName, dictionaries);
						}
					}
					else
					{
						securityTokenDescriptor.Claims.Add(column.ColumnName, getPayloadDictionary(columnValue));
					}
				}
			}
		}

		private static Dictionary<string, object> getPayloadDictionary(object value)
		{
			if (value != null)
			{
				var valueType = value.GetType();
				
				ISI.Extensions.DataContract.DataMemberPropertyInfo[] properties = null;

				if (valueType.IsDefined(typeof(System.Runtime.Serialization.DataContractAttribute), false))
				{
					properties = ISI.Extensions.DataContract.GetDataMemberPropertyInfos(valueType).OrderBy(property => property.Order).ToArray();
				}
				else
				{
					properties = valueType.GetProperties().Where(propertyInfo => propertyInfo.CanRead).Select(property => new ISI.Extensions.DataContract.DataMemberPropertyInfo(new() { Name = property.Name }, property, false)).OrderBy(property => property.Order).ToArray();
				}

				var payloadDictionary = new Dictionary<string, object>();

				var serializerContractUuidAttribute = ((ISI.Extensions.Serialization.SerializerContractUuidAttribute[])(valueType.GetCustomAttributes(typeof(ISI.Extensions.Serialization.SerializerContractUuidAttribute), false))).FirstOrDefault();
				if (serializerContractUuidAttribute != null)
				{
					payloadDictionary.Add(ISI.Extensions.Serialization.SerializerContractUuidAttribute.SerializerContractUuidKey, serializerContractUuidAttribute.SerializerContractUuid);
				}

				foreach (var property in properties)
				{
					var propertyValue = property.PropertyInfo.GetValue(value);

					if (propertyValue != null)
					{
						if (property.PropertyInfo.PropertyType.IsPrimitive)
						{
							payloadDictionary.Add(property.DataMemberAttribute?.Name ?? property.PropertyInfo.Name, propertyValue);
						}
						else if (property.PropertyInfo.PropertyType.IsGenericType && (property.PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) && (new System.ComponentModel.NullableConverter(property.PropertyInfo.PropertyType)).UnderlyingType.IsPrimitive)
						{
							payloadDictionary.Add(property.DataMemberAttribute?.Name ?? property.PropertyInfo.Name, propertyValue);
						}
						else if (propertyValue is string stringValue)
						{
							if (!string.IsNullOrWhiteSpace(stringValue))
							{
								payloadDictionary.Add(property.DataMemberAttribute?.Name ?? property.PropertyInfo.Name, stringValue);
							}
						}
						else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(property.PropertyInfo.PropertyType))
						{
							var elementType = property.PropertyInfo.PropertyType.GetInterfaces()
								.Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
								.Select(t => t.GetGenericArguments().NullCheckedFirstOrDefault()).NullCheckedFirstOrDefault();

							var propertyValues = propertyValue as System.Collections.IEnumerable;

							if (elementType.IsPrimitive || (elementType == typeof(string)))
							{
								payloadDictionary.Add(property.DataMemberAttribute?.Name ?? property.PropertyInfo.Name, propertyValue);
							}
							else
							{
								var dictionaries = new List<Dictionary<string, object>>();

								foreach (var propertyValuesValue in propertyValues)
								{
									dictionaries.Add(getPayloadDictionary(propertyValuesValue));
								}

								payloadDictionary.Add(property.DataMemberAttribute?.Name ?? property.PropertyInfo.Name, dictionaries.ToArray());
							}
						}
						else
						{
							payloadDictionary.Add(property.DataMemberAttribute?.Name ?? property.PropertyInfo.Name, getPayloadDictionary(propertyValue));
						}
					}
				}

				return payloadDictionary;
			}

			return null;
		}

		public static void AddToPayload<TValue>(this List<System.Security.Claims.Claim> claims, TValue value, ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer = null)
			where TValue : class, new()
		{
			var columns = ISI.Extensions.Columns.ColumnCollection<TValue>.GetDefault(jsonSerializer);

			foreach (var column in columns)
			{
				var columnValue = column.GetValue(value);

				if (column.PropertyType.IsPrimitive)
				{
					claims.Add(new System.Security.Claims.Claim(column.ColumnName, columnValue.ToString()));
				}
				else if (columnValue is string stringValue)
				{
					claims.Add(new System.Security.Claims.Claim(column.ColumnName, stringValue));
				}
				else if ((jsonSerializer != null) && (columnValue != null))
				{
					claims.Add(new System.Security.Claims.Claim(column.ColumnName, jsonSerializer.Serialize(columnValue, false)));
				}
			}
		}

		public static T DeserializePayload<T>(this System.IdentityModel.Tokens.Jwt.JwtSecurityToken jwtSecurityToken, ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer = null)
			where T : class, new()
		{
			var response = new T();

			var columns = ISI.Extensions.Columns.ColumnCollection<T>.GetDefault(jsonSerializer);

			foreach (var column in columns)
			{
				if (jwtSecurityToken.Payload.TryGetValue(column.ColumnName, out var value))
				{
					column.SetValue(response, column.TransformValue(value));
				}
			}

			return response;
		}

		public static T DeserializeHeader<T>(this System.IdentityModel.Tokens.Jwt.JwtSecurityToken jwtSecurityToken, ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer = null)
			where T : class, new()
		{
			var response = new T();

			var columns = ISI.Extensions.Columns.ColumnCollection<T>.GetDefault(jsonSerializer);

			foreach (var column in columns)
			{
				if (jwtSecurityToken.Header.TryGetValue(column.ColumnName, out var value))
				{
					column.SetValue(response, column.TransformValue(value));
				}
			}

			return response;
		}

		public static bool TryGetSerializedJsonWebKey(this System.IdentityModel.Tokens.Jwt.JwtSecurityToken jwtSecurityToken, out string serializedJsonWebKey)
		{
			if (jwtSecurityToken.Header.TryGetValue("jwk", out var jwk))
			{
				serializedJsonWebKey = string.Format("{0}", jwk);

				return true;
			}

			serializedJsonWebKey = null;
			
			return false;
		}
	}
}
