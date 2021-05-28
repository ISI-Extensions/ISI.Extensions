#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class NugetApi_Tests
	{
		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			var configurationBuilder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
			var configuration = configurationBuilder.Build();

			var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
				.AddOptions()
				.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

			services.AddAllConfigurations(configuration)

				//.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory>()
				.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory, Microsoft.Extensions.Logging.LoggerFactory>()
				.AddLogging(builder => builder
						.AddConsole()
				//.AddFilter(level => level >= Microsoft.Extensions.Logging.LogLevel.Information)
				)
				.AddSingleton<Microsoft.Extensions.Logging.ILogger>(_ => new ISI.Extensions.TextWriterLogger(TestContext.Progress))

				.AddSingleton<ISI.Extensions.DateTimeStamper.IDateTimeStamper, ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper>()

				.AddSingleton<ISI.Extensions.JsonSerialization.IJsonSerializer, ISI.Extensions.JsonSerialization.Newtonsoft.NewtonsoftJsonSerializer>()
				.AddSingleton<ISI.Extensions.Serialization.ISerialization, ISI.Extensions.Serialization.Serialization>()

				.AddConfigurationRegistrations(configuration)
				.ProcessServiceRegistrars()
				;

			var serviceProvider = services.BuildServiceProvider<ISI.Extensions.DependencyInjection.Iunq.ServiceProviderBuilder>(configuration);

			serviceProvider.SetServiceLocator();
		}

		[Test]
		public void Nuspec_Test()
		{
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			var nuspec = nugetApi.GenerateNuspecFromProject(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GenerateNuspecFromProjectRequest()
			{
				ProjectFullName = @"F:\ISI\ISI.FrameWork\src\ISI.Wrappers\ISI.Wrappers.MassTransit\ISI.Wrappers.MassTransit.csproj",
				TryGetPackageVersion = (string package, out string version) =>
					 {
						 if (package.StartsWith("ISI.Extensions", StringComparison.InvariantCultureIgnoreCase))
						 {
							 version = "10.0.*";
							 return true;
						 }

						 if (package.StartsWith("ISI.Libraries", StringComparison.InvariantCultureIgnoreCase))
						 {
							 version = "10.0.*";
							 return true;
						 }

						 version = string.Empty;
						 return false;
					 }
			}).Nuspec;

			nuspec.Version = "2.0.0.0";
			//nuspec.IconUri = new Uri(@"https://github.com/ISI-Extensions/ISI.Extensions/Lantern.png");
			//nuspec.ProjectUri = new Uri(@"https://github.com/ISI-Extensions/ISI.Extensions");
			nuspec.Title = "ISI.Libraries";
			nuspec.Description = "ISI.Libraries";
			nuspec.Copyright = string.Format("Copyright (c) {0}, Integrated Solutions, Inc.", DateTime.Now.Year);
			nuspec.Authors = new[] { "Integrated Solutions, Inc." };
			nuspec.Owners = new[] { "Integrated  Solutions, Inc." };


			var xxx = nugetApi.BuildNuspec(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.BuildNuspecRequest()
			{
				Nuspec = nuspec,
			});
		}



		[Test]
		public void GetLatestPackageVersion_Test()
		{
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			var packageVersion4 = nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "ISI.Libraries",
			}).NugetPackageKey.Version;

			var packageVersion = nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "ISI.Libraries",
			}).NugetPackageKey.Version;

			var packageVersion2 = nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "Microsoft.CSharp",
			}).NugetPackageKey.Version;

			var packageVersion3 = nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "SevenZipSharp",
			}).NugetPackageKey.Version;
		}


		private bool HasChanges(string original, string newVersion)
		{
			string getCompressed(string value) => value
				.Replace(" ", string.Empty)
				.Replace("\t", string.Empty)
				.Replace("\r", string.Empty)
				.Replace("\n", string.Empty);

			return !string.Equals(getCompressed(original), getCompressed(newVersion), StringComparison.InvariantCultureIgnoreCase);
		}

		[Test]
		public void UpdatePackageVersions_Test()
		{
			var solutionApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();

			solutionApi.UpdateNugetPackages(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.UpdateNugetPackagesRequest()
			{
				SolutionFullNames = new[]
				{
					//@"F:\ISI\ISI.FrameWork",
					//@"F:\ISI\Clients\SitePro\SitePro.Extensions",
					//@"F:\ISI\Clients\SitePro\SitePro.Services",
					//@"F:\ISI\Clients\SitePro\SitePro.Connect.ServiceApplication",
					//@"F:\ISI\Clients\SitePro\SitePro.Connect.Api.WebApplication",
					//@"F:\ISI\Internal Projects\ISI.Desktop",
					//@"F:\ISI\Internal Projects\ISI.WebApplication",
					//@"F:\ISI\ISI.FrameWork",


				@"F:\ISI\Clients\TFS\Tristar.Scheduler",
				@"F:\ISI\Clients\TFS\Comcast.XfinityRewards.2019",
				@"F:\ISI\Clients\TFS\Tristar.Processor",
				@"F:\ISI\Clients\TFS\Comcast.ReferAFriend2019",
				@"F:\ISI\Clients\TFS\Tristar.Portal",
				@"F:\ISI\Clients\TFS\Tristar.Fulfillment.Portal.WebApplication",
				@"F:\ISI\Clients\TFS\Comcast.Teachers.Portal.Web",
				@"F:\ISI\Clients\TFS\Tristar.Vault.Portal.Web",
				@"F:\ISI\Clients\TFS\Comcast.Business.Retro.Prime.Web",
				@"F:\ISI\Clients\TFS\Comcast.Military.Portal.Web",
				@"F:\ISI\Clients\TFS\Tristar.Carrier",
				@"F:\ISI\Clients\TFS\Comcast.Business.TenMinuteAdvantage.Web",
				@"F:\ISI\Clients\TFS\Comcast.XmTechSales.Web",
				//@"F:\ISI\Clients\TFS\FedEx.Services",
				//@"F:\ISI\Clients\TFS\SheerID.Services",
				@"F:\ISI\Clients\TFS\Comcast.Military.Registrations",
				@"F:\ISI\Clients\TFS\Tristar.Libraries",
				@"F:\ISI\Clients\TFS\Tristar.Worker",
				@"F:\ISI\Clients\TFS\Tristar.Identity",
				@"F:\ISI\Clients\TFS\Tristar.Journal",
				@"F:\ISI\Clients\TFS\Tristar.Telephony",
				@"F:\ISI\Clients\TFS\Tristar.WebServices.Interface",
				@"F:\ISI\Clients\TFS\Tristar.AddressTools",
				@"F:\ISI\Clients\TFS\Tristar.ImageTools",
				@"F:\ISI\Clients\TFS\Tristar.Libraries.Ssh",
				@"F:\ISI\Clients\TFS\Tristar.SecurityTools",
				@"F:\ISI\Clients\TFS\Tristar.ShippingTools",
				@"F:\ISI\Clients\TFS\Tristar.SourceManager",
				@"F:\ISI\Clients\TFS\Tristar.Marketing.Public",
				@"F:\ISI\Clients\TFS\BCS.Rewards",
				@"F:\ISI\Clients\TFS\BullpenRecruitingCenter",
				@"F:\ISI\Clients\TFS\Comcast.Military.Public.Web",
				@"F:\ISI\Clients\TFS\Comcast.BusinessRetention",
				@"F:\ISI\Clients\TFS\Comcast.DareToCompare",
				@"F:\ISI\Clients\TFS\Comcast.EnterpriseHealthcare",
				@"F:\ISI\Clients\TFS\Comcast.BusinessIncentiveWest",
				@"F:\ISI\Clients\TFS\Comcast.Hyperbuild",
				@"F:\ISI\Clients\TFS\Comcast.Ndw",
				@"F:\ISI\Clients\TFS\Comcast.Teachers.Registrations",
				@"F:\ISI\Clients\TFS\Comcast.XfinityRewards",
				@"F:\ISI\Clients\TFS\Comcast.XfinityRewards.v2",
				@"F:\ISI\Clients\TFS\SHC.EGift",
				@"F:\ISI\Clients\TFS\SpectrumEnterprise.BusinessClass",
				@"F:\ISI\Clients\TFS\SpectrumEnterprise.Referral",
				@"F:\ISI\Clients\TFS\Tristar.DocumentStorage",
				@"F:\ISI\Clients\TFS\Tristar.SecureData",
				},
				CommitWorkingCopyToSourceControl = true,
				IgnorePackageIds = new[]
				{
					"ISI.CMS.T4CMS.FileSystem",
					"ISI.CMS.T4CMS.SqlServer",
					"ISI.Libraries.T4LocalContent",
					"ISI.Libraries.T4LocalContent.Embedded",
					"ISI.Libraries.T4LocalContent.Resources",
					"ISI.Libraries.T4LocalContent.VirtualFiles",
					"ISI.Libraries.T4LocalContent.Web",
					"ISI.Libraries.T4LocalContent.WebPortableArea",
					"ISI.Extensions.T4LocalContent",
					"ISI.Extensions.T4LocalContent.Embedded",
					"ISI.Extensions.T4LocalContent.Resources",
					"ISI.Extensions.T4LocalContent.VirtualFiles",
					"ISI.Extensions.T4LocalContent.Web",
					"ISI.Extensions.T4LocalContent.WebPortableArea",
					"Microsoft.ClearScript",
					"jQuery",
					"AccumailGoldConnections.NETToolkit",
					"nsoftware.InPay",
					"nsoftware.InPtech",
					"nsoftware.InShip",
					"nsoftware.IPWorksSSH",
				}
			});
		}
	}
}
