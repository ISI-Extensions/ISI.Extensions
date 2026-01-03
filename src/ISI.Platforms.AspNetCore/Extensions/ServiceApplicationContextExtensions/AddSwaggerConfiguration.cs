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
using ISI.Extensions.Extensions;
using ISI.Platforms.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ISI.Platforms.AspNetCore.Extensions
{
	public static partial class ServiceApplicationContextExtensions
	{
		public static ServiceApplicationContext AddSwaggerConfiguration(this ServiceApplicationContext context, string applicationName = null, int? version = null, bool useBearer = false)
		{
			applicationName ??= context.RootAssembly.FullName.Split(new[] { ',' }).First();

			if (!version.HasValue && Version.TryParse(ISI.Extensions.SystemInformation.GetAssemblyVersion(context.RootAssembly), out var assemblyVersion))
			{
				version = assemblyVersion.Major;
			}

			context.AddWebStartupConfigureServices(services =>
			{
				services.AddSwaggerGen(swaggerGenOptions =>
				{
					swaggerGenOptions.CustomOperationIds(apiDescription => apiDescription.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name.TrimEnd("Async") : null);

					swaggerGenOptions.SwaggerDoc($"v{version}", new Microsoft.OpenApi.OpenApiInfo()
					{
						Title = applicationName,
						Version = $"v{version}",
					});

					if (useBearer)
					{
						swaggerGenOptions.AddSecurityDefinition(ISI.Extensions.WebClient.HeaderCollection.Keys.Bearer, new Microsoft.OpenApi.OpenApiSecurityScheme()
						{
							Name = ISI.Extensions.WebClient.HeaderCollection.Keys.Authorization,
							Type = Microsoft.OpenApi.SecuritySchemeType.Http,
							Scheme = ISI.Extensions.WebClient.HeaderCollection.Keys.Bearer,
							In = Microsoft.OpenApi.ParameterLocation.Header,
							Description = "Bearer Authorization header using the Bearer scheme.",
						});
						swaggerGenOptions.AddSecurityRequirement(document => new Microsoft.OpenApi.OpenApiSecurityRequirement()
						{
							{ new Microsoft.OpenApi.OpenApiSecuritySchemeReference(ISI.Extensions.WebClient.HeaderCollection.Keys.Bearer, document), new List<string>() },
						});
					}
				});

				services.AddSwaggerGenNewtonsoftSupport();
			});

			context.AddConfigureWebApplication(webApplication =>
			{
				webApplication.UseSwagger();
				webApplication.UseSwaggerUI(swaggerUIOptions =>
						{
							swaggerUIOptions.SwaggerEndpoint($"/swagger/v{version}/swagger.json", $"{applicationName} v{version}");
						});
			});

			return context;
		}
	}
}