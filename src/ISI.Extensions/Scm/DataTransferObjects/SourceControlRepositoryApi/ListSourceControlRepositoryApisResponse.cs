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
		public bool UseApiUrl { get; set; }
		public string ApiUrlDescription { get; set; }
		public bool UseApiToken { get; set; }
		public string ApiTokenDescription { get; set; }
		public bool UseRepositoryNamespace { get; set; }
		public string RepositoryNamespaceDescription { get; set; }
	}
}