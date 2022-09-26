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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.JsonSerialization.Newtonsoft
{
	public class SerializableArrayOrInstanceJsonConverter : global::Newtonsoft.Json.JsonConverter
	{
		public override bool CanWrite => false;
		public override bool CanRead => true;

		public override bool CanConvert(Type objectType) => objectType.Implements<ISI.Extensions.Serialization.ISerializableArrayOrInstance>();

		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{

		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			var token = global::Newtonsoft.Json.Linq.JToken.Load(reader);

			var instance = Activator.CreateInstance(objectType) as ISI.Extensions.Serialization.ISerializableArrayOrInstance;

			var memberType = objectType.GenericTypeArguments.NullCheckedFirstOrDefault();

			if (token.Type == global::Newtonsoft.Json.Linq.JTokenType.Array)
			{
				var arrayType = typeof(IEnumerable<>).MakeGenericType(memberType);

				instance.SetValues((token.ToObject(arrayType, serializer) as IEnumerable).Cast<object>());
			}
			else
			{
				instance.SetValues(new[] { token.ToObject(memberType, serializer) });
			}

			return instance;
		}
	}

	[GetJsonConverter]
	public class GetSerializableArrayOrInstanceJsonConverter : IGetJsonConverter
	{
		private static global::Newtonsoft.Json.JsonConverter _jsonConverter = null;
		private static readonly object _jsonConverterLock = new();

		public global::Newtonsoft.Json.JsonConverter GetJsonConverter()
		{
			if (_jsonConverter == null)
			{
				lock (_jsonConverterLock)
				{
					_jsonConverter ??= new SerializableArrayOrInstanceJsonConverter();
				}
			}

			return _jsonConverter;
		}
	}
}