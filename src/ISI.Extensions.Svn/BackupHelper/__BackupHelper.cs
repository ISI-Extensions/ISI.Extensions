using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Svn
{
	public partial class BackupHelper
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		protected string RepositoriesPath { get; }

		public BackupHelper(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			string repositoriesPath)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			RepositoriesPath = repositoriesPath;
		}
	}
}