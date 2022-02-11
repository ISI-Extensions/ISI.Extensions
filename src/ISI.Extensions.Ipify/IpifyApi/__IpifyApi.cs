using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Ipify.DataTransferObjects.IpifyApi;

namespace ISI.Extensions.Ipify
{
	public partial class IpifyApi
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		protected string Version { get; }

		public IpifyApi(
			Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;

			Version = ISI.Extensions.SystemInformation.GetAssemblyVersion(this.GetType().Assembly);
		}

		protected ISI.Extensions.WebClient.HeaderCollection GetHeaders()
		{
			var headers = new ISI.Extensions.WebClient.HeaderCollection();

			headers.Add("user-agent", string.Format("ISI.Extensions.Ipify/{0}", Version));

			return headers;
		}
	}
}