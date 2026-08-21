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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using ISI.Extensions.Nuget.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using SerializableDTOs = ISI.Extensions.Nuget.SerializableModels.Nuget;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public class PackageSource
		{
			public string Key { get; set; }
			public string Url { get; set; }
			public string ProtocolVersion { get; set; }
			public TimeSpan? CooldownTimeSpan { get; set; }
		}

		private static IDictionary<string, PackageSource[]> _sourcesByConfigFile = null;
		private static IDictionary<string, PackageSource[]> SourcesByConfigFile => _sourcesByConfigFile ??= new Dictionary<string, PackageSource[]>(StringComparer.InvariantCultureIgnoreCase);
		private static readonly object _sourcesByConfigFileLock = new();

		private IEnumerable<PackageSource> GetPackageSourcesFromConfigFileArguments(IEnumerable<string> nugetConfigFullNames)
		{
			var response = new List<PackageSource>();

			foreach (var nugetConfigFullName in nugetConfigFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty))
			{
				if (!SourcesByConfigFile.TryGetValue(nugetConfigFullName, out var sources))
				{
					lock (_sourcesByConfigFileLock)
					{
						if (!SourcesByConfigFile.TryGetValue(nugetConfigFullName, out sources))
						{
							var _sources = new Dictionary<string, PackageSource>(StringComparer.InvariantCultureIgnoreCase);

							if (System.IO.File.Exists(nugetConfigFullName))
							{
								var nugetConfigXml = System.Xml.Linq.XElement.Parse(System.IO.File.ReadAllText(nugetConfigFullName));

								foreach (var packageSources in nugetConfigXml.GetElementsByLocalName("packageSources"))
								{
									foreach (var packageSource in packageSources.GetElementsByLocalName("add"))
									{
										var packageSourceKey = packageSource.GetAttributeByLocalName("key")?.Value ?? string.Empty;

										var packageSourceUrl = packageSource.GetAttributeByLocalName("value")?.Value ?? string.Empty;

										var protocolVersion = packageSource.GetAttributeByLocalName("protocolVersion")?.Value ?? string.Empty;

										var cooldownTimeSpan = packageSource.GetAttributeByLocalName("cooldown")?.Value ?? string.Empty;

										if (!string.IsNullOrWhiteSpace(packageSourceUrl))
										{
											_sources.Add(packageSourceKey, new PackageSource()
											{
												Key = packageSourceKey,
												Url = packageSourceUrl,
												ProtocolVersion = protocolVersion,
												CooldownTimeSpan = cooldownTimeSpan.ToTimeSpanNullable(),
											});
										}
									}
								}

								foreach (var disabledPackageSources in nugetConfigXml.GetElementsByLocalName("disabledPackageSources"))
								{
									foreach (var disabledPackageSource in disabledPackageSources.GetElementsByLocalName("add"))
									{
										var disabledPackageSourceKey = disabledPackageSource.GetAttributeByLocalName("key")?.Value ?? string.Empty;

										if (!string.IsNullOrWhiteSpace(disabledPackageSourceKey))
										{
											_sources.Remove(disabledPackageSourceKey);
										}
									}
								}
							}

							sources = _sources.Values.ToArray();
							SourcesByConfigFile.Add(nugetConfigFullName, sources);
						}
					}
				}

				response.AddRange(sources);
			}

			return response;
		}

		private IEnumerable<string> GetSourcesFromConfigFileArguments(IEnumerable<string> nugetConfigFullNames, string formatMask = "-Source \"{source}\"")
		{
			var arguments = new List<string>();

			var packageSources = GetPackageSourcesFromConfigFileArguments(nugetConfigFullNames);

			foreach (var packageSource in packageSources)
			{
				arguments.Add(string.Format(formatMask.Replace("{source}", packageSource.Url), packageSource.Url));
			}

			return arguments;
		}
	}
}