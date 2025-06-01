#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Xml.XPath;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi;

namespace ISI.Extensions.Jenkins
{
	public partial class JenkinsApi
	{
		public DTOs.GetWorkspaceDetailsResponse GetWorkspaceDetails(DTOs.GetWorkspaceDetailsRequest request)
		{
			var response = new DTOs.GetWorkspaceDetailsResponse();

			response.JobConfigXml = GetJobConfigXml(new()
			{
				JenkinsUrl = request.JenkinsUrl,
				UserName = request.UserName,
				ApiToken = request.ApiToken,
				SslProtocols = request.SslProtocols,
				JobId = request.JobId,
			})?.ConfigXml ?? string.Empty;

			if (!string.IsNullOrWhiteSpace(response.JobConfigXml))
			{
				var jobConfigElement = System.Xml.Linq.XElement.Parse(response.JobConfigXml.Replace("xml version=\"1.1\"", "xml version=\"1.0\"").Replace("xml version='1.1'", "xml version='1.0'"));

				response.WorkspaceDirectory = jobConfigElement.GetElementByLocalName("customWorkspace")?.Value ?? string.Empty;

				var scmElement = jobConfigElement.GetElementByLocalName("scm");
				if (scmElement != null)
				{
					var plugin = (scmElement.GetAttributeByLocalName("plugin")?.Value ?? string.Empty).Split(new[] { '@' }).FirstOrDefault() ?? string.Empty;

					if (string.Equals(plugin, "git", StringComparison.InvariantCultureIgnoreCase))
					{
						var userRemoteConfigsElement = scmElement.GetElementByLocalName("userRemoteConfigs");
						if (userRemoteConfigsElement != null)
						{
							var hudsonPluginsGitUserRemoteConfigElement = userRemoteConfigsElement.GetElementByLocalName("hudson.plugins.git.UserRemoteConfig");
							if (hudsonPluginsGitUserRemoteConfigElement != null)
							{
								response.SourceControlUrl = hudsonPluginsGitUserRemoteConfigElement.GetElementByLocalName("url")?.Value ?? string.Empty;
								if (!string.IsNullOrWhiteSpace(response.SourceControlUrl))
								{
									response.SourceControlTypeUuid = ISI.Extensions.Git.GitApi.SourceControlTypeUuid.ToGuid();
								}
							}
						}
					}
					else if (string.Equals(plugin, "subversion", StringComparison.InvariantCultureIgnoreCase))
					{
						var locationsElement = scmElement.GetElementByLocalName("locations");
						if (locationsElement != null)
						{
							var hudsonScmSubversionSCMModuleLocationElement = locationsElement.GetElementByLocalName("hudson.scm.SubversionSCM_-ModuleLocation");
							if (hudsonScmSubversionSCMModuleLocationElement != null)
							{
								response.SourceControlUrl = hudsonScmSubversionSCMModuleLocationElement.GetElementByLocalName("remote")?.Value ?? string.Empty;
								if (!string.IsNullOrWhiteSpace(response.SourceControlUrl))
								{
									response.SourceControlTypeUuid = ISI.Extensions.Git.GitApi.SourceControlTypeUuid.ToGuid();
								}
							}
						}
					}
				}
			}

			response.WorkspaceDirectory = response.WorkspaceDirectory.Replace("${JOB_NAME}", request.JobId);

			return response;
		}
	}
}