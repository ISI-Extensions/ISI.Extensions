using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.XenOrchestra.DataTransferObjects.XenOrchestraApi;

namespace ISI.Extensions.XenOrchestra
{
	public partial class XenOrchestraApi
	{
		protected Configuration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public XenOrchestraApi(
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