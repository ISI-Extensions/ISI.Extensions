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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.DependencyInjection.Iunq
{
	public class ServiceTypeBuilderDictionary
	{

		private readonly System.Collections.Concurrent.ConcurrentDictionary<Type, ServiceTypeBuilder> _serviceTypeBuilders;

		public ServiceTypeBuilderDictionary(Configuration configuration)
		{
			_serviceTypeBuilders = new(Environment.ProcessorCount * 2, configuration.ServiceTypeBuilderDictionaryInitialSize);
		}

		public void Add(ServiceTypeBuilder serviceTypeBuilder)
		{
			if (!_serviceTypeBuilders.TryAdd(serviceTypeBuilder.ServiceType, serviceTypeBuilder))
			{
				_serviceTypeBuilders[serviceTypeBuilder.ServiceType] = serviceTypeBuilder;
			}
		}

		public bool ContainsKey(Type serviceType)
		{
			return _serviceTypeBuilders.ContainsKey(serviceType);
		}

		public ServiceTypeBuilder Get(Type serviceType)
		{
			return _serviceTypeBuilders[serviceType];
		}

		public ServiceTypeBuilder GetOrAdd(Type serviceType, Func<Type, ServiceTypeBuilder> add)
		{
			return _serviceTypeBuilders.GetOrAdd(serviceType, add);
		}

		public bool TryGetValue(Type serviceType, out ServiceTypeBuilder serviceTypeBuilder)
		{
			return _serviceTypeBuilders.TryGetValue(serviceType, out serviceTypeBuilder);
		}

		public void Remove(Type serviceType)
		{
			_serviceTypeBuilders.TryRemove(serviceType, out _);
		}
	}
}
