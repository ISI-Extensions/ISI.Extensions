using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Nginx.DataTransferObjects.NginxApi;
using SerializableDTOs = ISI.Extensions.Nginx.SerializableModels;

namespace ISI.Extensions.Nginx
{
	public partial class NginxApi
	{
		private string GetNginxSettingsFullName()
		{
			var nginxSettingsFullName = Configuration.NginxSettingsFullName;

			if (!string.IsNullOrWhiteSpace(nginxSettingsFullName) && nginxSettingsFullName.StartsWith(ISI.Extensions.ConfigurationValueReaders.FileNameDeMaskedConfigurationValueReader.PrefixWithColon, StringComparison.InvariantCultureIgnoreCase))
			{
				nginxSettingsFullName = ISI.Extensions.IO.Path.GetFileNameDeMasked(nginxSettingsFullName.TrimStart(ISI.Extensions.ConfigurationValueReaders.FileNameDeMaskedConfigurationValueReader.PrefixWithColon));
			}

			if (!string.IsNullOrWhiteSpace(nginxSettingsFullName))
			{
				System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(nginxSettingsFullName));
			}

			return nginxSettingsFullName;
		}
	}
}