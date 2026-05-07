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
