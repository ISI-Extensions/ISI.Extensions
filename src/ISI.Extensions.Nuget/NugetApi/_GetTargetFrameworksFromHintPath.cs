using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Nuget.DataTransferObjects.NugetApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Nuget
{
	public partial class NugetApi
	{
		private NugetPackageKeyTargetFramework[] GetTargetFrameworksFromHintPath(string hintPath)
		{
			return new[]
			{
				new NugetPackageKeyTargetFramework()
				{
					TargetFramework = NuGet.Frameworks.NuGetFramework.Parse(hintPath.Split(new[] { '\\', '/' })[2]),
					Assemblies = new[]
					{
						new NugetPackageKeyTargetFrameworkAssembly()
						{
							AssemblyFileName = System.IO.Path.GetFileName(hintPath),
							HintPath = hintPath,
						}
					}
				}
			};
		}
	}
}