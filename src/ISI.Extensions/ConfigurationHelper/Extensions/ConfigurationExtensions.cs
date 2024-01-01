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
using System.Reflection;
using System.Text;
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.TypeLocator.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.ConfigurationHelper.Extensions
{
	public static class ConfigurationExtensions
	{
		public static Microsoft.Extensions.Configuration.IConfiguration AddAllConfigurations(this Microsoft.Extensions.Configuration.IConfiguration configuration, Microsoft.Extensions.DependencyInjection.IServiceCollection services)
		{
			var configurationTypes = ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<ISI.Extensions.ConfigurationHelper.IConfiguration>();

			foreach (var configurationType in configurationTypes)
			{
				AddConfigurationHandler(services, null, null, configuration, configurationType);
			}

			return configuration;
		}

		public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddAllConfigurations(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			var configurationTypes = ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<ISI.Extensions.ConfigurationHelper.IConfiguration>();

			foreach (var configurationType in configurationTypes)
			{
				AddConfigurationHandler(services, null, null, configuration, configurationType);
			}

			return services;
		}


		public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddConfiguration<TConfiguration>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, string configurationSectionName, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			AddConfigurationHandler(services, null, configurationSectionName, configuration, typeof(TConfiguration));

			return services;
		}

		public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddConfiguration<TConfiguration>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			AddConfigurationHandler(services, null, null, configuration, typeof(TConfiguration));

			return services;
		}

		public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddConfiguration(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, string configurationSectionName, Type configurationType, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			AddConfigurationHandler(services, null, configurationSectionName, configuration, configurationType);

			return services;
		}

		public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddConfiguration(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, Type configurationType, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			AddConfigurationHandler(services, null, null, configuration, configurationType);

			return services;
		}





		private static void AddConfigurationHandler(Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.IServiceProvider serviceProvider, string configurationSectionName, Microsoft.Extensions.Configuration.IConfiguration configuration, Type configurationType)
		{
			if (string.IsNullOrWhiteSpace(configurationSectionName))
			{
				if (configurationType.GetCustomAttribute(typeof(ISI.Extensions.ConfigurationHelper.ConfigurationAttribute)) is ConfigurationAttribute configurationAttribute)
				{
					configurationSectionName = configurationAttribute.ConfigurationSectionName;
				}
				else
				{
					configurationSectionName = string.Format("{0}.{1}", configurationType.Namespace, configurationType.Name);
				}
			}

			var config = Activator.CreateInstance(configurationType);

			configuration.Bind(configurationSectionName, config);

			services?.AddSingleton(configurationType, config);
			serviceProvider?.RegisterInstance(configurationType, config, ServiceLifetime.Singleton);

			var configurationIOptionType = typeof(Microsoft.Extensions.Options.IOptions<>).MakeGenericType(configurationType);
			var configurationOptionType = typeof(Microsoft.Extensions.Options.OptionsWrapper<>).MakeGenericType(configurationType);

			serviceProvider?.Register(configurationIOptionType, configurationOptionType, ServiceLifetime.Singleton);
			services?.AddSingleton(configurationIOptionType, configurationOptionType);
		}
	}
}
