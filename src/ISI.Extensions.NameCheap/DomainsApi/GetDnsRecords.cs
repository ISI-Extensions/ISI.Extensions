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
		public DTOs.GetDnsRecordsResponse GetDnsRecords(DTOs.GetDnsRecordsRequest request)
		{
			var response = new DTOs.GetDnsRecordsResponse();

			var domainNamePieces = request.Domain.Split(new[] { '.' });

			var uri = request.GetUrl(Configuration);
			uri.Path = "xml.response";

			var page = 1;
			var pageSize = 100;

			var dnsRecords = new List<ISI.Extensions.Dns.DnsRecord>();

			var @continue = true;
			while (@continue)
			{
				var formData = new ISI.Extensions.WebClient.Rest.FormDataCollection();
				formData.SetUserNameClientIp(request, IpifyApi, Configuration);
				formData.Add("Command", "namecheap.domains.dns.getHosts");
				formData.Add("SLD", domainNamePieces.First());
				formData.Add("TLD", domainNamePieces.Last());
				formData.Add("Page", page);
				formData.Add("PageSize", pageSize);

				var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteFormRequestXmlPost<SerializableModels.DomainsApi.GetDnsRecordsResponse>(uri.Uri, request.GetHeaders(Configuration), formData, true);

				response.EmailType = apiResponse.CommandResponse?.DomainDNSGetHostsResult?.EmailType;

				dnsRecords.AddRange( (apiResponse.CommandResponse?.DomainDNSGetHostsResult?.Hosts).ToNullCheckedArray(host => new ISI.Extensions.Dns.DnsRecord()
				{
					Name = host.Name,
					Data = host.Address,
					Priority = host.MXPref,
					Protocol = host.AssociatedAppTitle,
					Service = host.FriendlyName,
					Ttl = TimeSpan.FromSeconds(host.TTL),
					RecordType = ISI.Extensions.Enum<ISI.Extensions.Dns.RecordType>.Parse(host.Type)
				}));

				@continue = ((apiResponse.CommandResponse?.DomainDNSGetHostsResult?.Hosts).NullCheckedCount() == pageSize);

				page++;
			}

			response.DnsRecords = dnsRecords.ToArray();

			return response;
		}
	}
}