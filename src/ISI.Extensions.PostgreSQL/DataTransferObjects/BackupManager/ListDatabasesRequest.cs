using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.PostgreSQL.DataTransferObjects.BackupManager
{
	public interface IListDatabasesRequest
	{
		ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; }
	}
	public class ListDatabasesRequest : IListDatabasesRequest
	{
		public string Host { get; set; }
		public int? Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
	public class ListDatabasesUsingConnectionStringRequest : IListDatabasesRequest
	{
		public string ConnectionString { get; set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; set; }
	}
}