﻿#region Copyright & License
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
using System.Text;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class NugetHelper_Tests
	{
		[Test]
		public void Nuspec_Test()
		{
			var nugetHelper = new ISI.Extensions.Nuget.NugetHelper(new ConsoleLogger());

			var nuspec = nugetHelper.GenerateNuspecFromProject(@"F:/ISI/Clients/ICS/ICS.Libraries/src/ICS.Libraries/ICS.Libraries/ICS.Libraries.csproj", package =>
			{
				if (package.StartsWith("ISI.Extensions", StringComparison.InvariantCultureIgnoreCase))
				{
					return "10.0.*";
				}

				if (package.StartsWith("ICS.Libraries", StringComparison.InvariantCultureIgnoreCase))
				{
					return "2.0.*";
				}


				return string.Empty;
			});

			nuspec.Version = "2.0.0.0";
			//nuspec.IconUri = new Uri(@"https://github.com/ICS/ICS.Libraries/ICS.png");
			//nuspec.ProjectUri = new Uri(@"https://github.com/ICS/ICS.Libraries");
			nuspec.Title = "ICS.Libraries";
			nuspec.Description = "ICS.Libraries";
			nuspec.Copyright = string.Format("Copyright (c) {0}, Integrated Control Solutions, Inc.", DateTime.Now.Year);
			nuspec.Authors = new[] { "Integrated Control Solutions, Inc." };
			nuspec.Owners = new[] { "Integrated Control Solutions, Inc." };


			var xxx = nugetHelper.BuildNuspec(new ISI.Extensions.Nuget.DataTransferObjects.NugetHelper.BuildNuspecRequest()
			{
				Nuspec = nuspec,
			});
		}

		[Test]
		public void NupkgPush_Test()
		{
			var nugetHelper = new ISI.Extensions.Nuget.NugetHelper(new ConsoleLogger());

			nugetHelper.NupkgPush(new ISI.Extensions.Nuget.DataTransferObjects.NugetHelper.NupkgPushRequest()
			{
				NupkgFullNames = new[] { @"F:\ISI\Clients\ICS\ICS.Scripts\Nuget\ICS.Scripts.1.1.7758.4975.nupkg" },
				UseNugetPush = false,
				RepositoryUri = new Uri("https://nuget.swdcentral.com"),
				ApiKey = "32a33689-f2df-4b01-b341-917560dc6858",
			});
		}

		[Test]
		public void ParseCsProj_Test()
		{
			var nugetHelper = new ISI.Extensions.Nuget.NugetHelper(new ConsoleLogger());

			{
				var nugetPackageKeys = nugetHelper.ParseCsProj(@"F:\ISI\Internal Projects\ISI.Extensions\src\ISI.Extensions.Nuget\ISI.Extensions.Nuget.csproj");
			}

			{
				var nugetPackageKeys = nugetHelper.ParseCsProj(@"F:\ISI\Internal Projects\ISI.BuildTools\src\ISI.BuildTools.Tests\ISI.BuildTools.Tests.csproj");
			}
		}

		[Test]
		public void UpdatePackagesConfig_Test()
		{
			var nugetHelper = new ISI.Extensions.Nuget.NugetHelper(new ConsoleLogger());

			var nugetPackageKeys = new ISI.Extensions.Nuget.NugetPackageKeyDictionary();
			nugetPackageKeys.Merge(nugetHelper.ParseCsProj(@"F:\ISI\Internal Projects\ISI.Extensions\src\ISI.Extensions.Nuget\ISI.Extensions.Nuget.csproj"));
			nugetPackageKeys.Merge(nugetHelper.ParseCsProj(@"F:\ISI\Internal Projects\ISI.BuildTools\src\ISI.BuildTools.Tests\ISI.BuildTools.Tests.csproj"));

			var packagesConfig = System.IO.File.ReadAllText(@"F:\ISI\Internal Projects\ISI.BuildTools\src\ISI.BuildTools.Tests\packages.config");
			nugetHelper.UpdatePackagesConfig(packagesConfig, nugetPackageKeys);
		}
	}
}
