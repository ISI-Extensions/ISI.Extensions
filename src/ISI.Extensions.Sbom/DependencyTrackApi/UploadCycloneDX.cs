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
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Sbom.DataTransferObjects.DependencyTrackApi;
using SerializableDTOs = ISI.Extensions.Sbom.SerializableModels.DependencyTrackApi;

namespace ISI.Extensions.Sbom
{
	public partial class DependencyTrackApi
	{
		public DTOs.UploadCycloneDXResponse UploadCycloneDX(DTOs.IUploadCycloneDXRequest request)
		{
			var response = new DTOs.UploadCycloneDXResponse();

			var projectUuid = (request as DTOs.IUploadCycloneDXWithProjectUuidRequest)?.ProjectUuid;
			var projectName = (request as DTOs.IUploadCycloneDXWithProjectNameRequest)?.ProjectName;
			var projectTags = (request as DTOs.IUploadCycloneDXWithProjectNameRequest)?.ProjectTags;
			var autoCreate = (request as DTOs.IUploadCycloneDXWithProjectNameRequest)?.AutoCreate ?? false;
			var projectVersion = (request as DTOs.IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest)?.ProjectVersion;
			var isLatestProjectVersion = (request as DTOs.IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest)?.IsLatestProjectVersion ?? false;

			var parentProjectUuid = (request as DTOs.IUploadCycloneDXWithParentProjectUuidRequest)?.ParentProjectUuid;
			var parentProjectName = (request as DTOs.IUploadCycloneDXWithParentProjectNameRequest)?.ParentProjectName;
			var parentProjectVersion = (request as DTOs.IUploadCycloneDXWithProjectVersionAndParentProjectVersionRequest)?.ParentProjectVersion;

			var bom = (request as DTOs.IUploadCycloneDXWithCycloneDXRequest)?.CycloneDX;
			var cycloneDXStream = (request as DTOs.IUploadCycloneDXWithCycloneDXStreamRequest)?.CycloneDXStream;
			if (request is DTOs.IUploadCycloneDXWithCycloneDXFullNameRequest uploadCycloneDXWithCycloneDXFullNameRequest)
			{
				cycloneDXStream = System.IO.File.OpenRead(uploadCycloneDXWithCycloneDXFullNameRequest.CycloneDXFullName);
			}
			if (cycloneDXStream != null)
			{
				bom = cycloneDXStream.TextReadToEnd();
			}
			cycloneDXStream?.Dispose();
			cycloneDXStream = null;

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			try
			{
				var uri = GetApiUri(request);
				uri.SetPathAndQueryString("api/v1/bom");

				var restResponse = (ISI.Extensions.WebClient.Rest.IRestResponseWrapper)null;

				if (true || (bom.Length < 2048))
				{
					var restRequest = new SerializableDTOs.UploadCycloneDxRequest()
					{
						ProjectUuid = projectUuid,
						ProjectName = projectName,
						ProjectVersion = projectVersion.ToString(),
						ProjectTags = projectTags.ToNullCheckedArray(projectTag => new SerializableDTOs.UploadCycloneDxRequestTag()
						{
							Name = projectTag,
						}),
						AutoCreate = autoCreate,
						ParentProjectUuid = parentProjectUuid,
						ParentProjectName = parentProjectName,
						ParentProjectVersion = parentProjectVersion?.ToString(),
						IsLatestProjectVersion = isLatestProjectVersion,
						Bom = System.Text.Encoding.UTF8.GetBytes(bom),
					};

					restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPut(new ISI.Extensions.WebClient.Rest.RestResponseTypeCollection()
					{
						{ System.Net.HttpStatusCode.OK, typeof(SerializableDTOs.UploadCycloneDxResponse) },
						{ System.Net.HttpStatusCode.BadRequest, typeof(ISI.Extensions.WebClient.Rest.JsonSerializedResponse<SerializableDTOs.UploadCycloneDxResponseError[]>) },
						{ System.Net.HttpStatusCode.Unauthorized, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
						{ System.Net.HttpStatusCode.Forbidden, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
						{ System.Net.HttpStatusCode.NotFound, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
					}, uri.Uri, GetHeaders(request), restRequest, true);
				}
				else
				{
					var formValues = new System.Collections.Specialized.NameValueCollection()
					{
						{ "project", projectUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens) },
						{ "autoCreate", autoCreate.TrueFalse() },
						{ "projectName", projectName },
						{ "projectVersion", projectVersion.ToString() },
						{ "projectTags", string.Join(",", projectTags ?? []) },
						{ "parentName", parentProjectName },
						{ "parentVersion", parentProjectVersion?.ToString() },
						{ "parentUUID", parentProjectUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens) },
						{ "isLatest", isLatestProjectVersion.TrueFalse() },
						{ "bom", bom },
					};

					restResponse = ISI.Extensions.WebClient.Upload.UploadFile(new ISI.Extensions.WebClient.Rest.RestResponseTypeCollection()
					{
						{ System.Net.HttpStatusCode.OK, typeof(SerializableDTOs.UploadCycloneDxResponse) },
						{ System.Net.HttpStatusCode.BadRequest, typeof(ISI.Extensions.WebClient.Rest.JsonSerializedResponse<SerializableDTOs.UploadCycloneDxResponseError[]>) },
						{ System.Net.HttpStatusCode.Unauthorized, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
						{ System.Net.HttpStatusCode.Forbidden, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
						{ System.Net.HttpStatusCode.NotFound, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
					}, uri.Uri, GetHeaders(request), null, "none", formValues: formValues);
				}

				switch (restResponse.StatusCode)
				{
					case System.Net.HttpStatusCode.OK:
						response.Token = restResponse.GetResponse<SerializableDTOs.UploadCycloneDxResponse>()?.Token;
						break;

					case System.Net.HttpStatusCode.BadRequest:
						response.BadRequest = true;
						response.Errors = restResponse.GetResponse<SerializableDTOs.UploadCycloneDxResponseError[]>().ToNullCheckedArray(error => new DTOs.UploadCycloneDxResponseError()
						{
							Type = error.Type,
							Status = error.Status,
							Title = error.Title,
							Detail = error.Detail,
							Instance = error.Instance,
							Errors = error.Errors.ToNullCheckedArray(),
						});
						break;

					case System.Net.HttpStatusCode.Unauthorized:
						response.Unauthorized = true;
						break;

					case System.Net.HttpStatusCode.Forbidden:
						response.Forbidden = true;
						break;

					case System.Net.HttpStatusCode.NotFound:
						response.NotFound = true;
						break;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "UploadCycloneDX Failed\n{0}", exception.ErrorMessageFormatted());
			}

			return response;
		}
	}
}