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
using System.Text;
using ISI.Platforms.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Platforms.AspNetCore.Users.Extensions
{
	public static class ContextExtensions
	{
		public class AddUsersRequest
		{
			public string SiteLayout_cshtml { get; set; }
			
			public string Policy { get; set; }
			public string Roles { get; set; }
			public string AuthenticationSchemes { get; set; }

			public Type AuthenticationApiType { get; set; }
			public Type AuthenticationApiWithUserType { get; set; }
			public Type AuthorizationApiType { get; set; }

			public bool UseApiKeys { get; set; } = true;
		}
		
		public static IServiceApplicationContextAddActions AddUsers(this IServiceApplicationContextAddActions context, AddUsersRequest request)
		{
			if(!string.IsNullOrWhiteSpace(request.SiteLayout_cshtml))
			{
				ISI.Platforms.AspNetCore.Users.Models.SiteModel.SiteLayout_cshtml = request.SiteLayout_cshtml;
				ISI.Platforms.AspNetCore.Users.Models.SiteModel.UseApiKeys = request.UseApiKeys;
			}

			if (!string.IsNullOrWhiteSpace(request.Policy))
			{
				ISI.Platforms.AspNetCore.Users.UsersAuthorizeAttribute.Policy = request.Policy;
			}

			if (!string.IsNullOrWhiteSpace(request.Roles))
			{
				ISI.Platforms.AspNetCore.Users.UsersAuthorizeAttribute.Roles = request.Roles;
			}

			if (!string.IsNullOrWhiteSpace(request.AuthenticationSchemes))
			{
				ISI.Platforms.AspNetCore.Users.UsersAuthorizeAttribute.AuthenticationSchemes = request.AuthenticationSchemes;
			}

			context.AddWebStartupMvcBuilder(mvcBuilder =>
			{
				mvcBuilder.AddApplicationPart(typeof(ISI.Platforms.AspNetCore.Users.Routes).Assembly);
			});

			if(request?.AuthenticationApiWithUserType != null)
			{
				context.AddWebStartupConfigureServices(services =>
				{
					services
						.AddSingleton(typeof(ISI.Extensions.Security.IAuthenticationApi), request.AuthenticationApiWithUserType)
						.AddSingleton(typeof(ISI.Extensions.Security.IAuthenticationApiWithUser), request.AuthenticationApiWithUserType)
						;
				});
			}
			else if (request?.AuthenticationApiType != null)
			{
				context.AddWebStartupConfigureServices(services =>
				{
					services
						.AddSingleton(typeof(ISI.Extensions.Security.IAuthenticationApi), request.AuthenticationApiType)
						;
				});
			}

			if (request?.AuthorizationApiType != null)
			{
				context.AddWebStartupConfigureServices(services =>
				{
					services
						.AddSingleton(typeof(ISI.Extensions.Security.IAuthorizationApi), request.AuthorizationApiType)
						;
				});
			}


			return context;
		}
	}
}
