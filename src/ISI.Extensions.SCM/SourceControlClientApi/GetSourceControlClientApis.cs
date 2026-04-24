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

			var sourceControlTypeUuids = new HashSet<Guid>(request.SourceControlTypeUuids ?? []);

			response.SourceControlClientApis = SourceControlClientApis
				.NullCheckedWhere(sourceControlClientApi => sourceControlTypeUuids.Contains(sourceControlClientApi.SourceControlTypeUuid))
				.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			return response;
		}
	}
}