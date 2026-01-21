using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.ScmManager.DataTransferObjects.ScmManagerApi;
using SerializableDTOs = ISI.Extensions.ScmManager.SerializableModels;

namespace ISI.Extensions.ScmManager
{
	public partial class ScmManagerApi
	{
		public DTOs.CreateRepositoryResponse CreateRepository(DTOs.CreateRepositoryRequest request)
		{
			var response = new DTOs.CreateRepositoryResponse();

			var uri = GetApiUri(request);
			uri.SetPathAndQueryString($"api/v2/repositories");
			if (request.Initialize)
			{
				uri.AddQueryStringParameter("initialize", "true");
			}

			var apiRequest = new SerializableDTOs.CreateRepositoryRequest()
			{
				Namespace = request.Namespace,
				Name = request.Name,
				Type = request.Type,
			};

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost(new ISI.Extensions.WebClient.Rest.RestResponseTypeCollection()
			{
				{ System.Net.HttpStatusCode.Created, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.Unauthorized, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.Forbidden, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.Conflict, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.InternalServerError, typeof(ISI.Extensions.WebClient.Rest.SerializedResponse<SerializableDTOs.CreateRepositoryErrorResponse>) },
			}, uri.Uri, GetHeaders(request, "*/*", "application/vnd.scmm-repository+json;v=2"), apiRequest, true, cookieContainer: GetCookieContainer(request));

			if ((apiResponse.StatusCode == System.Net.HttpStatusCode.Created) && (apiResponse.Response is ISI.Extensions.WebClient.Rest.TextResponse textResponse) && textResponse.ResponseHeaders.TryGetValue("Location", out var repoUrl))
			{
				response.Url = repoUrl;
			}

			return response;
		}
	}
}