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
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ISI.Platforms.AspNetCore
{
	public interface ICookieAndBearerAuthenticationSettings : IAuthenticationSettings
	{
		string CookieName { get; }
		string ApiKeyHeaderName { get; }
		string AuthorizationHeaderName { get; }
	}

	public class CookieAndBearerAuthenticationSettings : ICookieAndBearerAuthenticationSettings
	{
		private const string DefaultAuthenticationHandlerName = "CookieAndBearerAuthenticationHandler";
		private const string DefaultCookieName = "Authentication";

		private static string _authenticationHandlerName = DefaultAuthenticationHandlerName;
		public string AuthenticationHandlerName
		{
			get => _authenticationHandlerName;
			set => _authenticationHandlerName = string.IsNullOrWhiteSpace(value) ? DefaultAuthenticationHandlerName : value;
		}

		private static string _cookieName = DefaultCookieName;
		public string CookieName
		{
			get => _cookieName;
			set => _cookieName = string.IsNullOrWhiteSpace(value) ? DefaultCookieName : value;
		}

		private static string _apiKeyHeaderName = DefaultCookieName;
		public string ApiKeyHeaderName
		{
			get => _apiKeyHeaderName;
			set => _apiKeyHeaderName =  value;
		}

		private static string _authorizationHeaderName = ISI.Extensions.WebClient.HeaderCollection.Keys.Authorization;
		public string AuthorizationHeaderName
		{
			get => _authorizationHeaderName;
			set => _authorizationHeaderName = string.IsNullOrWhiteSpace(value) ? ISI.Extensions.WebClient.HeaderCollection.Keys.Authorization : value;
		}
	}

	public class CookieAndBearerAuthenticationHandler<TCookieAndBearerAuthenticationSettings> : AbstractAuthenticationHandler, ISI.Platforms.AspNetCore.ICookieAuthenticationHandler
		where TCookieAndBearerAuthenticationSettings : ICookieAndBearerAuthenticationSettings, new()
	{
		protected ICookieAndBearerAuthenticationSettings CookieAndBearerAuthenticationSettings { get; }

		public CookieAndBearerAuthenticationHandler(
			ISI.Platforms.AspNetCore.Configuration configuration,
			Microsoft.Extensions.Options.IOptionsMonitor<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions> options,
			Microsoft.Extensions.Logging.ILoggerFactory logger,
			System.Text.Encodings.Web.UrlEncoder encoder,
			Microsoft.AspNetCore.Authentication.ISystemClock clock,
			ISI.Extensions.IAuthenticationIdentityApi authenticationIdentityApi)
			: base(configuration, options, logger, encoder, clock, authenticationIdentityApi)
		{
			CookieAndBearerAuthenticationSettings = new TCookieAndBearerAuthenticationSettings();
		}

		string ISI.Platforms.AspNetCore.ICookieAuthenticationHandler.AuthenticationHandlerName => CookieAndBearerAuthenticationSettings.AuthorizationHeaderName;
		string ISI.Platforms.AspNetCore.ICookieAuthenticationHandler.CookieName => CookieAndBearerAuthenticationSettings.CookieName;

		protected override string GetAuthenticationHandlerName() => CookieAndBearerAuthenticationSettings.CookieName;

		protected override bool TryGetAuthenticationHeaderValue(out string authenticationHeaderValue)
		{
			if(!string.IsNullOrWhiteSpace(CookieAndBearerAuthenticationSettings.ApiKeyHeaderName))
			{
				if (Request.Headers.TryGetValue(CookieAndBearerAuthenticationSettings.ApiKeyHeaderName, out var _authenticationHeaderValue))
				{
					authenticationHeaderValue = _authenticationHeaderValue.NullCheckedFirstOrDefault();

					return true;
				}
			}

			if(!string.IsNullOrWhiteSpace(CookieAndBearerAuthenticationSettings.AuthorizationHeaderName))
			{
				if (Request.Headers.TryGetValue(CookieAndBearerAuthenticationSettings.AuthorizationHeaderName, out var _authenticationHeaderValue))
				{
					authenticationHeaderValue = _authenticationHeaderValue.NullCheckedFirstOrDefault();

					return true;
				}
			}

			authenticationHeaderValue = null;

			return false;
		}

		protected override bool TryGetAuthenticationCookieValue(out string authenticationCookieValue)
		{
			if (Request.Cookies.TryGetValue(CookieAndBearerAuthenticationSettings.CookieName, out var _authenticationCookieValue))
			{
				authenticationCookieValue = _authenticationCookieValue;

				return true;
			}

			authenticationCookieValue = null;

			return false;
		}
	}
}
