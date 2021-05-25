using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ISI.Extensions.Scm.SerializableModels.JenkinsServiceApi
{
	[DataContract]
	public partial class UpdateNugetPackagesRequest
	{
		[DataMember(Name = "settingsFullName", EmitDefaultValue = false)]
		public string SettingsFullName { get; set; }

		[DataMember(Name = "jenkinsUrl", EmitDefaultValue = false)]
		public string JenkinsUrl { get; set; }

		[DataMember(Name = "userName", EmitDefaultValue = false)]
		public string UserName { get; set; }

		[DataMember(Name = "apiToken", EmitDefaultValue = false)]
		public string ApiToken { get; set; }

		[DataMember(Name = "jobIds", EmitDefaultValue = false)]
		public string[] JobIds { get; set; }

		[DataMember(Name = "filterByJobNameSuffix", EmitDefaultValue = false)]
		public string FilterByJobIdSuffix { get; set; }

		[DataMember(Name = "ignorePackageIds", EmitDefaultValue = false)]
		public string[] IgnorePackageIds { get; set; }
	}
}