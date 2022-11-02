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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.DependencyInjection.Iunq
{
	public class ServiceProvider : ISI.Extensions.DependencyInjection.IServiceProvider
	{
		protected Container Container { get; }

		protected Configuration Configuration { get; }

		public ServiceCache UnnamedScopedServiceCache { get; }
		public IDictionary<string, ServiceCache> NamedScopedServiceCaches { get; }

		public ServiceProvider(Configuration configuration)
			: this(configuration, new(configuration))
		{

		}
		internal ServiceProvider(Configuration configuration, Container container)
		{
			Configuration = configuration;

			Container = container;

			UnnamedScopedServiceCache = new(configuration);
			NamedScopedServiceCaches = new System.Collections.Concurrent.ConcurrentDictionary<string, ServiceCache>(Environment.ProcessorCount * 2, Configuration.NamedServiceTypeBuilderDictionaryInitialSize);
		}

		System.IServiceProvider Microsoft.Extensions.DependencyInjection.IServiceScope.ServiceProvider => this;

		Microsoft.Extensions.DependencyInjection.IServiceScope Microsoft.Extensions.DependencyInjection.IServiceScopeFactory.CreateScope() => new ServiceProvider(Configuration, Container);

		object Microsoft.Extensions.DependencyInjection.ISupportRequiredService.GetRequiredService(Type serviceType) => GetService(null, serviceType, true, null);

		object System.IServiceProvider.GetService(Type serviceType) => GetService(null, serviceType, false, null);

		void ISI.Extensions.DependencyInjection.IServiceProvider.Register(string name, Type serviceType, ISI.Extensions.DependencyInjection.IRegistrationDeclaration registrationDeclaration) => Register(name, serviceType, registrationDeclaration);




		private (ServiceTypeBuilder ServiceTypeBuilder, object Instance) Register(string name, Type serviceType, ISI.Extensions.DependencyInjection.IRegistrationDeclaration registrationDeclaration)
		{
			ServiceTypeBuilder serviceTypeBuilder = null;
			object instance = null;

			switch (registrationDeclaration)
			{
				case RegistrationDeclarationByInstance registrationDeclarationByInstance:
					instance = registrationDeclarationByInstance.Instance;
					break;
				case RegistrationDeclarationByMapToType registrationDeclarationByMapToType:
					if (serviceType.IsGenericTypeDefinition && serviceType.ContainsGenericParameters)
					{
						var genericServiceTypeMapSet = Container.GetOrCreateGenericServiceTypeMaps(name, serviceType);

						if (genericServiceTypeMapSet != null)
						{
							genericServiceTypeMapSet.GenericServiceTypeRegistrationDeclarations.Remove(serviceType);
							genericServiceTypeMapSet.GenericServiceTypeRegistrationDeclarations.Add(serviceType, registrationDeclaration);
						}
					}
					else
					{
						serviceTypeBuilder = new(serviceType, ExpressionBuilder.Create(registrationDeclarationByMapToType.MapToType), registrationDeclarationByMapToType.BuildUpService ?? ((serviceProvider, _) => { }), registrationDeclaration.ServiceLifetime);
					}
					break;
				case RegistrationDeclarationByCreateService registrationDeclarationByCreateService:
					serviceTypeBuilder = new(serviceType, registrationDeclarationByCreateService.CreateService ?? ExpressionBuilder.Create(serviceType), registrationDeclarationByCreateService.BuildUpService ?? ((serviceProvider, _) => { }), registrationDeclaration.ServiceLifetime);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(registrationDeclaration));
			}

			if (string.IsNullOrEmpty(name))
			{
				UnnamedScopedServiceCache.Expire(serviceType);
				Container.UnnamedSingletonServiceCache.Expire(serviceType);

				if (instance != null)
				{
					switch (registrationDeclaration.ServiceLifetime)
					{
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped:
							UnnamedScopedServiceCache.Register(serviceType, instance);
							break;
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton:
							Container.UnnamedSingletonServiceCache.Register(serviceType, instance);
							break;
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient:
							throw new InvalidServiceLifetimeException(registrationDeclaration.ServiceLifetime);
					}
				}

				Container.UnnamedServiceTypeBuilders.Remove(serviceType);
				if (serviceTypeBuilder != null)
				{
					Container.UnnamedServiceTypeBuilders.Add(serviceTypeBuilder);
				}
			}
			else
			{
				if (Container.NamedSingletonServiceCaches.TryGetValue(name, out var namedSingletonServiceCache))
				{
					namedSingletonServiceCache.Expire(serviceType);
				}

				if (NamedScopedServiceCaches.TryGetValue(name, out var namedScopedServiceCache))
				{
					namedScopedServiceCache.Expire(serviceType);
				}

				if (instance != null)
				{
					switch (registrationDeclaration.ServiceLifetime)
					{
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped:
							UnnamedScopedServiceCache.Register(serviceType, instance);
							break;
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton:
							Container.UnnamedSingletonServiceCache.Register(serviceType, instance);
							break;
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient:
							throw new InvalidServiceLifetimeException(registrationDeclaration.ServiceLifetime);
					}
				}

				if (!Container.NamedServiceTypeBuilders.TryGetValue(name, out var namedServiceTypeBuilder))
				{
					Container.NamedServiceTypeBuilders.Add(name, (namedServiceTypeBuilder = new(Configuration)));
				}

				namedServiceTypeBuilder.Remove(serviceType);
				if (serviceTypeBuilder != null)
				{
					namedServiceTypeBuilder.Add(serviceTypeBuilder);
				}
			}

			return (ServiceTypeBuilder: serviceTypeBuilder, Instance: instance);
		}


		public object GetService(string name, Type serviceType, bool isRequired, Func<ISI.Extensions.DependencyInjection.IRegistrationDeclaration> getRegistrationDeclaration)
		{
			ServiceTypeBuilder serviceTypeBuilder = null;
			object instance = null;

			void createIfMissingServiceTypeBuilder(ServiceTypeBuilderDictionary serviceTypeBuilders)
			{
				if (!serviceTypeBuilders.TryGetValue(serviceType, out serviceTypeBuilder))
				{
					var registrationDeclaration = getRegistrationDeclaration?.Invoke();

					if ((registrationDeclaration == null) && serviceType.IsConstructedGenericType)
					{
						var genericServiceTypeMapSet = Container.GetOrCreateGenericServiceTypeMaps(name, serviceType);

						if ((genericServiceTypeMapSet?.GenericServiceTypeRegistrationDeclarations).NullCheckedAny())
						{
							switch (genericServiceTypeMapSet.GenericServiceTypeRegistrationDeclarations.First().Value)
							{
								case RegistrationDeclarationByCreateService registrationDeclarationByCreateService:
									registrationDeclaration = new RegistrationDeclarationByCreateService()
									{
										ServiceLifetime = registrationDeclarationByCreateService.ServiceLifetime,
										CreateService = registrationDeclarationByCreateService.CreateService,
										BuildUpService = registrationDeclarationByCreateService.BuildUpService,
									};
									break;

								case RegistrationDeclarationByInstance registrationDeclarationByInstance:
									break;

								case RegistrationDeclarationByMapToType registrationDeclarationByMapToType:
									var mapToType = registrationDeclarationByMapToType.MapToType.MakeGenericType(serviceType.GenericTypeArguments);

									registrationDeclaration = new RegistrationDeclarationByMapToType()
									{
										ServiceLifetime = genericServiceTypeMapSet.GenericServiceTypeRegistrationDeclarations.First().Value.ServiceLifetime,
										MapToType = mapToType,
									};
									break;

								default:
									throw new ArgumentOutOfRangeException();
							}
						}
					}

					if ((registrationDeclaration == null) && !serviceType.IsInterface && serviceType.IsGenericTypeDefinition && !serviceType.ContainsGenericParameters)
					{
						registrationDeclaration = new RegistrationDeclarationByMapToType()
						{
							ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient,
							MapToType = serviceType,
						};
					}

					if ((registrationDeclaration == null) && !serviceType.IsInterface)
					{
						registrationDeclaration = new RegistrationDeclarationByMapToType()
						{
							ServiceLifetime = Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient,
							MapToType = serviceType,
						};
					}


					if (registrationDeclaration != null)
					{
						serviceTypeBuilder = Register(name, serviceType, registrationDeclaration).ServiceTypeBuilder;
					}
				}
			}

			if (string.IsNullOrEmpty(name))
			{
				if (UnnamedScopedServiceCache.TryGetInstance(serviceType, out instance))
				{
					return instance;
				}

				if (Container.UnnamedSingletonServiceCache.TryGetInstance(serviceType, out instance))
				{
					return instance;
				}

				createIfMissingServiceTypeBuilder(Container.UnnamedServiceTypeBuilders);

				if (serviceTypeBuilder != null)
				{
					instance = serviceTypeBuilder.Create(this);

					switch (serviceTypeBuilder.ServiceLifetime)
					{
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton:
							Container.UnnamedSingletonServiceCache.Register(serviceType, instance);
							break;
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped:
							UnnamedScopedServiceCache.Register(serviceType, instance);
							break;
					}

					return instance;
				}
			}
			else
			{
				if (NamedScopedServiceCaches.TryGetValue(name, out var namedScopedServiceCache) && namedScopedServiceCache.TryGetInstance(serviceType, out instance))
				{
					return instance;
				}

				if (Container.NamedSingletonServiceCaches.TryGetValue(name, out var namedSingletonServiceCache) && namedSingletonServiceCache.TryGetInstance(serviceType, out instance))
				{
					return instance;
				}

				if (!Container.NamedServiceTypeBuilders.TryGetValue(name, out var namedServiceTypeBuilders))
				{
					Container.NamedServiceTypeBuilders.Add(name, (namedServiceTypeBuilders = new(Configuration)));
				}

				createIfMissingServiceTypeBuilder(namedServiceTypeBuilders);

				if (serviceTypeBuilder != null)
				{
					instance = serviceTypeBuilder.Create(this);

					switch (serviceTypeBuilder.ServiceLifetime)
					{
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton:
							if (namedSingletonServiceCache == null)
							{
								Container.NamedSingletonServiceCaches.Add(name, (namedSingletonServiceCache = new(Configuration)));
							}
							namedSingletonServiceCache.Register(serviceType, instance);
							break;
						case Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped:
							if (namedScopedServiceCache == null)
							{
								NamedScopedServiceCaches.Add(name, (namedScopedServiceCache = new(Configuration)));
							}
							namedScopedServiceCache.Register(serviceType, instance);
							break;
					}

					return instance;
				}
			}

			if (serviceType.IsConstructedGenericType && (serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)) && (serviceType.GenericTypeArguments.Length == 1))
			{
				var enumeratedServiceType = serviceType.GenericTypeArguments.First();

				var service = GetService(name, enumeratedServiceType, false, null);

				if (service == null)
				{
					instance =  Array.CreateInstance(enumeratedServiceType, 0);
				}
				else
				{
					instance = Array.CreateInstance(enumeratedServiceType, 1);
					((Array) instance).SetValue(service, 0);
				}

				return instance;
			}

			if (isRequired)
			{
				throw new CannotCreateInstanceException(serviceType, null);
			}

#if DEBUG
			Console.WriteLine("serviceType not found \"{0}\"", serviceType.FullName);
#endif

			return null;
		}

		public TService GetService<TService>(string name, bool isRequired, Func<ISI.Extensions.DependencyInjection.IRegistrationDeclaration> getRegistrationDeclaration)
			where TService : class
		{
			return GetService(name, typeof(TService), isRequired, getRegistrationDeclaration) as TService;
		}

		#region IsRegistered
		public bool IsRegistered(string name, Type serviceType)
		{
			if (string.IsNullOrEmpty(name))
			{
				if (UnnamedScopedServiceCache.HasInstance(serviceType))
				{
					return true;
				}
				if (Container.UnnamedSingletonServiceCache.HasInstance(serviceType))
				{
					return true;
				}
				if (Container.UnnamedServiceTypeBuilders.ContainsKey(serviceType))
				{
					return true;
				}

				return false;
			}

			if (NamedScopedServiceCaches.TryGetValue(name, out var namedScopedServiceCache) && namedScopedServiceCache.HasInstance(serviceType))
			{
				return true;
			}
			if (Container.NamedSingletonServiceCaches.TryGetValue(name, out var namedSingletonServiceCache) && namedSingletonServiceCache.HasInstance(serviceType))
			{
				return true;
			}
			if (Container.NamedServiceTypeBuilders.TryGetValue(name, out var namedServiceTypeBuilder) && namedServiceTypeBuilder.ContainsKey(serviceType))
			{
				return true;
			}

			return false;
		}

		public bool IsRegistered<TService>(string name = null)
		{
			return IsRegistered(name, typeof(TService));
		}
		#endregion

		public void Dispose()
		{
			UnnamedScopedServiceCache?.Dispose();

			foreach (var namedScopedServiceCache in NamedScopedServiceCaches)
			{
				namedScopedServiceCache.Value?.Dispose();
			}
		}
	}
}
