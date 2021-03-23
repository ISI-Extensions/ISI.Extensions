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

			var nuspec = nugetHelper.GenerateNuspecFromProject(@"F:\ISI\Internal Projects\ISI.Extensions\src\ISI.Extensions.Nuget\ISI.Extensions.Nuget.csproj", package =>
			{
				if (package.StartsWith("ISI.Extensions", StringComparison.InvariantCultureIgnoreCase))
				{
					return "10.0.*";
				}

				return string.Empty;
			});

			var xxx = nugetHelper.BuildNuspec(new ISI.Extensions.Nuget.DataTransferObjects.NugetHelper.BuildNuspecRequest()
			{
				Nuspec = nuspec,
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
