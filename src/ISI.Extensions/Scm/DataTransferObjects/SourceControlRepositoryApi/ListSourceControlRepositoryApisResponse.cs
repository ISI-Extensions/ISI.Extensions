using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Scm.DataTransferObjects.SourceControlRepositoryApi
{
	public class ListSourceControlRepositoryApisResponse
	{
		public IEnumerable<ListSourceControlRepositoryApisResponseSourceControlRepositoryApi> SourceControlRepositoryApis { get; set; }
	}

	public class ListSourceControlRepositoryApisResponseSourceControlRepositoryApi
	{
		public Guid SourceControlRepositoryTypeUuid { get; set; }
		public string Description { get; set; }
		public string RepositoryType { get; set; }
	}
}