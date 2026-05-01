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
		private void SetNginxSettings(string nginxSettingsFullName, NginxSettings nginxSettings)
		{
			if (System.IO.File.Exists(nginxSettingsFullName))
			{
				System.IO.File.Delete(nginxSettingsFullName);
			}

			using (var stream = System.IO.File.OpenWrite(nginxSettingsFullName))
			{
				JsonSerializer.Serialize(SerializableDTOs.NginxSettingsV1.ToSerializable(nginxSettings), stream, true);
			}
		}
	}
}