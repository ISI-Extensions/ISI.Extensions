#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using System.Reflection;
using System.Threading.Tasks;
using global::Swashbuckle.AspNetCore.SwaggerGen;

namespace ISI.Extensions.AspNetCore.Swashbuckle
{
	public class BearerTokenOperationFilter : global::Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
	{
		public void Apply(Microsoft.OpenApi.Models.OpenApiOperation operation, global::Swashbuckle.AspNetCore.SwaggerGen.OperationFilterContext context)
		{
			if (context.ApiDescription.ActionDescriptor is Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor descriptor)
			{
				// If [AllowAnonymous] is not applied or [Authorize] or Custom Authorization filter is applied on either the endpoint or the controller
				if (!context.ApiDescription.CustomAttributes().Any((a) => a is Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute)
				    && (context.ApiDescription.CustomAttributes().Any((a) => a is Microsoft.AspNetCore.Authorization.AuthorizeAttribute)
				        || descriptor.ControllerTypeInfo.GetCustomAttribute<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>() != null))
				{
					//operation.Parameters ??= new List<Microsoft.OpenApi.Models.OpenApiParameter>();
					//operation.Parameters.Add(new Microsoft.OpenApi.Models.OpenApiParameter()
					//{
					//	Description = "Bearer token",
					//	Name = "Authorization",
					//	In = Microsoft.OpenApi.Models.ParameterLocation.Header,
					//	Required = true
					//});


					operation.Security ??= new List<Microsoft.OpenApi.Models.OpenApiSecurityRequirement>();

					operation.Security.Add(new()
					{
						{
							new()
							{
								Name = ISI.Extensions.WebClient.HeaderCollection.Keys.Authorization,
								In = Microsoft.OpenApi.Models.ParameterLocation.Header,
								BearerFormat = "Bearer token",

								Reference = new()
								{
									Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
									Id = ISI.Extensions.WebClient.HeaderCollection.Keys.Bearer,
								}
							},
							new string[]{ }
						}
					});
				}
			}
		}
	}
}
