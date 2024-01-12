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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.JsonSerialization
{
	[ISI.Extensions.TypeLocator(typeof(Serialization.ISerializer))]
	public class JsonSerializer : ISI.Extensions.JsonSerialization.IJsonSerializer, ISI.Extensions.Serialization.ISerializer
	{
		public Serialization.SerializationFormat SerializationFormat => Serialization.SerializationFormat.Json;

		public bool UsesDataContract => false;

		public string ContentType => ISI.Extensions.MimeTypes.Json;

		public HashSet<Type> GetSerializableInterfaceTypes() => new();

		public object Deserialize(Type type, string serializedValue)
		{
			return System.Text.Json.JsonSerializer.Deserialize(serializedValue, type);
		}

		public object Deserialize(Type type, System.IO.Stream stream)
		{
			return System.Text.Json.JsonSerializer.Deserialize(stream.TextReadToEnd(), type);
		}

		public string Serialize(Type type, object value, bool friendlyFormatted = false)
		{
			var options = new System.Text.Json.JsonSerializerOptions
			{
				WriteIndented = friendlyFormatted,
			};

			return System.Text.Json.JsonSerializer.Serialize(value, type, options);
		}

		public void Serialize(Type type, object value, System.IO.Stream toStream, bool friendlyFormatted = false)
		{
			using (var jsonWriter = new System.Text.Json.Utf8JsonWriter(toStream))
			{
				var options = new System.Text.Json.JsonSerializerOptions
				{
					WriteIndented = friendlyFormatted,
				};

				System.Text.Json.JsonSerializer.Serialize(jsonWriter, value, type, options);
			}
		}
	}
}
