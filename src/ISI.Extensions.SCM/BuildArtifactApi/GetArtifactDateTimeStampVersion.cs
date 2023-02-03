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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.BuildArtifactApi;
using SerializableDTOs = ISI.Extensions.Scm.SerializableModels.BuildArtifactApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Scm
{
	public partial class BuildArtifactApi
	{
		public DTOs.GetArtifactDateTimeStampVersionResponse GetArtifactDateTimeStampVersion(DTOs.GetArtifactDateTimeStampVersionRequest request)
		{
			Logger.LogInformation(string.Format("GetArtifactDateTimeStampVersion, BuildArtifactManagementUrl: {0}", request.BuildArtifactManagementUrl));

			var endPointVersion = GetEndpointVersion(request.BuildArtifactManagementUrl);

			if (endPointVersion >= 4)
			{
				return GetArtifactDateTimeStampVersionV4(request);
			}

			return GetArtifactDateTimeStampVersionV1(request);
		}

		private DTOs.GetArtifactDateTimeStampVersionResponse GetArtifactDateTimeStampVersionV1(DTOs.GetArtifactDateTimeStampVersionRequest request)
		{
			var response = new DTOs.GetArtifactDateTimeStampVersionResponse();
			
			using (var remoteManagementClient = ISI.Extensions.Scm.ServiceReferences.Scm.RemoteManagementClient.GetClient(request.BuildArtifactManagementUrl))
			{
				response.ArtifactDateTimeStampVersion = remoteManagementClient.GetArtifactDateTimeStampVersionAsync(request.AuthenticationToken, request.DateTimeStampVersion).GetAwaiter().GetResult();
			}

			return response;
		}

		private DTOs.GetArtifactDateTimeStampVersionResponse GetArtifactDateTimeStampVersionV4(DTOs.GetArtifactDateTimeStampVersionRequest request)
		{
			var response = new DTOs.GetArtifactDateTimeStampVersionResponse();
			
			var uri = new UriBuilder(request.BuildArtifactManagementUrl);
			uri.SetPathAndQueryString("api/v4/get-artifact-date-time-stamp");

			var restRequest = new SerializableDTOs.GetArtifactDateTimeStampVersionRequest()
			{
				DateTimeStampVersion = request.DateTimeStampVersion,
			};

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.GetArtifactDateTimeStampVersionRequest, SerializableDTOs.GetArtifactDateTimeStampVersionResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request.AuthenticationToken), restRequest, false);

			response.ArtifactDateTimeStampVersion = restResponse?.Response?.ArtifactDateTimeStampVersion;

			if (restResponse?.Error?.Exception != null)
			{
				Logger.LogError(restResponse.Error.Exception, "Error");
			}

			return response;
		}
	}
}