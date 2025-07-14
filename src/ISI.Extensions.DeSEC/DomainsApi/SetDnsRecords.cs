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
using ISI.Extensions.DeSEC.Extensions;
using DTOs = ISI.Extensions.DeSEC.DataTransferObjects.DomainsApi;
using SerializableDTOs = ISI.Extensions.DeSEC.SerializableModels.DomainsApi;

namespace ISI.Extensions.DeSEC
{
	public partial class DomainsApi
	{
		public DTOs.SetDnsRecordsResponse SetDnsRecords(DTOs.SetDnsRecordsRequest request)
		{
			var response = new DTOs.SetDnsRecordsResponse();

			foreach (var dnsRecordsGroupedByType in request.DnsRecords.GroupBy(dnsRecord => dnsRecord.RecordType))
			{
				foreach (var dnsRecordsGroupedByName in dnsRecordsGroupedByType.GroupBy(dnsRecord => dnsRecord.Name))
				{
					var uri = new UriBuilder(request.GetUrl(Configuration));
					uri.AddDirectoryToPath($"api/v1/domains/{request.Domain}/rrsets/{dnsRecordsGroupedByName.Key}/{dnsRecordsGroupedByType.Key.GetAbbreviation()}/");

					var existingDnsRecord = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SerializableDTOs.DnsRecord>(uri.Uri, request.GetHeaders(Configuration), false);
					
					var dnsRecord = new SerializableDTOs.DnsRecord()
					{
						SubName = dnsRecordsGroupedByName.Key,
						Ttl = (long)dnsRecordsGroupedByName.First().Ttl.TotalSeconds,
						RecordType = dnsRecordsGroupedByType.Key.GetAbbreviation(),
					};

					if (dnsRecord.Ttl < 3600)
					{
						dnsRecord.Ttl = 3600;
					}

					switch (dnsRecordsGroupedByType.Key)
					{
						case ISI.Extensions.Dns.RecordType.TextRecord:
							var records = new HashSet<string>((existingDnsRecord?.Records).ToNullCheckedArray(record => record.Trim(' ', '"'), NullCheckCollectionResult.Empty));
							records.UnionWith(dnsRecordsGroupedByName.ToNullCheckedArray(record => record.Data));
							dnsRecord.Records = records.ToNullCheckedArray(record => $"\"{record}\"");
							break;

						default:
							dnsRecord.Records = dnsRecordsGroupedByName.ToNullCheckedArray(record => record.Data);
							break;
					}

					if (existingDnsRecord != null)
					{
						var existingDnsRecordsX = ISI.Extensions.WebClient.Rest.ExecuteJsonPut<SerializableDTOs.DnsRecord, ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, request.GetHeaders(Configuration), dnsRecord, true);
					}
					else
					{
						uri = new UriBuilder(request.GetUrl(Configuration));
						uri.AddDirectoryToPath($"api/v1/domains/{request.Domain}/rrsets/");
						var existingDnsRecordsX = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.DnsRecord, ISI.Extensions.WebClient.Rest.TextResponse>(uri.Uri, request.GetHeaders(Configuration), dnsRecord, true);
					}
				}
			}

			return response;
		}
	}
}