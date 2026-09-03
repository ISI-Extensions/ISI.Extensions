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
using DTOs = ISI.Extensions.Cloudflare.DataTransferObjects.CloudflareApi;
using SerializableDTOs = ISI.Extensions.Cloudflare.SerializableModels;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Cloudflare
{
	public partial class CloudflareApi
	{
		//Accepted Permissions (at least one required)
		//DNS Write
		public DTOs.SetDnsRecordsResponse SetDnsRecords(DTOs.SetDnsRecordsRequest request)
		{
			var response = new DTOs.SetDnsRecordsResponse();

			EnsureZoneId(request);

			var existingDnsRecords = (SerializableDTOs.DnsRecord[])null;

			var dnsRecords = new List<ISI.Extensions.Dns.DnsRecord>();

			try
			{
				var uri = GetUrl(request);
				uri.AddDirectoryToPath("zones/{zoneId}/dns_records".Replace("{zoneId}", request.ZoneId));

				var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet<SerializableDTOs.ListDnsRecordsResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), false);

				if (restResponse.Error != null)
				{
					throw restResponse.Error.Exception;
				}

				existingDnsRecords = restResponse?.Response?.DnsRecords.ToNullCheckedArray();
			}
			catch (Exception exception)
			{
				Logger.LogError(exception, "ListDnsRecords Failed\n{0}", exception.ErrorMessageFormatted());
			}

			foreach (var dnsRecord in request.DnsRecords)
			{
				switch (dnsRecord.RecordType)
				{
					case ISI.Extensions.Dns.RecordType.AddressRecord:
						break;
					case ISI.Extensions.Dns.RecordType.CanonicalNameRecord:
						break;
					case ISI.Extensions.Dns.RecordType.TextRecord:
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				var restRequest = existingDnsRecords.NullCheckedFirstOrDefault(existingDnsRecord =>
					string.Equals((string.IsNullOrWhiteSpace(existingDnsRecord.SubName) ? "@" : existingDnsRecord.Name), (string.IsNullOrWhiteSpace(dnsRecord.Name) ? "@" : existingDnsRecord.Name), StringComparison.InvariantCultureIgnoreCase) &&
					(ISI.Extensions.Enum<ISI.Extensions.Dns.RecordType>.ParseAbbreviation(existingDnsRecord.RecordType) == dnsRecord.RecordType) &&
					((dnsRecord.RecordType != ISI.Extensions.Dns.RecordType.TextRecord) || string.Equals(existingDnsRecord.Content, $"\"{dnsRecord.Data}\"", StringComparison.InvariantCulture)));

				if (restRequest == null)
				{
					restRequest = ISI.Extensions.Cloudflare.SerializableModels.DnsRecord.ToSerializable(dnsRecord, request.ZoneName);

					try
					{
						var uri = GetUrl(request);
						uri.AddDirectoryToPath("zones/{zoneId}/dns_records".Replace("{zoneId}", request.ZoneId));

						var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPost<SerializableDTOs.DnsRecord, SerializableDTOs.SetDnsRecordsResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), restRequest, false);

						if (restResponse.Error != null)
						{
							throw restResponse.Error.Exception;
						}

						dnsRecords.Add(restResponse?.Response?.DnsRecord?.Export());
					}
					catch (Exception exception)
					{
						Logger.LogError(exception, "SetDnsRecords (Post) Failed\n{0}", exception.ErrorMessageFormatted());
					}
				}
				else
				{
					restRequest.Content = (dnsRecord.RecordType == ISI.Extensions.Dns.RecordType.TextRecord ? $"\"{dnsRecord.Data}\"" : dnsRecord.Data);
					restRequest.Ttl = (int)dnsRecord.Ttl.TotalSeconds;
					restRequest.Proxied = dnsRecord.Proxied;
					restRequest.Comment = dnsRecord.Comment;

					try
					{
						var uri = GetUrl(request);
						uri.AddDirectoryToPath("zones/{zoneId}/dns_records/{dnsRecordId}".Replace("{zoneId}", request.ZoneId).Replace("{dnsRecordId}", restRequest.DnsRecordKey));

						var restResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPatch<SerializableDTOs.DnsRecord, SerializableDTOs.SetDnsRecordsResponse, ISI.Extensions.WebClient.Rest.UnhandledExceptionResponse>(uri.Uri, GetHeaders(request), restRequest, false);

						if (restResponse.Error != null)
						{
							throw restResponse.Error.Exception;
						}

						dnsRecords.Add(restResponse?.Response?.DnsRecord?.Export());
					}
					catch (Exception exception)
					{
						Logger.LogError(exception, "SetDnsRecords (Patch) Failed\n{0}", exception.ErrorMessageFormatted());
					}
				}
			}

			response.DnsRecords = dnsRecords.ToArray();

			return response;
		}
	}
}