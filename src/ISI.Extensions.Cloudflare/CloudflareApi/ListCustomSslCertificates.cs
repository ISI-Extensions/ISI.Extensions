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
using DTOs = ISI.Extensions.Cloudflare.DataTransferObjects.CloudflareApi;
using SerializableDTOs = ISI.Extensions.Cloudflare.SerializableModels;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Cloudflare
{
	public partial class CloudflareApi
	{
		//Accepted Permissions (at least one required)
		//Access: Mutual TLS Certificates Write Access: Mutual TLS Certificates Read SSL and Certificates Write SSL and Certificates Read
		public DTOs.ListCustomSslCertificatesResponse ListCustomSslCertificates(DTOs.ListCustomSslCertificatesRequest request)
		{
			var response = new DTOs.ListCustomSslCertificatesResponse();

			EnsureZoneId(request);

			var uri = GetUrl(request);
			uri.AddDirectoryToPath("zones/{zoneId}/custom_certificates".Replace("{zoneId}", request.ZoneId));

			try
			{
				var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SerializableDTOs.ListCustomSslCertificatesResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), false);

				if (restResponse.Error != null)
				{
					throw restResponse.Error.Exception;
				}

				response.SslCertificates = (restResponse?.Response?.SslCertificates).ToNullCheckedArray(sslCertificate => sslCertificate.Export());
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "ListCustomSslCertificates Failed\n{0}", exception.ErrorMessageFormatted());
			}

			return response;
		}
	}
}