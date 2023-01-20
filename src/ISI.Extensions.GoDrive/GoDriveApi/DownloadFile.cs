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
using DTOs = ISI.Extensions.GoDrive.DataTransferObjects.GoDriveApi;

namespace ISI.Extensions.GoDrive
{
	public partial class GoDriveApi
	{
		public DTOs.DownloadFileResponse DownloadFile(DTOs.DownloadFileRequest request)
		{
			var response = new DTOs.DownloadFileResponse();

			var cookieContainer = new System.Net.CookieContainer();

			var downloadFileResponse = ISI.Extensions.WebClient.Rest.ExecuteGet<ISI.Extensions.WebClient.Rest.StreamResponse>(FormatUrl(request.FileName.DirectoryUrl), new(), true, cookieContainer: cookieContainer);

			downloadFileResponse.Stream.Rewind();

			var viewState = GetViewState(downloadFileResponse.Stream.TextReadToEnd());

			var origin = (new UriBuilder(FormatUrl(request.FileName.DirectoryUrl))
			{
				Path = string.Empty,
			}).Uri.ToString();

			var headers = new ISI.Extensions.WebClient.HeaderCollection();
			headers.Add("Origin", origin);
			headers.Add("Referer", FormatUrl(request.FileName.DirectoryUrl));
			headers.Add("Sec-Fetch-Dest", "document");
			headers.Add("Sec-Fetch-Mode", "navigate");
			headers.Add("Sec-Fetch-Site", "same-origin");
			headers.Add("Sec-Fetch-User", "?1");
			
			var formData = new ISI.Extensions.WebClient.Rest.FormDataCollection();
			formData.Add("fileTable_selection", string.Empty);
			formData.Add("fileList_SUBMIT", "1");
			formData.Add("javax.faces.ViewState", viewState);
			formData.Add(request.FileName.FileKey, request.FileName.FileKey);

			var downloadResponse = ISI.Extensions.WebClient.Rest.ExecutePost<ISI.Extensions.WebClient.Rest.FormDataCollection, ISI.Extensions.WebClient.Rest.StreamResponse>(FormatUrl(request.FileName.DirectoryUrl), headers, formData, true, cookieContainer: cookieContainer);

			response.Stream = downloadResponse.Stream;

			return response;
		}
	}
}