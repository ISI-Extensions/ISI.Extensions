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
	public class NugetApi_Tests
	{
		[Test]
		public void Nuspec_Test()
		{
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ConsoleLogger());

			var nuspec = nugetApi.GenerateNuspecFromProject(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GenerateNuspecFromProjectRequest()
			{
				ProjectFullName = @"F:\ISI\ISI.FrameWork\src\ISI.Wrappers\ISI.Wrappers.MassTransit\ISI.Wrappers.MassTransit.csproj",
				GetPackageVersion = package =>
					{
						if (package.StartsWith("ISI.Extensions", StringComparison.InvariantCultureIgnoreCase))
						{
							return "10.0.*";
						}

						if (package.StartsWith("ISI.Libraries", StringComparison.InvariantCultureIgnoreCase))
						{
							return "10.0.*";
						}

						return string.Empty;
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
			var nugetApi = new ISI.Extensions.Nuget.NugetApi(new ConsoleLogger());

			var packageNugetServers = new Dictionary<string, string>();
			packageNugetServers.Add("ISI.*", "https://nuget.isi-net.com");

			var mainNugetPackageForConsideration = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
			mainNugetPackageForConsideration.Add("JQuery");

			var xxx = nugetApi.GetLatestPackageVersion(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetLatestPackageVersionRequest()
			{
				PackageId = "ISI.Libraries",
				PackageNugetServers = packageNugetServers,
				MainNugetPackageForConsideration = mainNugetPackageForConsideration
			});
			var yyy = nugetApi.GetLatestPackageVersion(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetLatestPackageVersionRequest()
			{
				PackageId = "JQuery",
				PackageNugetServers = packageNugetServers,
				MainNugetPackageForConsideration = mainNugetPackageForConsideration
			});
		}
	}
}
