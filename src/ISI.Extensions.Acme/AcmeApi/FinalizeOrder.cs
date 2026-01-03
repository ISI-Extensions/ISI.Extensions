#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
		public DTOs.FinalizeOrderResponse FinalizeOrder(DTOs.FinalizeOrderRequest request)
		{
			var response = new DTOs.FinalizeOrderResponse();

			var uri = new Uri(request.FinalizeOrderUrl);

			var acmeRequest = new ISI.Extensions.Acme.SerializableModels.AcmeOrders.FinalizeOrderRequest()
			{
				CertificateSigningRequest = UrlEncode(request.CertificateSigningRequest),
			};

			var securityTokenDescriptor = GetSecurityTokenDescriptor(request.HostContext, request.HostContext.AccountKey);
			securityTokenDescriptor.AdditionalHeaderClaims.Add(HeaderKey.Nonce, request.HostContext.Nonce);
			securityTokenDescriptor.AdditionalHeaderClaims.Add(HeaderKey.Url, uri.ToString());

			securityTokenDescriptor.AddToPayload(acmeRequest);

			var jsonWebToken = CreateToken(securityTokenDescriptor);

			var signedJwt = new ISI.Extensions.JsonJwt.SerializableEntities.SignedJwt(jsonWebToken);

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var acmeResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost(new ISI.Extensions.WebClient.Rest.RestResponseTypeCollection()
			{
				{ System.Net.HttpStatusCode.OK, typeof(ISI.Extensions.WebClient.Rest.SerializedResponse<ISI.Extensions.Acme.SerializableModels.AcmeOrders.FinalizeOrderResponse>) },
				{ System.Net.HttpStatusCode.Forbidden, typeof(ISI.Extensions.WebClient.Rest.SerializedResponse<ISI.Extensions.Acme.SerializableModels.AcmeOrders.ForbiddenResponse>) },
			}, uri, GetHeaders(request), signedJwt, true);

			switch (acmeResponse.Response)
			{
				case ISI.Extensions.WebClient.Rest.SerializedResponse<ISI.Extensions.Acme.SerializableModels.AcmeOrders.FinalizeOrderResponse> finalizeOrderResponse:
					response.Order = finalizeOrderResponse.Response.NullCheckedConvert(source => new ISI.Extensions.Acme.Order()
					{
						OrderStatus = source.OrderStatus,
						RequestExpiresDateTimeUtc = source.RequestExpiresDateTimeUtc,
						CertificateNotBeforeDateTimeUtc = source.CertificateNotBeforeDateTimeUtc,
						CertificateNotAfterDateTimeUtc = source.CertificateNotAfterDateTimeUtc,
						CertificateIdentifiers = source.CertificateIdentifiers.ToNullCheckedArray(certificateIdentifier => new ISI.Extensions.Acme.OrderCertificateIdentifier()
						{
							CertificateIdentifierType = certificateIdentifier.CertificateIdentifierType,
							CertificateIdentifierValue = certificateIdentifier.CertificateIdentifierValue,
						}),
						AuthorizationUrls = source.AuthorizationUrls.ToNullCheckedArray(),
						FinalizeOrderUrl = source.FinalizeOrderUrl,
						GetCertificatesUrl = source.GetCertificateUrl,
					});

					response.Error = finalizeOrderResponse.Response?.Error.NullCheckedConvert(error => new ISI.Extensions.Acme.OrderError()
					{
						ErrorType = error.ErrorType,
						Detail = error.Detail,
						Status = error.Status,
						SubProblems = error.SubProblems.ToNullCheckedArray(subProblem => new ISI.Extensions.Acme.OrderErrorSubProblem()
						{
							ErrorType = subProblem.ErrorType,
							Detail = subProblem.Detail,
							Identifier = subProblem.Identifier.NullCheckedConvert(identifier => new ISI.Extensions.Acme.OrderCertificateIdentifier()
							{
								CertificateIdentifierType = identifier.CertificateIdentifierType,
								CertificateIdentifierValue = identifier.CertificateIdentifierValue,
							}),
						}),
					});
					break;

				case ISI.Extensions.WebClient.Rest.SerializedResponse<ISI.Extensions.Acme.SerializableModels.AcmeOrders.ForbiddenResponse> forbiddenResponse:
					response.OrderNotReady = ((forbiddenResponse.Response?.Type ?? string.Empty).IndexOf("orderNotReady", StringComparison.InvariantCultureIgnoreCase) >= 0);
					response.OrderNotReadyDetail = forbiddenResponse.Response?.Detail;
					break;
			}

			if ((acmeResponse.Response is ISI.Extensions.WebClient.Rest.IResponseHeaders responseHeaders) && responseHeaders.ResponseHeaders.TryGetValue(HeaderKey.ReplayNonce, out var nonce))
			{
				request.HostContext.Nonce = nonce;
			}

			return response;
		}
	}
}