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
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.Serialization
{
	public partial class Serialization
	{
		public ISI.Extensions.Serialization.ISerializer GetSerializer<TSerializerContract>(ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			return GetSerializer(typeof(TSerializerContract), serializationFormat);
		}
		public ISI.Extensions.Serialization.ISerializer GetSerializer(Type serializerContractType, ISI.Extensions.Serialization.SerializationFormat serializationFormat = ISI.Extensions.Serialization.SerializationFormat.Unknown)
		{
			if (serializerContractType.IsArray)
			{
				serializerContractType = serializerContractType.GetElementType();
			}

			if (!MappedSerializers.TryGetValue(serializerContractType, out var serializer))
			{
				var preferredSerializerAttribute = ((ISI.Extensions.Serialization.PreferredSerializerAttribute[]) (serializerContractType.GetCustomAttributes(typeof(ISI.Extensions.Serialization.PreferredSerializerAttribute), false))).FirstOrDefault();

				if (preferredSerializerAttribute == null)
				{
					var dataContractAttribute = ((System.Runtime.Serialization.DataContractAttribute[]) (serializerContractType.GetCustomAttributes(typeof(System.Runtime.Serialization.DataContractAttribute), false))).FirstOrDefault();

					if (dataContractAttribute == null)
					{
						var collectionDataContractAttribute = ((System.Runtime.Serialization.CollectionDataContractAttribute[]) (serializerContractType.GetCustomAttributes(typeof(System.Runtime.Serialization.CollectionDataContractAttribute), false))).FirstOrDefault();

						if (collectionDataContractAttribute == null)
						{
							serializer = DefaultSerializer;

							AddMapping(serializerContractType, serializer.GetType());
						}
						else
						{
							serializer = DefaultDataContractSerializer;

							AddMapping(serializerContractType, serializer.GetType());
						}
					}
					else
					{
						serializer = DefaultDataContractSerializer;

						AddMapping(serializerContractType, serializer.GetType());
					}
				}
				else
				{
					AddMapping(serializerContractType, preferredSerializerAttribute.PreferredSerializerType);

					serializer = MappedSerializers[serializerContractType];

					if (!AvailableSerializers.ContainsKey(preferredSerializerAttribute.PreferredSerializerType))
					{
						AvailableSerializers.TryAdd(preferredSerializerAttribute.PreferredSerializerType, serializer);
					}
				}
			}

			if ((serializationFormat != ISI.Extensions.Serialization.SerializationFormat.Unknown) && (serializer.SerializationFormat != serializationFormat))
			{
				serializer = AvailableSerializers.FirstOrDefault(s => ((s.Value.SerializationFormat == serializationFormat) && (s.Value.UsesDataContract == serializer.UsesDataContract))).Value;
			}

			return serializer;
		}
	}
}
