using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.BitBucket.DataTransferObjects.BitBucketManagerApi
{
	public class CreateRepositoryRequest : IRequest
	{
		public string BitBucketApiToken { get; set; }

		public string Workspace { get; set; }

		public string Scm { get; set; } = "git";
		public string Name { get; set; }
		public string ProjectKey { get; set; } = "NET";

		public bool IsPrivate { get; set; } = true;
	}
}