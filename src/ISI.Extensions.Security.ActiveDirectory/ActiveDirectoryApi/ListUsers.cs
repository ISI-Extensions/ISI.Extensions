#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Security.ActiveDirectory.DataTransferObjects.ActiveDirectoryApi;

namespace ISI.Extensions.Security.ActiveDirectory
{
	public partial class ActiveDirectoryApi
	{
		public DTOs.ListUsersResponse ListUsers(DTOs.ListUsersRequest request)
		{
			var response = new DTOs.ListUsersResponse();
			
			var users = new List<ISI.Extensions.Security.ActiveDirectory.User>();

			try
			{
				if (string.IsNullOrWhiteSpace(request.DomainName))
				{
					request.DomainName = string.Format("LDAP://{0}", GetCurrentDomainName(new()).DomainName);
				}

				var directoryEntry = new System.DirectoryServices.DirectoryEntry(request.DomainName);

				var directorySearcher = new System.DirectoryServices.DirectorySearcher(directoryEntry);
				directorySearcher.Filter = "(&(objectCategory=User)(objectClass=person))";
				directorySearcher.PropertiesToLoad.Add(UserPropertyKey.NameKey);
				directorySearcher.PropertiesToLoad.Add(UserPropertyKey.EmailAddressKey);
				directorySearcher.PropertiesToLoad.Add(UserPropertyKey.FirstNameKey);
				directorySearcher.PropertiesToLoad.Add(UserPropertyKey.LastNameKey);
				directorySearcher.PropertiesToLoad.Add(UserPropertyKey.UserNameKey);
				directorySearcher.PropertiesToLoad.Add(UserPropertyKey.DistinguishedNameKey);
				directorySearcher.PropertiesToLoad.Add(UserPropertyKey.RolesKey);
				var searchResults = directorySearcher.FindAll();

				foreach (System.DirectoryServices.SearchResult searchResult in searchResults)
				{
					users.Add(new()
					{
						Name = GetPropertyValue(searchResult, UserPropertyKey.NameKey),
						EmailAddress = GetPropertyValue(searchResult, UserPropertyKey.EmailAddressKey),
						FirstName = GetPropertyValue(searchResult, UserPropertyKey.FirstNameKey),
						LastName = GetPropertyValue(searchResult, UserPropertyKey.LastNameKey),
						UserName = GetPropertyValue(searchResult, UserPropertyKey.UserNameKey),
						DistinguishedName = GetPropertyValue(searchResult, UserPropertyKey.DistinguishedNameKey),
						Roles = GetPropertyValues(searchResult, UserPropertyKey.RolesKey),
					});
				}
			}
			catch
			{
			}

			response.Users = users;

			return response;
		}
	}
}