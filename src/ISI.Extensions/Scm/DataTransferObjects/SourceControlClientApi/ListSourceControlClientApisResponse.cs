using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi
{
	public class ListSourceControlClientApisResponse
	{
		public IEnumerable<ListSourceControlClientApisResponseSourceControlClientApi> SourceControlClientApis { get; set; }
	}

	public class ListSourceControlClientApisResponseSourceControlClientApi
	{
		public Guid SourceControlTypeUuid { get; set; }
		public string Description { get; set; }
		public string RepositoryType { get; set; }
	}
}