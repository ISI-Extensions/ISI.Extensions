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
using System.Threading.Tasks;
using ISI.Platforms.AspNetCore.Extensions;
using ISI.Platforms.Extensions;
using ISI.Platforms.ServiceApplication.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Platforms.ServiceApplication.Test
{
	public class Program
	{
		public const string AuthorizationPolicyName = "ServiceApplication-Policy";
		public const string AuthorizationCookieName = "ServiceApplication-Authentication";

		public static void Main(string[] args)
		{
			//args = ["uninstall"];

			var context = new ServiceApplicationContext(typeof(Program))
			{
				LoggerConfigurator = new ISI.Platforms.Serilog.LoggerConfigurator(),

				Args = args,
			};

			context.AddCookieAndBearerAuthentication(new()
			{
				PolicyName = AuthorizationPolicyName,
				CookieName = AuthorizationCookieName,
			});

			context.AddSignalR = true;
			context.AddWebStartupUseEndpoints(endpointRouteBuilder => { endpointRouteBuilder.MapHub<ISI.Platforms.ServiceApplication.Test.Hubs.ChatHub>(ISI.Platforms.ServiceApplication.Services.Test.ChatHubApi.HubUrlPattern); });


			context.AddWebStartupConfigureServices(services => { services.AddSingleton<ISI.Extensions.IAuthenticationIdentityApi, AuthenticationIdentityApi>(); });

			context.AddSwaggerConfiguration(useBearer: true);

			//context.AddCors(new[] { "*" });

			context.AddEnterpriseCacheManager();

			var chatHubApi = (ISI.Platforms.ServiceApplication.Services.Test.IChatHubApi)null;

			context.AddPostStartup(host =>
			{
				chatHubApi = host.Services.GetService<ISI.Platforms.ServiceApplication.Services.Test.IChatHubApi>();

				chatHubApi.ConnectAsync(new()).GetAwaiter().GetResult();
			});

			context.SetConfiguration();


			if (!context.ServiceSetup())
			{
				var webApplication = context.CreateWebApplication();

				webApplication.ConfigureWebApplication(context);

				webApplication.Run();
			}
		}
	}
}