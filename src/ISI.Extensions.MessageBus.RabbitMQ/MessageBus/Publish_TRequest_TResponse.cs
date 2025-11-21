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
		public override async Task<TResponse> PublishAsync<TRequest, TResponse>(string channelName, TRequest request, MessageBusMessageHeaderCollection headers = null, TimeSpan? timeout = null, TimeSpan? timeToLive = null, System.Threading.CancellationToken cancellationToken = default)
			where TRequest : class
			where TResponse : class
		{
			return await PublishAsync<TRequest, TResponse>(channelName, typeof(TRequest), request, headers, timeout, timeToLive, cancellationToken);
		}

		public override async Task<TResponse> PublishAsync<TRequest, TResponse>(string channelName, Type requestType, TRequest request, MessageBusMessageHeaderCollection headers = null, TimeSpan? timeout = null, TimeSpan? timeToLive = null, System.Threading.CancellationToken cancellationToken = default)
			where TRequest : class
			where TResponse : class
		{
			throw new NotImplementedException();
		}
	}
}