using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi;

namespace ISI.Extensions.Scm
{
	public partial class SourceControlClientApi
	{
		public DTOs.GetSourceControlClientApisResponse GetSourceControlClientApis(DTOs.GetSourceControlClientApisRequest request)
		{
			var response = new DTOs.GetSourceControlClientApisResponse();

			var sourceControlClientTypeUuids = new HashSet<Guid>(request.SourceControlClientTypeUuids ?? []);

			response.SourceControlClientApis = SourceControlClientApis
				.NullCheckedWhere(sourceControlClientApi => sourceControlClientTypeUuids.Contains(sourceControlClientApi.SourceControlClientTypeUuid))
				.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			return response;
		}
	}
}