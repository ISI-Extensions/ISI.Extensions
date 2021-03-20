#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Nuget
{
	public partial class NugetHelper
	{
		public IEnumerable<NugetKey> GetProjectNugetPackageDependencies(string projectFullName, Func<string, string> versionFinder = null)
		{
			versionFinder ??= s => string.Empty;

			var nugetKeys = new Dictionary<string, string>();
			
			var projectDirectory = System.IO.Path.GetDirectoryName(projectFullName);

			var packagesConfigFullName = System.IO.Path.Combine(projectDirectory, "packages.config");
			if (System.IO.File.Exists(packagesConfigFullName))
			{
				var packages = from packageTag in System.Xml.Linq.XElement.Load(packagesConfigFullName).Elements("package")
											 select new { Id = packageTag.Attribute("id").Value, Version = packageTag.Attribute("version").Value };

				foreach (var package in packages)
				{
					if (nugetKeys.TryGetValue(package.Id, out var version))
					{
						if (!string.Equals(package.Version, version, StringComparison.InvariantCultureIgnoreCase))
						{
							throw new Exception(string.Format("Multiple versions of {0} found", package.Id));
						}
					}
					else
					{
						nugetKeys.Add(package.Id, package.Version);
					}
				}
			}

			if (projectFullName.EndsWith(".csproj", StringComparison.InvariantCultureIgnoreCase))
			{
				var projectLines = System.IO.File.ReadAllLines(projectFullName);

				for (var lineIndex = 0; lineIndex < projectLines.Length; lineIndex++)
				{
					var projectLine = projectLines[lineIndex];

					if (projectLine.IndexOf("<PackageReference ", StringComparison.InvariantCulture) > 0)
					{
						var keyValues = projectLine.Replace("<PackageReference ", string.Empty).Replace("/>", string.Empty).Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Split(new[] { "=\"", "\"" }, StringSplitOptions.None)).ToDictionary(item => item[0], item => item[1], StringComparer.CurrentCultureIgnoreCase);

						if (!keyValues.TryGetValue("Include", out var package))
						{
							package = keyValues["Update"];
						}

						var packageVersion = keyValues["Version"];

						if (nugetKeys.TryGetValue(package, out var version))
						{
							if (!string.Equals(packageVersion, version, StringComparison.InvariantCultureIgnoreCase))
							{
								throw new Exception(string.Format("Multiple versions of {0} found", package));
							}
						}
						else
						{
							nugetKeys.Add(package, packageVersion);
						}
					}
					else if (projectLine.IndexOf("<ProjectReference ", StringComparison.InvariantCulture) > 0)
					{
						var keyValues = projectLine.Replace("<ProjectReference ", string.Empty).Replace("/>", string.Empty).Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Split(new[] { "=\"", "\"" }, StringSplitOptions.None)).ToDictionary(item => item[0], item => item[1], StringComparer.CurrentCultureIgnoreCase);

						if (keyValues.TryGetValue("Include", out var projectReference))
						{
							var package = System.IO.Path.GetFileNameWithoutExtension(projectReference);

							nugetKeys.Add(package, versionFinder(package));
						}
					}
				}
			}

			return nugetKeys.Select(nugetKey => new NugetKey()
			{
				Package = nugetKey.Key,
				Version = nugetKey.Value,
			});
		}
	}
}