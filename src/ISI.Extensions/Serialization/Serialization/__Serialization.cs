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
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.Serialization
{
	public partial class Serialization : ISerialization
	{
		protected ISI.Extensions.Serialization.Configuration Configuration { get; }
		
		protected System.IServiceProvider ServiceProvider { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		protected readonly ISI.Extensions.Serialization.ISerializer DefaultSerializer = null;
		protected readonly ISI.Extensions.Serialization.ISerializer DefaultDataContractSerializer = null;
		protected readonly System.Collections.Concurrent.ConcurrentDictionary<Type, ISI.Extensions.Serialization.ISerializer> AvailableSerializers = null;
		protected readonly System.Collections.Concurrent.ConcurrentDictionary<Type, ISI.Extensions.Serialization.ISerializer> MappedSerializers = null;

		protected readonly System.Collections.Concurrent.ConcurrentDictionary<Type, bool> SerializationFormattingShowDeclaration = null;
		protected readonly System.Collections.Concurrent.ConcurrentDictionary<Type, bool> SerializationFormattingFriendlyFormatted = null;

		protected readonly System.Collections.Concurrent.ConcurrentDictionary<Type, string> SerializerContractNameLookupBySerializerContractType = null;
		protected readonly System.Collections.Concurrent.ConcurrentDictionary<string, Type> SerializerContractTypeLookupBySerializerContractName = null;

		protected readonly System.Collections.Concurrent.ConcurrentDictionary<Type, Guid> SerializerContractUuidLookupBySerializerContractType = null;
		protected readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, Type> SerializerContractTypeLookupBySerializerContractUuid = null;

		public Serialization(
			ISI.Extensions.Serialization.Configuration configuration,
			System.IServiceProvider serviceProvider,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Configuration = configuration;
			ServiceProvider = serviceProvider;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;

			AvailableSerializers = new System.Collections.Concurrent.ConcurrentDictionary<Type, ISI.Extensions.Serialization.ISerializer>();
			foreach (var exportedType in ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<ISI.Extensions.Serialization.ISerializer>())
			{
				AvailableSerializers.TryAdd(exportedType, ServiceProvider.GetService(exportedType) as ISI.Extensions.Serialization.ISerializer);
			}

			var defaultSerializerType = Type.GetType(Configuration.DefaultSerializerType);
			if (defaultSerializerType == null)
			{
				throw new Exception(string.Format("Cannot find defaultSerializerType for \"{0}\"", Configuration.DefaultSerializerType));
			}
			DefaultSerializer = ServiceProvider.GetService(defaultSerializerType) as ISI.Extensions.Serialization.ISerializer;

			var defaultDataContractSerializerType = Type.GetType(Configuration.DefaultDataContractSerializerType);
			if (defaultDataContractSerializerType == null)
			{
				throw new Exception(string.Format("Cannot find defaultDataContractSerializerType for \"{0}\"", Configuration.DefaultDataContractSerializerType));
			}
			DefaultDataContractSerializer = ServiceProvider.GetService(defaultDataContractSerializerType) as ISI.Extensions.Serialization.ISerializer;

			AvailableSerializers.TryAdd(defaultSerializerType, DefaultSerializer);
			AvailableSerializers.TryAdd(defaultDataContractSerializerType, DefaultDataContractSerializer);

			MappedSerializers = new System.Collections.Concurrent.ConcurrentDictionary<Type, ISI.Extensions.Serialization.ISerializer>();

			SerializationFormattingShowDeclaration = new System.Collections.Concurrent.ConcurrentDictionary<Type, bool>();
			SerializationFormattingFriendlyFormatted = new System.Collections.Concurrent.ConcurrentDictionary<Type, bool>();

			//SerializerContracts
			{
				SerializerContractNameLookupBySerializerContractType = new System.Collections.Concurrent.ConcurrentDictionary<Type, string>();

				foreach (var exportedType in ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<ISI.Extensions.Serialization.ISerializerContract>())
				{
					var serializerContractName = exportedType.AssemblyQualifiedNameWithoutVersion();

					SerializerContractNameLookupBySerializerContractType.TryAdd(exportedType, serializerContractName);
				}
			}

			//SerializerContractNames
			{
				SerializerContractTypeLookupBySerializerContractName = new System.Collections.Concurrent.ConcurrentDictionary<string, Type>();

				foreach (var exportedType in ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<ISI.Extensions.Serialization.ISerializerContractName>())
				{
					var serializerObjectContractNames = new SerializerObjectContractNames(exportedType);

					if (!SerializerContractNameLookupBySerializerContractType.TryAdd(exportedType, serializerObjectContractNames.ContractName))
					{
						SerializerContractNameLookupBySerializerContractType[exportedType] = serializerObjectContractNames.ContractName;
					}
					foreach (var alias in serializerObjectContractNames.ContractNameAliases)
					{
						SerializerContractTypeLookupBySerializerContractName.TryAdd(alias, exportedType);
					}
				}
			}

			//SerializerContractUuids
			{
				SerializerContractUuidLookupBySerializerContractType = new System.Collections.Concurrent.ConcurrentDictionary<Type, Guid>();
				SerializerContractTypeLookupBySerializerContractUuid = new System.Collections.Concurrent.ConcurrentDictionary<Guid, Type>();

				foreach (var exportedType in ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<ISI.Extensions.Serialization.ISerializerContractUuid>())
				{
					var serializerContractUuidAttribute = ((SerializerContractUuidAttribute[])(exportedType.GetCustomAttributes(typeof(SerializerContractUuidAttribute), false))).FirstOrDefault();
					if (serializerContractUuidAttribute != null)
					{
						SerializerContractUuidLookupBySerializerContractType.TryAdd(exportedType, serializerContractUuidAttribute.SerializerContractUuid);
						if (!SerializerContractTypeLookupBySerializerContractUuid.TryAdd(serializerContractUuidAttribute.SerializerContractUuid, exportedType))
						{
							throw new Exception(string.Format("Multiple SerializerContractUuid found \"{0}\"", serializerContractUuidAttribute.SerializerContractUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens)));
						}
					}
				}
			}
		}
	}
}