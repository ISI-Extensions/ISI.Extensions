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
		public DTOs.CreateNewAccountResponse CreateNewAccount(DTOs.CreateNewAccountRequest request)
		{
			var response = new DTOs.CreateNewAccountResponse();

			var uri = new Uri(request.HostContext.HostDirectory.CreateNewAccountUrl);

			var acmeRequest = new ISI.Extensions.Acme.SerializableModels.AcmeAccounts.CreateNewAccountRequest()
			{
				AccountName = request.AccountName,
				Contacts = request.Contacts.ToNullCheckedArray(contact => $"mailto:{contact.TrimStart("mailto:", StringComparison.InvariantCultureIgnoreCase)}"),
				TermsOfServiceAgreed = request.TermsOfServiceAgreed,
				OnlyReturnExisting = request.OnlyReturnExisting,
			};

			var securityTokenDescriptor = GetSecurityTokenDescriptor(request.HostContext);

			var jsonWebKey = JsonSerializer.Deserialize<ISI.Extensions.JsonJwt.SerializableEntities.JsonWebKey>(request.HostContext.SerializedJsonWebKey);
			securityTokenDescriptor.AdditionalHeaderClaims.Add(HeaderKey.JsonWebKey, jsonWebKey.ToDictionary());
			securityTokenDescriptor.AdditionalHeaderClaims.Add(HeaderKey.Nonce, request.HostContext.Nonce);
			securityTokenDescriptor.AdditionalHeaderClaims.Add(HeaderKey.Url, uri.ToString());

			securityTokenDescriptor.AddToPayload(acmeRequest);

			var jsonWebToken = CreateToken(securityTokenDescriptor);

			var signedJwt = new ISI.Extensions.JsonJwt.SerializableEntities.SignedJwt(jsonWebToken);

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var acmeResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<ISI.Extensions.JsonJwt.SerializableEntities.SignedJwt, ISI.Extensions.WebClient.Rest.SerializedResponse<ISI.Extensions.Acme.SerializableModels.AcmeAccounts.CreateNewAccountResponse>>(uri, GetHeaders(request), signedJwt, true);

			if (acmeResponse.ResponseHeaders.TryGetValue(HeaderKey.ReplayNonce, out var nonce))
			{
				request.HostContext.Nonce = nonce;
			}

			if (acmeResponse.ResponseHeaders.TryGetValue(HeaderKey.Location, out var accountUrl))
			{
				request.HostContext.AccountKey = accountUrl;
			}

			response.Account = acmeResponse.Response.NullCheckedConvert(source => new Account()
			{
				AccountKey = accountUrl,
				SerializedJsonWebKey = JsonSerializer.Serialize(acmeResponse.Response.JsonWebKey),
				AccountStatus = acmeResponse.Response.AccountStatus,
				AccountName = acmeResponse.Response.AccountName,
				Contacts = acmeResponse.Response.Contacts.ToNullCheckedArray(),
				TermsOfServiceAgreed = request.TermsOfServiceAgreed,
				OnlyReturnExisting = request.OnlyReturnExisting,
			});

			return response;
		}
	}
}