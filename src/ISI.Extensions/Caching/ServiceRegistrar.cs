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
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Caching
{
	[ISI.Extensions.DependencyInjection.ServiceRegistrar]
	public class ServiceRegistrar : ISI.Extensions.DependencyInjection.IServiceRegistrarWithConfigurationRoot
	{
		public void ServiceRegister(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
		{

		}

		public void ServiceRegister(Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Configuration.IConfigurationRoot configurationRoot)
		{
			var configuration = configurationRoot.GetConfiguration<ISI.Extensions.Caching.Configuration>();

			if (!string.IsNullOrWhiteSpace(configuration?.CacheManagerImplementation) ||
					string.Equals(configuration?.CacheManagerImplementation ?? string.Empty, "Memory", StringComparison.InvariantCultureIgnoreCase))
			{
				services.AddSingleton<Microsoft.Extensions.Caching.Memory.IMemoryCache>(provider => new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions()));
				services.AddSingleton<ISI.Extensions.Caching.ICacheManager, ISI.Extensions.Caching.CacheManager<Microsoft.Extensions.Caching.Memory.IMemoryCache>>();
			}

			if (!string.IsNullOrWhiteSpace(configuration?.EnterpriseCacheManagerApi?.EnterpriseCacheManagerApiImplementation) ||
					string.Equals(configuration?.EnterpriseCacheManagerApi?.EnterpriseCacheManagerApiImplementation ?? string.Empty, "Null", StringComparison.InvariantCultureIgnoreCase))
			{
				services.AddSingleton<ISI.Extensions.Caching.IEnterpriseCacheManagerApi, NullEnterpriseCacheManagerApi>();
			}
		}
	}
}
