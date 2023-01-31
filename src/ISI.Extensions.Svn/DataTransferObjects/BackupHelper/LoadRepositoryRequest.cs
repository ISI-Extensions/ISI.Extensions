using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Svn.DataTransferObjects.BackupHelper
{
	public interface ILoadRepositoryRequest
	{
		ISI.Extensions.IStatusTracker StatusTracker { get; set; }
		DateTime? ExecutedDateTimeUtc { get; set; }
		string RepositoryKey { get; set; }
	}

	public class LoadRepositoryStreamRequest : ILoadRepositoryRequest
	{
		public ISI.Extensions.IStatusTracker StatusTracker { get; set; }
		public DateTime? ExecutedDateTimeUtc { get; set; }
		public string RepositoryKey { get; set; }
		public System.IO.Stream Stream { get; set; }
	}

	public class LoadRepositoryFileNameRequest : ILoadRepositoryRequest
	{
		public ISI.Extensions.IStatusTracker StatusTracker { get; set; }
		public DateTime? ExecutedDateTimeUtc { get; set; }
		public string RepositoryKey { get; set; }
		public string DumpFullName { get; set; }
	}
}