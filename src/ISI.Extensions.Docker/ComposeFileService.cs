#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Docker
{
	public class ComposeFileService
	{
		public string Name { get; set; }
		public string Image { get; set; }
		public string PullPolicy { get; set; }
		public string ContainerName { get; set; }
		public ComposeFileServiceBuild Build { get; set; }
		public string[] Command { get; set; }
		public string[] Entrypoint { get; set; }
		public IDictionary<string, string> Environment { get; set; }
		public string[] EnvFile { get; set; }
		public string WorkingDir { get; set; }
		public ComposeFileServicePort[] Ports { get; set; }
		public string[] Expose { get; set; }
		public string[] Volumes { get; set; }
		public IDictionary<string, ComposeFileServiceServiceDependency> DependsOn { get; set; }
		public string User { get; set; }
		public string[] Networks { get; set; }
		public string Restart { get; set; }
		public ComposeFileServiceDeploy Deploy { get; set; }
		public ComposeFileServiceHealthCheck HealthCheck { get; set; }
		public ComposeFileServiceLogging Logging { get; set; }
		public IDictionary<string, string> Labels { get; set; }
		public string DomainName { get; set; }
		public string Hostname { get; set; }
		public string Isolation { get; set; }
		public string Ipc { get; set; }
		public string MacAddress { get; set; }
		public string Pid { get; set; }
		public bool? Privileged { get; set; }
		public string[] CapAdd { get; set; }
		public string[] CapDrop { get; set; }
		public string CgroupParent { get; set; }
		public string[] Devices { get; set; }
		public string[] Dns { get; set; }
		public string[] DnsSearch { get; set; }
		public IDictionary<string, string> ExtraHosts { get; set; }
		public string[] GroupAdd { get; set; }
		public bool? Init { get; set; }
		public string[] Links { get; set; }
		public string[] ExternalLinks { get; set; }
		public string NetworkMode { get; set; }
		public string[] Profiles { get; set; }
		public bool? ReadOnly { get; set; }
		public string[] SecurityOpt { get; set; }
		public ComposeFileServiceSecretReference[] Secrets { get; set; }
		public ComposeFileServiceConfigReference[] Configs { get; set; }
		public string StopGracePeriod { get; set; }
		public string StopSignal { get; set; }
		public IDictionary<string, string> SysCtls { get; set; }
		public string[] Tmpfs { get; set; }
		public bool? StdinOpen { get; set; }
		public bool? Tty { get; set; }
		public IDictionary<string, ComposeFileServiceUlimit> Ulimits { get; set; }
	}
}
