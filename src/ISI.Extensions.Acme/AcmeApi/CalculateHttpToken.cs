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
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonJwt.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Acme.DataTransferObjects.AcmeApi;

namespace ISI.Extensions.Acme
{
	public partial class AcmeApi
	{
		public DTOs.CalculateHttpTokenResponse CalculateHttpToken(DTOs.CalculateHttpTokenRequest request)
		{
			var response = new DTOs.CalculateHttpTokenResponse();

			var domainPieces = request.Domain.Split('.', StringSplitOptions.RemoveEmptyEntries);
			domainPieces.Reverse();
			var domainQueue = new Queue<string>(domainPieces);

			var domain = new List<string>();
			domain.Insert(0, domainQueue.Dequeue());
			domain.Insert(0, domainQueue.Dequeue());

			while (domainQueue.Any())
			{
				var path = domainQueue.Dequeue();
				if (!string.Equals(path, "*", StringComparison.InvariantCultureIgnoreCase))
				{
					domain.Insert(0, path);
				}
			}
			response.Domain = string.Join(".", domain);

			var uri = new UriBuilder(Uri.UriSchemeHttp, response.Domain)
			{
				Path = $".well-known/acme-challenge/{request.ChallengeToken}",
			};

			response.Url = uri.Uri.ToString();

			using (var privateKey = System.Security.Cryptography.ECDsa.Create())
			{
				privateKey.ImportFromPem(request.HostContext.Pem);

				var securityKey = new Microsoft.IdentityModel.Tokens.ECDsaSecurityKey(privateKey);

				using (var sha = System.Security.Cryptography.SHA256.Create())
				{
					var jwkHash = securityKey.ComputeJwkThumbprint();
					var jwkThumb = UrlEncode(jwkHash);

					var keyAuthz = $"{request.ChallengeToken}.{jwkThumb}";
					var keyAuthzDig = sha.ComputeHash(Encoding.UTF8.GetBytes(keyAuthz));

					response.HttpToken = UrlEncode(keyAuthzDig);
				}
			}

			return response;
		}
	}
}