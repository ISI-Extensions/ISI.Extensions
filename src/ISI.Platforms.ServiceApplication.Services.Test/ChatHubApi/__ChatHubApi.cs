using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Platforms.ServiceApplication.Services.Test.DataTransferObjects.ChatHubApi;
using SerializableDTOs = ISI.Platforms.ServiceApplication.Services.Test.SerializableModels.ChatHub;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace ISI.Platforms.ServiceApplication.Services.Test
{
	public partial class ChatHubApi : IChatHubApi
	{
		public const string HubUrlPattern = "/chat-hub";

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		protected Microsoft.AspNetCore.SignalR.Client.HubConnection HubConnection { get; set; } = null;

		public ChatHubApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
		}
	}
}