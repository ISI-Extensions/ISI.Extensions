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
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Sbom.DataTransferObjects.DependencyTrackApi;
using SerializableDTOs = ISI.Extensions.Sbom.SerializableModels.DependencyTrackApi;

namespace ISI.Extensions.Sbom
{
	public partial class DependencyTrackApi
	{
		public DTOs.CreateProjectResponse CreateProject(DTOs.CreateProjectRequest request)
		{
			var response = new DTOs.CreateProjectResponse();

			var uri = GetApiUri(request);
			uri.SetPathAndQueryString("api/v1/project");

			var restRequest = new SerializableDTOs.CreateProjectRequest()
			{
				ProjectName = request.ProjectName,
				Classifier = GetClassifier(request.Classifier),
				//TeamUuid = request.TeamUuid,
				CollectionLogic = GetCollectionLogic(request.ProjectCollectionLogic),
				ParentProjectUuid = request.ParentProjectUuid,
				Description = request.Description,
				Tags = request.Tags.ToNullCheckedArray(tag => new SerializableDTOs.CreateProjectRequestTag()
				{
					Name = tag,
				}),
			};

			try
			{
				var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPut<SerializableDTOs.CreateProjectRequest, SerializableDTOs.CreateProjectResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), restRequest, false);

				if (restResponse.Error != null)
				{
					throw restResponse.Error.Exception;
				}

				response.Project = restResponse?.Response?.NullCheckedConvert(project => new DependencyTrackProject()
				{
					ProjectUuid = project.ProjectUuid,
					ProjectName = project.ProjectName,
					Classifier = ISI.Extensions.Enum<ComponentType?>.Parse(project.Classifier),
					//TeamUuid = project.TeamUuid,
					ProjectCollectionLogic = ISI.Extensions.Enum<CollectionLogic>.Parse(project.CollectionLogic),
					ParentProjectUuid = project.ParentProjectUuid,
					Description = project.Description,
					Tags = project.Tags.ToNullCheckedArray(tag => tag.Name),
				});
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "CreateProject Failed\n{0}", exception.ErrorMessageFormatted());
			}

			return response;
		}
	}
}