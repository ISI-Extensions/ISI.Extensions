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
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonJwt.Extensions;
using DTOs = ISI.Extensions.Acme.DataTransferObjects.AcmeApi;

namespace ISI.Extensions.Acme
{
	public partial class AcmeApi
	{
		public DTOs.CreateNewAcmeOrderResponse CreateNewAcmeOrder(DTOs.CreateNewAcmeOrderRequest request)
		{
			var response = new DTOs.CreateNewAcmeOrderResponse();

			var uri = new Uri(request.AcmeHostContext.AcmeHostDirectory.CreateNewOrderUrl);

			var acmeRequest = new ISI.Extensions.Acme.SerializableModels.Orders.CreateNewAcmeOrderRequest()
			{
				CertificateNotBeforeDateTimeUtc = request.CertificateNotBeforeDateTimeUtc,
				CertificateNotAfterDateTimeUtc = request.CertificateNotAfterDateTimeUtc,
				CertificateIdentifiers = request.CertificateIdentifiers.ToNullCheckedArray(certificateIdentifier => new ISI.Extensions.Acme.SerializableModels.Orders.AcmeOrderCertificateIdentifier()
				{
					CertificateIdentifierType = certificateIdentifier.CertificateIdentifierType,
					CertificateIdentifierValue = certificateIdentifier.CertificateIdentifierValue,
				}),
				PostRenewalActions = request.PostRenewalActions.ToNullCheckedArray(postRenewalAction =>
					{
						switch (postRenewalAction)
						{
							case AcmeOrderCertificateDomainPostRenewalActionAcmeAgentWebHook acmeOrderCertificateDomainPostRenewalActionAcmeAgentWebHook:
								return new ISI.Extensions.Acme.SerializableModels.Orders.AcmeOrderCertificateDomainPostRenewalActionAcmeAgentWebHook()
								{
									PushWebHooks = acmeOrderCertificateDomainPostRenewalActionAcmeAgentWebHook.PushWebHooks.ToNullCheckedArray(pushWebHook => new ISI.Extensions.Acme.SerializableModels.Orders.AcmeOrderCertificateDomainPostRenewalActionAcmeAgentWebHookPushWebHook()
									{
										CertificateType = pushWebHook.CertificateType,
										PostUrl = pushWebHook.PostUrl,
									})
								} as ISI.Extensions.Acme.SerializableModels.Orders.IAcmeOrderCertificateDomainPostRenewalAction;

							default:
								throw new ArgumentOutOfRangeException(nameof(postRenewalAction));
						}
					}),
			};

			var jwt = new ISI.Extensions.JsonJwt.Jwt();
			jwt.Header.Add(ISI.Extensions.JsonJwt.HeaderKey.Nonce, request.AcmeHostContext.Nonce);

			jwt.AddToPayload(acmeRequest, JsonSerializer);

			var signedJwt = JwtEncoder.BuildSignedJwt(request.AcmeHostContext.JwkAlgorithmKey, request.AcmeHostContext.SerializedJwk, request.AcmeHostContext.Pem, null, jwt);

#if DEBUG
			if (JwtEncoder.TryDecodeSignedJwt(signedJwt, null, out var _jwt))
			{
				var _request = _jwt.DeserializePayload<ISI.Extensions.Acme.SerializableModels.Orders.CreateNewAcmeOrderRequest>(JsonSerializer);
			}
#endif

			var acmeResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<ISI.Extensions.JsonJwt.SerializableEntities.SignedJwt, ISI.Extensions.WebClient.Rest.SerializedResponse<ISI.Extensions.Acme.SerializableModels.Orders.CreateNewAcmeOrderResponse>>(uri, GetHeaders(request), signedJwt, true);

			if (acmeResponse.ResponseHeaders.TryGetValue(ISI.Extensions.JsonJwt.HeaderKey.ReplayNonce, out var nonce))
			{
				request.AcmeHostContext.Nonce = nonce;
			}

			response.AcmeOrder = acmeResponse.Response.NullCheckedConvert(source => new AcmeOrder()
			{
				OrderStatus = source.OrderStatus,
				RequestExpiresDateTimeUtc = source.RequestExpiresDateTimeUtc,
				CertificateNotBeforeDateTimeUtc = source.CertificateNotBeforeDateTimeUtc,
				CertificateNotAfterDateTimeUtc = source.CertificateNotAfterDateTimeUtc,
				CertificateIdentifiers = source.CertificateIdentifiers.ToNullCheckedArray(certificateIdentifier => new AcmeOrderCertificateIdentifier()
				{
					CertificateIdentifierType = certificateIdentifier.CertificateIdentifierType,
					CertificateIdentifierValue = certificateIdentifier.CertificateIdentifierValue,
				}),
				AuthorizationsUrls = source.AuthorizationsUrls.ToNullCheckedArray(),
				FinalizeOrderUrl = source.FinalizeOrderUrl,
				GetCertificateUrl = source.GetCertificateUrl,
				Error = source.Error.NullCheckedConvert(error => new AcmeOrderError()
				{
					ErrorType = error.ErrorType,
					Detail = error.Detail,
					SubProblems = error.SubProblems.ToNullCheckedArray(subProblem => new AcmeOrderErrorSubProblem()
					{
						ErrorType = subProblem.ErrorType,
						Detail = subProblem.Detail,
						Identifier = subProblem.Identifier.NullCheckedConvert(identifier => new AcmeOrderCertificateIdentifier()
						{
							CertificateIdentifierType = identifier.CertificateIdentifierType,
							CertificateIdentifierValue = identifier.CertificateIdentifierValue,
						}),
					}),
				})
			});

			return response;
		}
	}
}