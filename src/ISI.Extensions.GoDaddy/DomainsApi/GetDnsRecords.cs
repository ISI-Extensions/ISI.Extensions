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
using SerializableDTOs = ISI.Extensions.GoDaddy.SerializableModels;

namespace ISI.Extensions.GoDaddy
{
	public partial class DomainsApi
	{
		public DTOs.GetDnsRecordsResponse GetDnsRecords(DTOs.GetDnsRecordsRequest request)
		{
			var response = new DTOs.GetDnsRecordsResponse();

			var uri = new UriBuilder(request.GetUrl(Configuration));
			uri.SetPathAndQueryString("/v1/domains/{domain}/records".Replace(new Dictionary<string, string>()
			{
				{"{domain}", request.Domain}
			}, StringComparer.InvariantCultureIgnoreCase));
			if (request.RecordType.HasValue)
			{
				uri.AddDirectoryToPath(request.RecordType.GetKey());

				if (!string.IsNullOrWhiteSpace(request.Name))
				{
					uri.AddDirectoryToPath(request.Name);
				}
			}
			else if (!string.IsNullOrWhiteSpace(request.Name))
			{
				throw new("Cannot specify a name without a record type");
			}

			var goDaddyResponse = ISI.Extensions.WebClient.Rest.ExecuteJsonGet(new()
			{
				{ System.Net.HttpStatusCode.OK, typeof(SerializableModels.DomainsApi.DnsRecord[]) },
				{ System.Net.HttpStatusCode.BadRequest, typeof(SerializableModels.Error) },
				{ System.Net.HttpStatusCode.Unauthorized, typeof(SerializableModels.Error) },
				{ System.Net.HttpStatusCode.Forbidden, typeof(SerializableModels.Error) },
				{ System.Net.HttpStatusCode.NotFound, typeof(SerializableModels.Error) },
				{ 422, typeof(SerializableModels.Error) }, //System.Net.HttpStatusCode.UnprocessableEntity
				{ 429, typeof(SerializableModels.Error) }, //System.Net.HttpStatusCode.TooManyRequests
				{ System.Net.HttpStatusCode.InternalServerError, typeof(SerializableModels.Error) },
				{ System.Net.HttpStatusCode.GatewayTimeout, typeof(SerializableModels.Error) },
			}, uri.Uri, request.GetHeaders(Configuration), true);

			switch (goDaddyResponse.Response)
			{
				case SerializableModels.DomainsApi.DnsRecord[] dnsRecords:
					response.DnsRecords = dnsRecords.ToNullCheckedArray(dnsRecord => dnsRecord.Export());
					break;

				case SerializableModels.Error error:
					response.Error = error.NullCheckedConvert(@error => @error.Export());
					break;
			}

			return response;
		}
	}
}