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
		public DTOs.ProcessNewOrderResponse ProcessNewOrder(DTOs.IProcessNewOrderRequest request)
		{
			switch (request)
			{
				case DTOs.ProcessNewOrderUsingDnsRequest processNewOrderUsingDnsRequest:
					return ProcessNewOrderUsingDns(processNewOrderUsingDnsRequest);

				case DTOs.ProcessNewOrderUsingExistingCertificateRequest processNewOrderUsingExistingCertificateRequest:
					return ProcessNewOrderUsingExistingCertificate(processNewOrderUsingExistingCertificateRequest);

				default:
					throw new ArgumentOutOfRangeException(nameof(request));
			}
		}

		private DTOs.ProcessNewOrderResponse ProcessNewOrderUsingExistingCertificate(DTOs.ProcessNewOrderUsingExistingCertificateRequest request)
		{
			var response = new DTOs.ProcessNewOrderResponse();

			var createNewOrderResponse = CreateNewOrder(new()
			{
				HostContext = request.HostContext,
				CertificateIdentifiers =
				[
					new ISI.Extensions.Acme.OrderCertificateIdentifier()
					{
						CertificateIdentifierType = ISI.Extensions.Acme.OrderCertificateIdentifierType.Dns,
						CertificateIdentifierValue = request.Domain,
					}
				],
			});

			var getOrderResponse = GetOrder(new()
			{
				HostContext = request.HostContext,
				OrderUrl = createNewOrderResponse.Order.OrderKey,
			});

			response.GetCertificatesUrl = getOrderResponse.Order.GetCertificatesUrl;

			return response;
		}

		private DTOs.ProcessNewOrderResponse ProcessNewOrderUsingDns(DTOs.ProcessNewOrderUsingDnsRequest request)
		{
			var response = new DTOs.ProcessNewOrderResponse();

			var domains = new List<string>();
			domains.Add(request.Domain);
			if (request.Domain.StartsWith("*."))
			{
				domains.Add(request.Domain.TrimStart("*."));
			}

			var createNewOrderResponse = CreateNewOrder(new()
			{
				HostContext = request.HostContext,
				CertificateNotBeforeDateTimeUtc = request.CertificateNotBeforeDateTimeUtc,
				CertificateNotAfterDateTimeUtc = request.CertificateNotAfterDateTimeUtc,
				CertificateIdentifiers = domains.ToNullCheckedArray(domain => new ISI.Extensions.Acme.OrderCertificateIdentifier()
				{
					CertificateIdentifierType = ISI.Extensions.Acme.OrderCertificateIdentifierType.Dns,
					CertificateIdentifierValue = domain,
				}),
			});

			var challengeUrls = new HashSet<string>(StringComparer.InvariantCulture);

			foreach (var authorizationUrl in createNewOrderResponse.Order.AuthorizationUrls)
			{
				var getAuthorizationResponse = GetAuthorization(new()
				{
					HostContext = request.HostContext,
					AuthorizationUrl = authorizationUrl,
				});

				foreach (var challenge in getAuthorizationResponse.Authorization.Challenges.NullCheckedWhere(challenge => (challenge.ChallengeType == ISI.Extensions.Acme.OrderCertificateIdentifierAuthorizationChallengeType.Dns01) && !challengeUrls.Contains(challenge.ChallengeUrl)))
				{
					var calculateDnsTokenResponse = CalculateDnsToken(new()
					{
						HostContext = request.HostContext,
						Domain = getAuthorizationResponse.Authorization.CertificateIdentifier.CertificateIdentifierValue,
						ChallengeToken = challenge.Token,
					});

					var dnsRecord = new ISI.Extensions.Dns.DnsRecord()
					{
						Data = calculateDnsTokenResponse.DnsToken,
						Name = calculateDnsTokenResponse.DnsRecordName,
						Ttl = TimeSpan.FromMinutes(10),
						RecordType = ISI.Extensions.Dns.RecordType.TextRecord,
					};

					var txtRecords = DomainsApi.GetTxtRecords(new()
					{
						Domain = calculateDnsTokenResponse.Domain,
						Name = calculateDnsTokenResponse.DnsRecordName,
						NameServer = Configuration.NameServer,
					}).Values;

					if (!txtRecords.NullCheckedAny(txtRecord => string.Equals(txtRecord, calculateDnsTokenResponse.DnsToken, StringComparison.InvariantCulture)))
					{
						request.SetDnsRecord(calculateDnsTokenResponse.Domain, dnsRecord);

						System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));
					}

					var completeChallengeResponse = CompleteChallenge(new()
					{
						HostContext = request.HostContext,
						ChallengeUrl = challenge.ChallengeUrl,
					});

					System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));

					var getChallengeResponse = GetChallenge(new()
					{
						HostContext = request.HostContext,
						ChallengeUrl = challenge.ChallengeUrl,
					});

					challengeUrls.Add(challenge.ChallengeUrl);
				}
			}

			System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));

			var createCertificateSigningRequestResponse = CreateCertificateSigningRequest(new()
			{
				CertificateSigningRequestParameters = new ISI.Extensions.Acme.CertificateSigningRequestParameters()
				{
					CountryName = request.CountryName,
					State = request.State,
					Locality = request.Locality,
					Organization = request.Organization,
					OrganizationUnit = request.OrganizationUnit,
					CommonName = request.Domain,
				},
			});

			response.PrivateKeyPem = createCertificateSigningRequestResponse.PrivateKeyPem;

			var finalizeOrderResponse = FinalizeOrder(new()
			{
				HostContext = request.HostContext,
				FinalizeOrderUrl = createNewOrderResponse.Order.FinalizeOrderUrl,
				CertificateSigningRequest = createCertificateSigningRequestResponse.CertificateSigningRequest,
			});

			System.Threading.Thread.Sleep(TimeSpan.FromMinutes(2));

			var getOrderResponse = GetOrder(new()
			{
				HostContext = request.HostContext,
				OrderUrl = createNewOrderResponse.Order.OrderKey,
			});


			var getCertificateResponse = GetCertificate(new()
			{
				GetCertificatesUrl = getOrderResponse.Order.GetCertificatesUrl,
			});

			response.CertificatePem = getCertificateResponse.CertificatePem;

			return response;
		}
	}
}