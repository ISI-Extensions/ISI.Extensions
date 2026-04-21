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
			uri.AddDirectoryToPath(request.Workspace);
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

				response.Url = uri.Uri.ToString();
			}

			return response;
		}
}
}