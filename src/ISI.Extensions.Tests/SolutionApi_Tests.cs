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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class SolutionApi_Tests
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
		public void Replacements_Test()
		{
			var replacements = new Dictionary<string, string>();
			//replacements.Add("./ICS.ico", "./SitePro.ico");
			//replacements.Add("rabbitmq://swdc-rabbitmq01.swdcentral.com", "rabbitmq://swdc-rabbitmq01.swdcentral.com")

			var findContents = new List<string>();
			//findContents.Add("ics.emails-beta@sitepro.com");

			var ignoreFindContents = new List<string>();
			//ignoreFindContents.Add("rabbitmq://swdc-rabbitmq01.swdcentral.com");
			//ignoreFindContents.Add("swdc-sql01.swdcentral.com");

			var logger = ISI.Extensions.ServiceLocator.Current.GetService<Microsoft.Extensions.Logging.ILogger>();
			var solutionApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();
			var sourceControlClientApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Scm.SourceControlClientApi>();

			var solutionFullNames = new List<string>();
			//solutionFullNames.Add(@"F:\ISI\ISI.FrameWork");
			solutionFullNames.Add(@"F:\ISI\Internal Projects\ISI.WebApplication");
			//solutionFullNames.AddRange(System.IO.File.ReadAllLines(@"S:\Central.SolutionFullNames.txt"));
			//solutionFullNames.AddRange(System.IO.File.ReadAllLines(@"S:\Connect.SolutionFullNames.txt"));

			var solutionDetailsSet = solutionFullNames.ToNullCheckedArray(solution => solutionApi.GetSolutionDetails(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.GetSolutionDetailsRequest()
			{
				Solution = solution,
			}).SolutionDetails, NullCheckCollectionResult.Empty).Where(solutionDetail => solutionDetail != null).ToArray();

			foreach (var solutionDetails in solutionDetailsSet.OrderBy(solutionDetails => solutionDetails.UpdateNugetPackagesPriority).ThenBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase))
			{
				using (solutionApi.GetSolutionLock(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.GetSolutionLockRequest()
				{
					SolutionFullName = solutionDetails.SolutionFullName,
					//AddToLog = addToLog,
				}).Lock)
				{
					logger.Log(LogLevel.Information, solutionDetails.SolutionName);

					var sourceFullNames = new List<string>();
					//sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "*.cs", System.IO.SearchOption.AllDirectories));
					//sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "*.cshtml", System.IO.SearchOption.AllDirectories));
					//sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "*.config", System.IO.SearchOption.AllDirectories));
					sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "*.csproj", System.IO.SearchOption.AllDirectories));
					//sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "build.cake", System.IO.SearchOption.AllDirectories));

					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\bin\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\obj\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\.svn\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\.git\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\.vs\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\_ReSharper.Caches\\", StringComparison.InvariantCultureIgnoreCase) >= 0);

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

					foreach (var sourceFullName in sourceFullNames)
					{
						var content = System.IO.File.ReadAllText(sourceFullName);
						var updatedContent = content;

						foreach (var findContent in findContents)
						{
							if ((content.IndexOf(findContent, StringComparison.CurrentCultureIgnoreCase) >= 0) && !ignoreFindContents.Any(ignoreFindContent => content.IndexOf(ignoreFindContent, StringComparison.CurrentCultureIgnoreCase) >= 0))
							{
								System.Diagnostics.Debugger.Break();
							}
						}

						foreach (var replacement in replacements)
						{
							updatedContent = updatedContent.Replace(replacement.Key, replacement.Value, StringComparison.InvariantCultureIgnoreCase);
						}

						//if (updatedContent.IndexOf("System.Diagnostics.EventTypeFilter", StringComparison.InvariantCultureIgnoreCase) >= 0)
						//{
						//	var lines = updatedContent.Split(new[] { '\r', '\n' });

						//	var line = lines.FirstOrDefault(line => line.IndexOf("System.Diagnostics.EventTypeFilter", StringComparison.InvariantCultureIgnoreCase) >= 0);

						//	if (!string.IsNullOrEmpty(line))
						//	{
						//		if (line.IndexOf("<!--", StringComparison.InvariantCultureIgnoreCase) < 0)
						//		{
						//			line = line.Trim();

						//			updatedContent = updatedContent.Replace(line, string.Format("<!--{0}-->", line));
						//		}
						//	}
						//}

						if ((updatedContent.IndexOf("<LangVersion>latest</LangVersion>", StringComparison.InvariantCultureIgnoreCase) >= 0) && 
						    (updatedContent.IndexOf("<RuntimeIdentifiers>win</RuntimeIdentifiers>", StringComparison.InvariantCultureIgnoreCase) <= 0))
						{
							updatedContent = updatedContent.Replace("<LangVersion>latest</LangVersion>", "<LangVersion>latest</LangVersion>\r\n\t\t<RuntimeIdentifiers>win</RuntimeIdentifiers>");
						}

						if (HasChanges(content, updatedContent))
						{
							System.IO.File.WriteAllText(sourceFullName, updatedContent);
							dirtyFileNames.Add(sourceFullName);
						}
					}

					if (dirtyFileNames.Any())
					{
						var commitLog = new StringBuilder();

						if (!sourceControlClientApi.Commit(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.CommitRequest()
						{
							FullNames = dirtyFileNames,
							LogMessage = "add <RuntimeIdentifiers>win</RuntimeIdentifiers>",
							AddToLog = log => commitLog.AppendLine(log),
						}).Success)
						{
							var exception = new Exception(string.Format("Error commiting \"{0}\"", solutionDetails.RootSourceDirectory));
							logger.LogError(exception.Message);
							throw exception;
						}
					}
				}
			}
		}

		[Test]
		public void ReplaceFiles_Test()
		{
			var replacements = new List<(string FileName, string ReplacementFullName)>();
			replacements.Add((FileName: "favicon.ico", ReplacementFullName: @"F:\ISI\Clients\ICS\SitePro.ico"));
			replacements.Add((FileName: "SitePro.ico", ReplacementFullName: @"F:\ISI\Clients\ICS\SitePro.ico"));

			var logger = ISI.Extensions.ServiceLocator.Current.GetService<Microsoft.Extensions.Logging.ILogger>();
			var solutionApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();
			var sourceControlClientApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Scm.SourceControlClientApi>();

			var solutionFullNames = System.IO.File.ReadAllLines(@"S:\Central.SolutionFullNames.txt");

			var solutionDetailsSet = solutionFullNames.ToNullCheckedArray(solution => solutionApi.GetSolutionDetails(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.GetSolutionDetailsRequest()
			{
				Solution = solution,
			}).SolutionDetails, NullCheckCollectionResult.Empty).Where(solutionDetail => solutionDetail != null).ToArray();

			foreach (var solutionDetails in solutionDetailsSet.OrderBy(solutionDetails => solutionDetails.UpdateNugetPackagesPriority).ThenBy(solutionDetails => solutionDetails.SolutionName, StringComparer.InvariantCultureIgnoreCase))
			{
				using (solutionApi.GetSolutionLock(new ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi.GetSolutionLockRequest()
				{
					SolutionFullName = solutionDetails.SolutionFullName,
					//AddToLog = addToLog,
				}).Lock)
				{
					logger.Log(LogLevel.Information, solutionDetails.SolutionName);

					var sourceFullNames = new List<string>();
					foreach (var replacement in replacements)
					{
						sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, replacement.FileName, System.IO.SearchOption.AllDirectories));
					}
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\src\\Publish\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\bin\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\obj\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\.svn\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\.git\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\.vs\\", StringComparison.InvariantCultureIgnoreCase) >= 0);
					sourceFullNames.RemoveAll(sourceFullName => sourceFullName.IndexOf("\\_ReSharper.Caches\\", StringComparison.InvariantCultureIgnoreCase) >= 0);

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

					foreach (var sourceFullName in sourceFullNames)
					{
						foreach (var replacement in replacements)
						{
							if (string.Equals(System.IO.Path.GetFileName(sourceFullName), replacement.FileName, StringComparison.InvariantCultureIgnoreCase))
							{
								System.IO.File.Copy(replacement.ReplacementFullName, sourceFullName, true);
								dirtyFileNames.Add(sourceFullName);
							}
						}
					}

					if (dirtyFileNames.Any())
					{
						var commitLog = new StringBuilder();

						if (!sourceControlClientApi.Commit(new ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi.CommitRequest()
						{
							FullNames = dirtyFileNames,
							LogMessage = "change icon file",
							AddToLog = log => commitLog.AppendLine(log),
						}).Success)
						{
							if (commitLog.ToString().IndexOf("Your branch is up to date with 'origin/main'.", StringComparison.InvariantCultureIgnoreCase) < 0)
							{
								var exception = new Exception(string.Format("Error commiting \"{0}\"", solutionDetails.RootSourceDirectory));
								logger.LogError(exception.Message);
								throw exception;
							}
						}
					}
				}
			}
		}
	}
}
