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
		public DTOs.GetNginxSettingsResponse GetNginxSettings(DTOs.GetNginxSettingsRequest request)
		{
			var response = new DTOs.GetNginxSettingsResponse();

			var nugetSettingsFullName = GetNginxSettingsFullName();

			response.NginxSettings = GetNginxSettings(nugetSettingsFullName);

			return response;
		}
	}
}