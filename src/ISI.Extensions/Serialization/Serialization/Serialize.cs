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
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.Serialization
{
	public partial class Serialization
	{
		public void Serialize(Type serializerContractType, object value, System.IO.Stream toStream, SerializationFormat serializationFormat = SerializationFormat.Unknown, bool? friendlyFormatted = null)
		{
			GetSerializer(serializerContractType, serializationFormat).Serialize(serializerContractType, value, toStream, friendlyFormatted.GetValueOrDefault(GetSerializationFormattingFriendlyFormatted(serializerContractType)));
		}

		public void Serialize<T>(object value, System.IO.Stream toStream, SerializationFormat serializationFormat = SerializationFormat.Unknown, bool? friendlyFormatted = null)
		{
			GetSerializer<T>(serializationFormat).Serialize(typeof(T), value, toStream, friendlyFormatted.GetValueOrDefault(GetSerializationFormattingFriendlyFormatted(typeof(T))));
		}

		public ISI.Extensions.Serialization.SerializedEntity Serialize(Type serializerContractType, object value, SerializationFormat serializationFormat = SerializationFormat.Unknown, bool? friendlyFormatted = null)
		{
			var serializer = GetSerializer(serializerContractType, serializationFormat);

			var serializerContractUuid = GetSerializerContractUuidFromSerializerContractType(serializerContractType);
			if (serializerContractUuid.IsNullOrEmpty() && serializerContractType.IsInterface)
			{
				serializerContractUuid = GetSerializerContractUuidFromSerializerContractType(value.GetType());
			}
			if (!serializerContractUuid.IsNullOrEmpty())
			{
				return new ISI.Extensions.Serialization.SerializedEntity(serializer.SerializationFormat, serializerContractUuid.Value, serializer.Serialize(serializerContractType, value, friendlyFormatted.GetValueOrDefault(GetSerializationFormattingFriendlyFormatted(serializerContractType))));
			}

			var serializerContractName = GetSerializerContractNameFromSerializerContractType(serializerContractType) ?? serializerContractType.AssemblyQualifiedNameWithoutVersion();

			return new ISI.Extensions.Serialization.SerializedEntity(serializer.SerializationFormat, serializerContractName, serializer.Serialize(serializerContractType, value, friendlyFormatted.GetValueOrDefault(GetSerializationFormattingFriendlyFormatted(serializerContractType))));
		}

		public ISI.Extensions.Serialization.SerializedEntity Serialize<T>(T value, SerializationFormat serializationFormat = SerializationFormat.Unknown, bool? friendlyFormatted = null)
			where T : class
		{
			var serializerContractType = typeof(T);

			//if (serializerContractType.IsInterface)
			//{
			//	serializerContractType = value.GetType();
			//}

			return Serialize(serializerContractType, value, serializationFormat, friendlyFormatted);
		}
	}
}