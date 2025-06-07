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
using ISI.Extensions.Security.Ldap.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Security.Ldap.DataTransferObjects.LdapApi;

namespace ISI.Extensions.Security.Ldap
{
	public partial class LdapApi
	{
		public DTOs.GetUsersResponse GetUsers(DTOs.GetUsersRequest request)
		{
			if (request == null)
			{
				throw new Exception("request == null");
			}

			if (MemoryCache == null)
			{
				throw new Exception("MemoryCache == null");
			}

			var response = new DTOs.GetUsersResponse();

			var users = new List<ISI.Extensions.Security.Directory.User>();

			var ldapSchema = string.Empty;
			var ldapHost = request.LdapHost;
			var ldapPort = request.LdapPort;

			using (var ldapConnection = request.GetLdapConnection(MemoryCache))
			{
				if (ldapConnection == null)
				{
					throw new Exception("ldapConnection == null");
				}

				ldapSchema = ldapConnection.LdapSchema;
				ldapHost = ldapConnection.LdapHost;
				ldapPort = ldapConnection.LdapPort;

				ldapConnection.Bind(request);

				var defaultNamingContext = ldapConnection.GetDefaultNamingContext();

				foreach (var userName in request.UserNames ?? [])
				{
					try
					{
						var ldapSearchResults = ldapConnection.SearchAsync($"CN=Users,{defaultNamingContext}", Novell.Directory.Ldap.LdapConnection.ScopeOne, $"(&(objectCategory=User)(objectClass=person)({ISI.Extensions.Security.Directory.UserPropertyKey.UserNameKey}={userName}))", [
							ISI.Extensions.Security.Directory.UserPropertyKey.NameKey,
							ISI.Extensions.Security.Directory.UserPropertyKey.EmailAddressKey,
							ISI.Extensions.Security.Directory.UserPropertyKey.FirstNameKey,
							ISI.Extensions.Security.Directory.UserPropertyKey.LastNameKey,
							ISI.Extensions.Security.Directory.UserPropertyKey.UserNameKey,
							ISI.Extensions.Security.Directory.UserPropertyKey.DistinguishedNameKey,
							ISI.Extensions.Security.Directory.UserPropertyKey.RolesKey
						], false).GetAwaiter().GetResult().GetAsyncEnumerator();

						try
						{
							while (ldapSearchResults.MoveNextAsync().GetAwaiter().GetResult())
							{
								users.Add(new()
								{
									Name = ldapSearchResults.Current.GetPropertyValue(ISI.Extensions.Security.Directory.UserPropertyKey.NameKey),
									EmailAddress = ldapSearchResults.Current.GetPropertyValue(ISI.Extensions.Security.Directory.UserPropertyKey.EmailAddressKey),
									FirstName = ldapSearchResults.Current.GetPropertyValue(ISI.Extensions.Security.Directory.UserPropertyKey.FirstNameKey),
									LastName = ldapSearchResults.Current.GetPropertyValue(ISI.Extensions.Security.Directory.UserPropertyKey.LastNameKey),
									UserName = ldapSearchResults.Current.GetPropertyValue(ISI.Extensions.Security.Directory.UserPropertyKey.UserNameKey),
									DistinguishedName = ldapSearchResults.Current.GetPropertyValue(ISI.Extensions.Security.Directory.UserPropertyKey.DistinguishedNameKey),
									Roles = ldapSearchResults.Current.GetPropertyValues(ISI.Extensions.Security.Directory.UserPropertyKey.RolesKey),
								});
							}
						}
						finally
						{
							ldapSearchResults?.DisposeAsync().GetAwaiter().GetResult();
						}
					}
					catch (Exception exception)
					{
						//#if DEBUG
						Logger.LogError(exception, $"AuthenticateUser\nSchema:{ldapSchema}\nHost:{ldapHost}\nPort:{ldapPort}\nMessage:{exception.Message}");
						//#endif
					}
				}
			}

			response.Users = users;

			return response;
		}
	}
}