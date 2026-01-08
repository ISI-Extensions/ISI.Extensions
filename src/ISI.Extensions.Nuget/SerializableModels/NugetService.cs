using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Nuget.SerializableModels
{
	public class NugetService
	{
		public class NugetSearchService
		{
			public static readonly string SearchQueryService = "SearchQueryService";
			public static readonly string SearchQueryService_300_beta = "SearchQueryService/3.0.0-beta"; //Alias of SearchQueryService
			public static readonly string SearchQueryService_300_rc = "SearchQueryService/3.0.0-rc"; //Alias of SearchQueryService
			public static readonly string SearchQueryService_350 = "SearchQueryService/3.5.0"; //Includes support for packageType query parameter
		}

		public class NugetCatalogService
		{
			public static readonly string Catalog = "Catalog/3.0.0";
		}

		public class NugetPackageMetadataService
		{
			public static readonly string RegistrationsBaseUrl = "RegistrationsBaseUrl";
			public static readonly string RegistrationsBaseUrl_300_beta = "RegistrationsBaseUrl/3.0.0-beta"; //Alias of RegistrationsBaseUrl
			public static readonly string RegistrationsBaseUrl_300_rc = "RegistrationsBaseUrl/3.0.0-rc"; //Alias of RegistrationsBaseUrl
			public static readonly string RegistrationsBaseUrl_340 = "RegistrationsBaseUrl/3.4.0"; //Gzipped responses
			public static readonly string RegistrationsBaseUrl_360 = "RegistrationsBaseUrl/3.6.0"; //Includes SemVer 2.0.0 packages
		}

		public class NugetPublishService
		{
			public static readonly string PackagePublish_200 = "PackagePublish/2.0.0";
		}

		public class NugetContentService
		{
			public static readonly string PackageBaseAddress_300 = "PackageBaseAddress/3.0.0";
		}
	}
}
