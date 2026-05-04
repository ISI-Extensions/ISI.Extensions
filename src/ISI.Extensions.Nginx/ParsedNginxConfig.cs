using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Nginx
{
	public class ParsedNginxConfig
	{
		public string FileName { get; set; }
		public string Content { get; set; }

		public Guid[] NginxManagerAgentNginxInstanceUuids { get; set; }

		public ParsedNginxConfigServer[] Servers { get; set; }
	}

	public class ParsedNginxConfigServer
	{
		public ParsedNginxConfigServerDnsAccount[] DnsAccounts { get; set; }

		public string Host { get; set; }
		public int Port { get; set; }
		public bool Ssl { get; set; }

		public ParsedNginxConfigServerLocation[] Locations { get; set; }

		public string Content { get; set; }
	}

	public class ParsedNginxConfigServerDnsAccount
	{
		public Guid DnsAccountUuid { get; set; }
		public ISI.Extensions.Dns.RecordType? DnsRecordType { get; set; }
		public string DnsRecordData { get; set; }
	}

	public class ParsedNginxConfigServerLocation
	{
		public string Location { get; set; }
		public string ProxyPassUrl { get; set; }

		public string Content { get; set; }
	}
}
