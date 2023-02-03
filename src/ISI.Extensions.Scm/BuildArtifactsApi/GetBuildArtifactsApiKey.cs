#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.BuildArtifactsApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.BuildArtifactsApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Scm
{
	public partial class BuildArtifactsApi
	{
		public DTOs.GetBuildArtifactsApiKeyResponse GetBuildArtifactsApiKey(DTOs.GetBuildArtifactsApiKeyRequest request)
		{
			Logger.LogInformation(string.Format("GetBuildArtifactsApiKey, BuildArtifactsApiUrl: {0}", request.BuildArtifactsApiUrl));

			var endPointVersion = GetEndpointVersion(request.BuildArtifactsApiUrl);

			if (endPointVersion >= 4)
			{
				return GetBuildArtifactsApiKeyV4(request);
			}

			return GetBuildArtifactsApiKeyV1(request);
		}

		private DTOs.GetBuildArtifactsApiKeyResponse GetBuildArtifactsApiKeyV1(DTOs.GetBuildArtifactsApiKeyRequest request)
		{
			var response = new DTOs.GetBuildArtifactsApiKeyResponse();

			var tryAttemptsLeft = request.MaxTries;
			while (tryAttemptsLeft > 0)
			{
				try
				{
					using (var remoteManagementClient = ISI.Extensions.Scm.ServiceReferences.Scm.RemoteManagementClient.GetClient(request.BuildArtifactsApiUrl))
					{
						response.BuildArtifactsApiKey = remoteManagementClient.GetAuthenticationTokenAsync(request.UserName, request.Password).GetAwaiter().GetResult();
					}

					tryAttemptsLeft = 0;
				}
				catch (Exception exception)
				{
					tryAttemptsLeft--;
					if (tryAttemptsLeft < 0)
					{
						Logger.LogError("Error getting authentication token");
						throw;
					}

					Logger.LogError(string.Format("Error getting authentication token, Sleeping for {0} seconds", request.ExceptionSleepForInSeconds));

					System.Threading.Thread.Sleep(TimeSpan.FromSeconds(request.ExceptionSleepForInSeconds));
				}
			}


			return response;
		}

		private DTOs.GetBuildArtifactsApiKeyResponse GetBuildArtifactsApiKeyV4(DTOs.GetBuildArtifactsApiKeyRequest request)
		{
			var response = new DTOs.GetBuildArtifactsApiKeyResponse();

			var uri = new UriBuilder(request.BuildArtifactsApiUrl);
			uri.SetPathAndQueryString("api/v4/authenticate-user-name-password");

			var restRequest = new SerializableDTOs.AuthenticateUserNamePasswordRequest()
			{
				UserName = request.UserName,
				Password = request.Password,
			};

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.AuthenticateUserNamePasswordRequest, SerializableDTOs.AuthenticateUserNamePasswordResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, new(), restRequest, false);

			response.BuildArtifactsApiKey = restResponse?.Response?.BuildArtifactsApiKey;

			if (restResponse?.Error?.Exception != null)
			{
				Logger.LogError(restResponse.Error.Exception, "Error");
			}

			return response;
		}
	}
}