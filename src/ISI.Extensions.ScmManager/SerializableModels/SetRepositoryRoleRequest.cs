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
	public class SetRepositoryRoleRequest
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string GrantTo { get; set; }

		[DataMember(Name = "verbs", EmitDefaultValue = false)]
		public string[] Verbs { get; set; }

		[DataMember(Name = "role", EmitDefaultValue = false)]
		public string Role { get; set; }

		[DataMember(Name = "groupPermission", EmitDefaultValue = false)]
		public bool GrantToIsGroup { get; set; }
	}
}