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
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Dns.DataTransferObjects.DomainsApi;

namespace ISI.Extensions.Dns
{
	public abstract class AbstractDomainsApi
	{
		public DTOs.GetTxtRecordsResponse GetTxtRecords(DTOs.GetTxtRecordsRequest request)
		{
			var response = new DTOs.GetTxtRecordsResponse();

			response.Values = GetTxtRecords(request.Domain, request.Name, request.NameServer).ToArray();

			if (!response.Values.NullCheckedAny())
			{
				var nameServer = GetNameServers(new()
				{
					Domain = request.Domain,
				}).NameServers.NullCheckedFirstOrDefault();

				if (!string.IsNullOrWhiteSpace(nameServer))
				{
					response.Values = GetTxtRecords(request.Domain, request.Name, nameServer).ToArray();
				}
			}

			return response;
		}

		private IEnumerable<string> GetTxtRecords(string domain, string name, string nameServer)
		{
			var fqdn = $"{name}.{domain}";

			var arguments = new List<string>();
			arguments.Add("-q=txt");
			arguments.Add(fqdn);
			if (!string.IsNullOrWhiteSpace(nameServer))
			{
				arguments.Add(nameServer);
			}

			var nslookupResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				ProcessExeFullName = "nslookup",
				Arguments = arguments,
			});

			var values = new List<string>();

			var lines = nslookupResponse.Output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

			for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
			{
				var line = lines[lineIndex].Trim();

				if (line.StartsWith(fqdn, StringComparison.InvariantCultureIgnoreCase))
				{
					if (line.EndsWith("="))
					{
						line = $"{line} {lines[++lineIndex]}";
					}

					var value = line.Split(['"'], StringSplitOptions.RemoveEmptyEntries).Last().Trim();

					values.Add(value);
				}
			}

			return values;
		}

		public DTOs.GetNameServersResponse GetNameServers(DTOs.GetNameServersRequest request)
		{
			var response = new DTOs.GetNameServersResponse();

			var arguments = new List<string>();
			arguments.Add("-type=ns");
			arguments.Add(request.Domain);
			arguments.Add(request.NameServer);

			var nslookupResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				ProcessExeFullName = "nslookup",
				Arguments = arguments,
			});

			var values = new List<string>();

			var lines = nslookupResponse.Output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

			for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
			{
				var line = lines[lineIndex].Trim();

				if (line.StartsWith(request.Domain, StringComparison.InvariantCultureIgnoreCase))
				{
					if (line.EndsWith("="))
					{
						line = $"{line} {lines[++lineIndex]}";
					}

					var value = line.Split(['='], StringSplitOptions.RemoveEmptyEntries).Last().Trim();

					values.Add(value);
				}
			}

			response.NameServers = values.ToArray();

			return response;
		}
	}
}
