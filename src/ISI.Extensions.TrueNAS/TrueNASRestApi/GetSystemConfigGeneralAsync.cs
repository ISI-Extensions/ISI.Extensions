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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.TrueNAS.DataTransferObjects.TrueNASApi;
using SerializableDTOs = ISI.Extensions.TrueNAS.SerializableModels.Rest;

namespace ISI.Extensions.TrueNAS
{
	public partial class TrueNASRestApi
	{
		public async Task<DTOs.GetSystemConfigGeneralResponse> GetSystemConfigGeneralAsync(DTOs.GetSystemConfigGeneralRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.GetSystemConfigGeneralResponse();

			var uri = GetApiUri(request);
			uri.SetPathAndQueryString("api/v2.0/system/general");

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SerializableDTOs.GetGeneralInformationResponse>(uri.Uri, GetHeaders(request), true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);

			response.SystemConfigGeneralId = apiResponse?.SystemConfigGeneralId ?? 0;
			response.Certificate = (apiResponse?.UiCertificate).NullCheckedConvert(certificate => new DTOs.GetSystemConfigGeneralResponseCertificate()
			{
				CertificateId = certificate.CertificateId,
				Type = certificate.Type,
				CertificateName = certificate.CertificateName,
				Certificate = certificate.Certificate,
				PrivateKey = certificate.PrivateKey,
				Csr = certificate.Csr,
				//RenewDays = certificate.RenewDays,
				//AddToTrustedStore = certificate.AddToTrustedStore,
				RootPath = certificate.RootPath,
				CertificatePath = certificate.CertificatePath,
				PrivateKeyPath = certificate.PrivateKeyPath,
				CertType = certificate.CertType,
				CertTypeExisting = certificate.CertTypeExisting,
				CertTypeCsr = certificate.CertTypeCsr,
				//CertTypeCa = certificate.CertTypeCa,
				ChainList = certificate.ChainList.ToNullCheckedArray(),
				KeyLength = certificate.KeyLength,
				KeyType = certificate.KeyType,
				Common = certificate.Common,
				San = certificate.San.ToNullCheckedArray(),
				Dn = certificate.Dn,
				SubjectNameHash = certificate.SubjectNameHash,
				Extensions = certificate.Extensions.NullCheckedConvert(extension => new DTOs.GetSystemConfigGeneralResponseExtensions()
				{
					AuthorityKeyIdentifier = extension.AuthorityKeyIdentifier,
					SubjectKeyIdentifier = extension.SubjectKeyIdentifier,
					KeyUsage = extension.KeyUsage,
					BasicConstraints = extension.BasicConstraints,
					ExtendedKeyUsage = extension.ExtendedKeyUsage,
					CertificatePolicies = extension.CertificatePolicies,
					AuthorityInfoAccess = extension.AuthorityInfoAccess,
					SubjectAltName = extension.SubjectAltName,
					CtPrecertScts = extension.CtPrecertScts,
				}),
				DigestAlgorithm = certificate.DigestAlgorithm,
				Lifetime = certificate.Lifetime,
				From = certificate.From,
				Until = certificate.Until,
				Serial = certificate.Serial,
				Chain = certificate.Chain,
				Fingerprint = certificate.Fingerprint,
				//Expired = certificate.Expired,
				Parsed = certificate.Parsed,
			});
			;
			//response.UiHttpsPort = apiResponse?.UiHttpsPort;
			//response.UiHttpsRedirect = apiResponse?.UiHttpsRedirect;
			//response.UiHttpsProtocols = apiResponse?.UiHttpsProtocols.ToNullCheckedArray();
			response.UiPort = apiResponse?.UiPort ?? 0;
			response.UiAddress = apiResponse?.UiAddress.ToNullCheckedArray();
			response.UiV6Address = apiResponse?.UiV6Address.ToNullCheckedArray();
			//response.UiConsoleMessage = apiResponse?.UiConsoleMessage;
			//response.UiXFrameOptions = apiResponse?.UiXFrameOptions;
			response.Kbdmap = apiResponse?.Kbdmap;
			response.Timezone = apiResponse?.Timezone;
			response.UsageCollection = apiResponse?.UsageCollection ?? false;
			response.WizardShown = apiResponse?.WizardShown ?? false;
			response.UsageCollectionIsSet = apiResponse?.UsageCollectionIsSet ?? false;
			//response.DsAuth = apiResponse?.DsAuth;

			return response;
		}
	}
}