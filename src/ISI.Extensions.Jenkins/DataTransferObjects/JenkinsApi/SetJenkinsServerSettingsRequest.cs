using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi
{
	public class SetJenkinsServerSettingsRequest
	{
		public ISI.Extensions.Jenkins.JenkinsServer JenkinsServer { get; set;  }
	}
}