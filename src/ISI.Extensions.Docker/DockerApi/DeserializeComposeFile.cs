using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Docker.DataTransferObjects.DockerApi;
using SERIALIZABLEMODELS = ISI.Extensions.Docker.SerializableModels;

namespace ISI.Extensions.Docker
{
	public partial class DockerApi
	{
		public DTOs.DeserializeComposeFileResponse DeserializeComposeFile(DTOs.IDeserializeComposeFileRequest request)
		{
			var response = new DTOs.DeserializeComposeFileResponse();

			var deserializer = new YamlDotNet.Serialization.DeserializerBuilder()
				.WithTypeConverter(new SERIALIZABLEMODELS.ComposeFileServicePortTypeConverter())
				.IgnoreUnmatchedProperties() // don't throw an exception if there are unknown properties
				.Build();

			var serializedComposeFile = (string)null;

			switch (request)
			{
				case DTOs.DeserializeComposeFileRequest deserializeComposeFileRequest:
					serializedComposeFile = System.IO.File.ReadAllText(deserializeComposeFileRequest.ComposeFileFullName);
					break;

				case DTOs.DeserializeComposeFileStreamRequest deserializeComposeFileStreamRequest:
					deserializeComposeFileStreamRequest.ComposeFileStream.Rewind();
					serializedComposeFile = deserializeComposeFileStreamRequest.ComposeFileStream.ReadAsStringToEnd();
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(request));
			}

			var serializableComposeFile = deserializer.Deserialize<SERIALIZABLEMODELS.ComposeFile>(serializedComposeFile);

			foreach (var composeFileService in (serializableComposeFile.Services ?? new()).Values)
			{
				foreach (var port in composeFileService.Ports ?? [])
				{
					if (port.StartIndex.HasValue && port.EndIndex.HasValue)
					{
						port.RawValue = serializedComposeFile.Substring(port.StartIndex.GetValueOrDefault(), port.EndIndex.GetValueOrDefault() - port.StartIndex.GetValueOrDefault());
					}
				}
			}

