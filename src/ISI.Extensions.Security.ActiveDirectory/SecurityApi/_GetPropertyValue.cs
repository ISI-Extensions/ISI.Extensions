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
		private string GetPropertyValue(System.DirectoryServices.SearchResult searchResult, string propertyKey)
		{
			try
			{
				if (searchResult.Properties[propertyKey].Count > 0)
				{
					return string.Format("{0}", searchResult.Properties[propertyKey][0]);
				}
			}
			catch
			{
			}

			return string.Empty;
		}
	}
}