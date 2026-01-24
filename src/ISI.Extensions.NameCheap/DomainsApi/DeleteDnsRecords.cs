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
using ISI.Extensions.NameCheap.Extensions;
using DTOs = ISI.Extensions.NameCheap.DataTransferObjects.DomainsApi;

namespace ISI.Extensions.NameCheap
{
	public partial class DomainsApi
	{
		public DTOs.DeleteDnsRecordsResponse DeleteDnsRecords(DTOs.DeleteDnsRecordsRequest request)
		{
			var response = new DTOs.DeleteDnsRecordsResponse();

			var getDnsRecordsResponse = GetDnsRecords(new()
			{
				Url = request.Url,
				ApiUser = request.ApiUser,
				ApiKey = request.ApiKey,
				Domain = request.Domain,
			});

			var dnsRecords = new List<ISI.Extensions.Dns.DnsRecord>(getDnsRecordsResponse?.DnsRecords ?? []);

			foreach (var dnsRecord in request.DnsRecords)
			{
				dnsRecords.RemoveAll(d => d.Matches(dnsRecord));
			}

			var domainNamePieces = request.Domain.Split(new[] { '.' });

			var uri = request.GetUrl(Configuration);

			var formData = new ISI.Extensions.WebClient.Rest.FormDataCollection();
			formData.SetUserNameClientIp(request, IpifyApi, Configuration);
			formData.Add("Command", "namecheap.domains.dns.getHosts");
			formData.Add("SLD", domainNamePieces.First());
			formData.Add("TLD", domainNamePieces.Last());
			formData.Add("EmailType", getDnsRecordsResponse.EmailType);

			void addDnsRecordKeyValue(int dnsRecordIndex, string key, string value)
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					uri.AddQueryStringParameter($"{key}{dnsRecordIndex}", value);
				}
			}

			for (var dnsRecordIndex = 1; dnsRecordIndex <= dnsRecords.Count; dnsRecordIndex++)
			{
				var dnsRecord = dnsRecords[dnsRecordIndex - 1];

				addDnsRecordKeyValue(dnsRecordIndex, "HostName", dnsRecord.Name);
				addDnsRecordKeyValue(dnsRecordIndex, "RecordType", dnsRecord.RecordType.GetAbbreviation());
				addDnsRecordKeyValue(dnsRecordIndex, "Address", dnsRecord.Data);
				if (dnsRecord.Priority != 10)
				{
					addDnsRecordKeyValue(dnsRecordIndex, "MXPref", $"{dnsRecord.Priority}");
				}
				addDnsRecordKeyValue(dnsRecordIndex, "AssociatedAppTitle", dnsRecord.Protocol);
				addDnsRecordKeyValue(dnsRecordIndex, "FriendlyName", dnsRecord.Service);
				addDnsRecordKeyValue(dnsRecordIndex, "TTL", $"{dnsRecord.Ttl.TotalSeconds}");
			}
			
			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteFormRequestPost<ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, request.GetHeaders(Configuration), formData, true);
			
			return response;
		}
	}
}