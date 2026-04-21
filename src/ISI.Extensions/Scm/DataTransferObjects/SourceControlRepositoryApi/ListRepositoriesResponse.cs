using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Scm.DataTransferObjects.SourceControlRepositoryApi
{
	public class ListRepositoriesResponse
	{
		public IEnumerable<ISI.Extensions.Scm.Repository> Repositories { get; set; }
	}
}