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
		public override Task StopAsync(System.Threading.CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}