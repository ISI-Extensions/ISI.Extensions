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
using SerializableModels = ISI.Extensions.NameCheap.SerializableModels.SslCertificatesApi;

namespace ISI.Extensions.NameCheap
{
	public partial class SslCertificatesApi
	{
		public DTOs.CreateCertificateResponse CreateCertificate(DTOs.CreateCertificateRequest request)
		{
			var response = new DTOs.CreateCertificateResponse();

			request.CertificateType = UpdateCertificateType(request.CertificateType);

			var uri = request.GetUrl(Configuration);
			uri.Path = "xml.response";
			uri.SetUserNameClientIp(request, IpifyApi, Configuration);
			uri.AddQueryStringParameter("Command", "namecheap.ssl.create");
			uri.AddQueryStringParameter("Years", request.Years);
			uri.AddQueryStringParameter("Type", request.CertificateType.GetAbbreviation());
			if (request.SansToAdd.HasValue)
			{
				uri.AddQueryStringParameter("SANStoADD", request.SansToAdd.Value);
			}

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteXmlGet<SerializableModels.SslCertificatesApi.CreateCertificateResponse>(uri.Uri, null, true);

			response.Success = apiResponse.CommandResponse?.SslCreateResult?.Success ?? false;
			response.VendorCertificateKey = apiResponse.CommandResponse?.SslCreateResult?.SslCertificate?.VendorCertificateKey;
			response.CertificateType = ISI.Extensions.Enum<NameCheapSslCertificateType?>.ParseAbbreviation(apiResponse.CommandResponse?.SslCreateResult?.SslCertificate?.CertificateType);
			response.Years = apiResponse.CommandResponse?.SslCreateResult?.SslCertificate?.Years ?? 0;
			response.CertificateStatus = ISI.Extensions.Enum<NameCheapSslCertificateStatus?>.ParseAbbreviation(apiResponse.CommandResponse?.SslCreateResult?.SslCertificate?.Status);

			response.WarningCode = apiResponse.Warnings?.Warning?.WarningCode;
			response.WarningDescription = apiResponse.Warnings?.Warning?.WarningDescription;

			response.ErrorCode = apiResponse.Errors?.Error?.ErrorCode;
			response.ErrorDescription = apiResponse.Errors?.Error?.ErrorDescription;

			return response;
		}
	}
}