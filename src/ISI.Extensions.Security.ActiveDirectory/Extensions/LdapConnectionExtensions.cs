﻿#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using ISI.Extensions.Security.ActiveDirectory.Extensions;
using DTOs = ISI.Extensions.Security.ActiveDirectory.DataTransferObjects.ActiveDirectoryApi;

namespace ISI.Extensions.Security.ActiveDirectory.Extensions
{
	internal static class LdapConnectionExtensions
	{
		public static void Connect(this Novell.Directory.Ldap.LdapConnection ldapConnection, DTOs.ILdapRequest request)
		{
			var ldapPort = request.LdapPort;

			if (request.LdapSecureSocketLayer)
			{
				//Console.WriteLine("ldapConnection.SecureSocketLayer = true");
				ldapConnection.SecureSocketLayer = true;
				ldapPort ??= 689;
			}

			ldapPort ??= 389;

			if (request.LdapStartTls)
			{
				//Console.WriteLine("ldapConnection.StartTls()");
				ldapConnection.StartTls();
			}

			//Console.WriteLine($"ldapConnection.Host = {request.LdapHost}");
			//Console.WriteLine($"ldapConnection.Port = {ldapPort}");
			ldapConnection.Connect(request.LdapHost, ldapPort.Value);
		}

		public static void Bind(this Novell.Directory.Ldap.LdapConnection ldapConnection, DTOs.ILdapRequestWithBindCredentials request)
		{
			//Console.WriteLine($"ldapConnection.LdapBindUserName = {request.LdapBindUserName}");
			//Console.WriteLine($"ldapConnection.LdapBindPassword = {request.LdapBindPassword}");

			ldapConnection.Bind(request.LdapBindUserName, request.LdapBindPassword);
		}

		public static string GetDefaultNamingContext(this Novell.Directory.Ldap.LdapConnection ldapConnection)
		{
			var ldapSearchResults = ldapConnection.Search(string.Empty, Novell.Directory.Ldap.LdapConnection.ScopeBase, "(objectClass=*)", [UserPropertyKey.DefaultNamingContext], false);

			return ldapSearchResults.First().GetPropertyValue(UserPropertyKey.DefaultNamingContext);
		}

		public static string GetFQDN(this Novell.Directory.Ldap.LdapConnection ldapConnection)
		{
			var defaultNamingContext = ldapConnection.GetDefaultNamingContext();

			return string.Join(".", defaultNamingContext.Split([','], StringSplitOptions.RemoveEmptyEntries).Select(value => value.Split(new[] { '=' }).Last()));
		}
	}
}
