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
using ISI.Extensions.Nuget.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		public DTOs.UpdateAssemblyRedirectsResponse UpdateAssemblyRedirects(DTOs.UpdateAssemblyRedirectsRequest request)
		{
			var response = new DTOs.UpdateAssemblyRedirectsResponse();

			var nugetPackageKeys = request.NugetPackageKeys.ToArray();

			var csProjXml = System.Xml.Linq.XElement.Parse(request.CsProjXml);

			var targetFrameworkVersion = GetTargetFrameworkVersionFromCsProjXml(csProjXml);

			var projectXml = System.Xml.Linq.XElement.Parse(request.AppConfigXml);

			var runtimeSectionElement = projectXml.GetElementByLocalName("runtime");

			var assemblyBindingElement = runtimeSectionElement?.GetElementByLocalName("assemblyBinding");

			var removeAssemblyRedirects = new HashSet<string>(request.RemoveAssemblyRedirects ?? Array.Empty<string>(), StringComparer.InvariantCultureIgnoreCase);

			var upsertAssemblies = request.UpsertAssemblyRedirectsNugetPackageKeys
				.NullCheckedSelectMany(nugetPackageKey => nugetPackageKey.GetTargetFrameworkAssembly(targetFrameworkVersion)?.Assemblies, NullCheckCollectionResult.Empty)
				.ToDictionary(assembly => assembly.AssemblyName, assembly => assembly, StringComparer.InvariantCultureIgnoreCase);

			bool doCheckWildcardRemoveAssemblyRedirect = removeAssemblyRedirects.Any(removeAssemblyRedirect => removeAssemblyRedirect.EndsWith("*", StringComparison.InvariantCultureIgnoreCase));

			bool isRemoveAssemblyRedirectsWildcardMatch(string assemblyName)
			{
				if (doCheckWildcardRemoveAssemblyRedirect)
				{
					while (!string.IsNullOrWhiteSpace(assemblyName))
					{
						if (removeAssemblyRedirects.Contains(string.Format("{0}*", assemblyName)))
						{
							return true;
						}

						assemblyName = assemblyName.Substring(0, assemblyName.Length - 1);
					}
				}

				return false;
			}

			if (assemblyBindingElement != null)
			{
				foreach (var dependentAssembly in assemblyBindingElement.GetElementsByLocalName("dependentAssembly"))
				{
					var assemblyIdentity = dependentAssembly.GetElementByLocalName("assemblyIdentity");
					var bindingRedirect = dependentAssembly.GetElementByLocalName("bindingRedirect");

					if (bindingRedirect != null)
					{
						var assemblyName = assemblyIdentity.GetAttributeByLocalName("name")?.Value ?? string.Empty;
						var newVersion = bindingRedirect.GetAttributeByLocalName("newVersion")?.Value ?? string.Empty;

						if (removeAssemblyRedirects.Contains(assemblyName) || isRemoveAssemblyRedirectsWildcardMatch(assemblyName))
						{
							dependentAssembly.Remove();
						}
						else
						{
							upsertAssemblies.Remove(assemblyName);

							foreach (var nugetPackageKey in nugetPackageKeys)
							{
								var assembly = nugetPackageKey.GetTargetFrameworkAssembly(targetFrameworkVersion)?.Assemblies.NullCheckedFirstOrDefault(a => string.Equals(a.AssemblyName, assemblyName, StringComparison.InvariantCultureIgnoreCase));

								if (!string.IsNullOrWhiteSpace(assembly?.AssemblyVersion) && !string.Equals(assembly.AssemblyVersion, newVersion))
								{
									bindingRedirect.Attribute("oldVersion").Value = string.Format("0.0.0.0-{0}", assembly.AssemblyVersion);
									bindingRedirect.Attribute("newVersion").Value = assembly.AssemblyVersion;
								}
							}
						}
					}
				}

				foreach (var upsertAssembly in upsertAssemblies.Values)
				{
					var dependentAssemblyElement = new System.Xml.Linq.XElement("dependentAssembly");

					var assemblyIdentityElement = new System.Xml.Linq.XElement("assemblyIdentity");
					assemblyIdentityElement.Add(new System.Xml.Linq.XAttribute("name", upsertAssembly.AssemblyName));
					assemblyIdentityElement.Add(new System.Xml.Linq.XAttribute("publicKeyToken", upsertAssembly.PublicKeyToken));
					assemblyIdentityElement.Add(new System.Xml.Linq.XAttribute("culture", "neutral"));
					dependentAssemblyElement.Add(assemblyIdentityElement);

					var bindingRedirectElement = new System.Xml.Linq.XElement("bindingRedirect");
					bindingRedirectElement.Add(new System.Xml.Linq.XAttribute("oldVersion", string.Format("0.0.0.0-{0}", upsertAssembly.AssemblyVersion)));
					bindingRedirectElement.Add(new System.Xml.Linq.XAttribute("newVersion", upsertAssembly.AssemblyVersion));
					dependentAssemblyElement.Add(bindingRedirectElement);

					assemblyBindingElement.Add(dependentAssemblyElement);
				}
			}

			response.AppConfigXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n{0}", projectXml).Replace(" xmlns=\"\"", string.Empty);

			return response;
		}
	}
}