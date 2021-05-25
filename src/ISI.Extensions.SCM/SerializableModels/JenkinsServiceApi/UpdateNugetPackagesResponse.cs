using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ISI.Extensions.Scm.SerializableModels.JenkinsServiceApi
{
	[DataContract]
	public partial class UpdateNugetPackagesResponse
	{
		[DataMember(EmitDefaultValue = false)]
		public string StatusTrackerKey { get; set; }
	}
}