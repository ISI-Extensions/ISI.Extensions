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
using DTOs = ISI.Extensions.ScmManager.DataTransferObjects.ScmManagerApi;
using SerializableDTOs = ISI.Extensions.ScmManager.SerializableModels;

namespace ISI.Extensions.ScmManager
{
	public partial class ScmManagerApi
	{
		public DTOs.ExportRepositoryResponse ExportRepository(DTOs.ExportRepositoryRequest request)
		{
			var response = new DTOs.ExportRepositoryResponse();

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			//Start Export
			{
				var uri = GetApiUri(request);
				uri.SetPathAndQueryString($"api/v2/repositories/{request.Namespace}/{request.Name}/export/{request.Type}");
				if (request.Compress)
				{
					uri.AddQueryStringParameter("compress", request.Compress.TrueFalse(false, BooleanExtensions.TextCase.Lower));
				}

				var apiRequest = new SerializableDTOs.ExportRepositoryRequest()
				{
					Async = true,
				};

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.ExportRepositoryRequest, ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, GetHeaders(request, "*/*", "application/vnd.scmm-repositoryExport+json;v=2"), apiRequest, true, cookieContainer: GetCookieContainer(request));
			}

			//Wait for finish
			var downloadUrl = (string)null;
			while (string.IsNullOrWhiteSpace(downloadUrl))
			{
				System.Threading.Thread.Sleep(TimeSpan.FromMinutes(1));

				var uri = GetApiUri(request);
				uri.SetPathAndQueryString($"api/v2/repositories/{request.Namespace}/{request.Name}/export/info");

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SerializableDTOs.ExportRepositoryInfoResponse>(uri.Uri, GetHeaders(request, "*/*"), true, cookieContainer: GetCookieContainer(request));

				if (string.Equals(apiResponse.Status, "FINISHED", StringComparison.InvariantCultureIgnoreCase))
				{
					downloadUrl = apiResponse.Links.Download.Href;
				}
			}

			ISI.Extensions.WebClient.Download.DownloadFile(downloadUrl, GetHeaders(request, "*/*"), request.DownloadStream, cookieContainer: GetCookieContainer(request));

			return response;
		}
	}
}