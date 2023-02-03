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
		public DTOs.SetBuildArtifactEnvironmentDateTimeStampVersionResponse SetBuildArtifactEnvironmentDateTimeStampVersion(DTOs.SetBuildArtifactEnvironmentDateTimeStampVersionRequest request)
		{
			Logger.LogInformation(string.Format("SetArtifactEnvironmentDateTimeStampVersion, BuildArtifactsApiUrl: {0}", request.BuildArtifactsApiUrl));

			var endPointVersion = GetEndpointVersion(request.BuildArtifactsApiUrl);

			if (endPointVersion >= 4)
			{
				return SetBuildArtifactEnvironmentDateTimeStampVersionV4(request);
			}

			return SetBuildArtifactEnvironmentDateTimeStampVersionV1(request);
		}

		private DTOs.SetBuildArtifactEnvironmentDateTimeStampVersionResponse SetBuildArtifactEnvironmentDateTimeStampVersionV1(DTOs.SetBuildArtifactEnvironmentDateTimeStampVersionRequest request)
		{
			var response = new DTOs.SetBuildArtifactEnvironmentDateTimeStampVersionResponse();

			var buildArtifactsApiUri = new UriBuilder(request.BuildArtifactsApiUrl);
			buildArtifactsApiUri.AddDirectoryToPath("build-artifacts/set-artifact-environment-dateTimeStamp-version");

			buildArtifactsApiUri.AddQueryStringParameter("artifactName", request.BuildArtifactName);
			buildArtifactsApiUri.AddQueryStringParameter("environment", request.Environment);
			buildArtifactsApiUri.AddQueryStringParameter("dateTimeStampVersion", request.DateTimeStampVersion.Value);

			Logger.LogInformation(string.Format("SetArtifactEnvironmentDateTimeStampVersion, BuildArtifactsApiUrl: {0}", buildArtifactsApiUri.Uri));

			buildArtifactsApiUri.AddQueryStringParameter("authenticationToken", request.BuildArtifactsApiKey);

			//response.Status = ISI.Extensions.WebClient.Rest.ExecuteTextGet(sourceUri.Uri, new ISI.Extensions.WebClient.HeaderCollection(), true);

			var webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(buildArtifactsApiUri.Uri);
			webRequest.Method = System.Net.WebRequestMethods.Http.Get;

			using (var webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse())
			{
				//var httpWebResponse = webResponse as System.Net.HttpWebResponse;

				if ((webResponse != null) && (webResponse.StatusCode != System.Net.HttpStatusCode.OK))
				{
					var encoding = Encoding.GetEncoding(webResponse.CharacterSet);

					using (var responseStream = new System.IO.StreamReader(webResponse.GetResponseStream(), encoding))
					{
						response.Status = responseStream.ReadToEnd();

						throw new(string.Format("{0}: {1}\n{2}", webResponse.StatusCode, webResponse.StatusDescription, response.Status));
					}
				}
			}

			return response;
		}

		private DTOs.SetBuildArtifactEnvironmentDateTimeStampVersionResponse SetBuildArtifactEnvironmentDateTimeStampVersionV4(DTOs.SetBuildArtifactEnvironmentDateTimeStampVersionRequest request)
		{
			var response = new DTOs.SetBuildArtifactEnvironmentDateTimeStampVersionResponse();

			var uri = new UriBuilder(request.BuildArtifactsApiUrl);
			uri.SetPathAndQueryString("api/v4/set-artifact-environment-date-time-stamp-version");

			var restRequest = new SerializableDTOs.SetArtifactEnvironmentDateTimeStampVersionRequest()
			{
				ArtifactName = request.BuildArtifactName,
				Environment = request.Environment,
				DateTimeStampVersion = request.DateTimeStampVersion.Value,
			};

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.SetArtifactEnvironmentDateTimeStampVersionRequest, SerializableDTOs.SetArtifactEnvironmentDateTimeStampVersionResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request.BuildArtifactsApiKey), restRequest, false);

			if (restResponse?.Error?.Exception != null)
			{
				response.Status = restResponse.Error.Exception.ErrorMessageFormatted();

				Logger.LogError(restResponse.Error.Exception, "Error");
			}

			return response;
		}
	}
}