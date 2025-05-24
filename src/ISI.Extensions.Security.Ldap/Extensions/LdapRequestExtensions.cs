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
	internal static class LdapRequestExtensions
	{
		public static System.DirectoryServices.Protocols.LdapConnection GetLdapConnection(this DTOs.ILdapRequest request)
		{
			const int defaultLdapPort = 389;
			const int defaultLdapsPort = 636;

			var ldapPort = request.LdapPort;

			if (request.LdapSecureSocketLayer ?? false)
			{
				ldapPort ??= defaultLdapsPort;
			}

			ldapPort ??= defaultLdapPort;

			if (request.LdapHost.IndexOf(',') >= 0)
			{
				var ldapServers = request.LdapHost.Split([',']).Select(ldapHost => ldapHost.Trim()).Where(ldapHost => !string.IsNullOrWhiteSpace(ldapHost)).ToArray();

				if (request is DTOs.ILdapRequestWithBindCredentials ldapRequestWithBindCredentials)
				{
					var ldapConnection = new System.DirectoryServices.Protocols.LdapConnection(new System.DirectoryServices.Protocols.LdapDirectoryIdentifier(ldapServers, ldapPort.GetValueOrDefault(), true, false))
					{
						AuthType = System.DirectoryServices.Protocols.AuthType.Basic,
						Credential = new(ldapRequestWithBindCredentials.LdapBindUserName, ldapRequestWithBindCredentials.LdapBindPassword),
					};

					ldapConnection.SessionOptions.ProtocolVersion = 3;
					ldapConnection.SessionOptions.ReferralChasing = System.DirectoryServices.Protocols.ReferralChasingOptions.None;
					ldapConnection.SessionOptions.SecureSocketLayer = request.LdapSecureSocketLayer ?? (ldapPort == defaultLdapsPort);

					ldapConnection.Bind();

					return ldapConnection;
				}
				else
				{
					var ldapConnection = new System.DirectoryServices.Protocols.LdapConnection(new System.DirectoryServices.Protocols.LdapDirectoryIdentifier(ldapServers, ldapPort.GetValueOrDefault(), true, false))
					{
						AuthType = System.DirectoryServices.Protocols.AuthType.Basic,
					};

					ldapConnection.SessionOptions.ProtocolVersion = 3;
					ldapConnection.SessionOptions.ReferralChasing = System.DirectoryServices.Protocols.ReferralChasingOptions.None;
					ldapConnection.SessionOptions.SecureSocketLayer = request.LdapSecureSocketLayer ?? (ldapPort == defaultLdapsPort);

					return ldapConnection;
				}
			}
			else
			{
				if (request is DTOs.ILdapRequestWithBindCredentials ldapRequestWithBindCredentials)
				{
					var ldapConnection = new System.DirectoryServices.Protocols.LdapConnection(new System.DirectoryServices.Protocols.LdapDirectoryIdentifier(request.LdapHost, ldapPort.GetValueOrDefault()))
					{
						AuthType = System.DirectoryServices.Protocols.AuthType.Basic,
						Credential = new(ldapRequestWithBindCredentials.LdapBindUserName, ldapRequestWithBindCredentials.LdapBindPassword),
					};

					ldapConnection.SessionOptions.ProtocolVersion = 3;
					ldapConnection.SessionOptions.ReferralChasing = System.DirectoryServices.Protocols.ReferralChasingOptions.None;
					ldapConnection.SessionOptions.SecureSocketLayer = request.LdapSecureSocketLayer ?? (ldapPort == defaultLdapsPort);

					ldapConnection.Bind();

					return ldapConnection;
				}
				else
				{
					var ldapConnection = new System.DirectoryServices.Protocols.LdapConnection(new System.DirectoryServices.Protocols.LdapDirectoryIdentifier(request.LdapHost, ldapPort.GetValueOrDefault()))
					{
						AuthType = System.DirectoryServices.Protocols.AuthType.Basic,
					};

					ldapConnection.SessionOptions.ProtocolVersion = 3;
					ldapConnection.SessionOptions.ReferralChasing = System.DirectoryServices.Protocols.ReferralChasingOptions.None;
					ldapConnection.SessionOptions.SecureSocketLayer = request.LdapSecureSocketLayer ?? (ldapPort == defaultLdapsPort);

					return ldapConnection;
				}
			}
		}
	}
}