			response.ComposeFile = serializableComposeFile.NullCheckedConvert(source => new ComposeFile()
			{
				Name = source.Name,
				Version = source.Version,
				Services = (source.Services?.Values).ToNullCheckedArray(service => new ComposeFileService()
				{
					Name = service.Name,
					Image = service.Image,
					PullPolicy = service.PullPolicy,
					ContainerName = service.ContainerName,
					Build = service.Build.NullCheckedConvert(build => new ComposeFileServiceBuild()
					{
						Context = build.Context,
						Dockerfile = build.Dockerfile,
						Args = build.Args.ToNullCheckedDictionary(arg => arg.Key, arg => arg.Value),
						Target = build.Target,
						CacheFrom = build.CacheFrom.ToNullCheckedArray(),
						Labels = build.Labels.ToNullCheckedDictionary(label => label.Key, label => label.Value),
					}),
					Command = service.Command.ToNullCheckedArray(),
					Entrypoint = service.Entrypoint.ToNullCheckedArray(),
					Environment = service.Environment.ToNullCheckedDictionary(environment => environment.Key, environment => environment.Value),
					EnvFile = service.EnvFile.ToNullCheckedArray(),
					WorkingDir = service.WorkingDir,
					Ports = service.Ports.ToNullCheckedArray(port => new ComposeFileServicePort()
					{
						Target = port.Target,
						Published = port.Published,
						Protocol = port.Protocol,
						Mode = port.Mode,
						RawValue = port.RawValue,
						IsYaml = port.IsYaml,
						StartIndex = port.StartIndex,
						EndIndex = port.EndIndex,
					}),
					Expose = service.Expose.ToNullCheckedArray(),
					Volumes = service.Volumes.ToNullCheckedArray(),
					DependsOn = service.DependsOn.ToNullCheckedDictionary(dependsOn => dependsOn.Key, dependsOn => new ComposeFileServiceServiceDependency()
					{
						Condition = dependsOn.Value.Condition,
					}),
					User = service.User,
					Networks = service.Networks.ToNullCheckedArray(),
					Restart = service.Restart,
					Deploy = service.Deploy.NullCheckedConvert(deploy => new ComposeFileServiceDeploy()
					{
						Replicas = deploy.Replicas,
						Mode = deploy.Mode,
						Resources = deploy.Resources.NullCheckedConvert(resource => new ComposeFileServiceDeployResources()
						{
							Limits = resource.Limits.NullCheckedConvert(limits => new ComposeFileServiceDeployResourcesResourceSpec()
							{
								Cpus = limits.Cpus,
								Memory = limits.Memory,
							}),
							Reservations = resource.Reservations.NullCheckedConvert(reservation => new ComposeFileServiceDeployResourcesResourceSpec()
							{
								Cpus = reservation.Cpus,
								Memory = reservation.Memory,
							}),
						}),
						Placement = deploy.Placement.NullCheckedConvert(placement => new ComposeFileServiceDeployPlacement()
						{
							Constraints = placement.Constraints.ToNullCheckedArray(),
							Preferences = placement.Preferences.ToNullCheckedArray(preference => preference.ToNullCheckedDictionary(preferenceKeyValue => preferenceKeyValue.Key, preferenceKeyValue => preferenceKeyValue.Value)),
						}),
						UpdateConfig = deploy.UpdateConfig.NullCheckedConvert(updateConfig => new ComposeFileServiceDeployUpdateConfig()
						{
							Parallelism = updateConfig.Parallelism,
							Delay = updateConfig.Delay,
							FailureAction = updateConfig.FailureAction,
							Monitor = updateConfig.Monitor,
							MaxFailureRatio = updateConfig.MaxFailureRatio,
							Order = updateConfig.Order,
						}),
						RestartPolicy = deploy.RestartPolicy.NullCheckedConvert(restartPolicy => new ComposeFileServiceDeployRestartPolicy()
						{
							Condition = restartPolicy.Condition,
							Delay = restartPolicy.Delay,
							MaxAttempts = restartPolicy.MaxAttempts,
							Window = restartPolicy.Window,
						}),
						Labels = deploy.Labels.ToNullCheckedDictionary(label => label.Key, label => label.Value),
					}),
					HealthCheck = service.HealthCheck.NullCheckedConvert(healthCheck => new ComposeFileServiceHealthCheck()
					{
						Test = healthCheck.Test.ToNullCheckedArray(),
						Interval = healthCheck.Interval,
						Timeout = healthCheck.Timeout,
						Retries = healthCheck.Retries,
						StartPeriod = healthCheck.StartPeriod,
					}),
					Logging = service.Logging.NullCheckedConvert(logging => new ComposeFileServiceLogging()
					{
						Driver = logging.Driver,
						Options = logging.Options.ToNullCheckedDictionary(option => option.Key, option => option.Value),
					}),
					Labels = service.Labels.ToNullCheckedDictionary(label => label.Key, label => label.Value),
					DomainName = service.DomainName,
					Hostname = service.Hostname,
					Isolation = service.Isolation,
					Ipc = service.Ipc,
					MacAddress = service.MacAddress,
					Pid = service.Pid,
					Privileged = service.Privileged,
					CapAdd = service.CapAdd.ToNullCheckedArray(),
					CapDrop = service.CapDrop.ToNullCheckedArray(),
					CgroupParent = service.CgroupParent,
					Devices = service.Devices.ToNullCheckedArray(),
					Dns = service.Dns.ToNullCheckedArray(),
					DnsSearch = service.DnsSearch.ToNullCheckedArray(),
					ExtraHosts = service.ExtraHosts.ToNullCheckedDictionary(extraHost => extraHost.Key, extraHost => extraHost.Value),
					GroupAdd = service.GroupAdd.ToNullCheckedArray(),
					Init = service.Init,
					Links = service.Links.ToNullCheckedArray(),
					ExternalLinks = service.ExternalLinks.ToNullCheckedArray(),
					NetworkMode = service.NetworkMode,
					Profiles = service.Profiles.ToNullCheckedArray(),
					ReadOnly = service.ReadOnly,
					SecurityOpt = service.SecurityOpt.ToNullCheckedArray(),
					Secrets = service.Secrets.ToNullCheckedArray(secret => new ComposeFileServiceSecretReference()
					{
						Source = secret.Source,
						Target = secret.Target,
						Uid = secret.Uid,
						Gid = secret.Gid,
						Mode = secret.Mode,
					}),
					Configs = service.Configs.ToNullCheckedArray(config => new ComposeFileServiceConfigReference()
					{
						Source = config.Source,
						Target = config.Target,
						Uid = config.Uid,
						Gid = config.Gid,
						UnixFileMode = config.UnixFileMode,
					}),
					StopGracePeriod = service.StopGracePeriod,
					StopSignal = service.StopSignal,
					SysCtls = service.SysCtls.ToNullCheckedDictionary(sysCtls => sysCtls.Key, sysCtls => sysCtls.Value),
					Tmpfs = service.Tmpfs.ToNullCheckedArray(),
					StdinOpen = service.StdinOpen,
					Tty = service.Tty,
					Ulimits = service.Ulimits.ToNullCheckedDictionary(ulimits => ulimits.Key, ulimits => new ComposeFileServiceUlimit()
					{
						Soft = ulimits.Value.Soft,
						Hard = ulimits.Value.Hard,
					}),
				}),
				Networks = (source.Networks?.Values).ToNullCheckedArray(network => new ComposeFileNetwork()
				{
					Name = network.Name,
					Driver = network.Driver,
					DriverOpts = network.DriverOpts.ToNullCheckedDictionary(driverOpts => driverOpts.Key, driverOpts => driverOpts.Value),
					External = network.External,
					Labels = network.Labels.ToNullCheckedDictionary(label => label.Key, label => label.Value),
					Ipam = network.Ipam.NullCheckedConvert(ipam => new ComposeFileNetworkIpam()
					{
						Driver = ipam.Driver,
						Config = ipam.Config.ToNullCheckedArray(config => config.ToNullCheckedDictionary(configKeyValue => configKeyValue.Key, configKeyValue => configKeyValue.Value)),
						Options = ipam.Options.ToNullCheckedDictionary(option => option.Key, option => option.Value),
					}),
					Attachable = network.Attachable,
					Ingress = network.Ingress,
					Internal = network.Internal,
				}),
				Volumes = (source.Volumes?.Values).ToNullCheckedArray(volume => new ComposeFileVolume()
				{
					Name = volume.Name,
					Type = volume.Type,
					Target = volume.Target,
					Source = volume.Source,
					ReadOnly = volume.ReadOnly,
					Driver = volume.Driver,
					DriverOpts = volume.DriverOpts.ToNullCheckedDictionary(driverOpts => driverOpts.Key, driverOpts => driverOpts.Value),
					External = volume.External,
					Labels = volume.Labels.ToNullCheckedDictionary(label => label.Key, label => label.Value),
				}),
				Secrets = (source.Secrets?.Values).ToNullCheckedArray(secret => new ComposeFileSecret()
				{
					File = secret.File,
					External = secret.External,
					Name = secret.Name,
					Labels = secret.Labels.ToNullCheckedDictionary(label => label.Key, label => label.Value),
				}),
				Configs = (source.Configs?.Values).ToNullCheckedArray(config => new ComposeFileConfig()
				{
					Name = config.Name,
					File = config.File,
					External = config.External,
					Content = config.Content,
					Labels = config.Labels.ToNullCheckedDictionary(label => label.Key, label => label.Value),
				}),
				Extensions = source.Extensions.ToNullCheckedDictionary(extension => extension.Key, extension => extension.Value),
			});

			return response;
		}
	}
}