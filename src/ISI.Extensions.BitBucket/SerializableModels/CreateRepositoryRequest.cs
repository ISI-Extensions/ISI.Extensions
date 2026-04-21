using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.BitBucket.SerializableModels
{
	[DataContract]
	public class CreateRepositoryRequest
	{
		[DataMember(Name = "scm", EmitDefaultValue = false)]
		public string Scm { get; set; }

		[DataMember(Name = "is_Private", EmitDefaultValue = false)]
		public bool IsPrivate { get; set; }

		[DataMember(Name = "project", EmitDefaultValue = false)]
		public CreateRepositoryRequestProject Project { get; set; }
		}

	[DataContract]
		public class CreateRepositoryRequestProject
	{
		[DataMember(Name = "key", EmitDefaultValue = false)]
		public string Key { get; set; }
	}
}