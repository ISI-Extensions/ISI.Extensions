using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.NameCheap.DataTransferObjects.DomainsApi;

namespace ISI.Extensions.NameCheap
{
	[ISI.Extensions.DomainsApi(_dnsProviderUuid, "NameCheap")]
	public partial class DomainsApi : ISI.Extensions.Dns.AbstractDomainsApi, ISI.Extensions.Dns.IDomainsApi
	{
		internal const string _dnsProviderUuid = "14746b87-ec50-4eb5-ace9-ae685a76a328";
		public static Guid DnsProviderUuid { get; } = _dnsProviderUuid.ToGuid();

		protected Configuration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		protected ISI.Extensions.Ipify.IpifyApi IpifyApi { get; }

		public DomainsApi(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			ISI.Extensions.Ipify.IpifyApi ipifyApi)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			IpifyApi = ipifyApi;
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProvidersResponse ISI.Extensions.Dns.IDomainsApi.GetDnsProviders(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProvidersRequest request)
		{
			throw new NotImplementedException();
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.GetDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsRequest request)
		{
			var response = new ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsResponse();

			response.DnsRecords = GetDnsRecords(new DTOs.GetDnsRecordsRequest()
			{
				Url = request.Url,
				ApiUser = request.ApiUser,
				ApiKey = request.ApiKey,
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
				ApiUser = request.ApiUser,
				ApiKey = request.ApiKey,
				Domain = request.Domain,
				DnsRecords = request.DnsRecords,
			});

			return response;
		}
	}
}