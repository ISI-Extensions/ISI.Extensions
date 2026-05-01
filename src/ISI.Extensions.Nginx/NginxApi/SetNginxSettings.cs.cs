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
		public DTOs.SetNginxSettingsResponse SetNginxSettings(DTOs.SetNginxSettingsRequest request)
		{
			var response = new DTOs.SetNginxSettingsResponse();

			var nginxSettingsFullName = GetNginxSettingsFullName();

			if (!string.IsNullOrWhiteSpace(nginxSettingsFullName))
			{
				using (new ISI.Extensions.Locks.FileLock(nginxSettingsFullName))
				{
					SetNginxSettings(nginxSettingsFullName, request.NginxSettings);
				}
			}

			return response;
		}
	}
}