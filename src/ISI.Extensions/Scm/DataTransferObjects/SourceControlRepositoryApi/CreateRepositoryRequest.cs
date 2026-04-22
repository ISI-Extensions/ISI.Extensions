using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Scm.DataTransferObjects.SourceControlRepositoryApi
{
	public class CreateRepositoryRequest
	{
		public string ApiUrl { get; set; }
		public string ApiToken { get; set; }

		public string RepositoryNamespace { get; set; }
		public string RepositoryKey { get; set; }
		public bool IsPrivate { get; set; } = true;
	}
}