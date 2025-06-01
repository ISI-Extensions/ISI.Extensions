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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Platforms.AspNetCore
{
	public abstract class AbstractAuthenticationHandler : Microsoft.AspNetCore.Authentication.AuthenticationHandler<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions>, ISI.Platforms.AspNetCore.IAuthenticationHandler
	{
		protected ISI.Platforms.AspNetCore.Configuration Configuration { get; }
		protected ISI.Extensions.IAuthenticationIdentityApi AuthenticationIdentityApi { get; }

		public AbstractAuthenticationHandler(
			ISI.Platforms.AspNetCore.Configuration configuration,
			Microsoft.Extensions.Options.IOptionsMonitor<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions> options,
			Microsoft.Extensions.Logging.ILoggerFactory logger,
			System.Text.Encodings.Web.UrlEncoder encoder,
			Microsoft.AspNetCore.Authentication.ISystemClock clock,
			ISI.Extensions.IAuthenticationIdentityApi authenticationIdentityApi)
			: base(options, logger, encoder, clock)
		{
			Configuration = configuration;
			AuthenticationIdentityApi = authenticationIdentityApi;

			if (string.IsNullOrWhiteSpace(Configuration.Jwt.Issuer))
			{
				var platformsConfiguration = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Platforms.Configuration>();

				if (!string.IsNullOrWhiteSpace(platformsConfiguration.BaseUrl))
				{
					var uri = new UriBuilder(platformsConfiguration.BaseUrl);
					uri.SetPathAndQueryString(string.Empty);

					Configuration.Jwt.Issuer = uri.Uri.ToString().Trim('/');
				}
				else
				{
					var server = ISI.Extensions.ServiceLocator.Current.GetRequiredService<global::Microsoft.AspNetCore.Hosting.Server.IServer>();
					var addressFeature = server.Features.Get<global::Microsoft.AspNetCore.Hosting.Server.Features.IServerAddressesFeature>();

					var baseUrl = addressFeature.Addresses.NullCheckedFirstOrDefault(address => address.StartsWith("http", StringComparison.InvariantCultureIgnoreCase));

					if (!string.IsNullOrWhiteSpace(baseUrl))
					{
						var uri = new UriBuilder(baseUrl);
						uri.SetPathAndQueryString(string.Empty);

						Configuration.Jwt.Issuer = uri.Uri.ToString().Trim('/');
					}
				}
			}

			if (string.IsNullOrWhiteSpace(Configuration.Jwt.Issuer))
			{
				Configuration.Jwt.Issuer = System.Reflection.Assembly.GetExecutingAssembly()?.FullName?.Split(new[] { ',' }).First();
			}
		}

		protected abstract string GetAuthenticationHandlerName();

		public async Task<System.IdentityModel.Tokens.Jwt.JwtSecurityToken> GetJwtSecurityTokenAsync(ISI.Extensions.IAuthenticationIdentityUser authenticationIdentityUser, IEnumerable<System.Security.Claims.Claim> claims = null)
		{
			claims ??= await GetUserClaimsAsync(authenticationIdentityUser);

			var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.Jwt.EncryptionKey));
			var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

			return new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(Configuration.Jwt.Issuer, Configuration.Jwt.Issuer, claims, expires: DateTime.Now + Configuration.Jwt.ExpirationInterval, signingCredentials: signingCredentials);
		}

		public virtual async Task<IEnumerable<System.Security.Claims.Claim>> GetUserClaimsAsync(ISI.Extensions.IAuthenticationIdentityUser authenticationIdentityUser)
		{
			var listRolesResponse = await AuthenticationIdentityApi.ListRolesAsync(new());

			var roles = new HashSet<string>(authenticationIdentityUser.Roles, StringComparer.InvariantCultureIgnoreCase);

			var claims = new List<System.Security.Claims.Claim>();
			claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, $"{authenticationIdentityUser.FirstName} {authenticationIdentityUser.LastName}".Trim()));
			claims.Add(new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, authenticationIdentityUser.UserUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens)));
			claims.Add(new System.Security.Claims.Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)));

			claims.AddRange(listRolesResponse.Roles
				.Where(role => roles.Contains(role.Role))
				.ToNullCheckedArray(role => new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role.Role)));

			return claims;
		}

		protected abstract bool TryGetAuthenticationHeaderValue(out string authenticationHeaderValue);

		protected abstract bool TryGetAuthenticationCookieValue(out string authenticationCookieValue);

		protected override async Task<Microsoft.AspNetCore.Authentication.AuthenticateResult> HandleAuthenticateAsync()
		{
			if (TryGetAuthenticationHeaderValue(out var authenticationHeaderValue))
			{
				if (!string.IsNullOrWhiteSpace(authenticationHeaderValue))
				{
					if (System.Net.Http.Headers.AuthenticationHeaderValue.TryParse(authenticationHeaderValue, out var parsedValue))
					{
						if (!string.IsNullOrWhiteSpace(parsedValue.Parameter))
						{
							var jwtSecurityTokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

							if (jwtSecurityTokenHandler.CanReadToken(parsedValue.Parameter))
							{
								var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(parsedValue.Parameter);

								var userUuidClaim = jwtToken.Claims.NullCheckedFirstOrDefault(claim => string.Equals(claim.Type, System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, StringComparison.InvariantCultureIgnoreCase));

								if (userUuidClaim == null)
								{
									userUuidClaim = jwtToken.Claims.NullCheckedFirstOrDefault(claim => string.Equals(claim.Type, System.Security.Claims.ClaimTypes.Actor, StringComparison.InvariantCultureIgnoreCase));

									if (userUuidClaim != null)
									{
										var claims = jwtToken.Claims.ToList();

										claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Actor, userUuidClaim.Value));

										jwtToken = await GetJwtSecurityTokenAsync(null, claims);

										userUuidClaim = jwtToken.Claims.NullCheckedFirstOrDefault(claim => string.Equals(claim.Type, System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, StringComparison.InvariantCultureIgnoreCase));
									}
								}

								if (userUuidClaim != null)
								{
									var principal = new System.Security.Claims.ClaimsPrincipal([
										new System.Security.Claims.ClaimsIdentity(jwtToken.Claims)
									]);

									var ticket = new Microsoft.AspNetCore.Authentication.AuthenticationTicket(principal, Scheme.Name);

									return Microsoft.AspNetCore.Authentication.AuthenticateResult.Success(ticket);
								}
							}
						}
					}

					var validateApiKeyResponse = await AuthenticationIdentityApi.ValidateApiKeyAsync(new()
					{
						Url = $"{Request.GetDisplayUrl()}/{Request.QueryString}",
						ApiKey = authenticationHeaderValue.TrimStart(ISI.Extensions.WebClient.HeaderCollection.Keys.BearerAuthenticationPrefix).Trim(),
					});

					if (validateApiKeyResponse.UserUuid.HasValue)
					{
						var userUuid = validateApiKeyResponse.UserUuid;

						var getUsersResponse = await AuthenticationIdentityApi.GetUsersAsync(new()
						{
							UserUuids = [userUuid.Value],
						});

						var user = getUsersResponse.Users.NullCheckedFirstOrDefault();

						if ((user?.IsActive).GetValueOrDefault())
						{
							var claims = await GetUserClaimsAsync(user);

							var principal = new System.Security.Claims.ClaimsPrincipal([
								new System.Security.Claims.ClaimsIdentity(claims)
							]);

							var ticket = new Microsoft.AspNetCore.Authentication.AuthenticationTicket(principal, Scheme.Name);

							return Microsoft.AspNetCore.Authentication.AuthenticateResult.Success(ticket);
						}
					}
				}
			}

			if (TryGetAuthenticationCookieValue(out var authenticationCookieValue))
			{
				if (!string.IsNullOrWhiteSpace(authenticationCookieValue))
				{
					var jwtSecurityTokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

					if (jwtSecurityTokenHandler.CanReadToken(authenticationCookieValue))
					{
						var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(authenticationCookieValue);

						var principal = new System.Security.Claims.ClaimsPrincipal([
							new System.Security.Claims.ClaimsIdentity(jwtToken.Claims, GetAuthenticationHandlerName())
						]);

						var ticket = new Microsoft.AspNetCore.Authentication.AuthenticationTicket(principal, Scheme.Name);

						return Microsoft.AspNetCore.Authentication.AuthenticateResult.Success(ticket);
					}
				}
			}

			return Microsoft.AspNetCore.Authentication.AuthenticateResult.NoResult();
		}

		protected override Task HandleChallengeAsync(Microsoft.AspNetCore.Authentication.AuthenticationProperties properties)
		{
			var notAuthenticatedUrl = (string)null;

			if (AuthenticationIdentityApi is IAuthenticationIdentityApiWithGetUrls authenticationIdentityApiWithGetUrls)
			{
				if (!string.IsNullOrWhiteSpace(authenticationIdentityApiWithGetUrls.NotAuthenticatedUrl))
				{
					notAuthenticatedUrl = authenticationIdentityApiWithGetUrls.NotAuthenticatedUrl;
				}

				if (authenticationIdentityApiWithGetUrls.GetNotAuthenticatedUrl != null)
				{
					notAuthenticatedUrl = authenticationIdentityApiWithGetUrls.GetNotAuthenticatedUrl(Request.HttpContext);
				}
			}

			if (string.IsNullOrWhiteSpace(notAuthenticatedUrl))
			{
				notAuthenticatedUrl = Configuration.NotAuthenticatedUrl;
			}

			if (!string.IsNullOrWhiteSpace(notAuthenticatedUrl))
			{
				Context.Response.Redirect(notAuthenticatedUrl);
				return Task.CompletedTask;
			}

			return base.HandleChallengeAsync(properties);
		}

		protected override Task HandleForbiddenAsync(Microsoft.AspNetCore.Authentication.AuthenticationProperties properties)
		{
			var notAuthorizedUrl = (string)null;

			if (AuthenticationIdentityApi is IAuthenticationIdentityApiWithGetUrls authenticationIdentityApiWithGetUrls)
			{
				if (!string.IsNullOrWhiteSpace(authenticationIdentityApiWithGetUrls.NotAuthorizedUrl))
				{
					notAuthorizedUrl = authenticationIdentityApiWithGetUrls.NotAuthorizedUrl;
				}

				if (authenticationIdentityApiWithGetUrls.GetNotAuthorizedUrl != null)
				{
					notAuthorizedUrl = authenticationIdentityApiWithGetUrls.GetNotAuthorizedUrl(Request.HttpContext);
				}
			}

			if (string.IsNullOrWhiteSpace(notAuthorizedUrl))
			{
				notAuthorizedUrl = Configuration.NotAuthorizedUrl;
			}

			if (!string.IsNullOrWhiteSpace(notAuthorizedUrl))
			{
				Context.Response.Redirect(notAuthorizedUrl);
				return Task.CompletedTask;
			}

			return base.HandleForbiddenAsync(properties);
		}
	}
}
