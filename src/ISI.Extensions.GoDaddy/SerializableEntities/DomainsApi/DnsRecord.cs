using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.GoDaddy.SerializableEntities.DomainsApi
{
	[DataContract]
	public class DnsRecord
	{
		public static DnsRecord ToSerializable(ISI.Extensions.Dns.DnsRecord dnsRecord)
		{
			return new DnsRecord()
			{
				Data = dnsRecord.Data,
				Name = dnsRecord.Name,
				Port = dnsRecord.Port,
				Priority = dnsRecord.Priority,
				Protocol = dnsRecord.Protocol,
				Service = dnsRecord.Service,
				Ttl = dnsRecord.Ttl.TotalSeconds,
				Type = dnsRecord.Type.GetKey(),
				Weight = dnsRecord.Weight,
			};
		}

		public ISI.Extensions.Dns.DnsRecord Export()
		{
			return new ISI.Extensions.Dns.DnsRecord()
			{
				Data = Data,
				Name = Name,
				Port = Port,
				Priority = Priority,
				Protocol = Protocol,
				Service = Service,
				Ttl = TimeSpan.FromSeconds(Ttl),
				Type = ISI.Extensions.Enum<ISI.Extensions.Dns.RecordType>.Parse(Type),
				Weight = Weight,
			};
		}

		[DataMember(Name = "data", EmitDefaultValue = false)]
		public string Data { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "port", EmitDefaultValue = false)]
		public int Port { get; set; }

		[DataMember(Name = "priority", EmitDefaultValue = false)]
		public int Priority { get; set; }

		[DataMember(Name = "protocol", EmitDefaultValue = false)]
		public string Protocol { get; set; }

		[DataMember(Name = "service", EmitDefaultValue = false)]
		public string Service { get; set; }

		[DataMember(Name = "ttl", EmitDefaultValue = false)]
		public double Ttl { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "weight", EmitDefaultValue = false)]
		public int Weight { get; set; }
	}
}