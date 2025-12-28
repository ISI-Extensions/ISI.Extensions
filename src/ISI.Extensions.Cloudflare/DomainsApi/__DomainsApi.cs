using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Cloudflare.DataTransferObjects.DomainsApi;

namespace ISI.Extensions.Cloudflare
{
	[ISI.Extensions.DomainsApi(_dnsProviderUuid, "Cloudflare", false, null, false, null, true, "ApiKey")]
	public partial class DomainsApi : ISI.Extensions.Dns.AbstractDomainsApi, ISI.Extensions.Dns.IDomainsApi
	{
		internal const string _dnsProviderUuid = "d7cdff03-b762-4d5d-b0fc-fac6bc623fa8";
		public static Guid DnsProviderUuid { get; } = _dnsProviderUuid.ToGuid();

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		protected CloudflareApi CloudflareApi { get; }

		public DomainsApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			CloudflareApi cloudflareApi)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;

			CloudflareApi = cloudflareApi;
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProviderProfilesResponse ISI.Extensions.Dns.IDomainsApi.GetDnsProviderProfiles(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProvidersRequest request)
		{
			throw new NotImplementedException();
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.GetDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsRequest request)
		{
			var response = new ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsResponse();

			response.DnsRecords = GetDnsRecords(new DTOs.GetDnsRecordsRequest()
			{
				Url = request.Url,
				ApiToken = request.ApiKey,
				Domain = request.Domain,
			}).DnsRecords;

			return response;
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.SetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.SetDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.SetDnsRecordsRequest request)
		{
			var response = new ISI.Extensions.Dns.DataTransferObjects.DomainsApi.SetDnsRecordsResponse();

			SetDnsRecords(new DTOs.SetDnsRecordsRequest()
			{
				Url = request.Url,
				ApiToken = request.ApiKey,
				Domain = request.Domain,
				DnsRecords = request.DnsRecords,
			});

			return response;
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.DeleteDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.DeleteDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.DeleteDnsRecordsRequest request)
		{
			var response = new ISI.Extensions.Dns.DataTransferObjects.DomainsApi.DeleteDnsRecordsResponse();

			DeleteDnsRecords(new DTOs.DeleteDnsRecordsRequest()
			{
				Url = request.Url,
				ApiToken = request.ApiKey,
				Domain = request.Domain,
				DnsRecords = request.DnsRecords,
			});

			return response;
		}
	}
}