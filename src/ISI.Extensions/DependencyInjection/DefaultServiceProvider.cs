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
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.DependencyInjection
{
	public class DefaultServiceProvider : IServiceProvider
	{
		protected object Lock { get; } = new();

		protected IDictionary<Type, Type> KnownTypes { get; } = new Dictionary<Type, Type>();
		protected IDictionary<Type, Microsoft.Extensions.DependencyInjection.ServiceLifetime> LifeTimes { get; } = new Dictionary<Type, Microsoft.Extensions.DependencyInjection.ServiceLifetime>();

		protected IDictionary<Type, object> Singletons { get; } = new Dictionary<Type, object>();

		protected IDictionary<Type, Func<DefaultServiceProvider, object>> Creators { get; } = new Dictionary<Type, Func<DefaultServiceProvider, object>>();

		System.IServiceProvider IServiceScope.ServiceProvider => this;

		IServiceScope IServiceScopeFactory.CreateScope() => this;

		public void Register(string name, Type serviceType, IRegistrationDeclaration registrationDeclaration)
		{
			switch (registrationDeclaration)
			{
				case RegistrationDeclarationByCreateService registrationDeclarationByCreateService:
					lock (Lock)
					{
						if (Creators.ContainsKey(serviceType))
						{
							Creators.Remove(serviceType);
						}
						Creators.Add(serviceType, provider => registrationDeclarationByCreateService.CreateService(provider));

						if (LifeTimes.ContainsKey(serviceType))
						{
							LifeTimes.Remove(serviceType);
						}
						LifeTimes.Add(serviceType, registrationDeclarationByCreateService.ServiceLifetime);
					}
					break;

				case RegistrationDeclarationByInstance registrationDeclarationByInstance:
					lock (Lock)
					{
						if (KnownTypes.ContainsKey(serviceType))
						{
							KnownTypes.Remove(serviceType);
						}
						KnownTypes.Add(serviceType, registrationDeclarationByInstance.Instance.GetType());

						Singletons.Add(serviceType, registrationDeclarationByInstance.Instance);

						if (LifeTimes.ContainsKey(serviceType))
						{
							LifeTimes.Remove(serviceType);
						}
						LifeTimes.Add(serviceType, ServiceLifetime.Singleton);
					}
					break;

				case RegistrationDeclarationByMapToType registrationDeclarationByMapToType:
					lock (Lock)
					{
						if (KnownTypes.ContainsKey(serviceType))
						{
							KnownTypes.Remove(serviceType);
						}
						KnownTypes.Add(serviceType, registrationDeclarationByMapToType.MapToType);

						if (LifeTimes.ContainsKey(serviceType))
						{
							LifeTimes.Remove(serviceType);
						}
						LifeTimes.Add(serviceType, registrationDeclarationByMapToType.ServiceLifetime);
					}
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(registrationDeclaration));
			}
		}

		public object GetService(string name, Type serviceType, bool isRequired, Func<IRegistrationDeclaration> getRegistrationDeclaration)
		{
			if (Singletons.TryGetValue(serviceType, out var service))
			{
				return service;
			}

			if (!KnownTypes.TryGetValue(serviceType, out var knownType))
			{
				if (getRegistrationDeclaration == null)
				{
					knownType = serviceType;

					KnownTypes.Add(serviceType, knownType);
				}
				else
				{
					Register(name, serviceType, getRegistrationDeclaration());

					return GetService(name, serviceType, isRequired, null);
				}
			}

			if (!LifeTimes.TryGetValue(serviceType, out var serviceLifetime))
			{
				serviceLifetime = ServiceLifetime.Transient;
				LifeTimes.Add(serviceType, serviceLifetime);
			}

			try
			{
				switch (serviceLifetime)
				{
					case ServiceLifetime.Singleton:
					{
						var instance = CreateInstance(knownType);

						Singletons.Add(serviceType, instance);

						return instance;
					}

					case ServiceLifetime.Scoped:
					case ServiceLifetime.Transient:
						return CreateInstance(knownType);

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			catch (Exception exception)
			{
				throw new CannotCreateInstanceException(knownType, exception);
			}

		}

		private object CreateInstance(Type type)
		{
			object result = null;

			var container = System.Linq.Expressions.Expression.Parameter(this.GetType(), "container");

			if (!type.IsGenericTypeDefinition)
			{
				Func<DefaultServiceProvider, object> creator = null;

				try
				{
					try
					{
						if (!Creators.TryGetValue(type, out creator))
						{
							lock (Lock)
							{
								if (!Creators.TryGetValue(type, out creator))
								{
									var constructors = type.GetConstructors().ToList();

									var constructor = constructors.OrderByDescending(cstr => cstr.GetParameters().Length).FirstOrDefault();

									if ((constructor != null) && (constructor.GetParameters().Length != 0))
									{
										var parameters = new List<System.Linq.Expressions.Expression>();

										foreach (var parameter in constructor.GetParameters())
										{
											parameters.Add(System.Linq.Expressions.Expression.Call(container, "GetService", new Type[] { parameter.ParameterType }, System.Linq.Expressions.Expression.Constant(parameter.ParameterType, typeof(Type))));
										}

										var create = System.Linq.Expressions.Expression.New(constructor, parameters);

										creator = System.Linq.Expressions.Expression.Lambda<Func<DefaultServiceProvider, object>>(create, new[] { container }).Compile();
									}
									else
									{
										creator = resolver => Activator.CreateInstance(type);
									}

									Creators.Add(type, creator);
								}
							}
						}

						result = creator(this);
					}
#pragma warning disable CS0168 // Variable is declared but never used
					catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
					{
						result = Activator.CreateInstance(type);

						lock (Lock)
						{
							if (Creators.ContainsKey(type))
							{
								Creators[type] = resolver => Activator.CreateInstance(type);
							}
							else
							{
								Creators.Add(type, resolver => Activator.CreateInstance(type));
							}
						}
					}

				}
				catch (Exception exception)
				{
					throw new CannotCreateInstanceException(type, exception);
				}
			}

			return result ?? Activator.CreateInstance(type);
		}


		object System.IServiceProvider.GetService(Type serviceType) => GetService(null, serviceType, false, null);
		object ISupportRequiredService.GetRequiredService(Type serviceType) => GetService(null, serviceType, true, null);
		TService IServiceProvider.GetService<TService>(string name, bool isRequired, Func<IRegistrationDeclaration> getRegistrationDeclaration) => GetService(name, typeof(TService), isRequired, getRegistrationDeclaration) as TService;


		bool IServiceProvider.IsRegistered(string name, Type serviceType)
		{
			throw new NotImplementedException();
		}

		void IDisposable.Dispose()
		{
		}
	}
}
