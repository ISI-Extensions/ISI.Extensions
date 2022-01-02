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

namespace ISI.Extensions.DependencyInjection
{
	public class ExpressionBuilder
	{
		public static ISI.Extensions.DependencyInjection.CreateService Create(Type mapTo, InjectionParameter[] injections = null)
		{
			System.Linq.Expressions.ParameterExpression serviceProvider;
			serviceProvider = System.Linq.Expressions.Expression.Parameter(typeof(ISI.Extensions.DependencyInjection.IServiceProvider), nameof(serviceProvider));

			if (!mapTo.IsGenericTypeDefinition && !mapTo.ContainsGenericParameters)
			{
				var constructors = mapTo.GetConstructors().ToList();

				var constructorsWithImportingConstructorAttribute = constructors.Where(c => c.GetCustomAttributes(typeof(ISI.Extensions.DependencyInjection.ImportingConstructorAttribute), false).Any()).ToList();

				if (constructorsWithImportingConstructorAttribute.Any())
				{
					constructors = constructorsWithImportingConstructorAttribute;
				}

				if (injections == null)
				{
					injections = new InjectionParameter[] { };
				}
				else
				{
					constructors.RemoveAll(constructor => constructor.GetParameters().Length != injections.Length);
				}

				foreach (var injection in injections)
				{
					constructors.RemoveAll(constructor => !constructor.GetParameters().Any(parameter => string.Equals(parameter.Name, injection.ParameterName)));
				}

				constructors.RemoveAll(constructor => constructor.GetParameters().Any(parameter => (!parameter.ParameterType.IsClass && !parameter.ParameterType.IsInterface && !injections.Any(injection => string.Equals(injection.ParameterName, parameter.Name)))));

				var constructor = constructors.OrderByDescending(constructor => constructor.GetParameters().Length).FirstOrDefault();

				if (constructor != null)
				{
					var parameters = new List<System.Linq.Expressions.Expression>();

					foreach (var parameter in constructor.GetParameters())
					{
						var injection = injections.FirstOrDefault(i => string.Equals(i.ParameterName, parameter.Name));

						if (injection == null)
						{
							parameters.Add(System.Linq.Expressions.Expression.Call(serviceProvider, nameof(IServiceProvider.GetService), new Type[] { parameter.ParameterType }, System.Linq.Expressions.Expression.Constant(null, typeof(string)), System.Linq.Expressions.Expression.Constant(true, typeof(bool)), System.Linq.Expressions.Expression.Constant(null, typeof(Func<ISI.Extensions.DependencyInjection.IRegistrationDeclaration>))));
						}
						else
						{
							if (string.IsNullOrWhiteSpace(injection.ParameterValue))
							{
								if (string.IsNullOrWhiteSpace(injection.ResolverName))
								{
									parameters.Add(System.Linq.Expressions.Expression.Call(serviceProvider, nameof(IServiceProvider.GetService), new Type[] { injection.ServiceType ?? parameter.ParameterType }, System.Linq.Expressions.Expression.Constant(null, typeof(string)), System.Linq.Expressions.Expression.Constant(true, typeof(bool)), System.Linq.Expressions.Expression.Constant(null, typeof(Func<ISI.Extensions.DependencyInjection.IRegistrationDeclaration>))));
								}
								else
								{
									parameters.Add(System.Linq.Expressions.Expression.Call(serviceProvider, nameof(IServiceProvider.GetService), new Type[] { injection.ServiceType ?? parameter.ParameterType }, System.Linq.Expressions.Expression.Constant(injection.ResolverName), System.Linq.Expressions.Expression.Constant(true, typeof(bool)), System.Linq.Expressions.Expression.Constant(null, typeof(Func<ISI.Extensions.DependencyInjection.IRegistrationDeclaration>))));
								}
							}
							else
							{
								parameters.Add(System.Linq.Expressions.Expression.Constant(Convert.ChangeType(injection.ParameterValue, injection.ServiceType ?? parameter.ParameterType)));
							}
						}
					}

					var createService = System.Linq.Expressions.Expression.New(constructor, parameters);

					return System.Linq.Expressions.Expression.Lambda<ISI.Extensions.DependencyInjection.CreateService>(createService, new[] { serviceProvider }).Compile();
				}
			}

			return null;
		}

