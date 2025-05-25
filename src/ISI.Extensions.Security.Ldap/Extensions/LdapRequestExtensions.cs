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
using Microsoft.Extensions.Caching.Memory;
using DTOs = ISI.Extensions.Security.Ldap.DataTransferObjects.LdapApi;

namespace ISI.Extensions.Security.Ldap.Extensions
{
	internal static class LdapRequestExtensions
	{
		private static Random _random = null;
		private static Random Random => _random ??= new Random();

		public static Novell.Directory.Ldap.LdapConnection GetLdapConnection(this DTOs.ILdapRequest request, Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache)
		{
			const int defaultLdapPort = 389;
			const int defaultLdapsPort = 636;

			var ldapHost = request.LdapHost;
			var ldapPort = request.LdapPort;
			var ldapSecureSocketLayer = request.LdapSecureSocketLayer;

			if (Uri.TryCreate(ldapHost, UriKind.Absolute, out var ldapUri))
			{
				ldapHost = ldapUri.Host;

				switch (ldapUri.Scheme.ToLower())
				{
					case "ldap":
						ldapPort ??= defaultLdapPort;
						break;

					case "ldaps":
						ldapPort ??= defaultLdapsPort;
						ldapSecureSocketLayer = true;
						break;
				}
			}

			var ldapServers = GetLdapServers(ldapHost, memoryCache);

			if (ldapServers.NullCheckedAny())
			{
				ldapHost = ldapServers[Random.Next(ldapServers.Length)];
			}

			if (ldapSecureSocketLayer ?? false)
			{
				ldapPort ??= defaultLdapsPort;
			}

			ldapPort ??= defaultLdapPort;

			ldapSecureSocketLayer ??= (ldapPort == defaultLdapsPort);

			var ldapConnectionOptions = new Novell.Directory.Ldap.LdapConnectionOptions();

			if (ldapSecureSocketLayer ?? false)
			{
				if (request.ByPassRemoteCertificateValidation)
				{
					ldapConnectionOptions.ConfigureRemoteCertificateValidationCallback((sender, certificate, chain, errors) => true);
				}
			}

			var ldapConnection = new Novell.Directory.Ldap.LdapConnection(ldapConnectionOptions);

			if (ldapSecureSocketLayer ?? false)
			{
				//Console.WriteLine("ldapConnection.SecureSocketLayer = true");
				ldapConnection.SecureSocketLayer = true;
			}

			if (request.LdapStartTls ?? false)
			{
				//Console.WriteLine("ldapConnection.StartTls()");
				ldapConnection.StartTlsAsync().GetAwaiter().GetResult();
			}

			//Console.WriteLine($"ldapConnection.Host = {ldapHost}");
			//Console.WriteLine($"ldapConnection.Port = {ldapPort}");
			ldapConnection.ConnectAsync(ldapHost, ldapPort.Value).GetAwaiter().GetResult();

			return ldapConnection;
		}

		private static string[] GetLdapServers(string ldapHost, Microsoft.Extensions.Caching.Memory.IMemoryCache memoryCache)
		{
			var cacheKey = $"GetLdapServers-{ldapHost}-b04658fd-11e3-4782-b053-9109c9bdd51f";

			if (memoryCache.TryGetValue(cacheKey, out var cachedLdapServers))
			{
				if (cachedLdapServers is string[] value)
				{
					return value;
				}
			}

			var arguments = new List<string>();
			arguments.Add("-query=srv");
			arguments.Add($"_ldap._tcp.{ldapHost}");

			var nslookupResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				ProcessExeFullName = "nslookup",
				Arguments = arguments,
				Logger = new NullLogger(),
			});

			var values = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

			var lines = nslookupResponse.Output.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

			for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
			{
				var lineParts = lines[lineIndex].Trim().Split([' ', '\t', '='], StringSplitOptions.RemoveEmptyEntries);

				if ((lineParts.Length > 3) && string.Equals(lineParts[1], "internet", StringComparison.InvariantCultureIgnoreCase) && string.Equals(lineParts[2], "address", StringComparison.InvariantCultureIgnoreCase))
				{
					values.Add(lineParts[0]);
				}
			}

			var ldapServers = values.ToArray();

			using (var cacheEntry = memoryCache.CreateEntry(cacheKey))
			{
				cacheEntry.SetAbsoluteExpiration(DateTimeOffset.UtcNow + TimeSpan.FromHours(1));
				cacheEntry.SetValue(ldapServers);
			}
			
			return ldapServers;
		}
	}
}
