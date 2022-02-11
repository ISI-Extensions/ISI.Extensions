using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.Ipify.SerializableEntities
{
	[DataContract]
	public partial class GetExternalIPv4Response
	{
		[DataMember(Name = "ip", EmitDefaultValue = false)]
		public string IpAddress { get; set; }
	}
}