using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Security.DataTransferObjects.SecurityApi;

namespace ISI.Extensions.Security.ActiveDirectory
{
	public partial class SecurityApi
	{
		public DTOs.GetUsersResponse GetUsers(DTOs.GetUsersRequest request)
		{
			var response = new DTOs.GetUsersResponse();

			var users = new List<ISI.Extensions.Security.User>();

			if (string.IsNullOrWhiteSpace(request.DomainName))
			{
				request.DomainName = string.Format("LDAP://{0}", GetCurrentDomainName(new DTOs.GetCurrentDomainNameRequest()).DomainName);
			}

			foreach (var userName in request.UserNames)
			{
				try
				{
					var directoryEntry = new System.DirectoryServices.DirectoryEntry(request.DomainName);

					var directorySearcher = new System.DirectoryServices.DirectorySearcher(directoryEntry);
					directorySearcher.Filter = string.Format("(&(objectCategory=User)(objectClass=person)({0}={1}))", UserPropertyKey.UserNameKey, userName);
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
						users.Add(new ISI.Extensions.Security.User()
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
			}

			response.Users = users;

			return response;
		}
	}
}