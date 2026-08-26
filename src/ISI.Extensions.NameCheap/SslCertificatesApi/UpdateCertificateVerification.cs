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
		public DTOs.UpdateCertificateVerificationResponse UpdateCertificateVerification(DTOs.UpdateCertificateVerificationRequest request)
		{
			var response = new DTOs.UpdateCertificateVerificationResponse();

			var uri = request.GetUrl(Configuration);

			var formData = new ISI.Extensions.WebClient.Rest.FormDataCollection();
			formData.SetUserNameClientIp(request, IpifyApi, Configuration);
			formData.Add("Command", "namecheap.ssl.editdcvmethod");
			formData.Add("CertificateID", request.VendorCertificateKey);
			formData.Add("DCVMethod", "CNAME_CSR_HASH");

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteFormRequestXmlPost<SerializableModels.SslCertificatesApi.UpdateCertificateVerificationResponse>(uri.Uri, request.GetHeaders(Configuration), formData, true);

			response.EditValidationKey = apiResponse.CommandResponse?.SSLEditDCVMethodResult?.EditValidationKey;
			response.Success = apiResponse.CommandResponse?.SSLEditDCVMethodResult?.Success ?? false;

			if (apiResponse.CommandResponse?.SSLEditDCVMethodResult?.HttpDCValidation?.ValueAvailable ?? false)
			{
				response.HttpValidation = apiResponse.CommandResponse?.SSLEditDCVMethodResult?.HttpDCValidation?.NullCheckedConvert(httpDCValidation => new DTOs.UpdateCertificateVerificationResponseHttpValidation()
				{
					FileName = httpDCValidation.FileName,
					FileContent = httpDCValidation.FileContent,
				});
			}

			if (apiResponse.CommandResponse?.SSLEditDCVMethodResult?.DNSDCValidation?.ValueAvailable ?? false)
			{
				response.DNSValidation = apiResponse.CommandResponse?.SSLEditDCVMethodResult?.DNSDCValidation?.NullCheckedConvert(dnsDCValidation => new DTOs.UpdateCertificateVerificationResponseDNSValidation()
				{
					HostName = dnsDCValidation.HostName,
					Target = dnsDCValidation.Target,
				});
			}

			return response;
		}
	}
}