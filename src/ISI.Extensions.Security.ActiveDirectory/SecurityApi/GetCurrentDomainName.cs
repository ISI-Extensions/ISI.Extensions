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
		public DTOs.GetCurrentDomainNameResponse GetCurrentDomainName(DTOs.GetCurrentDomainNameRequest request)
		{
			var response = new DTOs.GetCurrentDomainNameResponse();
			
			var directoryEntry = new System.DirectoryServices.DirectoryEntry("LDAP://RootDSE");

			response.DomainName = string.Format("{0}", directoryEntry.Properties["defaultNamingContext"][0]);

			return response;
		}
	}
}