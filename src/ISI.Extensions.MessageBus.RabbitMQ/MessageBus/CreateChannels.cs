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
		protected override void CreateChannels(IServiceProvider serviceProvider, MessageBusBuildRequestCollection requests)
		{
			throw new NotImplementedException();
		}
	}
}