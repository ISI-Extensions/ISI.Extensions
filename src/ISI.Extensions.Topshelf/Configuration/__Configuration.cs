using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.Topshelf
{
	[ISI.Extensions.ConfigurationHelper.Configuration("ISI.Extensions.Topshelf")]
	public partial class Configuration : ISI.Extensions.ConfigurationHelper.IConfiguration
	{
		public LogOnAsConfiguration LogOnAs { get; set; } = null;
		public string ServicePrefix { get; set; }
		public string ServiceDescription { get; set; }
		public string ServiceDisplayName { get; set; }
		public string ServiceServiceName { get; set; }

		public TimeSpan StartTimeOut  { get; set; } = TimeSpan.FromSeconds(20);
		public TimeSpan StopTimeOut { get; set; } = TimeSpan.FromMinutes(1);
		public TimeSpan AdditionalStartTime { get; set; } = TimeSpan.FromMinutes(10);
		public TimeSpan AdditionalStopTime { get; set; } = TimeSpan.FromMinutes(10);
	}
}