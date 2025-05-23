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
using DTOs = ISI.Extensions.Security.Ldap.DataTransferObjects.LdapApi;

namespace ISI.Extensions.Security.Ldap.Extensions
{
	internal static class LdapConnectionExtensions
	{
		public static string GetDefaultNamingContext(this System.DirectoryServices.Protocols.LdapConnection ldapConnection)
		{
			var ldapSearchResponse = ldapConnection.SendRequest(new System.DirectoryServices.Protocols.SearchRequest(string.Empty, "(objectClass=*)", System.DirectoryServices.Protocols.SearchScope.Base, [ISI.Extensions.Security.Directory.UserPropertyKey.DefaultNamingContext])) as System.DirectoryServices.Protocols.SearchResponse;

			foreach (System.DirectoryServices.Protocols.SearchResultEntry entry in ldapSearchResponse.Entries)
			{
				foreach (var value in entry.Attributes.Values)
				{
					if (value is System.DirectoryServices.Protocols.DirectoryAttribute directoryAttribute)
					{
						if (string.Equals(directoryAttribute.Name, ISI.Extensions.Security.Directory.UserPropertyKey.DefaultNamingContext, StringComparison.CurrentCultureIgnoreCase))
						{
							var defaultNamingContext = (directoryAttribute.GetValues(typeof(string)) as string[]).NullCheckedFirstOrDefault();

							if (!string.IsNullOrWhiteSpace(defaultNamingContext))
							{
								return defaultNamingContext;
							}
						}
					}
				}
			}

			return null;
		}

		public static string GetFQDN(this System.DirectoryServices.Protocols.LdapConnection ldapConnection)
		{
			var defaultNamingContext = ldapConnection.GetDefaultNamingContext();

			return string.Join(".", defaultNamingContext.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(value => value.Split(new[] { '=' }).Last()));
		}
	}
}
