#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
