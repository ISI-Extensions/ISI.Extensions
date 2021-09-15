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

			var logger = ISI.Extensions.ServiceLocator.Current.GetService<Microsoft.Extensions.Logging.ILogger>();
			var solutionApi = ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.VisualStudio.SolutionApi>();

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
					sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "*.cs", System.IO.SearchOption.AllDirectories));
					sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "*.config", System.IO.SearchOption.AllDirectories));
					sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "*.csproj", System.IO.SearchOption.AllDirectories));
					sourceFullNames.AddRange(System.IO.Directory.GetFiles(solutionDetails.SolutionDirectory, "build.cake", System.IO.SearchOption.AllDirectories));

					foreach (var sourceFullName in sourceFullNames)
					{
						var content = System.IO.File.ReadAllText(sourceFullName);
						var updatedContent = content;

						foreach (var replacement in replacements)
						{
							updatedContent = updatedContent.Replace(replacement.Key, replacement.Value, StringComparison.InvariantCultureIgnoreCase);
						}

						if (HasChanges(content, updatedContent))
						{
							System.IO.File.WriteAllText(sourceFullName, updatedContent);
						}
					}
				}
			}
		}
	}
}
