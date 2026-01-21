using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.ScmManager.SerializableModels
{
	[DataContract]
	public class CreateRepositoryRequest
	{
		[DataMember(Name = "namespace", EmitDefaultValue = false)]
		public string Namespace { get; set; }

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }
	}
}