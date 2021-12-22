using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Telephony.DataTransferObjects.PhoneSwitchManagerApi;

namespace ISI.Extensions.Asterisk
{
	public partial class SwitchManager : ISI.Extensions.Telephony.IPhoneSwitchManagerApi
	{
		protected ISI.Extensions.Asterisk.Configuration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public SwitchManager(
			ISI.Extensions.Asterisk.Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
		}
	}
}