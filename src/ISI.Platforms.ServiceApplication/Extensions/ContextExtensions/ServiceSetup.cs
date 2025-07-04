﻿#region Copyright & License
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
using ISI.Extensions.Extensions;
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Platforms.Extensions;
using Microsoft.Extensions.Configuration;
using DTOs = ISI.Platforms.ServiceApplication.DataTransferObjects.ServiceConfigurator;

namespace ISI.Platforms.ServiceApplication.Extensions
{
	public static partial class ContextExtensions
	{
		public const string InstallService = "install";
		public const string UninstallService = "uninstall";
		public const string StartService = "start";
		public const string StopService = "stop";

		public static void RunAs(this ServiceApplicationContext context, Microsoft.Extensions.Configuration.IConfigurationRoot configurationRoot)
		{
			context.RunAs(configurationRoot.GetConfiguration<ISI.Platforms.ServiceApplication.Configuration>());
		}
		public static void RunAs(this ServiceApplicationContext context, Configuration configuration = null)
		{
			var userName = configuration?.LogOnAs?.UserName;
			var password = configuration?.LogOnAs?.Password;

			if (string.IsNullOrEmpty(userName) || string.Equals(userName, "LocalSystem", StringComparison.InvariantCultureIgnoreCase))
			{
				context.ServiceUserName = WindowsServiceConfigurator.LocalServiceUserName;
				context.ServicePassword = null;
			}
			else if (string.Equals(userName, "LocalService", StringComparison.InvariantCultureIgnoreCase))
			{
				context.ServiceUserName = WindowsServiceConfigurator.LocalServiceUserName;
				context.ServicePassword = null;
			}
			else if (string.Equals(userName, "NetworkService", StringComparison.InvariantCultureIgnoreCase))
			{
				context.ServiceUserName = WindowsServiceConfigurator.NetworkServiceUserName;
				context.ServicePassword = null;
			}
			else
			{
				context.ServiceUserName = userName;
				context.ServicePassword = password;
			}
		}

		
		public static void SetDisplayName(this ServiceApplicationContext context, Microsoft.Extensions.Configuration.IConfigurationRoot configurationRoot)
		{
			context.SetDisplayName(configurationRoot.GetConfiguration<ISI.Platforms.ServiceApplication.Configuration>());
		}

		public static void SetDisplayName(this ServiceApplicationContext context, ISI.Platforms.ServiceApplication.Configuration configuration = null)
		{
			var serviceDisplayName = configuration.ServiceDisplayName;

			var servicePrefix = configuration?.ServicePrefix;
			if (!string.IsNullOrEmpty(servicePrefix))
			{
				serviceDisplayName = string.Format("{0}{1}", servicePrefix, serviceDisplayName);
			}

			context.ServiceDisplayName = serviceDisplayName;
		}

		public static void SetServiceName(this ServiceApplicationContext context, Microsoft.Extensions.Configuration.IConfigurationRoot configurationRoot)
		{
			context.SetServiceName(configurationRoot.GetConfiguration<ISI.Platforms.ServiceApplication.Configuration>());
		}
		public static void SetServiceName(this ServiceApplicationContext context, ISI.Platforms.ServiceApplication.Configuration configuration = null)
		{
			var serviceServiceName = configuration.ServiceServiceName;

			var servicePrefix = configuration?.ServicePrefix;
			if (!string.IsNullOrEmpty(servicePrefix))
			{
				serviceServiceName = string.Format("{0}{1}", servicePrefix, serviceServiceName);
			}

			context.ServiceName = serviceServiceName;
		}



		public static bool ServiceSetup(this ServiceApplicationContext context)
		{
			var args = new ISI.Extensions.InvariantCultureIgnoreCaseStringHashSet(context.Args);

			if (args.Contains(InstallService))
			{
				ServiceSetup_InstallService(context);

				return true;
			}

			if (args.Contains(UninstallService))
			{
				ServiceSetup_UninstallService(context);

				return true;
			}

			if (args.Contains(StartService))
			{
				ServiceSetup_StartService(context);

				return true;
			}

			if (args.Contains(StopService))
			{
				ServiceSetup_StopService(context);

				return true;
			}

			return false;
		}

		private static IServiceConfigurator GetServiceConfigurator() => ((Environment.OSVersion.Platform == PlatformID.Unix) ? new LinuxServiceConfigurator() : new WindowsServiceConfigurator());

		private static void ServiceSetup_InstallService(ServiceApplicationContext context)
		{
			var serviceConfigurator = GetServiceConfigurator();

			var executable = context.RootAssembly.Location;
			if (executable.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
			{
				executable = $"{executable.TrimEnd(".dll", StringComparison.InvariantCultureIgnoreCase)}{((Environment.OSVersion.Platform == PlatformID.Unix) ? string.Empty : ".exe")}";
				if (!System.IO.File.Exists(executable))
				{
					executable = context.RootAssembly.Location;
				}
			}

			serviceConfigurator.InstallService(new()
			{
				ServiceName = context.ServiceName,
				DisplayName = context.ServiceDisplayName,
				Executable = executable,
				UserName = context.ServiceUserName,
				Password = context.ServicePassword,
				WindowsStartMode = context.WindowsServiceStartMode,
				DelayStart = context.ServiceDelayStart,
			});
		}

		private static void ServiceSetup_UninstallService(ServiceApplicationContext context)
		{
			var serviceConfigurator = GetServiceConfigurator();

			serviceConfigurator.UnInstallService(new()
			{
				ServiceName = context.ServiceName,
			});
		}

		private static void ServiceSetup_StartService(ServiceApplicationContext context)
		{
			var serviceConfigurator = GetServiceConfigurator();

			serviceConfigurator.StartService(new()
			{
				ServiceName = context.ServiceName,
			});
		}

		private static void ServiceSetup_StopService(ServiceApplicationContext context)
		{
			var serviceConfigurator = GetServiceConfigurator();

			serviceConfigurator.StopService(new()
			{
				ServiceName = context.ServiceName,
			});
		}
	}
}
