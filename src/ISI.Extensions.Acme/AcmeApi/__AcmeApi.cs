using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonJwt.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Acme.DataTransferObjects.AcmeApi;

namespace ISI.Extensions.Acme
{
	public partial class AcmeApi : IAcmeApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get;  }

		public AcmeApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			JsonSerializer = jsonSerializer;
		}
	}
}