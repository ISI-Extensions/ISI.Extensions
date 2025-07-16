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
		//Access: Mutual TLS Certificates Write SSL and Certificates Write
		public DTOs.SetCustomSslConfigurationResponse SetCustomSslConfiguration(DTOs.SetCustomSslConfigurationRequest request)
		{
			var response = new DTOs.SetCustomSslConfigurationResponse();
			
			var logEntries = new List<DTOs.LogEntry>();
			void addLog(string description)
			{
				logEntries.Add(new DTOs.LogEntry()
				{
					DateTimeStampUtc = DateTimeStamper.CurrentDateTimeUtc(),
					Description = description,
				});
			}
			var logger = new AddToLogLogger((level, description) => addLog(description), Logger);

			EnsureZoneId(request);

			var restRequest = new SerializableDTOs.SetCustomSslConfigurationRequest()
			{
				Certificate = request.BundleCertificate,
				PrivateKey = request.KeyCertificate,
			};

			logger.LogInformation("ListCustomSslCertificates");
			var listCustomSslCertificatesResponse = ListCustomSslCertificates(new()
			{
				Url = request.Url,
				ApiToken = request.ApiToken,
				ZoneId = request.ZoneId,
				ZoneName = request.ZoneName,
			});

			var sslCertificate = listCustomSslCertificatesResponse.SslCertificates.NullCheckedFirstOrDefault(sslCertificate => sslCertificate.Hosts.NullCheckedAny(host => string.Equals(host, request.Domain, StringComparison.InvariantCultureIgnoreCase)));

			if (sslCertificate == null)
			{
				logger.LogInformation("Add CustomSslCertificates");

				var uri = GetUrl(request);
				uri.AddDirectoryToPath("zones/{zoneId}/custom_certificates".Replace("{zoneId}", request.ZoneId));

				try
				{
					var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.SetCustomSslConfigurationRequest, SerializableDTOs.SetCustomSslConfigurationResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), restRequest, false);

					if (restResponse.Error != null)
					{
						throw restResponse.Error.Exception;
					}

					response.SslCertificate = (restResponse?.Response?.SslCertificate)?.Export();

					logger.LogInformation("Added CustomSslCertificates");
				}
				catch (Exception exception)
				{
					logger.LogError(exception, "SetCustomSslConfiguration (Post) Failed\n{0}", exception.ErrorMessageFormatted());
					response.Errored = true;
				}
			}
			else
			{
				logger.LogInformation("Update CustomSslCertificates");
				
				var uri = GetUrl(request);
				uri.AddDirectoryToPath("zones/{zoneId}/custom_certificates/{sslCertificateId}".Replace("{zoneId}", request.ZoneId).Replace("{sslCertificateId}", sslCertificate.SslCertificateId));

				try
				{
					var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPatch<SerializableDTOs.SetCustomSslConfigurationRequest, SerializableDTOs.SetCustomSslConfigurationResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), restRequest, false);

					if (restResponse.Error != null)
					{
						throw restResponse.Error.Exception;
					}

					response.SslCertificate = (restResponse?.Response?.SslCertificate)?.Export();

					logger.LogInformation("Updated CustomSslCertificates");
				}
				catch (Exception exception)
				{
					logger.LogError(exception, "SetCustomSslConfiguration (Patch) Failed\n{0}", exception.ErrorMessageFormatted());
					response.Errored = true;
				}
			}
						
			response.LogEntries = logEntries.ToArray();

			return response;
		}
	}
}