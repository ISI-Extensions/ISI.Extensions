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
using Microsoft.Extensions.DependencyInjection;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Jenkins
{
	public partial class JenkinsSettings
	{
		private static ISI.Extensions.Serialization.ISerialization _serialization = null;
		protected static ISI.Extensions.Serialization.ISerialization Serialization => _serialization ??= ISI.Extensions.ServiceLocator.Current.GetService<ISI.Extensions.Serialization.ISerialization>();

		protected string SettingsFileName { get; }

		public JenkinsSettings()
		{
			var configurationDirectory = System.IO.Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "ISI.Extensions");

			System.IO.Directory.CreateDirectory(configurationDirectory);

			SettingsFileName = System.IO.Path.Combine(configurationDirectory, "jenkins.settings.json");
		}

		private ISI.Extensions.Jenkins.JenkinsServer Convert(ISI.Extensions.Jenkins.SerializableEntities.JenkinsSettingsJenkinsServer jenkinsServer)
		{
			if (jenkinsServer == null)
			{
				return null;
			}

			return new ISI.Extensions.Jenkins.JenkinsServer()
			{
				JenkinsServerUuid = jenkinsServer.JenkinsServerUuid,
				Description = jenkinsServer.Description,
				JenkinsUrl = jenkinsServer.JenkinsUrl,
				UserName = jenkinsServer.UserName,
				ApiToken = jenkinsServer.ApiToken,
				Directories = jenkinsServer.Directories ?? Array.Empty<string>(),
			};
		}

		private ISI.Extensions.Jenkins.SerializableEntities.JenkinsSettingsJenkinsServer Convert(ISI.Extensions.Jenkins.JenkinsServer jenkinsServer)
		{
			if (jenkinsServer == null)
			{
				return null;
			}

			return new ISI.Extensions.Jenkins.SerializableEntities.JenkinsSettingsJenkinsServer()
			{
				JenkinsServerUuid = jenkinsServer.JenkinsServerUuid,
				Description = jenkinsServer.Description,
				JenkinsUrl = jenkinsServer.JenkinsUrl,
				UserName = jenkinsServer.UserName,
				ApiToken = jenkinsServer.ApiToken,
				Directories = jenkinsServer.Directories ?? Array.Empty<string>(),
			};
		}
	}
}