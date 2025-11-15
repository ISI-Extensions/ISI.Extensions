using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Docker.DataTransferObjects.DockerApi;
using SERIALIZABLEMODELS = ISI.Extensions.Docker.SerializableModels;

namespace ISI.Extensions.Docker
{
	public partial class DockerApi
	{
		private string GetContainerImageReference(string containerRegistry, string containerRepository, string containerImageTag)
		{
			var containerImageReference = containerImageTag;

			if (string.IsNullOrWhiteSpace(containerRegistry))
			{
				if (!string.IsNullOrWhiteSpace(containerRepository))
				{
					containerImageReference = $"{containerRepository}:{containerImageTag}";
				}
			}
			else
			{
				if (string.IsNullOrWhiteSpace(containerRepository))
				{
					containerImageReference = $"{containerRegistry}/{containerImageTag}";
				}
				else
				{
					containerImageReference = $"{containerRegistry}/{containerRepository}:{containerImageTag}";
				}
			}

			return containerImageReference;
		}
	}
}