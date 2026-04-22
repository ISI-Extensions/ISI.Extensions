using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.GitHub.SerializableModels
{
	[DataContract]
	public class CreateRepositoryRequest
	{
		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "homepage", EmitDefaultValue = false)]
		public string HomepageUrl { get; set; }

		[DataMember(Name = "private", EmitDefaultValue = false)]
		public bool IsPrivate { get; set; }

		[DataMember(Name = "has_Issues", EmitDefaultValue = false)]
		public bool HasIssues { get; set; }

		[DataMember(Name = "has_Projects", EmitDefaultValue = false)]
		public bool HasProjects { get; set; }

		[DataMember(Name = "has_Wiki", EmitDefaultValue = false)]
		public bool HasWiki { get; set; }
	}
}