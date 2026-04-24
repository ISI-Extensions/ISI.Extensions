using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlRepositoryApi;

namespace ISI.Extensions.Scm
{
	public partial class SourceControlRepositoryApi
	{
		public DTOs.ListRepositoriesResponse ListRepositories(DTOs.ListRepositoriesRequest request)
		{
			var response = new DTOs.ListRepositoriesResponse();

			response.Repositories = GetSourceControlRepositoryApi(request.SourceControlRepositoryTypeUuid)?.ListRepositories(request)?.Repositories;

			return response;
		}
	}
}