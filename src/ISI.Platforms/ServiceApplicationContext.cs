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
using ISI.Extensions.Extensions;

namespace ISI.Platforms
{
	public delegate void ServiceApplicationContextHostBuilderConfigureServicesDelegate(Microsoft.Extensions.Hosting.IHostApplicationBuilder hostApplicationBuilder);
	public delegate void ServiceApplicationContextWebStartupMvcBuilderDelegate(Microsoft.Extensions.DependencyInjection.IMvcBuilder mvcBuilder);
	public delegate void ServiceApplicationContextWebStartupConfigureServicesDelegate(Microsoft.Extensions.DependencyInjection.IServiceCollection services);
	public delegate void ServiceApplicationContextWebStartupUseEndpointsDelegate(Microsoft.AspNetCore.Routing.IEndpointRouteBuilder endpointRouteBuilder);
	public delegate void ServiceApplicationContextConfigureWebApplicationDelegate(Microsoft.AspNetCore.Builder.WebApplication webApplication);
	public delegate void ServiceApplicationContextPreMessageBusBuildDelegate(Microsoft.Extensions.Hosting.IHost host);
	public delegate void ServiceApplicationContextPostStartupDelegate(Microsoft.Extensions.Hosting.IHost host);

	public interface IServiceApplicationContextAddActions
	{
		Microsoft.Extensions.Configuration.IConfigurationRoot ConfigurationRoot { get; set; }

		string ActiveEnvironment { get; set; }

		ISI.Extensions.MessageBus.GetAddMessageBusSubscriptionsDelegate GetAddMessageBusSubscriptions { get; set; }

		ServiceApplicationContextHostBuilderConfigureServicesDelegate HostBuilderConfigureServices { get; set; }

		ServiceApplicationContextWebStartupMvcBuilderDelegate WebStartupMvcBuilder { get; set; }
		ServiceApplicationContextWebStartupConfigureServicesDelegate WebStartupConfigureServices { get; set; }
		ServiceApplicationContextWebStartupUseEndpointsDelegate WebStartupUseEndpoints { get; set; }

		ServiceApplicationContextConfigureWebApplicationDelegate ConfigureWebApplication { get; set; }

		ServiceApplicationContextPreMessageBusBuildDelegate PreMessageBusBuild { get; set; }

		ServiceApplicationContextPostStartupDelegate PostStartup { get; set; }
	}

	public class ServiceApplicationContext : IServiceApplicationContextAddActions
	{
		public const string RunningAsServiceOption = "--runningAsService";

		public Type RootType { get; }
		public System.Reflection.Assembly RootAssembly { get; }

		public Microsoft.Extensions.Hosting.IHost Host { get; set; }

		public Microsoft.Extensions.Configuration.IConfigurationRoot ConfigurationRoot { get; private set; }
		Microsoft.Extensions.Configuration.IConfigurationRoot IServiceApplicationContextAddActions.ConfigurationRoot { get => ConfigurationRoot; set => ConfigurationRoot = value; }

		public ISI.Platforms.ILoggerConfigurator LoggerConfigurator { get; set; }

		public System.Diagnostics.TraceListener TraceListener { get; set; }

		public string ActiveEnvironment { get; private set; }
		string IServiceApplicationContextAddActions.ActiveEnvironment { get => ActiveEnvironment; set => ActiveEnvironment = value; }

		private string[] _args = null;

		public string[] Args
		{
			get => _args ?? [];
			set
			{
				_args = value;
				CommandLineArguments = new(value);
			}
		}

		public bool RunningAsService => !Environment.UserInteractive || Args.NullCheckedAny(arg => string.Equals(arg, RunningAsServiceOption));

		public ISI.Extensions.CommandLineArguments CommandLineArguments { get; private set; }

