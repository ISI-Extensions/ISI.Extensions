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
		public DTOs.CreateRepositoryResponse CreateRepository(DTOs.CreateRepositoryRequest request)
		{
			var response = new DTOs.CreateRepositoryResponse();

			response.SourceUrl = GetSourceControlRepositoryApi(request.SourceControlRepositoryTypeUuid)?.CreateRepository(request)?.SourceUrl;

			return response;
		}
	}
}