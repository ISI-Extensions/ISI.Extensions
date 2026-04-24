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
		public DTOs.ListSourceControlRepositoryApisResponse ListSourceControlRepositoryApis(DTOs.ListSourceControlRepositoryApisRequest request)
		{
			var response = new DTOs.ListSourceControlRepositoryApisResponse();

			response.SourceControlRepositoryApis = SourceControlRepositoryApis.ToNullCheckedArray(sourceControlRepositoryApi => new DTOs.ListSourceControlRepositoryApisResponseSourceControlRepositoryApi()
			{
				SourceControlRepositoryTypeUuid = sourceControlRepositoryApi.SourceControlRepositoryTypeUuid,
				Description = sourceControlRepositoryApi.Description,
				RepositoryType = sourceControlRepositoryApi.RepositoryType,
				UseApiUrl = sourceControlRepositoryApi.UseApiUrl,
				ApiUrlDescription = sourceControlRepositoryApi.ApiUrlDescription,
				UseApiToken = sourceControlRepositoryApi.UseApiToken,
				ApiTokenDescription = sourceControlRepositoryApi.ApiTokenDescription,
				UseRepositoryNamespace = sourceControlRepositoryApi.UseRepositoryNamespace,
				RepositoryNamespaceDescription = sourceControlRepositoryApi.RepositoryNamespaceDescription,
			});

			return response;
		}
	}
}