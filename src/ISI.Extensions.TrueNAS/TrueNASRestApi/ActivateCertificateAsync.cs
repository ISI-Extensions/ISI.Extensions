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
		public async Task<DTOs.ActivateCertificateResponse> ActivateCertificateAsync(DTOs.ActivateCertificateRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.ActivateCertificateResponse();

			var logEntries = new List<DTOs.LogEntry>();

			void addLog(string description)
			{
				logEntries.Add(new DTOs.LogEntry()
				{
					DateTimeStampUtc = DateTimeStamper.CurrentDateTimeUtc(),
					Description = description,
				});
			}

			try
			{
				var getSystemConfigGeneralResponse = await GetSystemConfigGeneralAsync(new()
				{
					TrueNASApiUrl = request.TrueNASApiUrl,
					TrueNASUserName = request.TrueNASUserName,
					TrueNASApiKey = request.TrueNASApiKey,
				}, cancellationToken);

				var activeCertificateId = getSystemConfigGeneralResponse?.Certificate?.CertificateId;
				var activeCertificateName = getSystemConfigGeneralResponse?.Certificate?.CertificateName;
				addLog($"Active Certificate: {activeCertificateName} ({activeCertificateId})");

				var createCertificateResponse = await CreateCertificateAsync(new()
				{
					TrueNASApiUrl = request.TrueNASApiUrl,
					TrueNASUserName = request.TrueNASUserName,
					TrueNASApiKey = request.TrueNASApiKey,
					CertificateName = request.CertificateName,
					KeyCertificate = request.KeyCertificate,
					BundleCertificate = request.BundleCertificate,
				}, cancellationToken);

				addLog("Pushed Certificate");

				await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

				var listCertificateChoicesResponse = await ListCertificateChoicesAsync(new()
				{
					TrueNASApiUrl = request.TrueNASApiUrl,
					TrueNASUserName = request.TrueNASUserName,
					TrueNASApiKey = request.TrueNASApiKey,
				}, cancellationToken);

				var certificates = listCertificateChoicesResponse.Certificates.ToNullCheckedDictionary(certificate => certificate.CertificateName, certificate => certificate.CertificateKey.ToInt(), StringComparer.InvariantCultureIgnoreCase, NullCheckDictionaryResult.Empty);

				if (certificates.TryGetValue(request.CertificateName, out var certificateId))
				{
					//Set new UI Certificate
					{
						var uri = GetApiUri(request);
						uri.SetPathAndQueryString("api/v2.0/system/general");

						var apiRequest = new SerializableDTOs.ActivateCertificateRequest()
						{
							CertificateIndex = certificateId,
						};

						var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPut<SerializableDTOs.ActivateCertificateRequest, ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, GetHeaders(request), apiRequest, true, serverCertificateValidationCallback: (sender, certificate, chain, errors) => true);

						addLog($"Activate Cert: {apiResponse}");
					}

					if (request.RemovePriorCertificate && activeCertificateId.HasValue)
					{
						await DeleteCertificateAsync(new()
						{
							TrueNASApiUrl = request.TrueNASApiUrl,
							TrueNASUserName = request.TrueNASUserName,
							TrueNASApiKey = request.TrueNASApiKey,
							CertificateId = activeCertificateId.Value,
						}, cancellationToken);

						addLog($"Deleted Old Certificate: {activeCertificateName} ({activeCertificateId})");
					}
				}

				await RestartUiAsync(new()
				{
					TrueNASApiUrl = request.TrueNASApiUrl,
					TrueNASUserName = request.TrueNASUserName,
					TrueNASApiKey = request.TrueNASApiKey,
					Delay = request.UiRestartDelay,
				}, cancellationToken);

				addLog("Restarted UI");

				await Task.Delay(TimeSpan.FromSeconds(request.UiRestartDelay + 2), cancellationToken);
			}
			catch (Exception exception)
			{
				addLog($"Exception: {exception.ErrorMessageFormatted()}");
				response.Errored = true;
			}

			response.LogEntries = logEntries.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			return response;
		}
	}
}