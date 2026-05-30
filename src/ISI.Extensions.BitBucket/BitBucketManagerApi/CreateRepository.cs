#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.BitBucket.DataTransferObjects.BitBucketManagerApi;
using SerializableDTOs = ISI.Extensions.BitBucket.SerializableModels;

namespace ISI.Extensions.BitBucket
{
	public partial class BitBucketManagerApi
	{
		public DTOs.CreateRepositoryResponse CreateRepository(DTOs.CreateRepositoryRequest request)
		{
			var response = new DTOs.CreateRepositoryResponse();

			var uri = GetApiUri(request);
			uri.AddDirectoryToPath("repositories");
			uri.AddDirectoryToPath(GetWorkspace(request));
			uri.AddDirectoryToPath(request.Name);

			var apiRequest = new SerializableDTOs.CreateRepositoryRequest()
			{
				Scm = request.Scm,
				IsPrivate = request.IsPrivate,
				Project = new()
				{
					Key = request.ProjectKey,
				},
			};

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.CreateRepositoryRequest, SerializableDTOs.CreateRepositoryResponse>(uri.Uri, GetHeaders(request), apiRequest, true);

			var url = apiResponse?.Links?.Clone?.NullCheckedFirstOrDefault(url => string.Equals(url.Name, Uri.UriSchemeHttps, StringComparison.InvariantCultureIgnoreCase))?.Href;

			if (!string.IsNullOrWhiteSpace(url))
			{
				uri = new UriBuilder(url)
				{
					UserName = null,
					Password = null
				};

				response.SourceUrl = uri.Uri.ToString();
			}

			return response;
		}
}
}