		public static Action<ISI.Extensions.DependencyInjection.IServiceProvider, T> BuildUp<T>(InjectionParameter[] injections = null)
		{
			if (injections != null)
			{
				System.Linq.Expressions.ParameterExpression serviceProvider;
				serviceProvider = System.Linq.Expressions.Expression.Parameter(typeof(ISI.Extensions.DependencyInjection.IServiceProvider), nameof(serviceProvider));

				System.Linq.Expressions.ParameterExpression instance;
				instance = System.Linq.Expressions.Expression.Parameter(typeof(T), nameof(instance));

				var expressions = new List<System.Linq.Expressions.Expression>();

				expressions.AddRange(BuildUp(serviceProvider, instance, injections));

				var buildUp = System.Linq.Expressions.Expression.Block(expressions);

				return System.Linq.Expressions.Expression.Lambda<Action<ISI.Extensions.DependencyInjection.IServiceProvider, T>>(buildUp, new[] { serviceProvider, instance }).Compile();
			}

			return null;
		}

		public static ISI.Extensions.DependencyInjection.BuildUpService BuildUpService(Type serviceType, InjectionParameter[] injections = null)
		{
			if (injections != null)
			{
				System.Linq.Expressions.ParameterExpression serviceProvider;
				serviceProvider = System.Linq.Expressions.Expression.Parameter(typeof(ISI.Extensions.DependencyInjection.IServiceProvider), nameof(serviceProvider));

				System.Linq.Expressions.ParameterExpression unTypedInstance;
				unTypedInstance = System.Linq.Expressions.Expression.Parameter(typeof(object), nameof(unTypedInstance));

				var expressions = new List<System.Linq.Expressions.Expression>();

				System.Linq.Expressions.ParameterExpression instance;
				instance = System.Linq.Expressions.Expression.Variable(serviceType, nameof(instance));

				expressions.Add(System.Linq.Expressions.Expression.Assign(instance, System.Linq.Expressions.Expression.Convert(unTypedInstance, serviceType)));

				expressions.AddRange(BuildUp(serviceProvider, instance, injections));

				var buildUpService = System.Linq.Expressions.Expression.Block(new[] { instance }, expressions);

				return System.Linq.Expressions.Expression.Lambda<ISI.Extensions.DependencyInjection.BuildUpService>(buildUpService, new[] { serviceProvider, unTypedInstance }).Compile();
			}

			return null;
		}

		private static IEnumerable<System.Linq.Expressions.Expression> BuildUp(System.Linq.Expressions.Expression serviceProvider, System.Linq.Expressions.Expression instance, InjectionParameter[] injections = null)
		{
			var expressions = new List<System.Linq.Expressions.Expression>();

			foreach (var injection in injections)
			{
				var property = System.Linq.Expressions.Expression.PropertyOrField(instance, injection.ParameterName);

				if (string.IsNullOrWhiteSpace(injection.ParameterValue))
				{
					if (string.IsNullOrWhiteSpace(injection.ResolverName))
					{
						expressions.Add(System.Linq.Expressions.Expression.Assign(property, System.Linq.Expressions.Expression.Call(serviceProvider, nameof(IServiceProvider.GetService), new Type[] { injection.ServiceType ?? property.Type }, System.Linq.Expressions.Expression.Constant(null, typeof(string)), System.Linq.Expressions.Expression.Constant(true, typeof(bool)), System.Linq.Expressions.Expression.Constant(null, typeof(Func<ISI.Extensions.DependencyInjection.IRegistrationDeclaration>)))));
					}
					else
					{
						expressions.Add(System.Linq.Expressions.Expression.Assign(property, System.Linq.Expressions.Expression.Call(serviceProvider, nameof(IServiceProvider.GetService), new Type[] { injection.ServiceType ?? property.Type }, System.Linq.Expressions.Expression.Constant(injection.ResolverName), System.Linq.Expressions.Expression.Constant(true, typeof(bool)), System.Linq.Expressions.Expression.Constant(null, typeof(Func<ISI.Extensions.DependencyInjection.IRegistrationDeclaration>)))));
					}
				}
				else
				{
					expressions.Add(System.Linq.Expressions.Expression.Assign(property, System.Linq.Expressions.Expression.Constant(Convert.ChangeType(injection.ParameterValue, injection.ServiceType ?? property.Type))));
				}
			}

			return expressions;
		}
	}
}
