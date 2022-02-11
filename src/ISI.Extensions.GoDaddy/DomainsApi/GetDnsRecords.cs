using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.GoDaddy.Extensions;
using DTOs = ISI.Extensions.GoDaddy.DataTransferObjects.DomainsApi;
using SERIALIZABLE = ISI.Extensions.GoDaddy.SerializableEntities;

namespace ISI.Extensions.GoDaddy
{
	public partial class DomainsApi
	{
		public DTOs.GetDnsRecordsResponse GetDnsRecords(DTOs.GetDnsRecordsRequest request)
		{
			var response = new DTOs.GetDnsRecordsResponse();

			var uri = new UriBuilder(request.GetUrl(Configuration));
			uri.SetPathAndQueryString("/v1/domains/{domain}/records".Replace(new Dictionary<string, string>()
			{
				{"{domain}", request.DomainName}
			}, StringComparer.InvariantCultureIgnoreCase));
			if (request.RecordType.HasValue)
			{
				uri.AddDirectoryToPath(request.RecordType.GetKey());

				if (!string.IsNullOrWhiteSpace(request.Name))
				{
					uri.AddDirectoryToPath(request.Name);
				}
			}
			else if (!string.IsNullOrWhiteSpace(request.Name))
			{
				throw new Exception("Cannot specify a name without a record type");
			}

			var goDaddyResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet(new ISI.Extensions.WebClient.Rest.RestResponseTypeCollection()
			{
				{ System.Net.HttpStatusCode.OK, typeof(SERIALIZABLE.DomainsApi.DnsRecord[]) },
				{ System.Net.HttpStatusCode.BadRequest, typeof(SERIALIZABLE.Error) },
				{ System.Net.HttpStatusCode.Unauthorized, typeof(SERIALIZABLE.Error) },
				{ System.Net.HttpStatusCode.Forbidden, typeof(SERIALIZABLE.Error) },
				{ System.Net.HttpStatusCode.NotFound, typeof(SERIALIZABLE.Error) },
				{ 422, typeof(SERIALIZABLE.Error) }, //System.Net.HttpStatusCode.UnprocessableEntity
				{ 429, typeof(SERIALIZABLE.Error) }, //System.Net.HttpStatusCode.TooManyRequests
				{ System.Net.HttpStatusCode.InternalServerError, typeof(SERIALIZABLE.Error) },
				{ System.Net.HttpStatusCode.GatewayTimeout, typeof(SERIALIZABLE.Error) },
			}, uri.Uri, request.GetHeaders(Configuration), true);

			switch (goDaddyResponse.Response)
			{
				case SERIALIZABLE.DomainsApi.DnsRecord[] dnsRecords:
					response.DnsRecords = dnsRecords.ToNullCheckedArray(dnsRecord => dnsRecord.Export());
					break;

				case SERIALIZABLE.Error error:
					response.Error = error.NullCheckedConvert(@error => @error.Export());
					break;
			}

			return response;
		}
	}
}