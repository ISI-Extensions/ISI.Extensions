using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.PostgreSQL.DataTransferObjects.BackupManager
{
	public interface IBackupDatabaseRequest
	{
		string Database { get; }
		string BackupDirectory { get; }
	}
	public class BackupDatabaseRequest : IBackupDatabaseRequest
	{
		public string Host { get; set; }
		public int Port { get; set; } = 5432;
		public string UserName { get; set; }
		public string Password { get; set; }

		public string Database { get; set; }
		public string BackupDirectory { get; set; }
	}
	public class BackupDatabaseUsingConnectionStringRequest : IBackupDatabaseRequest
	{
		public string ConnectionString { get; set; }
		
		public string Database { get; set; }
		public string BackupDirectory { get; set; }
	}
}