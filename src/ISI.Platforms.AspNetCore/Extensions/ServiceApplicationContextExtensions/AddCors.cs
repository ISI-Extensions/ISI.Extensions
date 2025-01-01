#region Copyright & License
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
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.Extensions;
using ISI.Platforms.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ISI.Platforms.AspNetCore.Extensions
{
	public static partial class ServiceApplicationContextExtensions
	{
		public static ServiceApplicationContext AddCors(this ServiceApplicationContext context, IEnumerable<string> corsPolicyOrigins = null, bool? allowAnyHeader = null, bool? allowAnyMethod = null, bool? allowCredentials = null)
		{
			context.AddHostBuilderConfigureServices(hostBuilder =>
			{
				var configuration = context.ConfigurationRoot.GetConfiguration<ISI.Platforms.AspNetCore.Configuration>();

				corsPolicyOrigins = new HashSet<string>(corsPolicyOrigins ?? [], StringComparer.InvariantCultureIgnoreCase);
				if (configuration?.Cors?.PolicyOrigins?.NullCheckedAny() ?? false)
				{
					((HashSet<string>)corsPolicyOrigins).UnionWith(configuration.Cors.PolicyOrigins);
				}

				if (!corsPolicyOrigins.Any())
				{
					throw new Exception("Cannot add CORS without PolicyOrigins");
				}

				hostBuilder.Services.AddCors(options =>
				{
					options.AddDefaultPolicy(policy =>
					{
						policy.WithOrigins(corsPolicyOrigins.ToArray());
						if (allowAnyHeader ?? configuration?.Cors?.AllowAnyHeader ?? true)
						{
							policy.AllowAnyHeader();
						}

						if (allowAnyMethod ?? configuration?.Cors?.AllowAnyMethod ?? true)
						{
							policy.AllowAnyMethod();
						}

						if (allowCredentials ?? configuration?.Cors?.AllowCredentials ?? true)
						{
							policy.AllowCredentials();
						}
						// see https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS#access-control-expose-headers
						// exposed headers such as X-Wrs-User-Role must be exposed during CORS transfers explicitly
						//policy.WithExposedHeaders(UserRoleHeaderHandler
						//	.AddResponseHeaderNames(UserUuidHeaderHandler.AddResponseHeaderNames()).ToArray());
					});
				});

			});

			context.AddConfigureWebApplication(applicationBuilder =>
			{
				applicationBuilder.UseCors();
			});

			return context;
		}
	}
}