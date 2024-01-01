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
using System.Text;

namespace ISI.Extensions.Services.Extensions
{
	public static class ServiceManagerExtensions
	{
		public static bool Start(this IServiceManager serviceManager, Microsoft.Extensions.Configuration.IConfigurationRoot configuration, string environment, string[] arguments, IServiceProvider serviceProvider = null) => serviceManager.Start(new ServiceContext() { Configuration = configuration, Environment = environment, Arguments = arguments, ServiceProvider = serviceProvider });
		public static bool Stop(this IServiceManager serviceManager, Microsoft.Extensions.Configuration.IConfigurationRoot configuration, string environment, string[] arguments, IServiceProvider serviceProvider = null) => serviceManager.Stop(new ServiceContext() { Configuration = configuration, Environment = environment, Arguments = arguments, ServiceProvider = serviceProvider });

		public static void BeforeInstall(this IServiceManager serviceManager, Microsoft.Extensions.Configuration.IConfigurationRoot configuration, string environment, string[] arguments, IServiceProvider serviceProvider = null) => serviceManager.BeforeInstall(new ServiceContext() { Configuration = configuration, Environment = environment, Arguments = arguments, ServiceProvider = serviceProvider });
		public static void AfterInstall(this IServiceManager serviceManager, Microsoft.Extensions.Configuration.IConfigurationRoot configuration, string environment, string[] arguments, IServiceProvider serviceProvider = null) => serviceManager.AfterInstall(new ServiceContext() { Configuration = configuration, Environment = environment, Arguments = arguments, ServiceProvider = serviceProvider });
		public static void BeforeUninstall(this IServiceManager serviceManager, Microsoft.Extensions.Configuration.IConfigurationRoot configuration, string environment, string[] arguments, IServiceProvider serviceProvider = null) => serviceManager.BeforeUninstall(new ServiceContext() { Configuration = configuration, Environment = environment, Arguments = arguments, ServiceProvider = serviceProvider });
		public static void AfterUninstall(this IServiceManager serviceManager, Microsoft.Extensions.Configuration.IConfigurationRoot configuration, string environment, string[] arguments, IServiceProvider serviceProvider = null) => serviceManager.AfterUninstall(new ServiceContext() { Configuration = configuration, Environment = environment, Arguments = arguments, ServiceProvider = serviceProvider });
	}
}
