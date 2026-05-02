using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Nginx
{
	public class NginxManagerServer
	{
		public Guid NginxManagerServerUuid { get; set; }

		public string Description { get; set; }

		public string NginxManagerApiUrl { get; set; }
		public string NginxManagerApiKey { get; set; }

		public string[] Directories { get; set; }

		public string DisplayDescription => (string.IsNullOrWhiteSpace(Description) ? NginxManagerApiUrl : Description);
	}
}
