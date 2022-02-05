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
		public DTOs.AuthenticateUserResponse AuthenticateUser(DTOs.AuthenticateUserRequest request)
		{
			var response = new DTOs.AuthenticateUserResponse();

			try
			{
				if (string.IsNullOrWhiteSpace(request.DomainName))
				{
					request.DomainName = string.Format("LDAP://{0}", GetCurrentDomainName(new DTOs.GetCurrentDomainNameRequest()).DomainName);
				}

				var directoryEntry = new System.DirectoryServices.DirectoryEntry(request.DomainName, request.UserName, request.Password);

				var directorySearcher = new System.DirectoryServices.DirectorySearcher(directoryEntry);

				var searchResult = directorySearcher.FindOne();

				response.Authenticated = true;
			}
			catch
			{
			}

			return response;
		}
	}
}