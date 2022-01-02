#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.ConfigurationHelper.Extensions
{
	public static partial class ConfigurationBuilderExtensions
	{
		public static ActiveEnvironmentConfig GetActiveEnvironmentConfig(this Microsoft.Extensions.Configuration.IConfigurationBuilder configurationBuilder, string environmentConfigPath = "")
		{
			var machineEnvironmentFileName = "MachineEnvironment.config";
			var machineEnvironmentVariableName = (string)null;

			EnvironmentsConfiguration environmentsConfiguration = null;
			EnvironmentsConfiguration.EnvironmentConfiguration environmentConfiguration = null;

			var machineName = System.Environment.MachineName;

			var environment = machineName;
			var environments = new List<string>();

			var fileProvider = configurationBuilder.GetFileProvider();

			if (!string.IsNullOrWhiteSpace(environmentConfigPath))
			{
				var machineEnvironmentConfigFileInfo = fileProvider.GetFileInfo(environmentConfigPath);

				if (machineEnvironmentConfigFileInfo.Exists)
				{
					using (var stream = machineEnvironmentConfigFileInfo.CreateReadStream())
					{
						environmentsConfiguration = System.Text.Json.JsonSerializer.Deserialize<EnvironmentsConfiguration>(stream.TextReadToEnd(), new System.Text.Json.JsonSerializerOptions()
						{
							PropertyNameCaseInsensitive = true,
							ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
							AllowTrailingCommas = true,
						});

						if (environmentsConfiguration != null)
						{
							if (!string.IsNullOrWhiteSpace(environmentsConfiguration.DefaultEnvironmentName))
							{
								environment = environmentsConfiguration.DefaultEnvironmentName;
							}
							if (!string.IsNullOrWhiteSpace(environmentsConfiguration.MachineEnvironmentFileName))
							{
								machineEnvironmentFileName = environmentsConfiguration.MachineEnvironmentFileName;
							}
							if (!string.IsNullOrWhiteSpace(environmentsConfiguration.MachineEnvironmentVariableName))
							{
								machineEnvironmentVariableName = environmentsConfiguration.MachineEnvironmentVariableName;
							}

							environmentConfiguration = environmentsConfiguration.Environments.NullCheckedFirstOrDefault(e => e.Machines.NullCheckedContains(machineName, StringComparer.InvariantCultureIgnoreCase));

							if ((environmentConfiguration == null) && !string.IsNullOrWhiteSpace(machineEnvironmentVariableName))
							{
								var machineEnvironmentVariableValue = System.Environment.GetEnvironmentVariable(machineEnvironmentVariableName);

								if (!string.IsNullOrWhiteSpace(machineEnvironmentVariableValue))
								{
									environmentConfiguration = environmentsConfiguration.Environments.NullCheckedFirstOrDefault(e => e.Machines.NullCheckedContains(machineEnvironmentVariableValue, StringComparer.InvariantCultureIgnoreCase));

									if (environmentConfiguration == null)
									{
										environmentConfiguration = environmentsConfiguration.Environments.NullCheckedFirstOrDefault(e => string.Equals(e.EnvironmentName, machineEnvironmentVariableValue, StringComparison.InvariantCultureIgnoreCase));
									}
								}
							}
						}
					}
				}
			}

			bool hasMachineEnvironmentConfig = false;
			if (!string.IsNullOrWhiteSpace(machineEnvironmentFileName))
			{
				var machineEnvironmentConfigFileInfo = fileProvider.GetFileInfo(machineEnvironmentFileName);

				if (machineEnvironmentConfigFileInfo.Exists)
				{
					var machineEnvironmentConfig = machineEnvironmentConfigFileInfo.CreateReadStream().TextReadToEnd();

					if (!string.IsNullOrWhiteSpace(machineEnvironmentConfig))
					{
						environment = machineEnvironmentConfig;
						hasMachineEnvironmentConfig = true;
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(machineEnvironmentVariableName) && !hasMachineEnvironmentConfig && (environmentConfiguration == null))
			{
				var machineEnvironmentVariableValue = System.Environment.GetEnvironmentVariable(machineEnvironmentVariableName);

				if (!string.IsNullOrWhiteSpace(machineEnvironmentVariableValue))
				{
					environment = machineEnvironmentVariableValue;
				}
			}

			environments.Add(environment);

			if (environmentsConfiguration != null)
			{
				if (environmentConfiguration != null)
				{
					environment = environmentConfiguration.EnvironmentName;
					environments.Add(environment);
				}

				var parentEnvironment = environment;
				while (!string.IsNullOrWhiteSpace(parentEnvironment))
				{
					var parentEnvironmentConfiguration = environmentsConfiguration.Environments.NullCheckedFirstOrDefault(e => string.Equals(parentEnvironment, e.EnvironmentName, StringComparison.InvariantCultureIgnoreCase));

					if (parentEnvironmentConfiguration == null)
					{
						parentEnvironment = string.Empty;
					}
					else
					{
						parentEnvironment = parentEnvironmentConfiguration.InheritsFromEnvironmentName;

						if (!string.IsNullOrWhiteSpace(parentEnvironment))
						{
							environments.Add(parentEnvironment);
						}
					}
				}
			}

			return new ActiveEnvironmentConfig(environment, environments.ToArray());
		}
	}
}