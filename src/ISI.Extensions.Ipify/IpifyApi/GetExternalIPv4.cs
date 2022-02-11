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
		public DTOs.GetExternalIPv4Response GetExternalIPv4()
		{
			var response = new DTOs.GetExternalIPv4Response();

			var uri = new UriBuilder("https://api.ipify.org");
			uri.AddQueryStringParameter("format", "json");

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SerializableEntities.GetExternalIPv4Response>(uri.Uri, GetHeaders(), true);

			response.IpAddress = apiResponse.IpAddress;

			return response;
		}
	}
}