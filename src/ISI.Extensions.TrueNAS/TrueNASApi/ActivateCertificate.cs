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
using DTOs = ISI.Extensions.TrueNAS.DataTransferObjects.TrueNASApi;
using SerializableDTOs = ISI.Extensions.TrueNAS.SerializableModels;

namespace ISI.Extensions.TrueNAS
{
	public partial class TrueNASApi
	{
		public DTOs.ActivateCertificateResponse ActivateCertificate(DTOs.ActivateCertificateRequest request)
		{
			var response = new DTOs.ActivateCertificateResponse();

			//{
			//	var uri = GetApiUri(request);
			//	uri.SetPathAndQueryString("api/v2.0/chart/release/id/minio");

			//	var apiResponse = Rest.ExecuteTextGet(uri.Uri, GetHeaders(request), false, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);
			//}

			var logEntries = new List<DTOs.LogEntry>();
			void addLog(string description)
			{
				logEntries.Add(new DTOs.LogEntry()
				{
					DateTimeStampUtc = DateTimeStamper.CurrentDateTimeUtc(),
					Description = description,
				});
			}

			var getVersionResponse = GetVersion(new()
			{
				TrueNASApiUrl = request.TrueNASApiUrl,
				TrueNASApiKey = request.TrueNASApiKey,
			});

			var activeCertificateId = (int?)null;
			var activeCertificateName = (string)null;
			{
				var uri = GetApiUri(request);
				uri.SetPathAndQueryString("api/v2.0/system/general");

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SerializableDTOs.GetGeneralInformationResponse>(uri.Uri, GetHeaders(request), true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);

				activeCertificateId = apiResponse?.UiCertificate?.Id;
				activeCertificateName = apiResponse?.UiCertificate?.Name;
			}

			//Push new Certificate
			{
				var uri = GetApiUri(request);
				uri.SetPathAndQueryString("api/v2.0/certificate");

				var apiRequest = new SerializableDTOs.SetCertificateRequest()
				{
					CertificateName = request.CertificateName,
					KeyCertificate = request.KeyCertificate,
					BundleCertificate = request.BundleCertificate,
				};

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.SetCertificateRequest, ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, GetHeaders(request), apiRequest, true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);

				addLog($"Push Cert: {apiResponse}");
			}

			int getNewCertificateIndex()
			{
				var uri = GetApiUri(request);
				uri.SetPathAndQueryString("api/v2.0/system/general/ui_certificate_choices");

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteTextGet(uri.Uri, GetHeaders(request), true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);

				foreach (var line in apiResponse.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
				{
					var certificatePieces = line.Split([':'], 2);

					if (certificatePieces.Length == 2)
					{
						var certificateIndex = certificatePieces[0].Trim(' ', '"').ToInt();
						if (certificateIndex > 0)
						{
							var certificateName = certificatePieces[1].Trim(' ', ',').Trim('"').Trim();

							if (!string.IsNullOrWhiteSpace(certificateName) && string.Equals(certificateName, request.CertificateName, StringComparison.InvariantCultureIgnoreCase))
							{
								return certificateIndex;
							}
						}
					}
				}

				throw new Exception("New certificate not found");
			}
			var newCertificateIndex = getNewCertificateIndex();

			//Set new UI Certificate
			{
				var uri = GetApiUri(request);
				uri.SetPathAndQueryString("api/v2.0/system/general");

				var apiRequest = new SerializableDTOs.ActivateCertificateRequest()
				{
					CertificateIndex = newCertificateIndex,
				};

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPut<SerializableDTOs.ActivateCertificateRequest, ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, GetHeaders(request), apiRequest, true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);

				addLog($"Activate Cert: {apiResponse}");
			}

			//https://raw.githubusercontent.com/acmesh-official/acme.sh/f981c782bb38015f4778913e9c3db26b57dde4e8/deploy/truenas.sh

			//if ((getVersionResponse.Product == TrueNASProduct.Core) && (version >= new Version("13")))
			//{
			//	{
			//		var uri = GetApiUri(request);
			//		uri.SetPathAndQueryString("api/v2.0/s3");

			//		var apiResponse = Rest.ExecuteTextGet(uri.Uri, GetHeaders(request), false, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);
			//	}

			//}

			//if ((getVersionResponse.Product == TrueNASProduct.Scale) && (version >= new Version("24.10")))
			//{
			//	{
			//		var uri = GetApiUri(request);
			//		uri.SetPathAndQueryString("api/v2.0/chart/release");

			//		var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SerializableDTOs.GetChartReleaseResponse[]>(uri.Uri, GetHeaders(request), true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);
			//	}

			//}





















			if (activeCertificateId.HasValue && request.RemovePriorCertificate)
			{
				//var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();

				var uri = GetApiUri(request);
				uri.SetPathAndQueryString($"api/v2.0/certificate/id/{activeCertificateId}");

				var apiRequest = new SerializableDTOs.DeleteCertificateRequest();

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonDelete<SerializableDTOs.DeleteCertificateRequest, ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, GetHeaders(request), apiRequest, true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);
			}

			{
				var uri = GetApiUri(request);
				uri.SetPathAndQueryString("api/v2.0/system/general/ui_restart");

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteTextGet(uri.Uri, GetHeaders(request), true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);

				addLog($"Delete Cert: {apiResponse}");
			}

			response.LogEntries = logEntries.ToArray();

			return response;
		}
	}
}