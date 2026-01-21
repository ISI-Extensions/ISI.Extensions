using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.ScmManager.DataTransferObjects.ScmManagerApi
{
	public class SetRepositoryRoleRequest : IRequest
	{
		public string ScmManagerApiUrl { get; set; }
		public string ScmManagerApiToken { get; set; }

		public string Namespace { get; set; }
		public string Name { get; set; }

		public string GrantTo { get; set; }
		public bool GrantToIsGroup { get; set; }

		public string Role { get; set; }
	}
}