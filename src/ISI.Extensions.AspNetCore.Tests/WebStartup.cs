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
using System.Threading.Tasks;
using ISI.Extensions.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace ISI.Extensions.AspNetCore.Tests
{
	public class WebStartup
	{
		public WebStartup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddControllersWithViews()
				//.AddRazorRuntimeCompilation(options => options.FileProviders.Add(new ISI.Extensions.VirtualFileVolumesFileProvider()))
				.AddNewtonsoftJson()
				;

//				.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
			//services.AddMicrosoftIdentityWebAppAuthentication(Configuration, Constants.AzureAdB2C, "X-OpenIdConnect", "X-Cookies");

			services
				.AddMvc()
				.AddRazorPagesOptions(options =>
				{
					options.Conventions.AllowAnonymousToPage("/Index");
				})
				.AddMvcOptions(options => { })
				.AddISIAspNetCore();
				;


			//services
			//	.AddAuthentication(AuthenticationHandler.AuthenticationHandlerName)
			//	.AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, AuthenticationHandler>(AuthenticationHandler.AuthenticationHandlerName, null)
			//	;

			//services.AddAuthorization(options =>
			//{
			//	options.AddPolicy(IsAuthenticatedAuthorizationPolicy.PolicyName, policy => policy.Requirements.Add(new IsAuthenticatedAuthorizationPolicy()));
			//});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment webHostingEnvironment)
		{
			if (webHostingEnvironment.IsDevelopment())
			{
				applicationBuilder.UseDeveloperExceptionPage();
			}

			applicationBuilder.UseSerilogRequestLogging(options =>
			{
				// Customize the message template
				options.MessageTemplate = "Handled {RequestPath}";

				// Emit debug-level events instead of the defaults
				options.GetLevel = (httpContext, elapsed, ex) => Serilog.Events.LogEventLevel.Debug;

				// Attach additional properties to the request completion event
				options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
				{
					diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
					diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
				};
			});

			applicationBuilder.UseDefaultFiles();

			applicationBuilder.UseStaticFiles();

			applicationBuilder.UseRouting();

			applicationBuilder.UseAuthentication();
			applicationBuilder.UseAuthorization();

			applicationBuilder.UseEndpoints(endpointRouteBuilder =>
			{
				endpointRouteBuilder.MapControllers();
			});
		}
	}
}
