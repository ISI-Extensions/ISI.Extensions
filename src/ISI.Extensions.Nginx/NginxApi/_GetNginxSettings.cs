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
		private NginxSettings GetNginxSettings(string nginxSettingsFullName)
		{
			var nginxSettings = (NginxSettings)null;

			if (!string.IsNullOrWhiteSpace(nginxSettingsFullName) && System.IO.File.Exists(nginxSettingsFullName))
			{
				try
				{
					using (var stream = System.IO.File.OpenRead(nginxSettingsFullName))
					{
						nginxSettings = JsonSerializer.Deserialize<SerializableDTOs.INginxSettings>(stream)?.Export();
					}
				}
				catch (Exception exception)
				{
					//Console.WriteLine(exception);
					//throw;
				}
			}

			nginxSettings ??= new();


			return nginxSettings;
		}
	}
}