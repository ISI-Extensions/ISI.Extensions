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
		private string[] GetPropertyValues(System.DirectoryServices.SearchResult searchResult, string propertyKey)
		{
			try
			{
				if (searchResult.Properties[propertyKey].Count > 0)
				{
					var values = new HashSet<string>();

					foreach (var property in searchResult.Properties[propertyKey])
					{
						var propertyValues = string.Format("{0}", property).Split(new[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);

						if (propertyValues.Length >= 2)
						{
							values.Add(propertyValues[1]);
						}
					}

					return values.ToArray();
				}
			}
			catch
			{
			}

			return Array.Empty<string>();
		}
	}
}