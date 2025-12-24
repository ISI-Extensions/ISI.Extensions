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
using ISI.Extensions.AWS.Extensions;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.AWS.DataTransferObjects.DomainsApi;

namespace ISI.Extensions.AWS
{
	public partial class DomainsApi
	{
		public DTOs.DeleteDnsRecordsResponse DeleteDnsRecords(DTOs.DeleteDnsRecordsRequest request)
		{
			var response = new DTOs.DeleteDnsRecordsResponse();

			var amazonRoute53Client = request.GetAmazonRoute53Client(Configuration);

			var hostedZoneId = GetHostedZoneId(amazonRoute53Client, request.Domain);

			if (!string.IsNullOrWhiteSpace(hostedZoneId))
			{
				var getDnsRecordsResponse = GetDnsRecords(new()
				{
					AmazonAccessKey = request.AmazonAccessKey,
					AmazonSecretKey = request.AmazonSecretKey,
					Domain = request.Domain,
				});

				var existingDnsRecords = new List<ISI.Extensions.Dns.DnsRecord>(getDnsRecordsResponse?.DnsRecords ?? []);

				var changes = new List<Amazon.Route53.Model.Change>();

				foreach (var dnsRecordsGroupedByRecordType in request.DnsRecords.GroupBy(dnsRecord => dnsRecord.RecordType))
				{
					foreach (var dnsRecordsGroupedByName in dnsRecordsGroupedByRecordType.GroupBy(dnsRecord => dnsRecord.Name, StringComparer.InvariantCultureIgnoreCase))
					{
						var name = $"{dnsRecordsGroupedByName.Key}.{request.Domain}".ToLower();
						var rrType = dnsRecordsGroupedByRecordType.Key.GetRRType();


						switch (dnsRecordsGroupedByRecordType.Key)
						{
							case ISI.Extensions.Dns.RecordType.TextRecord:
								{
									var dnsRecords = new List<ISI.Extensions.Dns.DnsRecord>();

									dnsRecords.AddRange(existingDnsRecords.Where(existingDnsRecord => (existingDnsRecord.RecordType == dnsRecordsGroupedByRecordType.Key) && string.Equals(existingDnsRecord.Name, dnsRecordsGroupedByName.Key, StringComparison.InvariantCultureIgnoreCase)));

									foreach (var dnsRecord in request.DnsRecords)
									{
										dnsRecords.RemoveAll(d => d.Matches(dnsRecord));
									}

									if (dnsRecords.Any())
									{
										var resourceRecords = new List<Amazon.Route53.Model.ResourceRecord>(dnsRecords.NullCheckedSelect(dnsRecord => new Amazon.Route53.Model.ResourceRecord($"\"{dnsRecord.Data.Trim('\"')}\"")));

										changes.Add(new Amazon.Route53.Model.Change()
										{
											Action = Amazon.Route53.ChangeAction.UPSERT,
											ResourceRecordSet = new Amazon.Route53.Model.ResourceRecordSet(name, rrType)
											{
												ResourceRecords = resourceRecords,
												TTL = (long)dnsRecordsGroupedByName.First().Ttl.TotalSeconds,
												Weight = dnsRecordsGroupedByName.First().Weight,
											},
										});
									}
									else
									{
										changes.Add(new Amazon.Route53.Model.Change()
										{
											Action = Amazon.Route53.ChangeAction.DELETE,
											ResourceRecordSet = new Amazon.Route53.Model.ResourceRecordSet(name, rrType),
										});
									}

									break;
								}

							default:
								changes.Add(new Amazon.Route53.Model.Change()
								{
									Action = Amazon.Route53.ChangeAction.DELETE,
									ResourceRecordSet = new Amazon.Route53.Model.ResourceRecordSet(name, rrType),
								});
								break;
						}

					}
				}

				var changeResourceRecordSetsResponse = amazonRoute53Client.ChangeResourceRecordSetsAsync(new Amazon.Route53.Model.ChangeResourceRecordSetsRequest()
				{
					HostedZoneId = hostedZoneId,
					ChangeBatch = new Amazon.Route53.Model.ChangeBatch()
					{
						Changes = changes,
					}
				}).GetAwaiter().GetResult();
			}

			return response;
		}
	}
}