		public ISI.Extensions.MessageBus.GetAddMessageBusSubscriptionsDelegate GetAddMessageBusSubscriptions { get; private set; }
		ISI.Extensions.MessageBus.GetAddMessageBusSubscriptionsDelegate IServiceApplicationContextAddActions.GetAddMessageBusSubscriptions { get => GetAddMessageBusSubscriptions; set => GetAddMessageBusSubscriptions = value; }

		public ServiceApplicationContextHostBuilderConfigureServicesDelegate HostBuilderConfigureServices { get; private set; }
		ServiceApplicationContextHostBuilderConfigureServicesDelegate IServiceApplicationContextAddActions.HostBuilderConfigureServices { get => HostBuilderConfigureServices; set => HostBuilderConfigureServices = value; }

		public ServiceApplicationContextWebStartupMvcBuilderDelegate WebStartupMvcBuilder { get; private set; }
		ServiceApplicationContextWebStartupMvcBuilderDelegate IServiceApplicationContextAddActions.WebStartupMvcBuilder { get => WebStartupMvcBuilder; set => WebStartupMvcBuilder = value; }

		public ServiceApplicationContextWebStartupConfigureServicesDelegate WebStartupConfigureServices { get; private set; }
		ServiceApplicationContextWebStartupConfigureServicesDelegate IServiceApplicationContextAddActions.WebStartupConfigureServices { get => WebStartupConfigureServices; set => WebStartupConfigureServices = value; }

		public bool AddSignalR { get; set; }

		public ServiceApplicationContextWebStartupUseEndpointsDelegate WebStartupUseEndpoints { get; private set; }
		ServiceApplicationContextWebStartupUseEndpointsDelegate IServiceApplicationContextAddActions.WebStartupUseEndpoints { get => WebStartupUseEndpoints; set => WebStartupUseEndpoints = value; }

		public ServiceApplicationContextConfigureWebApplicationDelegate ConfigureWebApplication { get; private set; }
		ServiceApplicationContextConfigureWebApplicationDelegate IServiceApplicationContextAddActions.ConfigureWebApplication { get => ConfigureWebApplication; set => ConfigureWebApplication = value; }

		public ServiceApplicationContextPreMessageBusBuildDelegate PreMessageBusBuild { get; private set; }
		ServiceApplicationContextPreMessageBusBuildDelegate IServiceApplicationContextAddActions.PreMessageBusBuild { get => PreMessageBusBuild; set => PreMessageBusBuild = value; }

		public ServiceApplicationContextPostStartupDelegate PostStartup { get; private set; }
		ServiceApplicationContextPostStartupDelegate IServiceApplicationContextAddActions.PostStartup { get => PostStartup; set => PostStartup = value; }

		public string GetDefaultAssemblyName()
		{
			const string windowsServiceSuffix = ".WindowsService";
			const string serviceApplicationSuffix = ".ServiceApplication";

			var assemblyName = RootAssembly.GetName().Name;

			if (assemblyName.EndsWith(windowsServiceSuffix))
			{
				assemblyName = assemblyName.Substring(0, assemblyName.Length - windowsServiceSuffix.Length);
			}
			else if (assemblyName.EndsWith(serviceApplicationSuffix))
			{
				assemblyName = assemblyName.Substring(0, assemblyName.Length - serviceApplicationSuffix.Length);
			}

			return assemblyName;
		}

		private string? _serviceName = null;
		public string ServiceName { get => _serviceName ??= GetDefaultAssemblyName(); set => _serviceName = value; }

		private string? _serviceDisplayName = null;
		public string ServiceDisplayName { get => _serviceDisplayName ??= GetDefaultAssemblyName(); set => _serviceDisplayName = value; }

		public string ServiceUserName { get; set; }
		public string ServicePassword { get; set; }

		public WindowsStartMode WindowsServiceStartMode { get; set; }
		public bool ServiceDelayStart { get; set; }

		public ServiceApplicationContext(Type rootType)
		{
			RootType = rootType;
			RootAssembly = rootType.Assembly;
		}
	}
}
