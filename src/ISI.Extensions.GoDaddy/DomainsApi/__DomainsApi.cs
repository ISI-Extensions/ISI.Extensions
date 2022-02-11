using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.GoDaddy.Extensions;
using DTOs = ISI.Extensions.GoDaddy.DataTransferObjects.DomainsApi;
using SERIALIZABLE = ISI.Extensions.GoDaddy.SerializableEntities;

namespace ISI.Extensions.GoDaddy
{
	public partial class DomainsApi
	{
		protected Configuration Configuration { get; }

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public DomainsApi(
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