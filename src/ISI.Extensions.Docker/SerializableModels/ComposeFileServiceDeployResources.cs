using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileServiceDeployResources
	{
		[YamlMember(Alias = "limits", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployResourcesResourceSpec Limits { get; set; }

		[YamlMember(Alias = "reservations", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeployResourcesResourceSpec Reservations { get; set; }
	}
}
