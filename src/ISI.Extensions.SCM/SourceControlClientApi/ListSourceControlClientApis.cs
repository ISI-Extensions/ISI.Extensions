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
		public DTOs.ListSourceControlClientApisResponse ListSourceControlClientApis(DTOs.ListSourceControlClientApisRequest request)
		{
			var response = new DTOs.ListSourceControlClientApisResponse();

			response.SourceControlClientApis = SourceControlClientApis.ToNullCheckedArray(sourceControlClientApi => new DTOs.ListSourceControlClientApisResponseSourceControlClientApi()
			{
				SourceControlTypeUuid = sourceControlClientApi.SourceControlTypeUuid,
				Description = sourceControlClientApi.Description,
				RepositoryType = sourceControlClientApi.RepositoryType
			});
			
			return response;
		}
	}
}