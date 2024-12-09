using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Alpine.DataTransferObjects.AlpineApi;

namespace ISI.Extensions.Alpine
{
	public partial class AlpineApi : IAlpineApi
	{
		protected Configuration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public AlpineApi(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
		}
	}
}