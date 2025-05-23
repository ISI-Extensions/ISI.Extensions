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
	internal static class SearchResultEntryExtensions
	{
		internal static ISI.Extensions.Security.Directory.User GetUser(this System.DirectoryServices.Protocols.SearchResultEntry searchResultEntry)
		{
			var user = new ISI.Extensions.Security.Directory.User();

			foreach (var value in searchResultEntry.Attributes.Values)
			{
				if (value is System.DirectoryServices.Protocols.DirectoryAttribute directoryAttribute)
				{
					if (string.Equals(directoryAttribute.Name, ISI.Extensions.Security.Directory.UserPropertyKey.NameKey, StringComparison.CurrentCultureIgnoreCase))
					{
						user.Name = (directoryAttribute.GetValues(typeof(string)) as string[]).NullCheckedFirstOrDefault();
					}
					else if (string.Equals(directoryAttribute.Name, ISI.Extensions.Security.Directory.UserPropertyKey.EmailAddressKey, StringComparison.CurrentCultureIgnoreCase))
					{
						user.EmailAddress = (directoryAttribute.GetValues(typeof(string)) as string[]).NullCheckedFirstOrDefault();
					}
					else if (string.Equals(directoryAttribute.Name, ISI.Extensions.Security.Directory.UserPropertyKey.FirstNameKey, StringComparison.CurrentCultureIgnoreCase))
					{
						user.FirstName = (directoryAttribute.GetValues(typeof(string)) as string[]).NullCheckedFirstOrDefault();
					}
					else if (string.Equals(directoryAttribute.Name, ISI.Extensions.Security.Directory.UserPropertyKey.LastNameKey, StringComparison.CurrentCultureIgnoreCase))
					{
						user.LastName = (directoryAttribute.GetValues(typeof(string)) as string[]).NullCheckedFirstOrDefault();
					}
					else if (string.Equals(directoryAttribute.Name, ISI.Extensions.Security.Directory.UserPropertyKey.UserNameKey, StringComparison.CurrentCultureIgnoreCase))
					{
						user.UserName = (directoryAttribute.GetValues(typeof(string)) as string[]).NullCheckedFirstOrDefault();
					}
					else if (string.Equals(directoryAttribute.Name, ISI.Extensions.Security.Directory.UserPropertyKey.DistinguishedNameKey, StringComparison.CurrentCultureIgnoreCase))
					{
						user.DistinguishedName = (directoryAttribute.GetValues(typeof(string)) as string[]).NullCheckedFirstOrDefault();
					}
					else if (string.Equals(directoryAttribute.Name, ISI.Extensions.Security.Directory.UserPropertyKey.RolesKey, StringComparison.CurrentCultureIgnoreCase))
					{
						var roles = new ISI.Extensions.InvariantCultureIgnoreCaseStringHashSet();

						foreach (var role in directoryAttribute.GetValues(typeof(string)) as string[] ?? [])
						{
							var propertyValues = string.Format("{0}", role).Split(['=', ','], StringSplitOptions.RemoveEmptyEntries);

							if (propertyValues.Length >= 2)
							{
								roles.Add(propertyValues[1]);
							}
						}

						user.Roles = roles.ToArray();
					}
				}
			}

			return user;
		}
	}
}