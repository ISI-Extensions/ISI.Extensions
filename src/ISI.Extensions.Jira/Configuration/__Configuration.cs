using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Jira
{
	[ISI.Extensions.ConfigurationHelper.Configuration(ConfigurationSectionName)]
	public partial class Configuration : ISI.Extensions.ConfigurationHelper.IConfiguration
	{
		public const string ConfigurationSectionName = "ISI.Extensions.Jira";

		public string JiraApiUrl { get; set; }
		public string JiraApiUserName { get; set; }
		public string JiraApiToken { get; set; }
		public System.Security.Authentication.SslProtocols SslProtocols { get; set; } = System.Security.Authentication.SslProtocols.Tls | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Ssl3;
	}
}