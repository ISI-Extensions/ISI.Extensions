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

namespace ISI.Extensions.Serialization
{
	public interface ISerialization
	{
		void AddSerializableType<TSerializable>();
		void AddSerializableType(Type serializableType);
		void AddMapping(Type type, Type serializerType);
		ISI.Extensions.Serialization.ISerializer GetSerializer<T>(ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		ISI.Extensions.Serialization.ISerializer GetSerializer(Type serializerContractType, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		string GetContentType<TSerializable>();
		string GetContentType(Type type);
		Type GetSerializerContractTypeFromSerializerContractName(string serializerContractName);
		string GetSerializerContractNameFromSerializerContractType(Type serializerContractType);
		Type GetSerializerContractTypeFromSerializerContractUuid(Guid serializerContractUuid);
		Guid? GetSerializerContractUuidFromSerializerContractType(Type serializerContractType);
		object Deserialize(ISI.Extensions.Serialization.ISerializedEntity serializedEntity);
		TSerializable Deserialize<TSerializable>(string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		object Deserialize(Type serializerContractType, string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		object Deserialize(Guid serializerContractUuid, string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		object Deserialize(string serializerContractName, string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TSerializable Deserialize<TSerializable>(System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		object Deserialize(Type serializerContractType, System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		object Deserialize(Guid serializerContractUuid, System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		object Deserialize(string serializerContractName, System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		void Serialize(Type serializerContractType, object value, System.IO.Stream toStream, SerializationFormat serializationFormat = SerializationFormat.Unknown, bool? friendlyFormatted = null);
		void Serialize<T>(object value, System.IO.Stream toStream, SerializationFormat serializationFormat = SerializationFormat.Unknown, bool? friendlyFormatted = null);
		ISI.Extensions.Serialization.SerializedEntity Serialize(Type serializerContractType, object value, SerializationFormat serializationFormat = SerializationFormat.Unknown, bool? friendlyFormatted = null);

		ISI.Extensions.Serialization.SerializedEntity Serialize<T>(T value, SerializationFormat serializationFormat = SerializationFormat.Unknown, bool? friendlyFormatted = null)
			where T : class;

		TDeserializeAs DeserializeAs<T, TDeserializeAs>(string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs>(string serializedValueWithSerializerContractHeader, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs>(IEnumerable<string> serializedValueWithSerializerContractHeader, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs>(Type serializerContractType, string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs>(Guid serializerContractUuid, string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs>(string serializerContractName, string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs, T>(System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs>(Type serializerContractType, System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs>(Guid serializerContractUuid, System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs>(string serializerContractName, System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
		TDeserializeAs DeserializeAs<TDeserializeAs>(ISI.Extensions.Serialization.ISerializedEntity serializedEntity, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown);
	}
}
