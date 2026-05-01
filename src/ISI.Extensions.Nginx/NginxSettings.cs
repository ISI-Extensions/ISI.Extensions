using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Nginx
{
	public class NginxSettings
	{
		public NginxManagerServer[] NginxManagerServers { get; set; }

		public FormLocationAndSize[] FormLocationAndSizes { get; set; }

		public int MaxCheckDirectoryDepth { get; set; } = 5;
	}
}
