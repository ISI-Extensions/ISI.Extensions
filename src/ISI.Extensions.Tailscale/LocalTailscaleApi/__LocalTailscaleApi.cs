using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Tailscale.DataTransferObjects.LocalTailscaleApi;
using SerializableDTOs = ISI.Extensions.Tailscale.SerializableModels.LocalTailscaleApi;

namespace ISI.Extensions.Tailscale
{
	public partial class LocalTailscaleApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		public LocalTailscaleApi(
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