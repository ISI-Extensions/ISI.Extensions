using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions.Asterisk
{
	[ISI.Extensions.ConfigurationHelper.Configuration(ConfigurationSectionName)]
	public partial class Configuration : ISI.Extensions.ConfigurationHelper.IConfiguration
	{
		public const string ConfigurationSectionName = "ISI.Extensions.Asterisk";

		public string ServerIpAddress { get; set; }
		public int ServerPort { get; set; } = 5038;
		public string UserName { get; set; }
		public string Password { get; set; }
	}
}