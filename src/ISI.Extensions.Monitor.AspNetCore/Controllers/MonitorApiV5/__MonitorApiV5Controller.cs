using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.AspNetCore.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Monitor.AspNetCore.Models.MonitorApiV5.SerializableModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.Monitor.AspNetCore.Controllers
{
	public partial class MonitorApiV5Controller : Controller
	{
		public IMonitorApi MonitorApi { get; set; }
		public ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		public MonitorApiV5Controller(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer,
			IMonitorApi monitorApi)
			: base(logger)
		{
			JsonSerializer = jsonSerializer;
			MonitorApi = monitorApi;
		}
	}
}