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

namespace ISI.Extensions.AspNetCore
{
	public class CorsMiddleware
	{
		private static string AccessControlAllowHeaders;
		private static string AccessControlAllowMethods;

		static CorsMiddleware()
		{
			AccessControlAllowHeaders = string.Join(", ", new[]
			{
				Microsoft.Net.Http.Headers.HeaderNames.Authorization,
				Microsoft.Net.Http.Headers.HeaderNames.ContentType,
				Microsoft.Net.Http.Headers.HeaderNames.AcceptLanguage,
				Microsoft.Net.Http.Headers.HeaderNames.Accept,
				"credentials",
			});

			AccessControlAllowMethods = string.Join(", ", new[]
			{
				Microsoft.AspNetCore.Http.HttpMethods.Post, 
				Microsoft.AspNetCore.Http.HttpMethods.Get, 
				Microsoft.AspNetCore.Http.HttpMethods.Put, 
				Microsoft.AspNetCore.Http.HttpMethods.Patch, 
				Microsoft.AspNetCore.Http.HttpMethods.Delete, 
				Microsoft.AspNetCore.Http.HttpMethods.Options,
			});
		}
		
		protected readonly Microsoft.AspNetCore.Http.RequestDelegate Next;

		public CorsMiddleware(Microsoft.AspNetCore.Http.RequestDelegate next)
		{
			Next = next;
		}

		public async Task InvokeAsync(Microsoft.AspNetCore.Http.HttpContext context)
		{
			if (!context.Request.Headers.TryGetValue(Microsoft.Net.Http.Headers.HeaderNames.Origin, out var origin))
			{
				context.Request.Headers.TryGetValue(Microsoft.Net.Http.Headers.HeaderNames.Origin.ToLower(), out origin);
			}

			context.Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.AccessControlAllowOrigin, origin);
			context.Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.AccessControlAllowHeaders, AccessControlAllowHeaders);
			context.Response.Headers.Add(Microsoft.Net.Http.Headers.HeaderNames.AccessControlAllowMethods, AccessControlAllowMethods);

			if (string.Equals(context.Request.Method, Microsoft.AspNetCore.Http.HttpMethods.Options, StringComparison.InvariantCultureIgnoreCase))
			{
				context.Response.StatusCode = (int)System.Net.HttpStatusCode.NoContent;
				return;
			}

			await Next(context);
		}
	}
}