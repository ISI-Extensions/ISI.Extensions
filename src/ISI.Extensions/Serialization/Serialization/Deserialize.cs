#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.Serialization
{
	public partial class Serialization
	{
		public T Deserialize<T>(string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			return (T)GetSerializer<T>(serializationFormat).Deserialize(typeof(T), serializedValue);
		}

		public object Deserialize(Type serializerContractType, string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			return GetSerializer(serializerContractType, serializationFormat).Deserialize(serializerContractType, serializedValue);
		}

		public object Deserialize(Guid serializerContractUuid, string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			var serializerContractType = GetSerializerContractTypeFromSerializerContractUuid(serializerContractUuid);

			return GetSerializer(serializerContractType, serializationFormat).Deserialize(serializerContractType, serializedValue);
		}

		public object Deserialize(string serializerContractName, string serializedValue, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			var serializerContractType = GetSerializerContractTypeFromSerializerContractName(serializerContractName);

			return GetSerializer(serializerContractType, serializationFormat).Deserialize(serializerContractType, serializedValue);
		}

		public T Deserialize<T>(System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			return (T)GetSerializer<T>(serializationFormat).Deserialize(typeof(T), fromStream);
		}

		public object Deserialize(Type serializerContractType, System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			return GetSerializer(serializerContractType, serializationFormat).Deserialize(serializerContractType, fromStream);
		}

		public object Deserialize(Guid serializerContractUuid, System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			var serializerContractType = GetSerializerContractTypeFromSerializerContractUuid(serializerContractUuid);

			return GetSerializer(serializerContractType, serializationFormat).Deserialize(serializerContractType, fromStream);
		}

		public object Deserialize(string serializerContractName, System.IO.Stream fromStream, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			var serializerContractType = GetSerializerContractTypeFromSerializerContractName(serializerContractName);

			return GetSerializer(serializerContractType, serializationFormat).Deserialize(serializerContractType, fromStream);
		}

		public object Deserialize(ISI.Extensions.Serialization.ISerializedEntity serializedEntity)
		{
			Type serializerContractType = null;

			if ((serializedEntity is IHasSerializerContractUuid hasSerializerContractUuid) && hasSerializerContractUuid.SerializerContractUuid.IsNullOrEmpty())
			{
				serializerContractType = GetSerializerContractTypeFromSerializerContractUuid(hasSerializerContractUuid.SerializerContractUuid.GetValueOrDefault());
			}

			if ((serializerContractType == null) && (serializedEntity is IHasSerializerContractName hasSerializerContractName) && !string.IsNullOrEmpty(hasSerializerContractName.SerializerContractName))
			{
				serializerContractType = GetSerializerContractTypeFromSerializerContractName(hasSerializerContractName.SerializerContractName);
			}

			return GetSerializer(serializerContractType, (serializedEntity as IHasSerializationFormat)?.SerializationFormat ?? SerializationFormat.Json).Deserialize(serializerContractType, serializedEntity.SerializedValue);
		}
	}
}