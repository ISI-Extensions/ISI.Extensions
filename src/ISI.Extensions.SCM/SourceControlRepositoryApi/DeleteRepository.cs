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
		public DTOs.DeleteRepositoryResponse DeleteRepository(DTOs.DeleteRepositoryRequest request)
		{
			var response = new DTOs.DeleteRepositoryResponse();

			response.Success = GetSourceControlRepositoryApi(request.SourceControlRepositoryTypeUuid)?.DeleteRepository(request)?.Success ?? false;

			return response;
		}
	}
}