#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Mandrill.DataTransferObjects.MandrillWebHooksApi;
using SerializableDTOs = ISI.Extensions.Mandrill.SerializableModels.MandrillWebHooksApi;

namespace ISI.Extensions.Mandrill
{
	public partial class MandrillWebHooksApi
	{
		public async Task<DTOs.UpdateWebHooksResponse> UpdateWebHooksAsync(DTOs.UpdateWebHooksRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.UpdateWebHooksResponse();

			if (MandrillProfilesApi.TryGetMandrillProfile(request.MandrillProfileUuid, out var mandrillProfile))
			{
				var uri = GetMessageApiUri(mandrillProfile);
				uri.AddDirectoryToPath("webHooks/info.json");

				var restRequest = new SerializableDTOs.UpdateWebHooksRequest()
				{
					ApiKey = mandrillProfile.ApiKey,
					WebHookKey = request.WebHookKey,
					WebHookUrl = request.WebHookUrl,
					Description = request.Description,
					Events = request.Events.ToNullCheckedArray(),
				};

				try
				{
					var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.UpdateWebHooksRequest, SerializableDTOs.UpdateWebHooksResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetWebClientHeaders(mandrillProfile), restRequest, false);

					if (restResponse?.Response != null)
					{
						response.Status = restResponse.Response.Status;
						response.Code = restResponse.Response.Code;
						response.Name = restResponse.Response.Name;
						response.Message = restResponse.Response.Message;
						response.WebHook = Convert(restResponse.Response, mandrillProfile.MandrillProfileUuid);
					}
				}
				catch (Exception exception)
				{
					Logger.LogError(exception, "UpdateWebHooks Failed\n{0}", exception.ErrorMessageFormatted());
				}
			}

			return response;
		}
	}
}