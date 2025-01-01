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
		public DTOs.CreateNewOrderResponse CreateNewOrder(DTOs.CreateNewOrderRequest request)
		{
			var response = new DTOs.CreateNewOrderResponse();

			var uri = new Uri(request.HostContext.HostDirectory.CreateNewOrderUrl);

			var acmeRequest = new ISI.Extensions.Acme.SerializableModels.AcmeOrders.CreateNewOrderRequest()
			{
				CertificateNotBeforeDateTimeUtc = request.CertificateNotBeforeDateTimeUtc,
				CertificateNotAfterDateTimeUtc = request.CertificateNotAfterDateTimeUtc,
				CertificateIdentifiers = request.CertificateIdentifiers.ToNullCheckedArray(certificateIdentifier => new ISI.Extensions.Acme.SerializableModels.AcmeOrders.OrderCertificateIdentifier()
				{
					CertificateIdentifierType = certificateIdentifier.CertificateIdentifierType,
					CertificateIdentifierValue = certificateIdentifier.CertificateIdentifierValue,
				}),
				PostRenewalActions = request.PostRenewalActions.ToNullCheckedArray(postRenewalAction =>
					{
						switch (postRenewalAction)
						{
							case ISI.Extensions.Acme.OrderCertificateDomainPostRenewalActionAcmeAgentNginxWebHook orderCertificateDomainPostRenewalActionAcmeAgentNginxWebHook:
								return new ISI.Extensions.Acme.SerializableModels.AcmeOrders.OrderCertificateDomainPostRenewalActionAcmeAgentNginxWebHook()
								{
									HeaderAuthenticationKey = orderCertificateDomainPostRenewalActionAcmeAgentNginxWebHook.HeaderAuthenticationKey,
									HeaderAuthenticationValue = orderCertificateDomainPostRenewalActionAcmeAgentNginxWebHook.HeaderAuthenticationValue,
									SetCertificatesUrl = orderCertificateDomainPostRenewalActionAcmeAgentNginxWebHook.SetCertificatesUrl,
								} as ISI.Extensions.Acme.SerializableModels.AcmeOrders.IOrderCertificateDomainPostRenewalAction;

							case ISI.Extensions.Acme.OrderCertificateDomainPostRenewalActionAcmeAgentNginxManagerAgentWebHook orderCertificateDomainPostRenewalActionAcmeAgentNginxManagerAgentWebHook:
								return new ISI.Extensions.Acme.SerializableModels.AcmeOrders.OrderCertificateDomainPostRenewalActionAcmeAgentNginxManagerAgentWebHook()
								{
									HeaderAuthenticationKey = orderCertificateDomainPostRenewalActionAcmeAgentNginxManagerAgentWebHook.HeaderAuthenticationKey,
									HeaderAuthenticationValue = orderCertificateDomainPostRenewalActionAcmeAgentNginxManagerAgentWebHook.HeaderAuthenticationValue,
									SetCertificatesUrl = orderCertificateDomainPostRenewalActionAcmeAgentNginxManagerAgentWebHook.SetCertificatesUrl,
								} as ISI.Extensions.Acme.SerializableModels.AcmeOrders.IOrderCertificateDomainPostRenewalAction;

							case ISI.Extensions.Acme.OrderCertificateDomainPostRenewalActionAcmeAgentWebHook orderCertificateDomainPostRenewalActionAcmeAgentWebHook:
								return new ISI.Extensions.Acme.SerializableModels.AcmeOrders.OrderCertificateDomainPostRenewalActionAcmeAgentWebHook()
								{
									HeaderAuthenticationKey = orderCertificateDomainPostRenewalActionAcmeAgentWebHook.HeaderAuthenticationKey,
									HeaderAuthenticationValue = orderCertificateDomainPostRenewalActionAcmeAgentWebHook.HeaderAuthenticationValue,
									SetCertificatesUrl = orderCertificateDomainPostRenewalActionAcmeAgentWebHook.SetCertificatesUrl,
								} as ISI.Extensions.Acme.SerializableModels.AcmeOrders.IOrderCertificateDomainPostRenewalAction;

							default:
								throw new ArgumentOutOfRangeException(nameof(postRenewalAction));
						}
					}),
			};

			var securityTokenDescriptor = GetSecurityTokenDescriptor(request.HostContext, request.HostContext.AccountKey);
			securityTokenDescriptor.AdditionalHeaderClaims.Add(HeaderKey.Nonce, request.HostContext.Nonce);
			securityTokenDescriptor.AdditionalHeaderClaims.Add(HeaderKey.Url, uri.ToString());

			securityTokenDescriptor.AddToPayload(acmeRequest);

			var jsonWebToken = CreateToken(securityTokenDescriptor);

#if DEBUG
			var jwtSecurityTokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

			if (jwtSecurityTokenHandler.CanReadToken(jsonWebToken))
			{
				var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(jsonWebToken);

				var YYY = jwtSecurityToken.DeserializePayload<ISI.Extensions.Acme.SerializableModels.AcmeOrders.CreateNewOrderRequest>(JsonSerializer);
			}
#endif
			
			var signedJwt = new ISI.Extensions.JsonJwt.SerializableEntities.SignedJwt(jsonWebToken);

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var acmeResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<ISI.Extensions.JsonJwt.SerializableEntities.SignedJwt, ISI.Extensions.WebClient.Rest.SerializedResponse<ISI.Extensions.Acme.SerializableModels.AcmeOrders.CreateNewOrderResponse>>(uri, GetHeaders(request), signedJwt, true);

			if (acmeResponse.ResponseHeaders.TryGetValue(HeaderKey.ReplayNonce, out var nonce))
			{
				request.HostContext.Nonce = nonce;
			}


			acmeResponse.ResponseHeaders.TryGetValue(HeaderKey.Location, out var orderUrl);

			response.Order = acmeResponse.Response.NullCheckedConvert(source => new Order()
			{
				OrderKey = orderUrl,
				OrderStatus = source.OrderStatus,
				RequestExpiresDateTimeUtc = source.RequestExpiresDateTimeUtc,
				CertificateNotBeforeDateTimeUtc = source.CertificateNotBeforeDateTimeUtc,
				CertificateNotAfterDateTimeUtc = source.CertificateNotAfterDateTimeUtc,
				CertificateIdentifiers = source.CertificateIdentifiers.ToNullCheckedArray(certificateIdentifier => new OrderCertificateIdentifier()
				{
					CertificateIdentifierType = certificateIdentifier.CertificateIdentifierType,
					CertificateIdentifierValue = certificateIdentifier.CertificateIdentifierValue,
				}),
				AuthorizationUrls = source.AuthorizationsUrls.ToNullCheckedArray(),
				FinalizeOrderUrl = source.FinalizeOrderUrl,
				GetCertificatesUrl = source.GetCertificateUrl,
			});

			response.Error = acmeResponse.Response?.Error.NullCheckedConvert(error => new OrderError()
			{
				ErrorType = error.ErrorType,
				Detail = error.Detail,
				Status = error.Status,
				SubProblems = error.SubProblems.ToNullCheckedArray(subProblem => new OrderErrorSubProblem()
				{
					ErrorType = subProblem.ErrorType,
					Detail = subProblem.Detail,
					Identifier = subProblem.Identifier.NullCheckedConvert(identifier => new OrderCertificateIdentifier()
					{
						CertificateIdentifierType = identifier.CertificateIdentifierType,
						CertificateIdentifierValue = identifier.CertificateIdentifierValue,
					}),
				}),
			});

			return response;
		}
	}
}