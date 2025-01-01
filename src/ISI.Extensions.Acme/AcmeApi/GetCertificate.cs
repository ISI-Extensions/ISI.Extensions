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
		public DTOs.GetCertificateResponse GetCertificate(DTOs.GetCertificateRequest request)
		{
			var response = new DTOs.GetCertificateResponse();

			var uri = new UriBuilder(request.GetCertificatesUrl);
			if (request.GetPrivateKey)
			{
				uri.AddQueryStringParameter("getPrivateKey", true);
			}
			else if (request.GetPrivateKeyPassword)
			{
				uri.AddQueryStringParameter("getPrivateKeyPassword", true);
			}
			else if (request.GetPfxCertificate)
			{
				uri.AddQueryStringParameter("getPfxCertificate", true);
			}
			else if (request.GetPfxCertificatePassword)
			{
				uri.AddQueryStringParameter("getPfxCertificatePassword", true);
			}

			var acmeResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, GetHeaders(request), true);

			if (request.GetPrivateKeyPassword || request.GetPfxCertificatePassword)
			{
				response.CertificatePassword = acmeResponse.Content;
			}
			else if (request.GetPfxCertificate)
			{
				response.CertificatePfx = Convert.FromBase64String(acmeResponse.Content);
			}
			else
			{
				response.CertificatePem = acmeResponse.Content;
			}

			return response;
		}
	}
}