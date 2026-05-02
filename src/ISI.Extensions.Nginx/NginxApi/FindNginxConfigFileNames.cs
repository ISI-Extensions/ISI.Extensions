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
		public DTOs.FindNginxConfigFileNamesResponse FindNginxConfigFileNames(DTOs.FindNginxConfigFileNamesRequest request)
		{
			var response = new DTOs.FindNginxConfigFileNamesResponse();

			var nginxConfigFileNames = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

			foreach (var path in request.Paths)
			{
				if (System.IO.File.Exists(path) && IsNginxConfigFile(new()
				    {
					    FileName = path,
				    }).IsNginxConfigFile)
				{
					nginxConfigFileNames.Add(path);
				}
				else if (System.IO.Directory.Exists(path))
				{
					nginxConfigFileNames.UnionWith(System.IO.Directory.EnumerateFiles(path, $"*{NginxConfigFileNameExtension}", System.IO.SearchOption.AllDirectories));
				}
			}

			response.NginxConfigFileNames = nginxConfigFileNames.ToArray();

			return response;
		}
	}
}