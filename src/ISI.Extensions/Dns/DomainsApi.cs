
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
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.Dns
{
	public class DomainsApi : ISI.Extensions.Dns.AbstractDomainsApi, IDomainsApi
	{
		protected IServiceProvider ServiceProvider { get; }

		public DomainsApi(
			IServiceProvider serviceProvider)
		{
			ServiceProvider = serviceProvider;
		}

		private IDictionary<Guid, (IDomainsApi DomainsApi, Guid DnsProviderUuid, string Description)> _domainsApisByDnsProviderUuid = null;
		protected IDictionary<Guid, (IDomainsApi DomainsApi, Guid DnsProviderUuid, string Description)> DomainsApisByDnsProviderUuid => _domainsApisByDnsProviderUuid ??= GetDomainsApisByDnsProviderUuid();

		private IDictionary<Guid, (IDomainsApi DomainsApi, Guid DnsProviderUuid, string Description)> GetDomainsApisByDnsProviderUuid()
		{
			var domainsApisByDnsProviderUuid = new Dictionary<Guid, (IDomainsApi DomainsApi, Guid DnsProviderUuid, string Description)>();

			foreach (var domainsApi in ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementations<IDomainsApi>(ServiceProvider))
			{
				var domainsApiAttribute = ((ISI.Extensions.DomainsApiAttribute[])(domainsApi.GetType().GetCustomAttributes(typeof(ISI.Extensions.DomainsApiAttribute), false))).FirstOrDefault();
				if (domainsApiAttribute != null)
				{
					domainsApisByDnsProviderUuid.Add(domainsApiAttribute.DnsProviderUuid, (DomainsApi: domainsApi, DnsProviderUuid: domainsApiAttribute.DnsProviderUuid, Description: domainsApiAttribute.Description));
				}
			}

			return domainsApisByDnsProviderUuid;
		}


		DTOs.GetDnsProvidersResponse ISI.Extensions.Dns.IDomainsApi.GetDnsProviders(DTOs.GetDnsProvidersRequest request)
		{
			var response = new DTOs.GetDnsProvidersResponse();

			response.DnsProviders = DomainsApisByDnsProviderUuid.Values.ToNullCheckedArray(dnsProvider => (DnsProviderUuid: dnsProvider.DnsProviderUuid, Description: dnsProvider.Description));

			return response;
		}

		DTOs.GetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.GetDnsRecords(DTOs.GetDnsRecordsRequest request)
		{
			if (DomainsApisByDnsProviderUuid.TryGetValue(request.DnsProviderUuid, out var domainsApi))
			{
				return domainsApi.DomainsApi.GetDnsRecords(request);
			}

			return new DTOs.GetDnsRecordsResponse();
		}

		DTOs.SetDnsRecordsResponse ISI.Extensions.Dns.IDomainsApi.SetDnsRecords(DTOs.SetDnsRecordsRequest request)
		{
			if (DomainsApisByDnsProviderUuid.TryGetValue(request.DnsProviderUuid, out var domainsApi))
			{
				return domainsApi.DomainsApi.SetDnsRecords(request);
			}

			return new DTOs.SetDnsRecordsResponse();
		}
	}
}
