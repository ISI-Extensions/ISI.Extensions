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
using DTOs = ISI.Extensions.GitHub.DataTransferObjects.GitHubManagerApi;
using SerializableDTOs = ISI.Extensions.GitHub.SerializableModels;

namespace ISI.Extensions.GitHub
{
	public partial class GitHubManagerApi
	{
		public DTOs.ListRepositoriesResponse ListRepositories(DTOs.ListRepositoriesRequest request)
		{
			var response = new DTOs.ListRepositoriesResponse();

			var repositories = new List<Repository>();

				var uri = GetApiUri(request);
				uri.SetPathAndQueryString("/orgs/{org}/repos".Replace("{org}", request.Organization));

				while (uri != null)
				{
#if DEBUG
				var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<ISI.Extensions.WebClient.Rest.SerializedResponse<SerializableDTOs.ListRepositoriesResponseRepository[]>>(uri.Uri, GetHeaders(request, "application/vnd.github+json", apiVersion: "2022-11-28"), true);

				repositories.AddRange(apiResponse.Response.ToNullCheckedArray(repository => new Repository()
				{
					Namespace = request.Organization,
					Name = repository.Name,
					Description = repository.Description,
					Contact = repository.Owner?.Login,
					CreationDate = repository.CreatedAt,
					Type = "git",
					//Archived = repository.Archived,
					//Exporting = repository.Exporting,
					LastModified = repository.UpdatedAt,
				}));

				uri = null;
				if (apiResponse.ResponseHeaders.TryGetValue("link", out var linkHeaderValue))
				{
					var links = linkHeaderValue.Split(',');

					var nextLink = links.NullCheckedFirstOrDefault(link => link.IndexOf("rel=\"next\"", StringComparison.InvariantCultureIgnoreCase) >= 0);

					if (!string.IsNullOrWhiteSpace(nextLink))
					{
						uri = new UriBuilder(nextLink.Split(';').First().Trim(' ', '<', '>'));
					}
				}
			}

			response.Repositories = repositories.ToArray();

			return response;
		}
	}
}