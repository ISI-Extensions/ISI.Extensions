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
using System.Runtime.Serialization;
using DTOs = ISI.Extensions.TrueNAS.DataTransferObjects.TrueNASApi;
using SerializableDTOs = ISI.Extensions.TrueNAS.SerializableModels.Rest;

namespace ISI.Extensions.TrueNAS
{
	public partial class TrueNASRestApi
	{
		public async Task<DTOs.ListCertificateChoicesResponse> ListCertificateChoicesAsync(DTOs.ListCertificateChoicesRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.ListCertificateChoicesResponse();

			var uri = GetApiUri(request);
			uri.SetPathAndQueryString("api/v2.0/system/general/ui_certificate_choices");

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteTextGet(uri.Uri, GetHeaders(request), true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);

			var certificates = new List<(string CertificateKey, string CertificateName)>();

			foreach (var line in apiResponse.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
			{
				var certificatePieces = line.Split([':'], 2);

				if (certificatePieces.Length == 2)
				{
					var certificateIndex = certificatePieces[0].Trim(' ', '"').ToInt();
					if (certificateIndex > 0)
					{
						var certificateName = certificatePieces[1].Trim(' ', ',').Trim('"').Trim();

						if (!string.IsNullOrWhiteSpace(certificateName))
						{
							certificates.Add((CertificateKey: $"{certificateIndex}", CertificateName: certificateName));
						}
					}
				}
			}

			response.Certificates = certificates.ToArray();

			return response;
		}
	}
}