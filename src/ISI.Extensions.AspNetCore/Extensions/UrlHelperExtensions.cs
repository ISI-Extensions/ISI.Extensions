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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.AspNetCore.Extensions
{
	public static class UrlHelperExtensions
	{
		public static string GenerateRouteUrl(this Microsoft.AspNetCore.Mvc.IUrlHelper urlHelper, string routeName, object routeValues = null, bool makeAbsolute = false, bool makeRootRelative = false, string overRideHost = null)
		{
			var url = urlHelper.RouteUrl(new()
			{
				RouteName = routeName,
				Values = new Microsoft.AspNetCore.Routing.RouteValueDictionary(routeValues),
			});

			if (makeAbsolute || makeRootRelative || !string.IsNullOrWhiteSpace(overRideHost))
			{
				var host = urlHelper.ActionContext.HttpContext.Request.Host.Host;
				var scheme = urlHelper.ActionContext.HttpContext.Request.Scheme;
				var port = urlHelper.ActionContext.HttpContext.Request.Host.Port;

				if (urlHelper.ActionContext.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Host", out var forwardedHosts))
				{
					var forwardedHost = forwardedHosts.NullCheckedFirstOrDefault() ?? string.Empty;

					if ((forwardedHost.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) || forwardedHost.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)) && Uri.TryCreate(forwardedHost, UriKind.Absolute, out var uri))
					{
						if (!string.IsNullOrWhiteSpace(uri.Scheme))
						{
							scheme = uri.Scheme;
						}
						if (!string.IsNullOrWhiteSpace(uri.Host))
						{
							host = uri.Host;
						}
						if (uri.Port > 0)
						{
							port = uri.Port;
						}
					}
					else
					{
						var hostParts = forwardedHost.Split([':'], 2);

						host = hostParts.First();

						if (hostParts.Length == 2)
						{
							port = hostParts[1].ToIntNullable();
						}
					}
				}

				if (urlHelper.ActionContext.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Proto", out var forwardedScheme))
				{
					scheme = forwardedScheme.NullCheckedFirstOrDefault();
				}

				if (urlHelper.ActionContext.HttpContext.Request.Headers.TryGetValue("X-Forwarded-Port", out var forwardedPort))
				{
					port = forwardedPort.NullCheckedFirstOrDefault().ToIntNullable();
				}

				if (!string.IsNullOrWhiteSpace(overRideHost))
				{
					if ((overRideHost.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) || overRideHost.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase)) && Uri.TryCreate(overRideHost, UriKind.Absolute, out var uri))
					{
						if (!string.IsNullOrWhiteSpace(uri.Scheme))
						{
							scheme = uri.Scheme;
						}
						if (!string.IsNullOrWhiteSpace(uri.Host))
						{
							host = uri.Host;
						}
						if (uri.Port > 0)
						{
							port = uri.Port;
						}
					}
					else
					{
						var hostParts = overRideHost.Split([':'], 2);

						host = hostParts.First();

						if (hostParts.Length == 2)
						{
							port = hostParts[1].ToIntNullable();
						}
					}
				}

				var uriBuilder = new UriBuilder(host)
				{
					Scheme = scheme,
				};

				if (port.HasValue)
				{
					uriBuilder.Port = port.Value;
				}

				uriBuilder.SetPathAndQueryString(url);
				url = uriBuilder.Uri.ToString();

				if (makeRootRelative)
				{
					uriBuilder = new UriBuilder(host)
					{
						Scheme = scheme,
					};

					if (port.HasValue)
					{
						uriBuilder.Port = port.Value;
					}

					url = url.Substring(uriBuilder.Uri.ToString().Length - 1);
				}
			}

			return url;
		}
	}
}
