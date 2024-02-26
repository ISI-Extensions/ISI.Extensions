using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Runtime.Serialization;

namespace ISI.Extensions.Mandrill.SerializableModels.MandrillWebhooksApi
{
	[DataContract]
	public class ListWebHooksResponse : List<WebHook>
	{
		[DataMember(Name = "status")]
		public string Status { get; set; }

		[DataMember(Name = "code")]
		public string Code { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "message")]
		public string Message { get; set; }
	}
}