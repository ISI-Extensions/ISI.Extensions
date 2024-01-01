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

namespace ISI.Extensions.DependencyInjection.Extensions
{
	public static partial class ServiceProviderExtensions
	{
		private static object BuildService(System.IServiceProvider serviceProvider, Func<IRegistrationDeclaration> getRegistrationDeclaration)
		{
			var registrationDeclaration = getRegistrationDeclaration();
			switch (registrationDeclaration)
			{
				case RegistrationDeclarationByInstance registrationDeclarationByInstance:
					return registrationDeclarationByInstance.Instance;

				case RegistrationDeclarationByMapToType registrationDeclarationByMapToType:
					if (registrationDeclarationByMapToType.BuildUpService == null)
					{
						return serviceProvider.GetService(registrationDeclarationByMapToType.MapToType);
					}
					break;
			}

			return null;
		}

		public static object GetService(this System.IServiceProvider serviceProvider, Type serviceType, Func<IRegistrationDeclaration> getRegistrationDeclaration)
		{
			if (serviceProvider is ISI.Extensions.DependencyInjection.IServiceProvider isiServiceProvider)
			{
				return isiServiceProvider.GetService(null, serviceType, false, getRegistrationDeclaration);
			}

			var service = serviceProvider.GetService(serviceType) ?? BuildService(serviceProvider, getRegistrationDeclaration);
			if (service != null)
			{
				return service;
			}

			throw new CannotInvokeForThisTypeOfServiceProviderException(serviceProvider);
		}

		public static object GetService(this System.IServiceProvider serviceProvider, string name, Type serviceType)
		{
			if (serviceProvider is ISI.Extensions.DependencyInjection.IServiceProvider isiServiceProvider)
			{
				return isiServiceProvider.GetService(name, serviceType, false, null);
			}

			var service = serviceProvider.GetService(name, serviceType);
			if (service != null)
			{
				return service;
			}

			throw new CannotInvokeForThisTypeOfServiceProviderException(serviceProvider);
		}

		public static object GetService(this System.IServiceProvider serviceProvider, Type serviceType)
		{
			if (serviceProvider is ISI.Extensions.DependencyInjection.IServiceProvider isiServiceProvider)
			{
				return isiServiceProvider.GetService(null, serviceType, false, null);
			}

			var service = serviceProvider.GetService(serviceType);
			if (service != null)
			{
				return service;
			}

			throw new CannotInvokeForThisTypeOfServiceProviderException(serviceProvider);
		}

		//public static TService GetService<TService>(this System.IServiceProvider serviceProvider)
		//	where TService : class
		//{
		//	if (serviceProvider is ISI.Extensions.DependencyInjection.IServiceProvider isiServiceProvider)
		//	{
		//		return isiServiceProvider.GetService(null, typeof(TService), false, null) as TService;
		//	}

		//	throw new CannotInvokeForThisTypeOfServiceProviderException(serviceProvider);
		//}

		public static TService GetService<TService>(this System.IServiceProvider serviceProvider, Func<IRegistrationDeclaration> getRegistrationDeclaration)
			where TService : class
		{
			if (serviceProvider is ISI.Extensions.DependencyInjection.IServiceProvider isiServiceProvider)
			{
				return isiServiceProvider.GetService(null, typeof(TService), false, getRegistrationDeclaration) as TService;
			}

			if ((serviceProvider.GetService(typeof(TService)) ?? BuildService(serviceProvider, getRegistrationDeclaration)) is TService service)
			{
				return service;
			}

			throw new CannotInvokeForThisTypeOfServiceProviderException(serviceProvider);
		}

		public static TService GetService<TService>(this System.IServiceProvider serviceProvider, Type serviceType, Func<IRegistrationDeclaration> getRegistrationDeclaration)
			where TService : class
		{
			if (serviceProvider is ISI.Extensions.DependencyInjection.IServiceProvider isiServiceProvider)
			{
				return isiServiceProvider.GetService(null, serviceType, false, getRegistrationDeclaration) as TService;
			}

			if ((serviceProvider.GetService(serviceType) ?? BuildService(serviceProvider, getRegistrationDeclaration)) is TService service)
			{
				return service;
			}

			throw new CannotInvokeForThisTypeOfServiceProviderException(serviceProvider);
		}
	}
}
