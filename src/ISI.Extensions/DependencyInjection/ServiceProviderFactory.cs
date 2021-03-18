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
using ISI.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.DependencyInjection
{
	public class ServiceProviderFactory : Microsoft.Extensions.DependencyInjection.IServiceProviderFactory<ContainerBuilder>
	{
		protected Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }
		protected Configuration DependencyInjectionConfiguration { get; }
		protected Type ServiceProviderBuilderType { get; }

		public ServiceProviderFactory(Microsoft.Extensions.Configuration.IConfiguration configuration, Type serviceProviderBuilderType = null)
		{
			Configuration = configuration;
			DependencyInjectionConfiguration = new Configuration();
			ServiceProviderBuilderType = serviceProviderBuilderType;

			configuration.Bind(ISI.Extensions.DependencyInjection.Configuration.ConfigurationSectionName, DependencyInjectionConfiguration);
		}
		
		public ContainerBuilder CreateBuilder(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
		{
			return new ContainerBuilder(DependencyInjectionConfiguration, services);
		}

		public System.IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
		{
			var serviceProviderBuilderType = ServiceProviderBuilderType ?? Type.GetType(DependencyInjectionConfiguration.ServiceProviderBuilderType);

			if (serviceProviderBuilderType == null)
			{
				throw new InvalidServiceProviderBuilderTypeException(DependencyInjectionConfiguration.ServiceProviderBuilderType);
			}

			var serviceProviderBuilder = Activator.CreateInstance(serviceProviderBuilderType) as IServiceProviderBuilder;

			var serviceProvider = serviceProviderBuilder.BuildServiceProvider(containerBuilder.Services, Configuration);

			serviceProvider.RegisterInstance<System.IServiceProvider>(serviceProvider, Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);

			return serviceProvider;
		}
	}
}
