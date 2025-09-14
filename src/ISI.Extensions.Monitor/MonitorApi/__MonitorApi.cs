using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Monitor.DataTransferObjects.MonitorApi;

namespace ISI.Extensions.Monitor
{
	public partial class MonitorApi : IMonitorApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected System.IServiceProvider ServiceProvider { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public MonitorApi(
			Microsoft.Extensions.Logging.ILogger logger,
			System.IServiceProvider serviceProvider,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Logger = logger;
			ServiceProvider = serviceProvider;
			DateTimeStamper = dateTimeStamper;
		}
	}
}