using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.SqlServer.DataTransferObjects.BackupManager
{
	public interface IBackupDatabaseRequest
	{
		string Database { get; }

		DateTime? FileNameDateTimeUtc { get; }

		string LocalBackupDirectory { get; }

		ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; }
	}
	public class BackupDatabaseRequest : IBackupDatabaseRequest
	{
		public string Host { get; set; }
		public int? Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }

		public string Database { get; set; }

		public DateTime? FileNameDateTimeUtc { get; set; }

		public string LocalBackupDirectory { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
	public class BackupDatabaseUsingConnectionStringRequest : IBackupDatabaseRequest
	{
		public string ConnectionString { get; set; }

		public string Database { get; set; }

		public DateTime? FileNameDateTimeUtc { get; set; }

		public string LocalBackupDirectory { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}