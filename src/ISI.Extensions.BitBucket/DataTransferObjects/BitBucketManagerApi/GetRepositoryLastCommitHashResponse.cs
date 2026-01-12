using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.BitBucket.DataTransferObjects.BitBucketManagerApi
{
	public class GetRepositoryLastCommitHashResponse
	{
		public string CommitHash { get; set; }
		public string Branch { get; set; }
	}
}