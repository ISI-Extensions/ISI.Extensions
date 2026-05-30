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
using DTOs = ISI.Extensions.NameCheap.DataTransferObjects.SslApi;
using SerializableModels = ISI.Extensions.NameCheap.SerializableModels.SslCertificatesApi;

namespace ISI.Extensions.NameCheap
{
	public partial class SslCertificatesApi
	{
		public DTOs.ListCertificatesResponse ListCertificates(DTOs.ListCertificatesRequest request)
		{
			var response = new DTOs.ListCertificatesResponse();

			var page = 1;

			var certificates = new List<DTOs.ListCertificatesResponseCertificate>();

			while (page > 0)
			{
				var uri = request.GetUrl(Configuration);
				uri.SetUserNameClientIp(request, IpifyApi, Configuration);
				uri.AddQueryStringParameter("Command", "namecheap.ssl.getList");
				uri.AddQueryStringParameter("Page", page);
				uri.AddQueryStringParameter("ListType", request.ListType.GetAbbreviation());
				if (!string.IsNullOrWhiteSpace(request.SearchTerm))
				{
					uri.AddQueryStringParameter("SearchTerm", request.SearchTerm);
				}

				var serviceResponse = ISI.Extensions.WebClient.Rest.ExecuteXmlGet<SerializableModels.SslCertificatesApi.ListCertificatesResponse>(uri.Uri, null, true);

				certificates.AddRange(serviceResponse?.CommandResponse?.Certificates?.ToNullCheckedArray(certificate => new DTOs.ListCertificatesResponseCertificate()
				{
					VendorCertificateKey = certificate.VendorCertificateKey,
					HostName = certificate.HostName,
					CertificateType = ISI.Extensions.Enum<NameCheapSslCertificateType>.Parse(certificate.CertificateType),
					PurchaseDate = certificate.PurchaseDate.ToDateTimeNullable(),
					ExpireDate = certificate.ExpireDate.ToDateTimeNullable(),
					ActivationExpireDate = certificate.ActivationExpireDate.ToDateTimeNullable(),
					IsExpired = certificate.IsExpired,
					Status = certificate.Status,
				}) ?? []);


				if (certificates.Count < (serviceResponse?.CommandResponse?.Paging?.TotalItems ?? 0))
				{
					page++;
				}
				else
				{
					page = 0;
				}
			}

			response.Certificates = certificates.ToArray();

			return response;
		}
	}
}