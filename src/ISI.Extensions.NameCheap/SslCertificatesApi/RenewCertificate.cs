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
		public DTOs.RenewCertificateResponse RenewCertificate(DTOs.RenewCertificateRequest request)
		{
			var response = new DTOs.RenewCertificateResponse();

			request.CertificateType = UpdateCertificateType(request.CertificateType);

			var uri = request.GetUrl(Configuration);
			
			var formData = new ISI.Extensions.WebClient.Rest.FormDataCollection();
			formData.SetUserNameClientIp(request, IpifyApi, Configuration);
			formData.Add("Command", "namecheap.ssl.renew");
			formData.Add("CertificateID", request.VendorCertificateKey);
			formData.Add("Years", request.Years);
			formData.Add("SSLType", request.CertificateType.GetAbbreviation());

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteFormRequestXmlPost<SerializableModels.SslCertificatesApi.RenewCertificateResponse>(uri.Uri, request.GetHeaders(Configuration), formData, true);

			response.VendorCertificateKey = apiResponse.CommandResponse?.SSLRenewResult?.VendorCertificateKey;
			response.CertificateType = ISI.Extensions.Enum<NameCheapSslCertificateType?>.ParseAbbreviation(apiResponse.CommandResponse?.SSLRenewResult?.CertificateType);
			response.Years = apiResponse.CommandResponse?.SSLRenewResult?.Years ?? 0;
			response.OrderId = apiResponse.CommandResponse?.SSLRenewResult?.OrderId;
			response.TransactionId = apiResponse.CommandResponse?.SSLRenewResult?.TransactionId;
			response.ChargedAmount = apiResponse.CommandResponse?.SSLRenewResult?.ChargedAmount;

			return response;
		}
	}
}