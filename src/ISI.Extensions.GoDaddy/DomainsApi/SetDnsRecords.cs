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
using ISI.Extensions.GoDaddy.Extensions;
using DTOs = ISI.Extensions.GoDaddy.DataTransferObjects.DomainsApi;
using SERIALIZABLE = ISI.Extensions.GoDaddy.SerializableModels;

namespace ISI.Extensions.GoDaddy
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
					if (response.Error == null)
					{
						var uri = new UriBuilder(request.GetUrl(Configuration));
						uri.SetPathAndQueryString("/v1/domains/{domain}/records".Replace(new Dictionary<string, string>()
						{
							{ "{domain}", request.Domain }
						}, StringComparer.InvariantCultureIgnoreCase));
						uri.AddDirectoryToPath(dnsRecordsGroupedByType.Key.GetKey());
						uri.AddDirectoryToPath(dnsRecordsGroupedByName.Key);

						var dnsRecords = dnsRecordsGroupedByName.ToNullCheckedArray(SERIALIZABLE.DomainsApi.DnsRecord.ToSerializable);

						var goDaddyResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonPut(new()
						{
							{ System.Net.HttpStatusCode.OK, typeof(SERIALIZABLE.DomainsApi.DnsRecord[]) },
							{ System.Net.HttpStatusCode.BadRequest, typeof(SERIALIZABLE.Error) },
							{ System.Net.HttpStatusCode.Unauthorized, typeof(SERIALIZABLE.Error) },
							{ System.Net.HttpStatusCode.Forbidden, typeof(SERIALIZABLE.Error) },
							{ System.Net.HttpStatusCode.NotFound, typeof(SERIALIZABLE.Error) },
							{ 422, typeof(SERIALIZABLE.Error) }, //System.Net.HttpStatusCode.UnprocessableEntity
							{ 429, typeof(SERIALIZABLE.Error) }, //System.Net.HttpStatusCode.TooManyRequests
							{ System.Net.HttpStatusCode.InternalServerError, typeof(SERIALIZABLE.Error) },
							{ System.Net.HttpStatusCode.GatewayTimeout, typeof(SERIALIZABLE.Error) },
						}, uri.Uri, request.GetHeaders(Configuration), dnsRecords, true);

						switch (goDaddyResponse.Response)
						{
							case SERIALIZABLE.Error error:
								response.Error = error.NullCheckedConvert(@error => @error.Export());
								break;
						}
					}
				}
			}

			return response;
		}
	}
}