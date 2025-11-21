using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.MessageBus.RabbitMQ
{
	public partial class MessageBus : ISI.Extensions.MessageBus.AbstractMessageBus
	{
		protected override TimeSpan DefaultResponseTimeOut => TimeSpan.FromMinutes(1);

		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		public MessageBus(
			ISI.Extensions.MessageBus.Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
			: base(configuration, logger)
		{
			JsonSerializer = jsonSerializer;
		}

		public override void Dispose()
		{
		}
	}
}