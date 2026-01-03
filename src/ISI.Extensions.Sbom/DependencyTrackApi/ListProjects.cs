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
using DTOs = ISI.Extensions.Sbom.DataTransferObjects.DependencyTrackApi;
using SerializableDTOs = ISI.Extensions.Sbom.SerializableModels.DependencyTrackApi;

namespace ISI.Extensions.Sbom
{
	public partial class DependencyTrackApi
	{
		public DTOs.ListProjectsResponse ListProjects(DTOs.ListProjectsRequest request)
		{
			var response = new DTOs.ListProjectsResponse();

			var projects = new List<DependencyTrackProject>();

			var page = 1;
			while (page > 0)
			{
				var uri = GetApiUri(request);
				uri.SetPathAndQueryString("api/v1/project");
				uri.AddQueryStringParameter("page", page);
#if DEBUG
				var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet(new ISI.Extensions.WebClient.Rest.RestResponseTypeCollection()
				{
					{ System.Net.HttpStatusCode.OK , typeof(ISI.Extensions.WebClient.Rest.JsonSerializedResponse<SerializableDTOs.ListProjectsResponse[]>)},
					{ System.Net.HttpStatusCode.Unauthorized , typeof(ISI.Extensions.WebClient.Rest.TextResponse)},
				}, uri.Uri, GetHeaders(request), true);

				switch (apiResponse.StatusCode)
				{
					case System.Net.HttpStatusCode.OK:
						if (apiResponse.Response is ISI.Extensions.WebClient.Rest.JsonSerializedResponse<SerializableDTOs.ListProjectsResponse[]> jsonSerializedResponse)
						{
							projects.AddRange(jsonSerializedResponse.Response.ToNullCheckedArray(project => new DependencyTrackProject()
							{
								ProjectUuid = project.ProjectUuid,
								ProjectName = project.ProjectName,
								Classifier = ISI.Extensions.Enum<ComponentType?>.Parse(project.Classifier),
								//TeamUuid = project.TeamUuid,
								ProjectCollectionLogic = ISI.Extensions.Enum<CollectionLogic>.Parse(project.CollectionLogic),
								ParentProjectUuid = project.ParentProjectUuid,
								Description = project.Description,
								Tags = project.Tags.ToNullCheckedArray(tag => tag.Name),
							}));

							if (jsonSerializedResponse.ResponseHeaders.TryGetValue("X-Total-Count", out var totalCount))
							{
								if (projects.Count < totalCount.ToInt())
								{
									page++;
								}
								else
								{
									page = -1;
								}
							}
							else
							{
								page = -1;
							}
						}
						else
						{
							page = -1;
						}

						break;

					case System.Net.HttpStatusCode.Unauthorized:
						response.Unauthorized = true;
						page = -1;
						break;
				}
			}

			response.Projects = projects.ToArray();

			return response;
		}
	}
}