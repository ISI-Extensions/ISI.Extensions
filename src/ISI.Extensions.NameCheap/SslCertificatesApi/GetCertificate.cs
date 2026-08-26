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
using ISI.Extensions.NameCheap.Extensions;
using DTOs = ISI.Extensions.NameCheap.DataTransferObjects.SslCertificatesApi;

namespace ISI.Extensions.NameCheap
{
	public partial class SslCertificatesApi
	{
		public DTOs.GetCertificateResponse GetCertificate(DTOs.GetCertificateRequest request)
		{
			var response = new DTOs.GetCertificateResponse();

			var uri = request.GetUrl(Configuration);
			uri.SetUserNameClientIp(request, IpifyApi, Configuration);
			uri.AddQueryStringParameter("Command", "namecheap.ssl.getinfo");
			uri.AddQueryStringParameter("certificateID", request.VendorCertificateKey);
			uri.AddQueryStringParameter("returncertificate", "true");
			uri.AddQueryStringParameter("returntype", "individual");
			
			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteXmlGet<SerializableModels.SslCertificatesApi.GetCertificateResponse>(uri.Uri, null, true);

			var certificates = new List<(ISI.Extensions.Certificates.CertificateType CertificateType, string Certificate)>();

			if (apiResponse?.CommandResponse?.SSLGetInfoResult?.CertificateDetails != null)
			{
				if (!string.IsNullOrWhiteSpace(apiResponse.CommandResponse.SSLGetInfoResult.CertificateDetails.CSR))
				{
					certificates.Add((CertificateType: ISI.Extensions.Certificates.CertificateType.CertificateSigningRequest, Certificate: apiResponse.CommandResponse.SSLGetInfoResult.CertificateDetails.CSR));
				}

				if (!string.IsNullOrWhiteSpace(apiResponse.CommandResponse.SSLGetInfoResult.CertificateDetails.Certificates?.Certificate))
				{
					certificates.Add((CertificateType: ISI.Extensions.Certificates.CertificateType.Crt, Certificate: apiResponse.CommandResponse.SSLGetInfoResult.CertificateDetails.Certificates.Certificate));
				}

				var caCertificate = string.Join("\n", (apiResponse.CommandResponse.SSLGetInfoResult.CertificateDetails?.Certificates?.CaCertificates ?? []).Select(certificate => certificate.Certificate));

				if (!string.IsNullOrWhiteSpace(caCertificate))
				{
					certificates.Add((CertificateType: ISI.Extensions.Certificates.CertificateType.CaBundleCrt, Certificate: caCertificate));
				}
			}

			response.Certificates = certificates.ToArray();

			return response;
		}
	}
}