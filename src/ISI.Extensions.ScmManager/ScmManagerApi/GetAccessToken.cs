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
		public DTOs.GetAccessTokenResponse GetAccessToken(DTOs.GetAccessTokenRequest request)
		{
			var response = new DTOs.GetAccessTokenResponse();

			var uri = GetApiUri(request);
			uri.SetPathAndQueryString("api/v2/auth/access_token");

			var apiRequest = new SerializableDTOs.GetAccessTokenRequest()
			{
				Username = request.Username,
				Password = request.Password,
			};

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var headers = new ISI.Extensions.WebClient.HeaderCollection();
			headers.Accept = "*/*";

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost(new ISI.Extensions.WebClient.Rest.RestResponseTypeCollection()
			{
				{ System.Net.HttpStatusCode.OK, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.NoContent, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.BadRequest, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.Unauthorized, typeof(ISI.Extensions.WebClient.Rest.TextResponse) },
				{ System.Net.HttpStatusCode.InternalServerError, typeof(ISI.Extensions.WebClient.Rest.SerializedResponse<SerializableDTOs.SetRepositoryRoleErrorResponse>) },
			}, uri.Uri, headers, apiRequest, true);

			if ((apiResponse.StatusCode == System.Net.HttpStatusCode.OK) && (apiResponse.Response is ISI.Extensions.WebClient.Rest.TextResponse textResponse))
			{
				response.ScmManagerApiToken = textResponse.Content;
			}

			return response;
		}
	}
}