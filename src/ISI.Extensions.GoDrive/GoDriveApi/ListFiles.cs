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

			var fileNames = new List<GoDriveFileName>();

			while (content.Length > 0)
			{
				const string key = "PrimeFaces.addSubmitParam('fileList'";
				var index = content.IndexOf(key);
				if (index >= 0)
				{
					content = content.Substring(index + key.Length);

					index = content.IndexOf("</a>");
					if (index >= 0)
					{
						var fileParts = content.Substring(0, index).Split(new[] {'>', '<', '\'', '{', '}', ','}, StringSplitOptions.RemoveEmptyEntries);
						content = content.Substring(index);

						fileNames.Add(new GoDriveFileName()
						{
							DirectoryUrl = request.DirectoryUrl,
							FileKey = fileParts.First(),
							FileName = fileParts.Last(),
						});
					}
					else
					{
						content = string.Empty;
					}
				}
				else
				{
					content = string.Empty;
				}
			}

			response.FileNames = fileNames.ToArray();
			
			return response;
		}
	}
}