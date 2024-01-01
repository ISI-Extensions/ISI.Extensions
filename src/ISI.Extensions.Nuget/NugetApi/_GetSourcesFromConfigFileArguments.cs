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
using ISI.Extensions.JsonSerialization.Extensions;
using ISI.Extensions.Nuget.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using SerializableDTOs = ISI.Extensions.Nuget.SerializableModels;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		private static IDictionary<string, string[]> _sourcesByConfigFile = null;
		private static IDictionary<string, string[]> SourcesByConfigFile => _sourcesByConfigFile ??= new Dictionary<string, string[]>(StringComparer.InvariantCultureIgnoreCase);
		private static readonly object _sourcesByConfigFileLock = new();

		private IEnumerable<string> GetSourcesFromConfigFileArguments(IEnumerable<string> nugetConfigFullNames)
		{
			var arguments = new List<string>();

			foreach (var nugetConfigFullName in nugetConfigFullNames.ToNullCheckedArray(NullCheckCollectionResult.Empty))
			{
				if (!SourcesByConfigFile.TryGetValue(nugetConfigFullName, out var sources))
				{
					lock (_sourcesByConfigFileLock)
					{
						if (!SourcesByConfigFile.TryGetValue(nugetConfigFullName, out sources))
						{
							var _sources = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

							if (System.IO.File.Exists(nugetConfigFullName))
							{
								var nugetConfigXml = System.Xml.Linq.XElement.Parse(System.IO.File.ReadAllText(nugetConfigFullName));

								foreach (var packageSources in nugetConfigXml.GetElementsByLocalName("packageSources"))
								{
									foreach (var packageSource in packageSources.GetElementsByLocalName("add"))
									{
										var packageSourceKey = packageSource.GetAttributeByLocalName("value")?.Value ?? string.Empty;

										if (!string.IsNullOrWhiteSpace(packageSourceKey))
										{
											_sources.Add(packageSourceKey);
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

							sources = _sources.ToArray();
							SourcesByConfigFile.Add(nugetConfigFullName, sources);
						}
					}
				}

				foreach (var source in sources)
				{
					arguments.Add(string.Format("-Source \"{0}\"", source));
				}
			}

			return arguments;
		}
	}
}