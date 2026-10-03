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
using SerializableDTOs = ISI.Extensions.TrueNAS.SerializableModels;

namespace ISI.Extensions.TrueNAS
{
	public partial class TrueNASApi
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

			using (var trueNASWebSocketApiWrapper = new TrueNASWebSocketApiWrapper())
			{
				var trueNASApiUrl = GetTrueNASApiUrl(request);
				var trueNASUserName = GetTrueNASUserName(request);
				var trueNASApiKey = GetTrueNASApiKey(request);

				await trueNASWebSocketApiWrapper.ExecuteAsync(trueNASApiUrl, trueNASUserName, trueNASApiKey, async trueNasWebSocketApi =>
				{
					try
					{
						var getSystemConfigGeneralResponse = await trueNasWebSocketApi.GetSystemConfigGeneralAsync();

						var activeCertificateId = getSystemConfigGeneralResponse?.Certificate?.CertificateId;
						var activeCertificateName = getSystemConfigGeneralResponse?.Certificate?.CertificateName;
						addLog($"Active Certificate: {activeCertificateName} ({activeCertificateId})");
						
						var createCertificateResponse = await trueNasWebSocketApi.CreateCertificateAsync(new()
						{
							CreateType = "CERTIFICATE_CREATE_IMPORTED",
							CertificateName = request.CertificateName,
							BundleCertificate = request.BundleCertificate,
							KeyCertificate = request.KeyCertificate,
						});

						addLog("Pushed Certificate");

						await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

						var listCertificateChoicesResponse = await trueNasWebSocketApi.ListCertificateChoicesAsync();

						var certificates = listCertificateChoicesResponse.ToNullCheckedDictionary(certificate => certificate.Value, certificate => certificate.Key.ToInt(), StringComparer.InvariantCultureIgnoreCase, NullCheckDictionaryResult.Empty);

						if (certificates.TryGetValue(request.CertificateName, out var certificateId))
						{
							var setSystemConfigGeneralResponse = await trueNasWebSocketApi.SetSystemConfigGeneralAsync(new()
							{
								CertificateId = certificateId,
							});

							if (setSystemConfigGeneralResponse.Certificate?.CertificateId == certificateId)
							{
								addLog($"New Certificate Set: {request.CertificateName} ({certificateId})");

								var restartUiResponse = await trueNasWebSocketApi.RestartUiAsync(request.UiRestartDelay);

								addLog("Restarted UI");

								await Task.Delay(TimeSpan.FromSeconds(request.UiRestartDelay + 2), cancellationToken);

								if (request.RemovePriorCertificate && activeCertificateId.HasValue)
								{
									var deleteCertificateResponse = await trueNasWebSocketApi.DeleteCertificateAsync(activeCertificateId.Value, false);

									addLog($"Deleted Old Certificate: {activeCertificateName} ({activeCertificateId})");
								}
							}
							else
							{
								addLog("New Certificate NOT Set");
								response.Errored = true;
							}
						}
						else
						{
							addLog("New Certificate Not found");
							response.Errored = true;
						}
					}
					catch (Exception exception)
					{
						addLog($"Exception: {exception.ErrorMessageFormatted()}");
						response.Errored = true;
					}

				}, cancellationToken);
			}

			response.LogEntries = logEntries.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			return response;
		}
	}
}