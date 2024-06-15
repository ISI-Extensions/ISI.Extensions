using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.NameCheap.DataTransferObjects.DomainsApi;

namespace ISI.Extensions.NameCheap
{
	public partial class DomainsApi
	{
		protected Configuration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		protected ISI.Extensions.Ipify.IpifyApi IpifyApi { get; }

		public DomainsApi(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			ISI.Extensions.Ipify.IpifyApi ipifyApi)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			IpifyApi = ipifyApi;
		}
	}
}