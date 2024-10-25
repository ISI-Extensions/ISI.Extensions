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
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using ISI.Platforms.Extensions;

namespace ISI.Platforms.AspNetCore.Extensions
{
	public static partial class ServiceApplicationContextExtensions
	{
		public class AddCookieAndBearerAuthenticationRequest
		{
			public string AuthenticationHandlerName { get; set; }

			public string PolicyName { get; set; }
			
			public string CookieName { get; set; }

			public string ApiKeyHeaderName { get; set; }
			
			public Microsoft.AspNetCore.Http.PathString LoginPath { get; set; } = null;
			public Microsoft.AspNetCore.Http.PathString LogoutPath { get; set; } = null;
		}


		//public static ServiceApplicationContext AddCookieAndBearerAuthentication(this ServiceApplicationContext context, string authenticationHandlerName = null, string policyName = null, string cookieName = null, string apiKeyHeaderName = null)
		public static ServiceApplicationContext AddCookieAndBearerAuthentication(this ServiceApplicationContext context, AddCookieAndBearerAuthenticationRequest request = null)
		{
			var authorizationPolicy = new HasUserUuidAuthorizationPolicy(request?.PolicyName);

			context.AddWebStartupConfigureServices(services =>
			{
				var authenticationHandlerSettings = new CookieAndBearerAuthenticationSettings();

				authenticationHandlerSettings.AuthenticationHandlerName = request?.AuthenticationHandlerName;
				authenticationHandlerSettings.CookieName = request?.CookieName;
				authenticationHandlerSettings.ApiKeyHeaderName = request?.ApiKeyHeaderName;

				services.AddSingleton<ISI.Platforms.AspNetCore.IAuthenticationHandler, CookieAndBearerAuthenticationHandler<CookieAndBearerAuthenticationSettings>>();

				var authenticationBuilders = services
					.AddAuthentication(authenticationHandlerSettings.AuthenticationHandlerName)
					.AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, CookieAndBearerAuthenticationHandler<CookieAndBearerAuthenticationSettings>>(authenticationHandlerSettings.AuthenticationHandlerName, null)
					;

				if ((request?.LoginPath != null) || (request?.LogoutPath != null))
				{
					authenticationBuilders
						.AddCookie(authenticationHandlerSettings.AuthenticationHandlerName, configureOptions =>
						{
							if (request?.LoginPath != null)
							{
								configureOptions.LoginPath = request.LoginPath;
							}

							if (request?.LogoutPath != null)
							{
								configureOptions.LogoutPath = request.LogoutPath;
							}
						});
				}

				services.AddAuthorization(options =>
				{
					options.AddPolicy(authorizationPolicy.PolicyName, policy => policy.Requirements.Add(authorizationPolicy));
				});
			});

			return context;
		}
	}
}