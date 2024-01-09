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
using System.Text;
using ISI.Extensions.JsonSerialization.Extensions;

namespace ISI.Extensions.JsonJwt.Extensions
{
	public static class JwtExtensions
	{
		public static Jwt SetJwkAlgorithmKey(this Jwt jwt, string jwkAlgorithmKey)
		{
			jwt.Header.Remove(HeaderKey.JwkAlgorithmKey);
			if (!string.IsNullOrWhiteSpace(jwkAlgorithmKey))
			{
				jwt.Header.Add(HeaderKey.JwkAlgorithmKey, jwkAlgorithmKey);
			}

			return jwt;
		}

		public static Jwt SetAcmeAccountKey(this Jwt jwt, string accountKey)
		{
			jwt.Header.Remove(HeaderKey.AcmeAccountKey);
			if (!string.IsNullOrWhiteSpace(accountKey))
			{
				jwt.Header.Add(HeaderKey.AcmeAccountKey, accountKey);
			}

			return jwt;
		}

		public static Jwt SetSerializedJwk(this Jwt jwt, string serializedJwk)
		{
			jwt.Header.Remove(HeaderKey.SerializedJwk);
			if (!string.IsNullOrWhiteSpace(serializedJwk))
			{
				jwt.Header.Add(HeaderKey.SerializedJwk, serializedJwk);
			}

			return jwt;
		}

		public static void AddToPayload<TValue>(this Jwt jwt, TValue value, ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer = null)
			where TValue : class, new()
		{
			var columns = ISI.Extensions.Columns.ColumnCollection<TValue>.GetDefault(jsonSerializer);

			foreach (var column in columns)
			{
				var columnValue = column.GetValue(value);

				if (column.PropertyType.IsPrimitive)
				{
					jwt.Payload.Add(column.ColumnName, columnValue.ToString());
				}
				else if (columnValue is string stringValue)
				{
					jwt.Payload.Add(column.ColumnName, stringValue);
				}
				else if ((jsonSerializer != null) && (columnValue != null))
				{
					jwt.Payload.Add(column.ColumnName, jsonSerializer.Serialize(columnValue, false));
				}
			}
		}

		public static T DeserializePayload<T>(this Jwt jwt, ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer = null)
			where T : class, new()
		{
			var response = new T();

			var columns = ISI.Extensions.Columns.ColumnCollection<T>.GetDefault(jsonSerializer);

			foreach (var column in columns)
			{
				if (jwt.Payload.TryGetValue(column.ColumnName, out var value))
				{
					column.SetValue(response, column.TransformValue(value));
				}
			}

			return response;
		}

		public static T DeserializeHeader<T>(this Jwt jwt, ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer = null)
			where T : class, new()
		{
			var response = new T();

			var columns = ISI.Extensions.Columns.ColumnCollection<T>.GetDefault(jsonSerializer);

			foreach (var column in columns)
			{
				if (jwt.Header.TryGetValue(column.ColumnName, out var value))
				{
					column.SetValue(response, column.TransformValue(value));
				}
			}

			return response;
		}

		public static string GetSerializedJwk(this Jwt jwt)
		{
			{
				if (jwt.Header.TryGetValue("jwt", out var header))
				{
					return header.ToString();
				}
			}

			{
				if (jwt.Header.TryGetValue("jwk", out var header))
				{
					return header.ToString();
				}
			}

			return null;
		}
	}
}
