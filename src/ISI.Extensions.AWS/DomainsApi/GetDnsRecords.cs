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
using ISI.Extensions.AWS.Extensions;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.AWS.DataTransferObjects.DomainsApi;

namespace ISI.Extensions.AWS
{
	public partial class DomainsApi
	{
		public DTOs.GetDnsRecordsResponse GetDnsRecords(DTOs.GetDnsRecordsRequest request)
		{
			var response = new DTOs.GetDnsRecordsResponse();

			var amazonRoute53Client = request.GetAmazonRoute53Client(Configuration);

			var hostedZoneId = GetHostedZoneId(amazonRoute53Client, request.Domain);

			if (!string.IsNullOrWhiteSpace(hostedZoneId))
			{
				var dnsRecords = new List<ISI.Extensions.Dns.DnsRecord>();

				var listResourceRecordSetsResponse = amazonRoute53Client.Paginators.ListResourceRecordSets(new Amazon.Route53.Model.ListResourceRecordSetsRequest(hostedZoneId));

				foreach (var resourceRecordSet in listResourceRecordSetsResponse.ResourceRecordSets.ToEnumerable())
				{
					var name = resourceRecordSet.Name.TrimEnd('.').TrimEnd(request.Domain, StringComparison.InvariantCultureIgnoreCase).TrimEnd('.');
					if (string.IsNullOrWhiteSpace(name))
					{
						name = "@";
					}

					foreach (var resourceRecord in resourceRecordSet.ResourceRecords ?? [])
					{
						var recordType = ISI.Extensions.Enum<ISI.Extensions.Dns.RecordType>.Parse(resourceRecordSet.Type.Value);
						var data = resourceRecord.Value;
						var priority = (new ISI.Extensions.Dns.DnsRecord()).Priority;

						switch (recordType)
						{
							case ISI.Extensions.Dns.RecordType.MailExchangeRecord:
							{
								var dataPieces = data.Split([' '], 2);
								data = dataPieces[1];
								priority = dataPieces[0].ToInt();
								break;
							}
							case ISI.Extensions.Dns.RecordType.TextRecord:
							{
								data = data.Trim('\"');
								break;
							}
						}

						dnsRecords.Add(new ISI.Extensions.Dns.DnsRecord()
						{
							Name = name,
							RecordType = recordType,
							//SetIdentifier = resourceRecordSet.SetIdentifier,
							//Region = resourceRecordSet.Region,
							//GeoLocation = resourceRecordSet.GeoLocation,
							//Failover = resourceRecordSet.Failover,
							//MultiValueAnswer = resourceRecordSet.MultiValueAnswer,
							//AliasTarget = resourceRecordSet.AliasTarget,
							//HealthCheckId = resourceRecordSet.HealthCheckId,
							//TrafficPolicyInstanceId = resourceRecordSet.TrafficPolicyInstanceId,
							//CidrRoutingConfig = resourceRecordSet.CidrRoutingConfig,
							//GeoProximityLocation = resourceRecordSet.GeoProximityLocation,
							Data = data,
							//Port = source.Port,
							Priority = priority,
							//Protocol = source.Protocol,
							//Service = source.Service,
							Ttl = (resourceRecordSet.TTL.HasValue ? TimeSpan.FromSeconds(resourceRecordSet.TTL.Value) : (new ISI.Extensions.Dns.DnsRecord()).Ttl),
							Weight = resourceRecordSet.Weight,
							//Proxied = source.Proxied,
							//Comment = source.Comment,
						});
					}
				}

				response.DnsRecords = dnsRecords.ToArray();
			}

			return response;
		}
	}
}