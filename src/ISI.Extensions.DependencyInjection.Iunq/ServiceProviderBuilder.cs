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
using ISI.Extensions.DependencyInjection.Extensions;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.DependencyInjection.Iunq
{
	public class ServiceProviderBuilder : IServiceProviderBuilder
	{
		public System.IServiceProvider BuildServiceProvider(Microsoft.Extensions.DependencyInjection.IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration, ISI.Extensions.DependencyInjection.IContainer container = null)
		{
			var iunqConfiguration = new Configuration();
			configuration.Bind(Configuration.ConfigurationSectionName, iunqConfiguration);

			var dependencyInjectionConfiguration = new ISI.Extensions.DependencyInjection.Configuration();
			configuration.Bind(ISI.Extensions.DependencyInjection.Configuration.ConfigurationSectionName, dependencyInjectionConfiguration);

			ISI.Extensions.DependencyInjection.IServiceProvider serviceProvider = new ServiceProvider(iunqConfiguration, (container as Container) ?? new Container(iunqConfiguration));

			serviceProvider.RegisterInstance<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>(serviceProvider, Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton);

			AddRegistrations(services, serviceProvider);

			serviceProvider.SetServiceLocator();

			return serviceProvider;
		}

		public class OrderedServiceDescriptor
		{
			public int Index { get; }
			public Microsoft.Extensions.DependencyInjection.ServiceDescriptor ServiceDescriptor { get; }

			public OrderedServiceDescriptor(int index, Microsoft.Extensions.DependencyInjection.ServiceDescriptor serviceDescriptor)
			{
				Index = index;
				ServiceDescriptor = serviceDescriptor;
			}
		}

		private void AddRegistrations(Microsoft.Extensions.DependencyInjection.IServiceCollection services, ISI.Extensions.DependencyInjection.IServiceProvider serviceProvider)
		{
			var serviceDescriptors = new List<OrderedServiceDescriptor>();
			for (var index = 0; index < services.Count; index++)
			{
				serviceDescriptors.Add(new(index, services[index]));
			}


			foreach (var serviceTypeGroup in serviceDescriptors.GroupBy(s => s.ServiceDescriptor.ServiceType))
			{
				//Console.WriteLine(" adding \"{0}\"", serviceTypeGroup.Key.FullName);

				var registrations = serviceTypeGroup.ToList();

				var registrationMultiples = registrations.Where(r => r.ServiceDescriptor.ImplementationType != null).GroupBy(r => r.ServiceDescriptor.ImplementationType).Where(rg => rg.Count() > 1).Select(registrationsGroup => registrationsGroup.First()).ToList();
				foreach (var registrationMultiple in registrationMultiples)
				{
					registrations.RemoveAll(r => r.ServiceDescriptor.ServiceType == registrationMultiple.ServiceDescriptor.ServiceType && r.ServiceDescriptor.ImplementationType == registrationMultiple.ServiceDescriptor.ImplementationType);
					registrations.Add(registrationMultiple);
				}

				{
					var registration = registrations.OrderBy(r => r.Index).Last().ServiceDescriptor;
					
					if (registration.ImplementationFactory != null)
					{
						var registrationDeclaration = new RegistrationDeclarationByCreateService()
						{
							ServiceLifetime = registration.Lifetime,
							CreateService = provider => registration.ImplementationFactory(provider),
							BuildUpService = null,
						};

						serviceProvider.Register(null, registration.ServiceType, registrationDeclaration);

						if (registration.ImplementationInstance != null)
						{
							serviceProvider.RegisterInstance(null, registration.ServiceType, registration.ImplementationInstance, registration.Lifetime);
						}
					}
					else if (registration.ImplementationType != null)
					{
						var registrationDeclaration = new RegistrationDeclarationByMapToType()
						{
							ServiceLifetime = registration.Lifetime,
							MapToType = registration.ImplementationType,
							BuildUpService = null,
						};

						serviceProvider.Register(null, registration.ServiceType, registrationDeclaration);

						if (registration.ImplementationInstance != null)
						{
							serviceProvider.RegisterInstance(null, registration.ServiceType, registration.ImplementationInstance, registration.Lifetime);
						}
					}
					else if (registration.ImplementationInstance != null)
					{
						var registrationDeclaration = new RegistrationDeclarationByInstance()
						{
							ServiceLifetime = registration.Lifetime,
							Instance = registration.ImplementationInstance,
						};

						serviceProvider.Register(null, registration.ServiceType, registrationDeclaration);
					}
				}

				if(registrations.Count > 1)
				{
					var serviceType = typeof(IEnumerable<>).MakeGenericType(serviceTypeGroup.Key);
					var createServices = new List<CreateService>();

					foreach (var registration in registrations.OrderBy(r => r.Index).Select(r => r.ServiceDescriptor))
					{
						if (registration.ImplementationFactory != null)
						{
							createServices.Add(provider => registration.ImplementationFactory(provider));
						}
						else if (registration.ImplementationType != null)
						{
							createServices.Add(provider => provider.GetService(registration.ImplementationType));

							var registrationDeclaration = new RegistrationDeclarationByMapToType()
							{
								ServiceLifetime = registration.Lifetime,
								MapToType = registration.ImplementationType,
								BuildUpService = null,
							};

							serviceProvider.Register(null, registration.ImplementationType, registrationDeclaration);

							if (registration.ImplementationInstance != null)
							{
								serviceProvider.RegisterInstance(null, registration.ImplementationType, registration.ImplementationInstance, registration.Lifetime);
							}
						}
						else if (registration.ImplementationInstance != null)
						{
							createServices.Add(provider => registration.ImplementationInstance);
						}
					}

					{
						var registrationDeclaration = new RegistrationDeclarationByCreateService()
						{
							ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton,
							CreateService = provider => createServices.Select(createService => createService(provider)).ToArray(),
						};

						serviceProvider.Register(null, serviceType, registrationDeclaration);
					}
				}
			}
		}
	}
}
