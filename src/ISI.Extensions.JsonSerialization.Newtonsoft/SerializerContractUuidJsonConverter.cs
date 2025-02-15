#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using ISI.Extensions.JsonSerialization.Newtonsoft.Extensions;
using ISI.Extensions.Serialization;
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.JsonSerialization.Newtonsoft
{
	public class SerializerContractUuidJsonConverter : global::Newtonsoft.Json.JsonConverter, IJsonConverterWithGetSerializableInterfaceTypes
	{
		protected Type[] SerializableInterfaceTypes { get; }
		protected System.Collections.Concurrent.ConcurrentDictionary<Type, Type> InterfaceTypesWithDefaultImplementationType { get; }
		protected System.Collections.Concurrent.ConcurrentDictionary<Guid, Type> SerializerContractUuidToImplementationType { get; }
		protected System.Collections.Concurrent.ConcurrentDictionary<Type, Guid> ImplementationTypeToSerializerContractUuid { get; }

		public override bool CanWrite => true;
		public override bool CanRead => true;

		public SerializerContractUuidJsonConverter(
			IEnumerable<Type> serializableInterfaceTypes,
			IDictionary<Type, Type> interfaceTypesWithDefaultImplementationType,
			IDictionary<Guid, Type> serializerContractUuidToImplementationType)
		{
			SerializableInterfaceTypes = serializableInterfaceTypes.ToNullCheckedArray(NullCheckCollectionResult.Empty);
			InterfaceTypesWithDefaultImplementationType = new(interfaceTypesWithDefaultImplementationType);
			SerializerContractUuidToImplementationType = new(serializerContractUuidToImplementationType);
			ImplementationTypeToSerializerContractUuid = new(serializerContractUuidToImplementationType.ToDictionary(keyValue => keyValue.Value, keyValue => keyValue.Key));
		}

		public override bool CanConvert(Type objectType) => (InterfaceTypesWithDefaultImplementationType.ContainsKey(objectType) || ImplementationTypeToSerializerContractUuid.ContainsKey(objectType));

		public override void WriteJson(global::Newtonsoft.Json.JsonWriter writer, object value, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			var jsonObject = new global::Newtonsoft.Json.Linq.JObject();

			var objectType = value.GetType();

			if (ImplementationTypeToSerializerContractUuid.TryGetValue(objectType, out var serializerContractUuid))
			{
				jsonObject.AddFirst(new global::Newtonsoft.Json.Linq.JProperty(ISI.Extensions.Serialization.SerializerContractUuidAttribute.SerializerContractUuidKey, serializerContractUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens)));
			}

			foreach (var propertyInfo in objectType.GetProperties())
			{
				if (propertyInfo.CanRead)
				{
					if (!propertyInfo.IgnorePropertyInfo())
					{
						var dataMemberAttribute = propertyInfo.GetCustomAttributes(true).OfType<System.Runtime.Serialization.DataMemberAttribute>().FirstOrDefault();

						var objectPropertyValue = propertyInfo.GetValue(value, null);
						if (objectPropertyValue != null)
						{
							jsonObject.Add(dataMemberAttribute?.Name ?? propertyInfo.Name, global::Newtonsoft.Json.Linq.JToken.FromObject(objectPropertyValue, serializer));
						}
					}
				}
			}

			jsonObject.WriteTo(writer);
		}

		public override object ReadJson(global::Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, global::Newtonsoft.Json.JsonSerializer serializer)
		{
			var implementationType = objectType;

			var jsonObject = global::Newtonsoft.Json.Linq.JObject.Load(reader);

			var serializerContractUuid = jsonObject.Value<string>(ISI.Extensions.Serialization.SerializerContractUuidAttribute.SerializerContractUuidKey).ToGuid();

			if (serializerContractUuid != Guid.Empty)
			{
				SerializerContractUuidToImplementationType.TryGetValue(serializerContractUuid, out implementationType);
			}
			else if (InterfaceTypesWithDefaultImplementationType.TryGetValue(implementationType, out var defaultImplementationType))
			{
				implementationType = defaultImplementationType;
			}
			
			if (implementationType == null)
			{
				throw new Exception($"implementationType not found for serializerContractUuid: {serializerContractUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens)}");
			}

			var implementation = Activator.CreateInstance(implementationType);

			foreach (var propertyInfo in implementationType.GetProperties().Where(p => p.CanRead && p.CanWrite))
			{
				if (!propertyInfo.IgnorePropertyInfo())
				{
					var dataMemberAttribute = propertyInfo.GetCustomAttributes(true).OfType<System.Runtime.Serialization.DataMemberAttribute>().FirstOrDefault();

					var jsonToken = jsonObject.SelectToken(dataMemberAttribute?.Name ?? propertyInfo.Name);

					if (jsonToken != null && (jsonToken.Type != global::Newtonsoft.Json.Linq.JTokenType.Null))
					{
						var propertyValue = jsonToken.ToObject(propertyInfo.PropertyType, serializer);

						propertyInfo.SetValue(implementation, propertyValue, null);
					}
				}
			}

			return implementation;
		}

		IEnumerable<Type> IJsonConverterWithGetSerializableInterfaceTypes.GetSerializableInterfaceTypes() => SerializableInterfaceTypes;
	}

	[GetJsonConverter]
	public class GetSerializerContractUuidJsonConverter : IGetJsonConverter
	{
		private static global::Newtonsoft.Json.JsonConverter _jsonConverter = null;
		private static readonly object _jsonConverterLock = new();

		public global::Newtonsoft.Json.JsonConverter GetJsonConverter()
		{
			if (_jsonConverter == null)
			{
				lock (_jsonConverterLock)
				{
					if (_jsonConverter == null)
					{
						var serializableInterfaceTypes = new HashSet<Type>();
						var interfaceTypesWithDefaultImplementationType = new System.Collections.Concurrent.ConcurrentDictionary<Type, Type>();
						var serializerContractUuidToImplementationType = new System.Collections.Concurrent.ConcurrentDictionary<Guid, Type>();

						foreach (var exportedType in ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<ISI.Extensions.Serialization.ISerializerContractUuid>())
						{
							var serializerContractUuidAttribute = ((ISI.Extensions.Serialization.SerializerContractUuidAttribute[])(exportedType.GetCustomAttributes(typeof(ISI.Extensions.Serialization.SerializerContractUuidAttribute), false))).FirstOrDefault();
							if (serializerContractUuidAttribute != null)
							{
								foreach (var interfaceType in exportedType.GetInterfaces())
								{
									serializableInterfaceTypes.Add(interfaceType);

									var serializerDefaultImplementationTypeAttribute = ((ISI.Extensions.Serialization.SerializerDefaultImplementationTypeAttribute[])(interfaceType.GetCustomAttributes(typeof(ISI.Extensions.Serialization.SerializerDefaultImplementationTypeAttribute), false))).FirstOrDefault();

									interfaceTypesWithDefaultImplementationType.TryAdd(interfaceType, serializerDefaultImplementationTypeAttribute?.DefaultImplementationType);
								}

								if (!serializerContractUuidToImplementationType.TryAdd(serializerContractUuidAttribute.SerializerContractUuid, exportedType))
								{
									throw new(string.Format("Multiple SerializerContractUuid found \"{0}\"", serializerContractUuidAttribute.SerializerContractUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens)));
								}
							}
						}

						_jsonConverter = new SerializerContractUuidJsonConverter(serializableInterfaceTypes, interfaceTypesWithDefaultImplementationType, serializerContractUuidToImplementationType);
					}
				}
			}

			return _jsonConverter;
		}
	}
}
