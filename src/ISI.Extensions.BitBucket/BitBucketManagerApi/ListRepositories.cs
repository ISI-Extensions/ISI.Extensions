#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
		public DTOs.ListRepositoriesResponse ListRepositories(DTOs.ListRepositoriesRequest request)
		{
			var response = new DTOs.ListRepositoriesResponse();
			
			var repositories = new List<Repository>();

			var uri = GetApiUri(request);
			uri.AddDirectoryToPath("repositories");
			if (!string.IsNullOrWhiteSpace(request.Workspace))
			{
				uri.AddDirectoryToPath(request.Workspace);
			}

			uri.UserName = "ron.muth@isi-net.com";

			while (uri != null)
			{
#if DEBUG
				var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SerializableDTOs.ListRepositoriesResponse>(uri.Uri, GetHeaders(request), true);

				repositories.AddRange(apiResponse.Repositories.ToNullCheckedArray(repository => new Repository()
				{
					Namespace = request.Workspace,
					Name = repository.Name,
					Description = repository.Description,
					//Contact = repository.Contact,
					CreationDate = repository.CreationDate,
					Type = repository.Type,
					LastModified = repository.LastModified,
				}));

				uri = string.IsNullOrWhiteSpace(apiResponse?.Next) ? null : new UriBuilder(apiResponse.Next);
			}

			response.Repositories = repositories.ToArray();

			return response;
		}
	}
}