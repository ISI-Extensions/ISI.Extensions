#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.DeSEC.DataTransferObjects.DomainsApi;
using SerializableDTOs = ISI.Extensions.DeSEC.SerializableModels.DomainsApi;

namespace ISI.Extensions.DeSEC
{
	[ISI.Extensions.DomainsApi(_dnsProviderUuid, "deSEC", false, "Url", false, null, true, "ApiKey")]
	public partial class DomainsApi : ISI.Extensions.Dns.AbstractDomainsApi, ISI.Extensions.Dns.IDomainsApi
	{
		internal const string _dnsProviderUuid = "367c3118-ff70-4203-993a-704e786d2c02";
		public static Guid DnsProviderUuid { get; } = _dnsProviderUuid.ToGuid();

		protected Configuration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public DomainsApi(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProviderProfilesResponse ISI.Extensions.Dns.IDomainsApi.GetDnsProviderProfiles(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProviderProfilesRequest request)
		{
			throw new NotImplementedException();
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.GetDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsRequest request)
		{
			var response = new ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsResponse();

			response.DnsRecords = GetDnsRecords(new DTOs.GetDnsRecordsRequest()
			{
				Url = request.ApiUrl,
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
				Url = request.ApiUrl,
				ApiKey = request.ApiKey,
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
				Url = request.ApiUrl,
				ApiKey = request.ApiKey,
				Domain = request.Domain,
				DnsRecords = request.DnsRecords,
			});

			return response;
		}
	}
}