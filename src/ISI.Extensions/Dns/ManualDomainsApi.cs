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

namespace ISI.Extensions.Dns
{
	[ISI.Extensions.DomainsApi(_dnsProviderUuid, "Manual", false, null, false, null, false, null)]
	public class ManualDomainsApi : ISI.Extensions.Dns.AbstractDomainsApi, ISI.Extensions.Dns.IDomainsApi
	{
		internal const string _dnsProviderUuid = "d83036c2-5fc3-48d7-9d14-945be141107e";
		public static Guid DnsProviderUuid { get; } = _dnsProviderUuid.ToGuid();

		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public ManualDomainsApi(
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProviderProfilesResponse ISI.Extensions.Dns.IDomainsApi.GetDnsProviderProfiles(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsProviderProfilesRequest request)
		{
			throw new NotImplementedException();
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.GetDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.GetDnsRecordsRequest request)
		{
			throw new NotImplementedException();
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.SetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.SetDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.SetDnsRecordsRequest request)
		{
			var response = new ISI.Extensions.Dns.DataTransferObjects.DomainsApi.SetDnsRecordsResponse();

			var stringBuilder = new StringBuilder();

			stringBuilder.AppendLine($"Domain Name: {request.Domain}");
			foreach (var dnsRecord in request.DnsRecords)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine($"RecordType: {dnsRecord.RecordType}");
				stringBuilder.AppendLine($"Name: {dnsRecord.Name}");
				stringBuilder.AppendLine($"Data: {dnsRecord.Data}");
			}

			//System.Diagnostics.Debugger.Break();

			return response;
		}

		ISI.Extensions.Dns.DataTransferObjects.DomainsApi.DeleteDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.DeleteDnsRecords(ISI.Extensions.Dns.DataTransferObjects.DomainsApi.DeleteDnsRecordsRequest request)
		{
			throw new NotImplementedException();
		}
	}
}