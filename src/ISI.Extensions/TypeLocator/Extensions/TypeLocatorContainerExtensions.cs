#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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

namespace ISI.Extensions.TypeLocator.Extensions
{
	public static class TypeLocatorContainerExtensions
	{
		public static Type[] GetImplementationTypes<TServiceType>(this ITypeLocatorContainer container) => container.GetImplementationTypes(typeof(TServiceType));


		public static object[] GetImplementations(this ITypeLocatorContainer container, Type serviceType)
		{
			return GetImplementations(container, serviceType, Activator.CreateInstance);
		}
		public static object[] GetImplementations(this ITypeLocatorContainer container, Type serviceType, System.IServiceProvider serviceProvider)
		{
			return GetImplementations(container, serviceType, serviceProvider.GetService);
		}
		public static object[] GetImplementations(this ITypeLocatorContainer container, Type serviceType, Func<Type, object> createItem)
		{
			var types = container.GetImplementationTypes(serviceType);

			return types.Select(createItem).ToArray();
		}


		public static TServiceType[] GetImplementations<TServiceType>(this ITypeLocatorContainer container)
		{
			return GetImplementations<TServiceType>(container, serviceType => (TServiceType)Activator.CreateInstance(serviceType));
		}
		public static TServiceType[] GetImplementations<TServiceType>(this ITypeLocatorContainer container, System.IServiceProvider serviceProvider)
		{
			return GetImplementations<TServiceType>(container, serviceType => (TServiceType)serviceProvider.GetService(serviceType));
		}
		public static TServiceType[] GetImplementations<TServiceType>(this ITypeLocatorContainer container, Func<Type, TServiceType> createItem)
		{
			var types = container.GetImplementationTypes(typeof(TServiceType));

			return types.Select(createItem).ToArray();
		}



		public static void ExecuteOnAll<TServiceType>(this ITypeLocatorContainer container, Action<TServiceType> action)
			where TServiceType : ISI.Extensions.TypeLocator.IExecute
		{
			ExecuteOnAll<TServiceType>(container, [action]);
		}
		public static void ExecuteOnAll<TServiceType>(this ITypeLocatorContainer container, System.IServiceProvider serviceProvider, Action<TServiceType> action)
			where TServiceType : ISI.Extensions.TypeLocator.IExecute
		{
			ExecuteOnAll<TServiceType>(container, serviceProvider, [action]);
		}
		public static void ExecuteOnAll<TServiceType>(this ITypeLocatorContainer container, Func<Type, TServiceType> createItem, Action<TServiceType> action)
			where TServiceType : ISI.Extensions.TypeLocator.IExecute
		{
			ExecuteOnAll<TServiceType>(container, createItem, [action]);
		}

		public static void ExecuteOnAll<TServiceType>(this ITypeLocatorContainer container, params Action<TServiceType>[] actions)
			where TServiceType : ISI.Extensions.TypeLocator.IExecute
		{
			ExecuteOnAll<TServiceType>(container, serviceType => (TServiceType)Activator.CreateInstance(serviceType), actions);
		}
		public static void ExecuteOnAll<TServiceType>(this ITypeLocatorContainer container, System.IServiceProvider serviceProvider, params Action<TServiceType>[] actions)
			where TServiceType : ISI.Extensions.TypeLocator.IExecute
		{
			ExecuteOnAll<TServiceType>(container, serviceType => (TServiceType)serviceProvider.GetService(serviceType), actions);
		}
		public static void ExecuteOnAll<TServiceType>(this ITypeLocatorContainer container, Func<Type, TServiceType> createItem, params Action<TServiceType>[] actions)
			where TServiceType : ISI.Extensions.TypeLocator.IExecute
		{
			foreach (var serviceType in container.GetImplementationTypes<TServiceType>())
			{
				var item = createItem(serviceType);

				foreach (var action in actions)
				{
					action(item);
				}
			}
		}
	}
}
