#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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

namespace ISI.Platforms.Extensions
{
	public static class ServiceApplicationContextExtensions
	{
		public static void SetConfigurationRoot(this IServiceApplicationContextAddActions context, Microsoft.Extensions.Configuration.IConfigurationRoot configurationRoot)
		{
			context.ConfigurationRoot = configurationRoot;
		}

		public static void SetActiveEnvironment(this IServiceApplicationContextAddActions context, string activeEnvironment)
		{
			context.ActiveEnvironment = activeEnvironment;
		}

		public static void AddGetAddMessageBusSubscriptions(this IServiceApplicationContextAddActions context, ISI.Extensions.MessageBus.GetAddMessageBusSubscriptionsDelegate getAddMessageBusSubscriptions)
		{
			var _getAddMessageBusSubscriptions = context.GetAddMessageBusSubscriptions;
			context.GetAddMessageBusSubscriptions = () =>
			{
				var messageBusBuildRequests = new List<ISI.Extensions.MessageBus.IMessageBusBuildRequest>();

				if (_getAddMessageBusSubscriptions != null)
				{
					messageBusBuildRequests.AddRange(_getAddMessageBusSubscriptions.Invoke());
				}

				messageBusBuildRequests.AddRange(getAddMessageBusSubscriptions.Invoke());

				return messageBusBuildRequests;
			};
		}

		public static void AddHostBuilderConfigureServices(this IServiceApplicationContextAddActions context, ServiceApplicationContextHostBuilderConfigureServicesDelegate action)
		{
			var hostBuilderConfigureServices = context.HostBuilderConfigureServices;
			context.HostBuilderConfigureServices = (hostContext) =>
			{
				hostBuilderConfigureServices?.Invoke(hostContext);

				action(hostContext);
			};
		}

		/*
		public static void AddWebHostBuilderConfigureServices(this IServiceApplicationContextAddActions context, ServiceApplicationContextWebHostBuilderConfigureServicesDelegate action)
		{
			var webHostBuilderConfigureServices = context.WebHostBuilderConfigureServices;
			context.WebHostBuilderConfigureServices = (hostContext, services) =>
			{
				webHostBuilderConfigureServices?.Invoke(hostContext, services);

				action(hostContext, services);
			};
		}
		*/

		public static void AddWebStartupMvcBuilder(this IServiceApplicationContextAddActions context, ServiceApplicationContextWebStartupMvcBuilderDelegate action)
		{
			var webStartupMvcBuilder = context.WebStartupMvcBuilder;
			context.WebStartupMvcBuilder = mvcBuilder =>
			{
				webStartupMvcBuilder?.Invoke(mvcBuilder);

				action(mvcBuilder);
			};
		}

		public static void AddWebStartupConfigureServices(this IServiceApplicationContextAddActions context, ServiceApplicationContextWebStartupConfigureServicesDelegate action)
		{
			var webStartupConfigureServices = context.WebStartupConfigureServices;
			context.WebStartupConfigureServices = services =>
			{
				webStartupConfigureServices?.Invoke(services);

				action(services);
			};
		}

		public static void AddWebStartupUseEndpoints(this IServiceApplicationContextAddActions context, ServiceApplicationContextWebStartupUseEndpointsDelegate action)
		{
			var webStartupUseEndpoints = context.WebStartupUseEndpoints;
			context.WebStartupUseEndpoints = services =>
			{
				webStartupUseEndpoints?.Invoke(services);

				action(services);
			};
		}

		public static void AddConfigureWebApplication(this IServiceApplicationContextAddActions context, ServiceApplicationContextConfigureWebApplicationDelegate action)
		{
			var configureApplication = context.ConfigureWebApplication;
			context.ConfigureWebApplication = webApplication =>
			{
				configureApplication?.Invoke(webApplication);

				action(webApplication);
			};
		}

		public static void AddPreMessageBusBuild(this IServiceApplicationContextAddActions context, ServiceApplicationContextPreMessageBusBuildDelegate action)
		{
			var preMessageBusBuild = context.PreMessageBusBuild;
			context.PreMessageBusBuild = host =>
			{
				preMessageBusBuild?.Invoke(host);

				action(host);
			};
		}

		public static void AddPostStartup(this IServiceApplicationContextAddActions context, ServiceApplicationContextPostStartupDelegate action)
		{
			var postStartup = context.PostStartup;
			context.PostStartup = host =>
			{
				postStartup?.Invoke(host);

				action(host);
			};
		}
	}
}
