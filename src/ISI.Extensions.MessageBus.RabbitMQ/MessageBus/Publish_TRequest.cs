using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.MessageBus.RabbitMQ
{
	public partial class MessageBus
	{
		public override async Task PublishAsync<TRequest>(TRequest request, MessageBusMessageHeaderCollection headers = null, System.Threading.CancellationToken cancellationToken = default)
			where TRequest : class
		{
			//await PublishAsync(GetChannelName<TRequest>(), request, headers, cancellationToken);
			throw new NotImplementedException();
		}

		public override async Task PublishAsync<TRequest>(string channelName, TRequest request, MessageBusMessageHeaderCollection headers = null, System.Threading.CancellationToken cancellationToken = default)
			where TRequest : class
		{
			throw new NotImplementedException();
		}
	}
}