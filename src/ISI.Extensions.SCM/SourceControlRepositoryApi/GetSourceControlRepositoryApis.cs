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
		public DTOs.GetSourceControlRepositoryApisResponse GetSourceControlRepositoryApis(DTOs.GetSourceControlRepositoryApisRequest request)
		{
			var response = new DTOs.GetSourceControlRepositoryApisResponse();

			var sourceControlRepositoryTypeUuids = new HashSet<Guid>(request.SourceControlRepositoryTypeUuids ?? []);

			response.SourceControlRepositoryApis = SourceControlRepositoryApis
				.NullCheckedWhere(sourceControlRepositoryApi => sourceControlRepositoryTypeUuids.Contains(sourceControlRepositoryApi.SourceControlRepositoryTypeUuid))
				.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			return response;
		}
	}
}