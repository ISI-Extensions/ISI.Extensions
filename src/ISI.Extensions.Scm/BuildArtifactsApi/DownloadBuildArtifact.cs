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
		public DTOs.DownloadBuildArtifactResponse DownloadBuildArtifact(DTOs.DownloadBuildArtifactRequest request)
		{
			Logger.LogInformation(string.Format("DownloadArtifact, BuildArtifactsApiUrl: {0}", request.BuildArtifactsApiUrl));

			var endPointVersion = GetEndpointVersion(request.BuildArtifactsApiUrl);

			if (endPointVersion >= 4)
			{
				return DownloadBuildArtifactV4(request);
			}

			return DownloadBuildArtifactV1(request);
		}

		private DTOs.DownloadBuildArtifactResponse DownloadBuildArtifactV1(DTOs.DownloadBuildArtifactRequest request)
		{
			var response = new DTOs.DownloadBuildArtifactResponse();

			var buildArtifactsApiUri = new UriBuilder(request.BuildArtifactsApiUrl);
			buildArtifactsApiUri.AddDirectoryToPath("build-artifacts/download-artifact");
			buildArtifactsApiUri.AddQueryStringParameter("artifactName", request.BuildArtifactName);
			buildArtifactsApiUri.AddQueryStringParameter("dateTimeStamp", request.DateTimeStamp.ToString());

			buildArtifactsApiUri.AddQueryStringParameter("authenticationToken", request.BuildArtifactsApiKey);

			using(var downloadFileResponse = ISI.Extensions.WebClient.Download.DownloadFile<ISI.Extensions.Stream.TempFileStream>(buildArtifactsApiUri.Uri, new(), 1427))// any larger will cause an SSL request to fail
			{
				using (var stream = System.IO.File.OpenWrite(request.TargetFileName))
				{
					downloadFileResponse.Stream.CopyTo(stream);
				}
			}

			return response;
		}

		private DTOs.DownloadBuildArtifactResponse DownloadBuildArtifactV4(DTOs.DownloadBuildArtifactRequest request)
		{
			var response = new DTOs.DownloadBuildArtifactResponse();

			var buildArtifactsApiUri = new UriBuilder(request.BuildArtifactsApiUrl);
			buildArtifactsApiUri.AddDirectoryToPath("api/v4/download-build-artifact");
			buildArtifactsApiUri.AddQueryStringParameter("buildArtifactName", request.BuildArtifactName);
			buildArtifactsApiUri.AddQueryStringParameter("dateTimeStamp", request.DateTimeStamp.ToString());

			using(var downloadFileResponse = ISI.Extensions.WebClient.Download.DownloadFile<ISI.Extensions.Stream.TempFileStream>(buildArtifactsApiUri.Uri, GetHeaders(request.BuildArtifactsApiKey), 1427))// any larger will cause an SSL request to fail
			{
				using (var stream = System.IO.File.OpenWrite(request.TargetFileName))
				{
					downloadFileResponse.Stream.CopyTo(stream);
				}
			}

			return response;
		}

	}
}