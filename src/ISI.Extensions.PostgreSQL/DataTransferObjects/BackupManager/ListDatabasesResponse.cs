using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.PostgreSQL.DataTransferObjects.BackupManager
{
	public class ListDatabasesResponse
	{
		public string[] Databases { get; set; }
	}
}