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

namespace ISI.Extensions.DependencyInjection.Iunq
{
	public class Container : ISI.Extensions.DependencyInjection.IContainer
	{
		protected Configuration Configuration { get; }

		public ServiceTypeBuilderDictionary UnnamedServiceTypeBuilders { get; }
		public IDictionary<string, ServiceTypeBuilderDictionary> NamedServiceTypeBuilders { get; }

		public IDictionary<string, GenericServiceTypeMaps> UnnamedGenericServiceTypeMapSets { get; }
		public IDictionary<string, IDictionary<string, GenericServiceTypeMaps>> NamedGenericServiceTypeMapSets { get; }

		public ServiceCache UnnamedSingletonServiceCache { get; }
		public IDictionary<string, ServiceCache> NamedSingletonServiceCaches { get; }

		public Container(Configuration configuration)
		{
			Configuration = configuration;

			UnnamedServiceTypeBuilders = new ServiceTypeBuilderDictionary(Configuration);
			NamedServiceTypeBuilders = new System.Collections.Concurrent.ConcurrentDictionary<string, ServiceTypeBuilderDictionary>(Environment.ProcessorCount * 2, Configuration.NamedServiceTypeBuilderDictionaryInitialSize);

			UnnamedGenericServiceTypeMapSets = new System.Collections.Concurrent.ConcurrentDictionary<string, GenericServiceTypeMaps>(Environment.ProcessorCount * 2, Configuration.GenericServiceTypeMapDictionaryInitialSize);
			NamedGenericServiceTypeMapSets = new System.Collections.Concurrent.ConcurrentDictionary<string, IDictionary<string, GenericServiceTypeMaps>>(Environment.ProcessorCount * 2, Configuration.GenericServiceTypeMapDictionaryInitialSize);

			UnnamedSingletonServiceCache = new ServiceCache(configuration);
			NamedSingletonServiceCaches = new System.Collections.Concurrent.ConcurrentDictionary<string, ServiceCache>(Environment.ProcessorCount * 2, Configuration.ServiceCacheDictionaryInitialSize);
		}

		public ServiceTypeBuilder GetOrCreateServiceTypeBuilder(string name, Type serviceType, Func<ServiceTypeBuilder> getServiceTypeBuilder)
		{
			if (string.IsNullOrEmpty(name))
			{
				{
					if (!UnnamedServiceTypeBuilders.TryGetValue(serviceType, out var serviceTypeBuilder))
					{
						serviceTypeBuilder = getServiceTypeBuilder?.Invoke();

						if (serviceTypeBuilder != null)
						{
							UnnamedServiceTypeBuilders.Add(serviceTypeBuilder);
						}

						return serviceTypeBuilder;
					}
				}
			}

			{
				if (!NamedServiceTypeBuilders.TryGetValue(name, out var serviceTypeBuilders))
				{
					NamedServiceTypeBuilders.Add(name, (serviceTypeBuilders = new ServiceTypeBuilderDictionary(Configuration)));
				}

				if (!serviceTypeBuilders.TryGetValue(serviceType, out var serviceTypeBuilder))
				{
					serviceTypeBuilder = getServiceTypeBuilder?.Invoke();

					if (serviceTypeBuilder != null)
					{
						serviceTypeBuilders.Add(serviceTypeBuilder);
					}

					return serviceTypeBuilder;
				}
			}

			return null;
		}

		public GenericServiceTypeMaps GetOrCreateGenericServiceTypeMaps(string name, Type serviceType)
		{
			var genericServiceTypeName = GenericServiceTypeMaps.GetGenericServiceTypeName(serviceType);
			GenericServiceTypeMaps genericServiceTypeMapSet = null;

			if (string.IsNullOrEmpty(name))
			{
				if (!UnnamedGenericServiceTypeMapSets.TryGetValue(genericServiceTypeName, out genericServiceTypeMapSet))
				{
					UnnamedGenericServiceTypeMapSets.Add(genericServiceTypeName, (genericServiceTypeMapSet = new GenericServiceTypeMaps(Configuration, serviceType)));
				}
			}
			else
			{
				if (!NamedGenericServiceTypeMapSets.TryGetValue(name, out var genericServiceTypeMapsSet))
				{
					NamedGenericServiceTypeMapSets.Add(name, (genericServiceTypeMapsSet = new System.Collections.Concurrent.ConcurrentDictionary<string, GenericServiceTypeMaps>(Environment.ProcessorCount * 2, Configuration.GenericServiceTypeMapDictionaryInitialSize)));
				}

				if (!genericServiceTypeMapsSet.TryGetValue(genericServiceTypeName, out genericServiceTypeMapSet))
				{
					genericServiceTypeMapsSet.Add(genericServiceTypeName, (genericServiceTypeMapSet = new GenericServiceTypeMaps(Configuration, serviceType)));
				}
			}

			return genericServiceTypeMapSet;
		}


		public void Dispose()
		{
			UnnamedSingletonServiceCache?.Dispose();

			foreach (var namedSingletonServiceCache in NamedSingletonServiceCaches)
			{
				namedSingletonServiceCache.Value?.Dispose();
			}
		}
	}
}
