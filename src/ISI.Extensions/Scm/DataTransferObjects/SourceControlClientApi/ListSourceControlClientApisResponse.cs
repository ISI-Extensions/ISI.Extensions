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
		public IEnumerable<(Guid SourceControlTypeUuid, string Description, string RepositoryType)> SourceControlClientApis { get; set; }
	}
}