#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Jenkins.DataTransferObjects.JenkinsApi;

namespace ISI.Extensions.Jenkins
{
	public partial class JenkinsApi
	{
		private class UrlPathFormat
		{
			public static readonly string GetCSRF = @"/crumbIssuer/api/xml?xpath=concat(//crumbRequestField,"":"",//crumb)";
			public static readonly string GetServiceConfiguration = "/configuration-as-code/export";
			public static readonly string GetNodeJson = "/api/json";
			public static readonly string GetNodeXml = "/api/xml";
			public static readonly string GetJobConfigXml = "/job/{jobId}/config.xml";
			public static readonly string GetJobStatusXml = "/job/{jobId}/api/xml";
			public static readonly string EnableJob = "/job/{jobId}/enable";
			public static readonly string DisableJob = "/job/{jobId}/disable";
			public static readonly string DeleteJob = "/job/{jobId}/doDelete";
			public static readonly string CreateJobConfigXml = "/createItem?name={jobId}"; //"&mode=job";
			public static readonly string GetJobDataJson = "/job/{jobId}/api/json";
			public static readonly string GetBuildDataJson = "/job/{jobId}/{build}/api/json";
			public static readonly string GetQueuedItemJson = "/queue/item/{0}/api/json";

			public static readonly string PostBuild = "/job/{jobId}/build?jenkins_status=1&jenkins_sleep=3";
			public static readonly string PostBuildWithParameters = "/job/{jobId}/buildWithParameters?jenkins_status=1&jenkins_sleep=3";

			public static readonly string QuietDown = "/quietDown";
			public static readonly string CancelQuietDown = "/cancelQuietDown";
			
			public static readonly string Restart = "/restart";
			public static readonly string SafeRestart = "/safeRestart";
			public static readonly string Exit = "/exit";
			public static readonly string SafeExit = "/safeExit";
		}
	}
}