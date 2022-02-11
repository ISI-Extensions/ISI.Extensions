using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Dns
{
	public class DnsRecord
	{
		public string Data { get; set; }
		public string Name { get; set; }
		public int Port { get; set; }
		public int Priority { get; set; }
		public string Protocol { get; set; }
		public string Service { get; set; }
		public TimeSpan Ttl { get; set; } = TimeSpan.FromHours(1);
		public RecordType Type { get; set; }
		public int Weight { get; set; }

		public override string ToString() => $"{Name} {Type} {Data}";
	}
}