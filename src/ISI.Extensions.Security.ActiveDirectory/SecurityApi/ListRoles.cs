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
		public DTOs.ListRolesResponse ListRoles(DTOs.ListRolesRequest request)
		{
			var response = new DTOs.ListRolesResponse();

			var roles = new HashSet<string>();

			try
			{
				if (string.IsNullOrWhiteSpace(request.DomainName))
				{
					request.DomainName = string.Format("LDAP://{0}", GetCurrentDomainName(new DTOs.GetCurrentDomainNameRequest()).DomainName);
				}

				var directoryEntry = new System.DirectoryServices.DirectoryEntry(request.DomainName);

				var directorySearcher = new System.DirectoryServices.DirectorySearcher(directoryEntry);
				directorySearcher.Filter = "(&(objectCategory=Group))";
				directorySearcher.PropertiesToLoad.Add(GroupPropertyKey.NameKey);
				var searchResults = directorySearcher.FindAll();

				foreach (System.DirectoryServices.SearchResult searchResult in searchResults)
				{
					roles.Add(GetPropertyValue(searchResult, GroupPropertyKey.NameKey));
				}
			}
			catch
			{
			}


			response.Roles = roles;

			return response;
		}
	}
}