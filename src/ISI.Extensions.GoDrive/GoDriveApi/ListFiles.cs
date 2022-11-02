#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
		public DTOs.ListFilesResponse ListFiles(DTOs.ListFilesRequest request)
		{
			var response = new DTOs.ListFilesResponse();

			var cookieContainer = new System.Net.CookieContainer();

			var downloadFileResponse = ISI.Extensions.WebClient.Rest.ExecuteGet<ISI.Extensions.WebClient.Rest.StreamResponse>(FormatUrl(request.DirectoryUrl), new(), true, cookieContainer: cookieContainer);

			downloadFileResponse.Stream.Rewind();

			var content = downloadFileResponse.Stream.TextReadToEnd();

			var viewState = GetViewState(content);

			response.FileNames = ListFiles(string.Empty, request.DirectoryUrl, cookieContainer, viewState, content, 0, request.Recursive).ToArray();

			return response;
		}

		private IEnumerable<IGoDrivePath> ListFiles(string directory, string directoryUrl, System.Net.CookieContainer cookieContainer, string viewState, string content, int skip, bool recursive)
		{
			const int pageSize = 250;

			var fileNames = new List<IGoDrivePath>();

			var subPageUri = GetSubPageUri(directoryUrl, content);

			if (!string.IsNullOrWhiteSpace(directory))
			{
				directory = string.Format("{0}/", directory.TrimEnd('/'));
			}

			var @continue = true;

			while (@continue)
			{
				@continue = false;

				var viewFileNames = ParseFilePaths(directoryUrl, directory, content);

				if (viewFileNames.NullCheckedAny())
				{
					@continue = true;

					fileNames.AddRange(viewFileNames);

					skip += pageSize;

					var origin = (new UriBuilder(FormatUrl(directoryUrl))
					{
						Path = string.Empty,
					}).Uri.ToString();

					var headers = new ISI.Extensions.WebClient.HeaderCollection();
					headers.Add("Origin", origin);
					headers.Add("Referer", FormatUrl(directoryUrl));
					headers.Add("Sec-Fetch-Dest", "empty");
					headers.Add("Sec-Fetch-Mode", "cors");
					headers.Add("Sec-Fetch-Site", "same-origin");

					var formData = new ISI.Extensions.WebClient.Rest.FormDataCollection();
					formData.Add("fileTable_pagination", "true");
					formData.Add("fileTable_first", skip);
					formData.Add("fileTable_rows", pageSize);
					formData.Add("fileTable_children", "true");
					formData.Add("fileTable_encodeFeature", "true");
					formData.Add("fileTable_rows", pageSize);
					formData.Add("fileTable_rppDD", string.Empty);
					formData.Add("fileList_SUBMIT", "1");
					formData.Add("javax.faces.ViewState", viewState);
					//formData.Add(subDirectory.FileKey, subDirectory.FileKey);

					var downloadResponse = ISI.Extensions.WebClient.Rest.ExecutePost<ISI.Extensions.WebClient.Rest.FormDataCollection, ISI.Extensions.WebClient.Rest.StreamResponse>(subPageUri, headers, formData, true, cookieContainer: cookieContainer);

					downloadResponse.Stream.Rewind();

					content = downloadResponse.Stream.TextReadToEnd();

					viewState = GetViewState(content);
				}
			}

			if (recursive)
			{
				var directories = fileNames.Where(fileName => fileName is GoDriveDirectory).Cast<GoDriveDirectory>().ToArray();

				foreach (var subDirectory in directories)
				{
					var origin = (new UriBuilder(FormatUrl(directoryUrl))
					{
						Path = string.Empty,
					}).Uri.ToString();

					var headers = new ISI.Extensions.WebClient.HeaderCollection();
					headers.Add("Origin", origin);
					headers.Add("Referer", FormatUrl(directoryUrl));
					headers.Add("Sec-Fetch-Dest", "empty");
					headers.Add("Sec-Fetch-Mode", "cors");
					headers.Add("Sec-Fetch-Site", "same-origin");

					var formData = new ISI.Extensions.WebClient.Rest.FormDataCollection();
					formData.Add("fileTable_selection", string.Empty);
					formData.Add("fileList_SUBMIT", "1");
					formData.Add("javax.faces.ViewState", viewState);
					formData.Add(subDirectory.FileKey, subDirectory.FileKey);

					var downloadResponse = ISI.Extensions.WebClient.Rest.ExecutePost<ISI.Extensions.WebClient.Rest.FormDataCollection, ISI.Extensions.WebClient.Rest.StreamResponse>(subPageUri, headers, formData, true, cookieContainer: cookieContainer);

					downloadResponse.Stream.Rewind();

					content = downloadResponse.Stream.TextReadToEnd();

					viewState = GetViewState(content);

					fileNames.AddRange(ListFiles(subDirectory.FileName, directoryUrl, cookieContainer, viewState, content, 0, recursive));
				}
			}

			return fileNames;
		}
	}
}