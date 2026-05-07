using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Docker
{
	public class ComposeFileNetwork
	{
		public string Name { get; set; }
		public string Driver { get; set; }
		public IDictionary<string, string> DriverOpts { get; set; }
		public bool? External { get; set; }
		public IDictionary<string, string> Labels { get; set; }
		public ComposeFileNetworkIpam Ipam { get; set; }
		public bool? Attachable { get; set; }
		public bool? Ingress { get; set; }
		public bool? Internal { get; set; }
	}
}
