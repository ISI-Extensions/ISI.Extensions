using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Ssh.DataTransferObjects.SshApi;

namespace ISI.Extensions.Ssh
{
	public partial class SshApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public SshApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
		}
	}
}