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

			var downloadFileResponse = ISI.Extensions.WebClient.Rest.ExecuteGet<ISI.Extensions.WebClient.Rest.StreamResponse>(FormatUrl(request.DirectoryUrl), new ISI.Extensions.WebClient.HeaderCollection(), true);

			downloadFileResponse.Stream.Rewind();

			var content = downloadFileResponse.Stream.TextReadToEnd();

			response.FileNames = ListFiles(string.Empty, request.DirectoryUrl, content, request.Recursive).ToArray();

			return response;
		}

		private IEnumerable<IGoDrivePath> ListFiles(string directory, string directoryUrl, string content, bool recursive)
		{
			var subPageUri = new UriBuilder(FormatUrl(directoryUrl));

			{
				var subPageIndex = content.IndexOf("id=\"fileList\"", StringComparison.InvariantCultureIgnoreCase);
				if (subPageIndex >= 0)
				{
					var formElement = content.Substring(subPageIndex);
					subPageIndex = formElement.IndexOf(">", StringComparison.InvariantCultureIgnoreCase);
					if (subPageIndex > 0)
					{
						formElement = formElement.Substring(0, subPageIndex);

						subPageIndex = formElement.IndexOf("action=\"", StringComparison.InvariantCultureIgnoreCase);

						var subPageParts = formElement.Substring(subPageIndex).Split(new[] { '\"' }, StringSplitOptions.RemoveEmptyEntries);

						subPageUri.Path = subPageParts[1];
						subPageUri.Query = string.Empty;
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(directory))
			{
				directory = string.Format("{0}/", directory.TrimEnd('/'));
			}

			var fileNames = new List<IGoDrivePath>();

			const string fileKey = "PrimeFaces.addSubmitParam('fileList'";
			const string directoryKey = "PrimeFaces.ab({s:&quot;fileTable";

			while (content.Length > 0)
			{
				var fileIndex = content.IndexOf(fileKey, StringComparison.InvariantCultureIgnoreCase);
				var directoryIndex = content.IndexOf(directoryKey, StringComparison.InvariantCultureIgnoreCase);

				if ((fileIndex < 0) && (directoryIndex < 0))
				{
					content = string.Empty;
				}
				else
				{
					if (fileIndex < 0)
					{
						fileIndex = int.MaxValue;
					}
					if (directoryIndex < 0)
					{
						directoryIndex = int.MaxValue;
					}

					if (fileIndex < directoryIndex)
					{
						content = content.Substring(fileIndex + fileKey.Length);

						fileIndex = content.IndexOf("</a>", StringComparison.InvariantCultureIgnoreCase);
						if (fileIndex >= 0)
						{
							var fileParts = content.Substring(0, fileIndex).Split(new[] { '>', '<', '\'', '{', '}', ',' }, StringSplitOptions.RemoveEmptyEntries);
							content = content.Substring(fileIndex);

							fileNames.Add(new GoDriveFile()
							{
								DirectoryUrl = directoryUrl,
								FileKey = fileParts.First(),
								FileName = string.Format("{0}{1}", directory, fileParts.Last()),
							});
						}
						else
						{
							content = string.Empty;
						}
					}
					else
					{
						content = content.Substring(directoryIndex + directoryKey.Length);

						directoryIndex = content.IndexOf("</a>", StringComparison.InvariantCultureIgnoreCase);
						if (directoryIndex >= 0)
						{
							var fileParts = string.Format("fileTable{0}", content.Substring(0, directoryIndex)).Split(new[] { '>', '<', '\'', '{', '}', ',', '&' }, StringSplitOptions.RemoveEmptyEntries);
							content = content.Substring(directoryIndex);

							fileNames.Add(new GoDriveDirectory()
							{
								DirectoryUrl = directoryUrl,
								FileKey = fileParts.First(),
								FileName = string.Format("{0}{1}", directory, fileParts.Last()),
							});
						}
						else
						{
							content = string.Empty;
						}

					}
				}
			}

			if (recursive)
			{
				var directories = fileNames.Where(fileName => fileName is GoDriveDirectory).Cast<GoDriveDirectory>().ToArray();

				//foreach (var subDirectory in directories)
				if(directories.Any())
				{
					var subDirectory = directories.First();

					var context = CreateContext(directoryUrl);

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
					formData.Add("javax.faces.ViewState", context.ViewState);
					formData.Add(subDirectory.FileKey, subDirectory.FileKey);

					var downloadResponse = ISI.Extensions.WebClient.Rest.ExecutePost<ISI.Extensions.WebClient.Rest.FormDataCollection, ISI.Extensions.WebClient.Rest.StreamResponse>(subPageUri.Uri, headers, formData, true, cookieContainer: context.CookieContainer);

					downloadResponse.Stream.Rewind();

					content = downloadResponse.Stream.TextReadToEnd();

					fileNames.AddRange(ListFiles(subDirectory.FileName, directoryUrl, content, recursive));
				}
			}

			return fileNames;
		}
	}
}