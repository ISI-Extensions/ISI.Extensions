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
using System.Text;
using Topshelf;
using HostConfigurator = Topshelf.HostConfigurators.HostConfigurator;

namespace ISI.Extensions.Topshelf.Extensions
{
	public static partial class HostConfiguratorExtensions
	{
		public static global::Topshelf.HostConfigurators.HostConfigurator RunAs(this global::Topshelf.HostConfigurators.HostConfigurator hostConfigurator, Configuration configuration = null)
		{
			var userName = configuration?.LogOnAs?.UserName;
			var password = configuration?.LogOnAs?.Password;

			if (string.IsNullOrEmpty(userName) || string.Equals(userName, "LocalSystem", StringComparison.InvariantCultureIgnoreCase))
			{
				hostConfigurator.RunAsLocalSystem();
			}
			else if (string.Equals(userName, "LocalService", StringComparison.InvariantCultureIgnoreCase))
			{
				hostConfigurator.RunAsLocalService();
			}
			else if (string.Equals(userName, "NetworkService", StringComparison.InvariantCultureIgnoreCase))
			{
				hostConfigurator.RunAsNetworkService();
			}
			else
			{
				hostConfigurator.RunAs(userName, password);
			}

			return hostConfigurator;
		}

		public static global::Topshelf.HostConfigurators.HostConfigurator SetDescription(this global::Topshelf.HostConfigurators.HostConfigurator hostConfigurator, Configuration configuration = null)
		{
			var serviceDescription = configuration?.ServiceDescription;
			if (string.IsNullOrEmpty(serviceDescription))
			{
				serviceDescription = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
			}

			var servicePrefix = configuration?.ServicePrefix;
			if (!string.IsNullOrEmpty(servicePrefix))
			{
				serviceDescription = string.Format("{0}{1}", servicePrefix, serviceDescription);
			}

			hostConfigurator.SetDescription(serviceDescription);

			return hostConfigurator;
		}

		public static global::Topshelf.HostConfigurators.HostConfigurator SetDisplayName(this global::Topshelf.HostConfigurators.HostConfigurator hostConfigurator, Configuration configuration = null)
		{
			var serviceDisplayName = configuration?.ServiceDisplayName;
			if (string.IsNullOrEmpty(serviceDisplayName))
			{
				serviceDisplayName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
				if (serviceDisplayName.EndsWith(".WindowsService"))
				{
					serviceDisplayName = serviceDisplayName.Substring(0, serviceDisplayName.Length - ".WindowsService".Length);
				}
				else if (serviceDisplayName.EndsWith(".ServiceApplication"))
				{
					serviceDisplayName = serviceDisplayName.Substring(0, serviceDisplayName.Length - ".ServiceApplication".Length);
				}
			}

			var servicePrefix = configuration?.ServicePrefix;
			if (!string.IsNullOrEmpty(servicePrefix))
			{
				serviceDisplayName = string.Format("{0}{1}", servicePrefix, serviceDisplayName);
			}

			hostConfigurator.SetDisplayName(serviceDisplayName);

			return hostConfigurator;
		}

		public static global::Topshelf.HostConfigurators.HostConfigurator SetServiceName(this global::Topshelf.HostConfigurators.HostConfigurator hostConfigurator, Configuration configuration = null)
		{
			var serviceServiceName = configuration?.ServiceServiceName;
			if (string.IsNullOrEmpty(serviceServiceName))
			{
				serviceServiceName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
				if (serviceServiceName.EndsWith(".WindowsService"))
				{
					serviceServiceName = serviceServiceName.Substring(0, serviceServiceName.Length - ".WindowsService".Length);
				}
				else if (serviceServiceName.EndsWith(".ServiceApplication"))
				{
					serviceServiceName = serviceServiceName.Substring(0, serviceServiceName.Length - ".ServiceApplication".Length);
				}
			}

			var servicePrefix = configuration?.ServicePrefix;
			if (!string.IsNullOrEmpty(servicePrefix))
			{
				serviceServiceName = string.Format("{0}{1}", servicePrefix, serviceServiceName);
			}

			hostConfigurator.SetServiceName(serviceServiceName);

			return hostConfigurator;
		}

		public static global::Topshelf.HostConfigurators.HostConfigurator SetStartTimeout(this global::Topshelf.HostConfigurators.HostConfigurator hostConfigurator, Configuration configuration)
		{
			hostConfigurator.SetStartTimeout(configuration.StartTimeOut);

			return hostConfigurator;
		}

		public static global::Topshelf.HostConfigurators.HostConfigurator SetStopTimeout(this global::Topshelf.HostConfigurators.HostConfigurator hostConfigurator, Configuration configuration)
		{
			hostConfigurator.SetStopTimeout(configuration.StopTimeOut);

			return hostConfigurator;
		}
	}
}
