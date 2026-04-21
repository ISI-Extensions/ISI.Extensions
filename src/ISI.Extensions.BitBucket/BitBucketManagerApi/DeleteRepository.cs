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
		public DTOs.DeleteRepositoryResponse DeleteRepository(DTOs.DeleteRepositoryRequest request)
		{
			var response = new DTOs.DeleteRepositoryResponse();

			var uri = GetApiUri(request);
			uri.AddDirectoryToPath("repositories");
			uri.AddDirectoryToPath(request.Workspace);
			uri.AddDirectoryToPath(request.Name);

#if DEBUG
			var xxx = ISI.Extensions.WebClient.Rest.GetEventHandler();
#endif

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteDelete(uri.Uri, GetHeaders(request), true);

			response.Success = (apiResponse == System.Net.HttpStatusCode.NoContent);

			return response;
		}
	}
}