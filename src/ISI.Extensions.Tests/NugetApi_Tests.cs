#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using ISI.Extensions.ConfigurationHelper.Extensions;
using ISI.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

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
		public void NupkgPush_To_ISI_SCM_Artifacts_WindowsService_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			var nupkgFullName = System.IO.Directory.EnumerateFiles(@"F:\ISI\Internal Projects\ISI.Extensions\Nuget", "*.nupkg").First();

			nugetApi.NupkgPush(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.NupkgPushRequest()
			{
				NupkgFullNames = new[] { nupkgFullName },
				ApiKey = "xxxx",
				RepositoryUri = new Uri("https://localhost:5001/nuget/v3/index.json"),
			});
		}

		[Test]
		public void SignNupkgs_Test()
		{
			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ISI.keyValue");
			var settings = ISI.Extensions.Scm.Settings.Load(settingsFullName, null);

			var codeSigningApi = new ISI.Extensions.VisualStudio.CodeSigningApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			codeSigningApi.SignNupkgs(new ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi.SignNupkgsRequest()
			{
				NupkgFullNames = System.IO.Directory.GetFiles(@"F:\ISI\Internal Projects\ISI.Extensions\Nuget"),
				TimeStampUri = new Uri(settings.CodeSigning.TimeStampUrl),
				CertificateFingerprint = settings.CodeSigning.CertificateFingerprint,
				//CertificatePath = File(settings.CodeSigning.CertificateFileName),
				//CertificatePassword = settings.CodeSigning.CertificatePassword,
				Verbosity = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi.CodeSigningVerbosity.Detailed,
			});
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

						 if (package.StartsWith("ISI.Extensions", StringComparison.InvariantCultureIgnoreCase))
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
			nuspec.Title = "ISI.Extensions";
			nuspec.Description = "ISI.Extensions";
			nuspec.Copyright = string.Format("Copyright (c) {0}, Integrated Solutions, Inc.", DateTime.Now.Year);
			nuspec.Authors = new[] { "Integrated Solutions, Inc." };
			nuspec.Owners = new[] { "Integrated  Solutions, Inc." };


			var xxx = nugetApi.BuildNuspec(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.BuildNuspecRequest()
			{
				Nuspec = nuspec,
			});
		}

		[Test]
		public void ListNugetPackageKeys_Test()
		{
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			var nugetPackageKeys = nugetApi.ListNugetPackageKeys(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.ListNugetPackageKeysRequest()
			{
				Source = @"F:\ISI\Internal Projects\ISI.Extensions\Nuget",
			}).NugetPackageKeys;
		}


		[Test]
		public void GetLatestPackageVersion_Test()
		{
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ISI.Extensions.TextWriterLogger(TestContext.Progress));

			var packageVersion4 = nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "ISI.Extensions",
			}).NugetPackageKey.Version;

			var packageVersion = nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "ISI.Extensions",
			}).NugetPackageKey.Version;

			//var packageVersion2 = nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			//{
			//	PackageId = "Microsoft.CSharp",
			//}).NugetPackageKey.Version;

			var packageVersion3 = nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "Aspose.Cells",
			}).NugetPackageKey;
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
			var nugetApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Nuget.NugetApi>();

			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();
			//nugetPackageKeys.TryAdd(nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			//{
			//	PackageId = "SkiaSharp",
			//	PackageVersion = "2.80.2",
			//}).NugetPackageKey);
			nugetPackageKeys.TryAdd(nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv",
				PackageVersion = "2.2.0",
			}).NugetPackageKey);
			nugetPackageKeys.TryAdd(nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "Microsoft.ClearScript",
				PackageVersion = "6.0.2",
			}).NugetPackageKey);
			nugetPackageKeys.TryAdd(nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			{
				PackageId = "Microsoft.AspNetCore.Mvc.NewtonsoftJson",
				PackageVersion = "5.0.13",
			}).NugetPackageKey);
			//nugetPackageKeys.TryAdd(nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			//{
			//	PackageId = "System.Memory",
			//	PackageVersion = "4.5.5",
			//}).NugetPackageKey);

			var upsertAssemblyRedirectsNugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();
			//upsertAssemblyRedirectsNugetPackageKeys.TryAdd(nugetApi.GetNugetPackageKey(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetPackageKeyRequest()
			//{
			//	PackageId = "System.Memory",
			//}).NugetPackageKey);

			var removeAssemblyRedirects = new List<string>();
			removeAssemblyRedirects.Add("Microsoft.Identity*");

			var solutionFullNames = new List<string>();
			//solutionFullNames.Add(@"F:\ISI\ISI.FrameWork");
			//solutionFullNames.Add(@"F:\ISI\Internal Projects\ISI.Telephony.WindowsService");
			solutionFullNames.Add(@"F:\ISI\Clients\TFS\Tristar.AddressTools");
			//solutionFullNames.AddRange(System.IO.File.ReadAllLines(@"S:\Tristar.SolutionFullNames.txt"));

			solutionApi.UpdateNugetPackages(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.UpdateNugetPackagesRequest()
			{
				SolutionFullNames = solutionFullNames,
				//UpdateWorkingCopyFromSourceControl = false,
				//CommitWorkingCopyToSourceControl = false,
				UpdateWorkingCopyFromSourceControl = true,
				CommitWorkingCopyToSourceControl = true,
				NugetPackageKeys = nugetPackageKeys,
				UpsertAssemblyRedirectsNugetPackageKeys = upsertAssemblyRedirectsNugetPackageKeys,
				RemoveAssemblyRedirects = removeAssemblyRedirects,
				IgnorePackageIds = new[]
				{
					"ISI.CMS.T4CMS",
					"ISI.CMS.T4CMS.MSSQL",
					"ISI.CMS.T4CMS.FileSystem",
					"ISI.CMS.T4CMS.SqlServer",
					"ISI.Extensions.T4LocalContent",
					"ISI.Extensions.T4LocalContent.Embedded",
					"ISI.Extensions.T4LocalContent.RazorEngine",
					"ISI.Extensions.T4LocalContent.Resources",
					"ISI.Extensions.T4LocalContent.VirtualFiles",
					"ISI.Extensions.T4LocalContent.Web",
					"ISI.Extensions.T4LocalContent.WebPortableArea",
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

		[Test]
		public void UpdateNugetConfigFiles_Test()
		{
			var logger = ISI.Extensions.ServiceLocator.Current.GetService<Microsoft.Extensions.Logging.ILogger>();
			var solutionApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();
			var sourceControlClientApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Scm.SourceControlClientApi>();

			var solutionFullNames = new List<string>();

			var sourceNugetConfigFullName = @"F:\ISI\Clients\Tristar\nuget.config";

			var solutionDetailsSet = solutionFullNames.ToNullCheckedArray(solution => solutionApi.GetSolutionDetails(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.GetSolutionDetailsRequest()
			{
				Solution = solution,
			}).SolutionDetails, ISI.Extensions.Extensions.NullCheckCollectionResult.Empty).Where(solutionDetail => solutionDetail != null).ToArray();

			foreach (var solutionDetails in solutionDetailsSet.OrderBy(solutionDetails => solutionDetails.UpdateNugetPackagesPriority).ThenBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase))
			{
				using (solutionApi.GetSolutionLock(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.GetSolutionLockRequest()
				{
					SolutionFullName = solutionDetails.SolutionFullName,
				}).Lock)
				{
					logger.Log(LogLevel.Information, solutionDetails.SolutionName);

					var dirtyFileNames = new HashSet<string>();

					if (!sourceControlClientApi.UpdateWorkingCopy(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.UpdateWorkingCopyRequest()
					{
						FullName = solutionDetails.RootSourceDirectory,
						IncludeExternals = true,
					}).Success)
					{
						var exception = new Exception(string.Format("Error updating \"{0}\"", solutionDetails.RootSourceDirectory));
						logger.LogError(exception.Message);
						throw exception;
					}

					var targetNugetConfigFullName = System.IO.Path.Combine(solutionDetails.SolutionDirectory, "nuget.config");

					System.IO.File.Copy(sourceNugetConfigFullName, targetNugetConfigFullName, true);

					dirtyFileNames.Add(targetNugetConfigFullName);

					if (dirtyFileNames.Any())
					{
						var commitLog = new StringBuilder();

						if (!sourceControlClientApi.Commit(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.CommitRequest()
						{
							FullNames = dirtyFileNames,
							LogMessage = "remove ISI from nuget.config",
							AddToLog = log => commitLog.AppendLine(log),
						}).Success)
						{
							var exception = new Exception(string.Format("Error committing \"{0}\"", solutionDetails.RootSourceDirectory));
							logger.LogError(exception.Message);
							throw exception;
						}
					}
				}
			}
		}
	}
}
