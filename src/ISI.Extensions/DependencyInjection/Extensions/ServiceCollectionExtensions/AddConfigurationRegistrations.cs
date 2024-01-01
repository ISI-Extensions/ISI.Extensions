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
using ISI.Extensions.Extensions;
using ISI.Extensions.TypeLocator.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.DependencyInjection.Extensions
{
	public static partial class ServiceCollectionExtensions
	{
		public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddConfigurationRegistrations(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			var dependencyInjectionConfiguration = new Configuration();
			configuration.Bind(ISI.Extensions.DependencyInjection.Configuration.ConfigurationSectionName, dependencyInjectionConfiguration);

			return AddConfigurationRegistrations(services, dependencyInjectionConfiguration);
		}

		public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddConfigurationRegistrations(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, ISI.Extensions.DependencyInjection.Configuration configuration)
		{
			foreach (var registration in configuration.Registrations.ToNullCheckedArray(NullCheckCollectionResult.Empty))
			{
				var serviceType = Type.GetType(registration.ServiceType);
				if (serviceType == null)
				{
					throw new(string.Format("type cannot be resolved: \"{0}\"", registration.ServiceType));
				}

				var mapToType = (string.IsNullOrWhiteSpace(registration.MapToType) ? (Type)null : Type.GetType(registration.MapToType));
				if (!string.IsNullOrWhiteSpace(registration.MapToType) && (mapToType == null))
				{
					throw new(string.Format("mapTo cannot be resolved: \"{0}\"", registration.MapToType));
				}

				ISI.Extensions.DependencyInjection.CreateService createService = null;

				if (registration.Constructor != null)
				{
					var injections = new List<InjectionParameter>();

					foreach (var constructorInjector in registration.Constructor.Injections.ToNullCheckedArray(ISI.Extensions.Extensions.NullCheckCollectionResult.Empty))
					{
						injections.Add(new()
						{
							ParameterName = constructorInjector.ParameterName,
							ParameterValue = constructorInjector.Value,
							ResolverName = constructorInjector.ResolverName,
							ServiceType = (string.IsNullOrWhiteSpace(constructorInjector.ServiceType) ? (Type)null : Type.GetType(constructorInjector.ServiceType)),
						});
					}

					createService = ExpressionBuilder.Create(mapToType ?? serviceType, injections.ToArray());
				}

				ISI.Extensions.DependencyInjection.BuildUpService buildUpService = null;

				if (registration.Properties.NullCheckedAny())
				{
					buildUpService = ExpressionBuilder.BuildUpService(mapToType ?? serviceType, registration.Properties.ToNullCheckedArray(property => new InjectionParameter()
					{
						ParameterName = property.PropertyName,
						ParameterValue = property.Value,
						ResolverName = property.ResolverName,
						ServiceType = (string.IsNullOrWhiteSpace(property.ServiceType) ? (Type)null : Type.GetType(property.ServiceType)),
					}));
				}

				if (createService == null)
				{
					services.Add(new(serviceType, mapToType, ISI.Extensions.DependencyInjection.Extensions.ServiceLifetimeExtensions.GetServiceLifetime(registration.ServiceLifetime)));
				}
				else
				{
					services.Add(new(serviceType, serviceProvider =>
					{
						if (serviceProvider is ISI.Extensions.DependencyInjection.IServiceProvider isiServiceProvider)
						{
							return createService(isiServiceProvider);
						}

						throw new NotImplementedException();
					}, ISI.Extensions.DependencyInjection.Extensions.ServiceLifetimeExtensions.GetServiceLifetime(registration.ServiceLifetime)));
				}
			}

			return services;
		}
	}
}
