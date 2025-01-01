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
using System.Reflection;
using System.Text;
using ISI.Extensions.Extensions;
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.ConfigurationHelper
{
	public class EnvironmentConfigurationProvider : Microsoft.Extensions.Configuration.ConfigurationProvider
	{
		private bool _loaded = false;
		private bool _showConfig { get; }

		public EnvironmentConfigurationProvider(bool showConfig)
		{
			_showConfig = showConfig;
		}

		private readonly Dictionary<string, string> _knownEnvironmentMaps = new()
		{
			{ "KESTREL_ENDPOINTS_HTTP_URL", "Kestrel:Endpoints:Http:Url"},
		};

		public override void Load()
		{
			if (!_loaded)
			{
				var environmentVariables = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

				foreach (var key in System.Environment.GetEnvironmentVariables().Keys)
				{
					environmentVariables.Add($"{key}", $"{System.Environment.GetEnvironmentVariable($"{key}")}");
				}

				var configurationTypes = ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementationTypes<ISI.Extensions.ConfigurationHelper.IConfiguration>();

				foreach (var configurationType in configurationTypes)
				{
					if (configurationType.GetCustomAttribute(typeof(ISI.Extensions.ConfigurationHelper.ConfigurationAttribute)) is ConfigurationAttribute configurationAttribute)
					{
						if (_showConfig)
						{
							System.Console.WriteLine($"  EV-SECTION ConfigurationSectionName => \"{configurationAttribute.ConfigurationSectionName}\"");
						}

						AddData(environmentVariables, configurationType, string.Format("{0}:", configurationAttribute.ConfigurationSectionName));
					}
				}

				const string connectionStringPrefix = "CONNECTION_STRING_";
				foreach (var keyValue in environmentVariables.Where(keyValue => keyValue.Key.StartsWith(connectionStringPrefix, StringComparison.InvariantCultureIgnoreCase)))
				{
					Data.Add(string.Format("ConnectionStrings:{0}", keyValue.Key.Substring(connectionStringPrefix.Length)), keyValue.Value);
				}

				foreach (var knownEnvironmentMap in _knownEnvironmentMaps)
				{
					if (environmentVariables.TryGetValue(knownEnvironmentMap.Key, out var value))
					{
						Data.Add(knownEnvironmentMap.Value, value);
					}
				}

				if (_showConfig)
				{
					foreach (var environmentVariable in Data)
					{
						System.Console.WriteLine($"  EV-MAP \"{environmentVariable.Key}\" => \"{environmentVariable.Value}\"");
					}
				}

				_loaded = true;
			}
		}

		private void AddData(IDictionary<string, string> environmentVariables, Type configurationType, string prefix)
		{
			var properties = configurationType.GetProperties();

			foreach (var property in properties)
			{
				if (_showConfig)
				{
					System.Console.WriteLine($"    CanRead => \"{property.CanRead.TrueFalse()}\"");
				}

				if (property.CanRead)
				{
					var environmentConfigurationAttribute = property.GetCustomAttribute<EnvironmentConfigurationVariableNameAttribute>();

					if (environmentConfigurationAttribute != null)
					{
						var environmentVariablePrefix = $"{prefix}{environmentConfigurationAttribute.EnvironmentVariableName}[";

						if (_showConfig)
						{
							System.Console.WriteLine($"    environmentVariablePrefix => \"{environmentVariablePrefix}\"");
						}
					}

					if (property.PropertyType == typeof(string[]))
					{
						if (environmentConfigurationAttribute != null)
						{
							var environmentVariablePrefix = $"{environmentConfigurationAttribute.EnvironmentVariableName}:";

							var values = environmentVariables
								.Where(value => value.Key.StartsWith(environmentVariablePrefix))
								.Select(value => new KeyValuePair<int?, string>(value.Key.Substring(environmentVariablePrefix.Length, value.Key.Length - environmentVariablePrefix.Length).ToIntNullable(), value.Value))
								.Where(value => value.Key.HasValue)
								.ToNullCheckedArray(value => new KeyValuePair<int, string>(value.Key.Value, value.Value))
								.OrderBy(value => value.Key)
								.ToNullCheckedArray(value => value.Value);

							if (values.NullCheckedAny())
							{
								for (var valueIndex = 0; valueIndex < values.Length; valueIndex++)
								{
									Data.Add(string.Format("{0}{1}:{2}", prefix, property.Name, valueIndex), string.Format("{0}", values[valueIndex]));
								}
							}
						}
					}
					else if ((property.PropertyType == typeof(string)) || (property.PropertyType == typeof(int)) || (property.PropertyType == typeof(bool)) ||  (property.PropertyType == typeof(TimeSpan)) || property.PropertyType.IsEnum)
					{
						if ((environmentConfigurationAttribute != null) && environmentVariables.TryGetValue(environmentConfigurationAttribute.EnvironmentVariableName, out var value))
						{
							if (_showConfig)
							{
								System.Console.WriteLine($"      {string.Format("{0}{1}", prefix, property.Name)} => \"{value}\"");
							}

							Data.Add(string.Format("{0}{1}", prefix, property.Name), string.Format("{0}", value));
						}
					}
					else if (property.PropertyType.IsClass)
					{
						if (property.PropertyType.Implements(typeof(System.Collections.IEnumerable)))
						{
							//var environmentVariablePrefix = $"{prefix}{environmentConfigurationAttribute.EnvironmentVariableName}[";

							//var values = environmentVariables
							//	.Where(value => value.Key.StartsWith(environmentVariablePrefix))
							//	.Select(value => new KeyValuePair<int?, string>(value.Key.Substring(environmentVariablePrefix.Length, value.Key.Length - environmentVariablePrefix.Length - 1).ToIntNullable(), value.Value))
							//	.Where(value => value.Key.HasValue)
							//	.ToNullCheckedArray(value => new KeyValuePair<int, string>(value.Key.Value, value.Value))
							//	.OrderBy(value => value.Key)
							//	.ToNullCheckedArray(value => value.Value);

							//if (values.NullCheckedAny())
							//{




							//var index = 0;
							//foreach (var item in enumerable)
							//{
							//	AddData(environmentVariables, propertyValue, string.Format("{0}{1}[{2}]:", prefix, property.Name, index++));
							//}
						}
						else
						{
							AddData(environmentVariables, property.PropertyType, string.Format("{0}{1}:", prefix, property.Name));
						}
					}
					else if (environmentConfigurationAttribute != null)
					{
						if (environmentVariables.TryGetValue(environmentConfigurationAttribute.EnvironmentVariableName, out var value))
						{
							Data.Add(string.Format("{0}{1}", prefix, property.Name), string.Format("{0}", value));
						}
					}
				}
			}
		}
	}
}
