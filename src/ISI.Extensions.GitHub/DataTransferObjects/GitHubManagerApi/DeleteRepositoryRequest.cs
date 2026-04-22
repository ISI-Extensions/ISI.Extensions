using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.GitHub.DataTransferObjects.GitHubManagerApi
{
	public class DeleteRepositoryRequest : IRequest
	{
		public string GitHubApiToken { get; set; }

		public string Organization { get; set; }

		public string Name { get; set; }
	}
}