#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using SerializableDTOs = ISI.Extensions.NameCheap.SerializableModels;

namespace ISI.Extensions.NameCheap
{
	public partial class DomainsApi
	{
		public DTOs.SetDnsRecordsResponse SetDnsRecords(DTOs.SetDnsRecordsRequest request)
		{
			var response = new DTOs.SetDnsRecordsResponse();

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
				var existingDnsRecord = dnsRecords.FirstOrDefault(d => d.Matches(dnsRecord));

				if (existingDnsRecord != null)
				{
					existingDnsRecord.Data = dnsRecord.Data;
					existingDnsRecord.Name = dnsRecord.Name;
					existingDnsRecord.Port = dnsRecord.Port;
					existingDnsRecord.Priority = dnsRecord.Priority;
					existingDnsRecord.Protocol = dnsRecord.Protocol;
					existingDnsRecord.Service = dnsRecord.Service;
					existingDnsRecord.Ttl = dnsRecord.Ttl;
					existingDnsRecord.RecordType = dnsRecord.RecordType;
					existingDnsRecord.Weight = dnsRecord.Weight;
				}
				else
				{
					dnsRecords.Add(dnsRecord);
				}
			}

			var domainNamePieces = request.Domain.Split(new[] { '.' });

			var uri = request.GetUrl(Configuration);
			uri.SetUserNameClientIp(request, IpifyApi, Configuration);
			uri.AddQueryStringParameter("Command", "namecheap.domains.dns.setHosts");
			uri.AddQueryStringParameter("SLD", domainNamePieces.First());
			uri.AddQueryStringParameter("TLD", domainNamePieces.Last());
			uri.AddQueryStringParameter("EmailType", getDnsRecordsResponse.EmailType);

			var dnsRecordKeyValues = new List<SerializableDTOs.SetDnsRecordsRequestDnsRecordKeyValue>();

			void addDnsRecordKeyValue(int dnsRecordIndex, string key, string value)
			{
				if (!string.IsNullOrWhiteSpace(value))
				{
					dnsRecordKeyValues.Add(new SerializableDTOs.SetDnsRecordsRequestDnsRecordKeyValue()
					{
						Key = $"{key}{dnsRecordIndex}",
						Value = value,
					});

					uri.AddQueryStringParameter($"{key}{dnsRecordIndex}", value);
				}
			}

			for (var dnsRecordIndex = 1; dnsRecordIndex <= dnsRecords.Count; dnsRecordIndex++)
			{
				var dnsRecord = dnsRecords[dnsRecordIndex - 1];

				addDnsRecordKeyValue(dnsRecordIndex, "HostName", dnsRecord.Name);
				addDnsRecordKeyValue(dnsRecordIndex, "RecordType", dnsRecord.RecordType.GetDescription());
				addDnsRecordKeyValue(dnsRecordIndex, "Address", dnsRecord.Data);
				addDnsRecordKeyValue(dnsRecordIndex, "MXPref", $"{dnsRecord.Priority}");
				addDnsRecordKeyValue(dnsRecordIndex, "AssociatedAppTitle", dnsRecord.Protocol);
				addDnsRecordKeyValue(dnsRecordIndex, "FriendlyName", dnsRecord.Service);
				addDnsRecordKeyValue(dnsRecordIndex, "TTL", $"{dnsRecord.Ttl.TotalSeconds}");
			}

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteTextGet(uri.Uri, request.GetHeaders(Configuration), true);

			return response;
		}
	}
}