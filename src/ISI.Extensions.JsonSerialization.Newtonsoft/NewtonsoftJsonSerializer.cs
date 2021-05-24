#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.JsonSerialization.Newtonsoft
{
	public class NewtonsoftJsonSerializer : ISI.Extensions.JsonSerialization.IJsonSerializer, ISI.Extensions.Serialization.ISerializer
	{
		public ISI.Extensions.Serialization.SerializationFormat SerializationFormat => ISI.Extensions.Serialization.SerializationFormat.Json;
		public bool UsesDataContract => true;
		public string ContentType => ISI.Extensions.MimeTypes.Json;

		public object Deserialize(Type type, string serializedValue)
		{
			return global::Newtonsoft.Json.JsonConvert.DeserializeObject(serializedValue, type, new [] { SerializerContractUuidJsonConverter.GetJsonConverter() });
		}

		public object Deserialize(Type type, System.IO.Stream stream)
		{
			return Deserialize(type, stream.TextReadToEnd());
		}

		public string Serialize(Type type, object value, bool friendlyFormatted = false)
		{
			if (value.GetType() == type)
			{
				return global::Newtonsoft.Json.JsonConvert.SerializeObject(value, (friendlyFormatted ? global::Newtonsoft.Json.Formatting.Indented : global::Newtonsoft.Json.Formatting.None), new [] { SerializerContractUuidJsonConverter.GetJsonConverter() });
			}

			var settings = new global::Newtonsoft.Json.JsonSerializerSettings()
			{
				ContractResolver = TypeContractResolver.GetTypeContractResolver(type),
				Converters = new [] { SerializerContractUuidJsonConverter.GetJsonConverter() },
			};

			return global::Newtonsoft.Json.JsonConvert.SerializeObject(value, (friendlyFormatted ? global::Newtonsoft.Json.Formatting.Indented : global::Newtonsoft.Json.Formatting.None), settings);
		}

		public void Serialize(Type type, object value, System.IO.Stream toStream, bool friendlyFormatted = false)
		{
			using (var writer = new System.IO.StreamWriter(toStream, System.Text.Encoding.UTF8, 1024, true))
			{
				writer.Write(Serialize(type, value, friendlyFormatted));
			}

			toStream.Flush();
		}
	}
}
