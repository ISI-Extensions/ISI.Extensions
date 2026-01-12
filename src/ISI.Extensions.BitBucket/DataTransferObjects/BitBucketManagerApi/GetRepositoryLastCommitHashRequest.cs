using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.BitBucket.DataTransferObjects.BitBucketManagerApi
{
	public class GetRepositoryLastCommitHashRequest : IRequest
	{
		public string BitBucketApiToken { get; set; }
		public string Workspace { get; set; }
		public string RepositoryKey { get; set; }
	}
}