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
	public class CookieAndBearerAuthenticationHandler : Microsoft.AspNetCore.Authentication.AuthenticationHandler<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions>, IAuthenticationHandler
	{
		public const string AuthenticationHandlerName = nameof(CookieAndBearerAuthenticationHandler);

		public static string CookieName { get; set; } = "Authentication";
		string IAuthenticationHandler.CookieName => CookieName;

		protected Configuration Configuration { get; }
		protected ISI.Extensions.IAuthenticationIdentityApi AuthenticationIdentityApi { get; }

		public CookieAndBearerAuthenticationHandler(
			Configuration configuration,
			Microsoft.Extensions.Options.IOptionsMonitor<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions> options,
			Microsoft.Extensions.Logging.ILoggerFactory logger,
			System.Text.Encodings.Web.UrlEncoder encoder,
			Microsoft.AspNetCore.Authentication.ISystemClock clock,
			ISI.Extensions.IAuthenticationIdentityApi authenticationIdentityApi)
			: base(options, logger, encoder, clock)
		{
			Configuration = configuration;
			AuthenticationIdentityApi = authenticationIdentityApi;
		}

		public async Task<System.IdentityModel.Tokens.Jwt.JwtSecurityToken> GetJwtSecurityTokenAsync(ISI.Extensions.IAuthenticationIdentityUser authenticationIdentityUser, IEnumerable<System.Security.Claims.Claim> claims = null)
		{
			claims ??= await GetUserClaimsAsync(authenticationIdentityUser);

			var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.Jwt.EncryptionKey));
			var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256);

			return new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(Configuration.Jwt.Issuer, Configuration.Jwt.Issuer, claims, expires: DateTime.Now + Configuration.Jwt.ExpirationInterval, signingCredentials: credentials);
		}

		public async Task<IEnumerable<System.Security.Claims.Claim>> GetUserClaimsAsync(ISI.Extensions.IAuthenticationIdentityUser authenticationIdentityUser)
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

		protected override async Task<Microsoft.AspNetCore.Authentication.AuthenticateResult> HandleAuthenticateAsync()
		{
			if (Request.Headers.ContainsKey(ISI.Extensions.WebClient.HeaderCollection.Keys.Authorization))
			{
				var authenticationHeaderValue = Request.Headers[ISI.Extensions.WebClient.HeaderCollection.Keys.Authorization];

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
									var principal = new System.Security.Claims.ClaimsPrincipal(new[]
									{
										new System.Security.Claims.ClaimsIdentity(jwtToken.Claims),
									});

									var ticket = new Microsoft.AspNetCore.Authentication.AuthenticationTicket(principal, Scheme.Name);

									return Microsoft.AspNetCore.Authentication.AuthenticateResult.Success(ticket);
								}
							}
						}
					}

					var validateApiKeyResponse = await AuthenticationIdentityApi.ValidateApiKeyAsync(new ()
					{
						ApiKey = authenticationHeaderValue.NullCheckedFirstOrDefault()?.TrimStart(ISI.Extensions.WebClient.HeaderCollection.Keys.Bearer).Trim(),
					});

					if (validateApiKeyResponse.UserUuid.HasValue)
					{
						var userUuid = validateApiKeyResponse.UserUuid;

						var getUsersResponse = await AuthenticationIdentityApi.GetUsersAsync(new ()
						{
							UserUuids = new[] { userUuid.Value },
						});

						var user = getUsersResponse.Users.NullCheckedFirstOrDefault();

						if ((user?.IsActive).GetValueOrDefault())
						{
							var claims = await GetUserClaimsAsync(user);

							var principal = new System.Security.Claims.ClaimsPrincipal(new[]
							{
								new System.Security.Claims.ClaimsIdentity(claims),
							});

							var ticket = new Microsoft.AspNetCore.Authentication.AuthenticationTicket(principal, Scheme.Name);

							return Microsoft.AspNetCore.Authentication.AuthenticateResult.Success(ticket);
						}
					}
				}

				return Microsoft.AspNetCore.Authentication.AuthenticateResult.Fail("Invalid Token");
			}

			if (Request.Cookies.ContainsKey(CookieName))
			{
				var authenticationCookieValue = Request.Cookies[CookieName];

				if (!string.IsNullOrWhiteSpace(authenticationCookieValue))
				{
					var jwtSecurityTokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

					if (jwtSecurityTokenHandler.CanReadToken(authenticationCookieValue))
					{
						var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(authenticationCookieValue);

						var principal = new System.Security.Claims.ClaimsPrincipal(new[]
						{
							new System.Security.Claims.ClaimsIdentity(jwtToken.Claims, AuthenticationHandlerName),
						});

						var ticket = new Microsoft.AspNetCore.Authentication.AuthenticationTicket(principal, Scheme.Name);

						return Microsoft.AspNetCore.Authentication.AuthenticateResult.Success(ticket);
					}
				}
			}

			return Microsoft.AspNetCore.Authentication.AuthenticateResult.NoResult();
		}
	}
}
