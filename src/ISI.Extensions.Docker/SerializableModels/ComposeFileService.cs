using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace ISI.Extensions.Docker.SerializableModels
{
	[YamlSerializable]
	public class ComposeFileService
	{
		[YamlMember(Alias = "name", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Name { get; set; }

		[YamlMember(Alias = "image", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Image { get; set; }

		[YamlMember(Alias = "pull_policy", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string PullPolicy { get; set; }

		[YamlMember(Alias = "container_name", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string ContainerName { get; set; }

		[YamlMember(Alias = "build", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceBuild Build { get; set; }

		[YamlMember(Alias = "command", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Command { get; set; }

		[YamlMember(Alias = "entrypoint", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Entrypoint { get; set; }

		[YamlMember(Alias = "environment", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Environment { get; set; }

		[YamlMember(Alias = "env_file", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] EnvFile { get; set; }

		[YamlMember(Alias = "working_dir", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string WorkingDir { get; set; }

		[YamlMember(Alias = "ports", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public ComposeFileServicePort[] Ports { get; set; }

		[YamlMember(Alias = "expose", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Expose { get; set; }

		[YamlMember(Alias = "volumes", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Volumes { get; set; }

		[YamlMember(Alias = "depends_on", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, ComposeFileServiceServiceDependency> DependsOn { get; set; }

		[YamlMember(Alias = "user", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string User { get; set; }

		[YamlMember(Alias = "networks", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Networks { get; set; }

		[YamlMember(Alias = "restart", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Restart { get; set; }

		[YamlMember(Alias = "deploy", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceDeploy Deploy { get; set; }

		[YamlMember(Alias = "healthcheck", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceHealthCheck HealthCheck { get; set; }

		[YamlMember(Alias = "logging", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public ComposeFileServiceLogging Logging { get; set; }

		[YamlMember(Alias = "labels", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> Labels { get; set; }

		[YamlMember(Alias = "domainname", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string DomainName { get; set; }

		[YamlMember(Alias = "hostname", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Hostname { get; set; }

		[YamlMember(Alias = "isolation", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Isolation { get; set; }

		[YamlMember(Alias = "ipc", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Ipc { get; set; }

		[YamlMember(Alias = "mac_address", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string MacAddress { get; set; }

		[YamlMember(Alias = "pid", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string Pid { get; set; }

		[YamlMember(Alias = "privileged", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? Privileged { get; set; }

		[YamlMember(Alias = "cap_add", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] CapAdd { get; set; }

		[YamlMember(Alias = "cap_drop", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] CapDrop { get; set; }

		[YamlMember(Alias = "cgroup_parent", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string CgroupParent { get; set; }

		[YamlMember(Alias = "devices", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Devices { get; set; }

		[YamlMember(Alias = "dns", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Dns { get; set; }

		[YamlMember(Alias = "dns_search", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] DnsSearch { get; set; }

		[YamlMember(Alias = "extra_hosts", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> ExtraHosts { get; set; }

		[YamlMember(Alias = "group_add", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] GroupAdd { get; set; }

		[YamlMember(Alias = "init", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? Init { get; set; }

		[YamlMember(Alias = "links", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Links { get; set; }

		[YamlMember(Alias = "external_links", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] ExternalLinks { get; set; }

		[YamlMember(Alias = "network_mode", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string NetworkMode { get; set; }

		[YamlMember(Alias = "profiles", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Profiles { get; set; }

		[YamlMember(Alias = "read_only", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? ReadOnly { get; set; }

		[YamlMember(Alias = "security_opt", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] SecurityOpt { get; set; }

		[YamlMember(Alias = "secrets", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public ComposeFileServiceSecretReference[] Secrets { get; set; }

		[YamlMember(Alias = "configs", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public ComposeFileServiceConfigReference[] Configs { get; set; }

		[YamlMember(Alias = "stop_grace_period", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string StopGracePeriod { get; set; }

		[YamlMember(Alias = "stop_signal", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public string StopSignal { get; set; }

		[YamlMember(Alias = "sysctls", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, string> SysCtls { get; set; }

		[YamlMember(Alias = "tmpfs", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public string[] Tmpfs { get; set; }

		[YamlMember(Alias = "stdin_open", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? StdinOpen { get; set; }

		[YamlMember(Alias = "tty", DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
		public bool? Tty { get; set; }

		[YamlMember(Alias = "ulimits", DefaultValuesHandling = DefaultValuesHandling.OmitEmptyCollections)]
		public Dictionary<string, ComposeFileServiceUlimit> Ulimits { get; set; }
	}
}
