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
		protected Configuration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.JsonSerialization.IJsonSerializer JsonSerializer { get; }

		public const string NginxConfigFileNameExtension = ".nginxConfig";

		public NginxApi(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.JsonSerialization.IJsonSerializer jsonSerializer)
		{
			Configuration = configuration;
			Logger = logger;
			JsonSerializer = jsonSerializer;
		}
	}
}