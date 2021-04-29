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
using System.Threading.Tasks;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class DeploymentManagerApi_Tests
	{
		[Test]
		public void DeployArtifact_Test()
		{
			var artifactName = "ICS.ReportingBodies.TxRRC.WindowsService";

			var settingsFullName = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("LocalAppData"), "Secrets", "ICS.keyValue");
			var settings = new ISI.Extensions.SimpleKeyValueStorage(settingsFullName);

			var scmApi = new ISI.Extensions.Scm.ScmApi(new ConsoleLogger());

			var authenticationToken = scmApi.GetAuthenticationToken(new ISI.Extensions.Scm.DataTransferObjects.ScmApi.GetAuthenticationTokenRequest()
			{
				ScmManagementUrl = settings.GetValue("ScmWebServiceUrl"),
				UserName = settings.GetValue("ActiveDirectoryUserName"),
				Password = settings.GetValue("ActiveDirectoryPassword"),
			}).AuthenticationToken;

			var buildArtifactApi = new ISI.Extensions.Scm.BuildArtifactApi(new ConsoleLogger());

			var dateTimeStampVersion = buildArtifactApi.GetBuildArtifactEnvironmentDateTimeStampVersion(new ISI.Extensions.Scm.DataTransferObjects.BuildArtifactApi.GetBuildArtifactEnvironmentDateTimeStampVersionRequest()
			{
				BuildArtifactManagementUrl = settings.GetValue("ScmWebServiceUrl"),
				AuthenticationToken = authenticationToken,
				ArtifactName = artifactName,
				Environment = "QA",
			}).DateTimeStampVersion;

			var deploymentManagerApi = new ISI.Extensions.Scm.DeploymentManagerApi(new ConsoleLogger());

			deploymentManagerApi.DeployArtifact(new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployArtifactRequest()
			{
				ServicesManagerUrl = settings.GetValue("UAT-AppServer-DeployManager-Url"),
				Password = settings.GetValue("UAT-AppServer-DeployManager-Password"),

				AuthenticationToken = authenticationToken,

				BuildArtifactManagementUrl = settings.GetValue("ScmWebServiceUrl").Replace("-ssh", string.Empty).Replace(":5100", string.Empty),
				ArtifactName = artifactName,
				ToDateTimeStamp = dateTimeStampVersion.Value,
				ToEnvironment = "UAT",
				ConfigurationKey = "UAT",
				Components = new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.IDeployComponent[]
				{
					new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentConsoleApplication()
					{
						PackageFolder = "ICS/ICS.ReportingBodies.TxRRC.MigrationTool",
						DeployToSubfolder = "ICS.ReportingBodies.TxRRC.MigrationTool",
						ConsoleApplicationExe = "ICS.ReportingBodies.TxRRC.MigrationTool.exe",
						ExecuteConsoleApplicationAfterInstall = true,
						ExecuteConsoleApplicationAfterInstallArguments = "-noWaitAtFinish",
					},
					new ISI.Extensions.Scm.DataTransferObjects.DeploymentManagerApi.DeployComponentWindowsService()
					{
						PackageFolder = "ICS/ICS.ReportingBodies.TxRRC.WindowsService",
						DeployToSubfolder = "ICS.ReportingBodies.TxRRC.WindowsService",
						WindowsServiceExe = "ICS.ReportingBodies.TxRRC.WindowsService.exe",
					},
				},
			});
		}
	}
}
