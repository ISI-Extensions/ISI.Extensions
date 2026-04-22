using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.GitHub.DataTransferObjects.GitHubManagerApi
{
	public class CreateRepositoryRequest : IRequest
	{
		public string GitHubApiToken { get; set; }

		public string Organization { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
		public string HomepageUrl { get; set; }
		public bool IsPrivate { get; set; } = true;
		public bool HasIssues { get; set; } = true;
		public bool HasProjects { get; set; } = true;
		public bool HasWiki { get; set; } = true;
	}
}