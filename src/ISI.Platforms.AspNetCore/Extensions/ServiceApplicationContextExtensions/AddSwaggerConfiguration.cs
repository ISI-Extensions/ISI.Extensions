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

					swaggerGenOptions.SwaggerDoc($"v{version}", new Microsoft.OpenApi.Models.OpenApiInfo()
					{
						Title = applicationName,
						Version = $"v{version}",
					});

					if (useBearer)
					{
						swaggerGenOptions.AddSecurityDefinition(ISI.Extensions.WebClient.HeaderCollection.Keys.Bearer, new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
						{
							Name = ISI.Extensions.WebClient.HeaderCollection.Keys.Authorization,
							Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
							Scheme = ISI.Extensions.WebClient.HeaderCollection.Keys.Bearer,
							In = Microsoft.OpenApi.Models.ParameterLocation.Header,
							Description = "Bearer Authorization header using the Bearer scheme.",
						});

						swaggerGenOptions.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
						{
							{
								new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
								{
									Reference = new Microsoft.OpenApi.Models.OpenApiReference()
									{
										Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
										Id = ISI.Extensions.WebClient.HeaderCollection.Keys.Bearer,
									}
								},
								new string[] { }
							}
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