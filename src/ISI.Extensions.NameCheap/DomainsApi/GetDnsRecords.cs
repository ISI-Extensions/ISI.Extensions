#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

			var domainNamePieces = request.DomainName.Split(new[] { '.' });

			var uri = IpifyApi.GetUrl(request, Configuration);
			uri.AddQueryStringParameter("Command", "namecheap.domains.dns.getHosts");
			uri.AddQueryStringParameter("SLD", domainNamePieces.First());
			uri.AddQueryStringParameter("TLD", domainNamePieces.Last());
			uri.AddQueryStringParameter("PageSize", 100);

			var apiResponse = ISI.Extensions.WebClient.Rest.ExecuteTextGet(uri.Uri, request.GetHeaders(Configuration), true);

			var apiResponseXml = System.Xml.Linq.XElement.Parse(apiResponse);
			var commandResponseXml = apiResponseXml.GetElementByLocalName("CommandResponse");
			var domainDNSGetHostsResultXml = commandResponseXml.GetElementByLocalName("DomainDNSGetHostsResult");
			var hostXmls = domainDNSGetHostsResultXml.GetElementsByLocalName("host");

			response.EmailType = domainDNSGetHostsResultXml.GetAttributeByLocalName("EmailType").Value;

			var dnsRecords = new List<ISI.Extensions.Dns.DnsRecord>();

			foreach (var hostXml in hostXmls)
			{
				var dnsType = ISI.Extensions.Enum<ISI.Extensions.Dns.RecordType>.Parse(hostXml.GetAttributeByLocalName("Type").Value);

				dnsRecords.Add(new()
				{
					Name = hostXml.GetAttributeByLocalName("Name").Value,
					Data = hostXml.GetAttributeByLocalName("Address").Value,
					Priority = hostXml.GetAttributeByLocalName("MXPref").Value.ToInt(),
					Protocol = hostXml.GetAttributeByLocalName("AssociatedAppTitle").Value,
					Service = hostXml.GetAttributeByLocalName("FriendlyName").Value,
					Ttl = TimeSpan.FromSeconds(hostXml.GetAttributeByLocalName("TTL").Value.ToLong()),
					RecordType = dnsType,
				});
			}

			response.DnsRecords = dnsRecords.ToArray();

			return response;
		}
	}
}