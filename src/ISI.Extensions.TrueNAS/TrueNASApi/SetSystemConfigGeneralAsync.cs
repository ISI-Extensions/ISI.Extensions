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
using SerializableDTOs = ISI.Extensions.TrueNAS.SerializableModels;

namespace ISI.Extensions.TrueNAS
{
	public partial class TrueNASApi
	{
		public async Task<DTOs.SetSystemConfigGeneralResponse> SetSystemConfigGeneralAsync(DTOs.SetSystemConfigGeneralRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.SetSystemConfigGeneralResponse();

			using (var trueNASWebSocketApiWrapper = new TrueNASWebSocketApiWrapper())
			{
				var trueNASApiUrl = GetTrueNASApiUrl(request);
				var trueNASUserName = GetTrueNASUserName(request);
				var trueNASApiKey = GetTrueNASApiKey(request);

				await trueNASWebSocketApiWrapper.ExecuteAsync(trueNASApiUrl, trueNASUserName, trueNASApiKey, async trueNasWebSocketApi =>
				{
					var setSystemConfigGeneralResponse = await trueNasWebSocketApi.SetSystemConfigGeneralAsync(new()
					{
						CertificateId = request.CertificateId,
						UiHttpsPort = request.UiHttpsPort,
						UiHttpsRedirect = request.UiHttpsRedirect,
						UiHttpsProtocols = request.UiHttpsProtocols.ToNullCheckedArray(),
						UiPort = request.UiPort,
						UiAddress = request.UiAddress.ToNullCheckedArray(),
						UiV6Address = request.UiV6Address.ToNullCheckedArray(),
						UiAllowList = request.UiAllowList.ToNullCheckedArray(),
						UiConsoleMessage = request.UiConsoleMessage,
						UiXFrameOptions = request.UiXFrameOptions,
						Kbdmap = request.Kbdmap,
						Timezone = request.Timezone,
						UsageCollection = request.UsageCollection,
						DsAuth = request.DsAuth,
						UiRestartDelayInSeconds = request.UiRestartDelayInSeconds,
						RollBackTimeoutInSeconds = request.RollBackTimeoutInSeconds,
					});

					response.Id = setSystemConfigGeneralResponse.Id;
					response.Certificate = setSystemConfigGeneralResponse.Certificate;
					response.UiHttpsPort = setSystemConfigGeneralResponse.UiHttpsPort;
					response.UiHttpsRedirect = setSystemConfigGeneralResponse.UiHttpsRedirect;
					response.UiHttpsProtocols = setSystemConfigGeneralResponse.UiHttpsProtocols.ToNullCheckedArray();
					response.UiPort = setSystemConfigGeneralResponse.UiPort;
					response.UiAddress = setSystemConfigGeneralResponse.UiAddress.ToNullCheckedArray();
					response.UiV6Address = setSystemConfigGeneralResponse.UiV6Address.ToNullCheckedArray();
					response.UiAllowList = setSystemConfigGeneralResponse.UiAllowList.ToNullCheckedArray();
					response.UiConsoleMessage = setSystemConfigGeneralResponse.UiConsoleMessage;
					response.UiXFrameOptions = setSystemConfigGeneralResponse.UiXFrameOptions;
					response.Kbdmap = setSystemConfigGeneralResponse.Kbdmap;
					response.Timezone = setSystemConfigGeneralResponse.Timezone;
					response.UsageCollection = setSystemConfigGeneralResponse.UsageCollection;
					response.WizardShown = setSystemConfigGeneralResponse.WizardShown;
					response.UsageCollectionIsSet = setSystemConfigGeneralResponse.UsageCollectionIsSet;
					response.DsAuth = setSystemConfigGeneralResponse.DsAuth;
				}, cancellationToken);
			}
			return response;
		}
	}
}