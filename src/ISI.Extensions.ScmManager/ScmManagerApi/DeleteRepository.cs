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
		public DTOs.DeleteRepositoryResponse DeleteRepository(DTOs.DeleteRepositoryRequest request)
		{
			var response = new DTOs.DeleteRepositoryResponse();

			var uri = GetApiUri(request);
			uri.SetPathAndQueryString($"api/v2/repositories/{request.Namespace}/{request.Name}");
#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteDelete(uri.Uri, GetHeaders(request), false, cookieContainer: GetCookieContainer(request));
			
			response.Success = (apiResponse == System.Net.HttpStatusCode.NoContent);

			return response;
		}
	}
}