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
		public DTOs.SetNginxManagerServerResponse SetNginxManagerServer(DTOs.SetNginxManagerServerRequest request)
		{
			var response = new DTOs.SetNginxManagerServerResponse();

			UpdateNginxSettings(new()
			{
				UpdateSettings = nginxSettings =>
				{
					var nginxManagerServers = new List<NginxManagerServer>(nginxSettings.NginxManagerServers ?? []);

					nginxManagerServers.RemoveAll(nginxManagerServer => nginxManagerServer.NginxManagerServerUuid == request.NginxManagerServer.NginxManagerServerUuid);

					nginxManagerServers.Add(request.NginxManagerServer);

					nginxSettings.NginxManagerServers = nginxManagerServers.ToArray();

					return true;
				},
			});

			return response;
		}
	}
}