#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.BuildArtifactApi;

namespace ISI.Extensions.Scm
{
	public partial class BuildArtifactApi
	{
		public DTOs.DownloadArtifactResponse DownloadArtifact(DTOs.DownloadArtifactRequest request)
		{
			var response = new DTOs.DownloadArtifactResponse();

			var remoteManagementUri = new UriBuilder(request.RemoteManagementUrl);
			remoteManagementUri.AddDirectoryToPath("build-artifacts/download-artifact");
			remoteManagementUri.AddQueryStringParameter("artifactName", request.ArtifactName);
			remoteManagementUri.AddQueryStringParameter("dateTimeStamp", request.DateTimeStamp);

			Logger.LogInformation(string.Format("DownloadArtifact, RemoteManagementUrl: {0}", remoteManagementUri.Uri));

			remoteManagementUri.AddQueryStringParameter("authenticationToken", request.AuthenticationToken);

			using(var downloadFileResponse = ISI.Extensions.WebClient.Download.DownloadFile<ISI.Extensions.Stream.TempFileStream>(remoteManagementUri.Uri, new ISI.Extensions.WebClient.HeaderCollection(), 1427))// any larger will cause an SSL request to fail
			{
				using (var stream = System.IO.File.OpenWrite(request.TargetFileName))
				{
					downloadFileResponse.Stream.CopyTo(stream);
				}
			}



			//var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sourceUri.Uri);
			//request.Method = System.Net.WebRequestMethods.Http.Get;


			//using (var response = (System.Net.HttpWebResponse)request.GetResponse())
			//{
			//	using (var sourceStream = response.GetResponseStream())
			//	{
			//		using (var targetStream = new System.IO.FileStream(TargetFileName, System.IO.FileMode.OpenOrCreate))
			//		{
			//			int chunkSize = 1427; // any larger will cause an SSL request to fail
			//			byte[] buffer = new byte[chunkSize];

			//			int readBlocks = sourceStream.Read(buffer, 0, chunkSize);

			//			while (readBlocks > 0)
			//			{
			//				targetStream.Write(buffer, 0, readBlocks);

			//				if ((readBlocks > 0) && (readBlocks < chunkSize))
			//				{
			//					System.Threading.Thread.Sleep(100);
			//				}

			//				if (readBlocks > 0) readBlocks = sourceStream.Read(buffer, 0, chunkSize);
			//			}

			//			targetStream.Flush();
			//		}
			//	}
			//}


			return response;
		}
	}
}