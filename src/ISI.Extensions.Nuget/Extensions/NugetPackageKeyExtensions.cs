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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nuget.Extensions
{
	public static class NugetPackageKeyExtensions
	{
		public static NugetPackageKeyTargetFramework GetTargetFrameworkAssembly(this NugetPackageKey nugetPackageKey, NuGet.Frameworks.NuGetFramework targetFramework)
		{
			var nuGetFrameworkFullComparer = new NuGet.Frameworks.NuGetFrameworkFullComparer();

			var frameworkReducer = new NuGet.Frameworks.FrameworkReducer();

			var possibleTargetFrameworks = nugetPackageKey.TargetFrameworks.NullCheckedWhere(framework => framework.TargetFramework != null).ToNullCheckedArray(framework => framework.TargetFramework, NullCheckCollectionResult.Empty);

			if (possibleTargetFrameworks.Any())
			{
				var possibleTargetFramework = frameworkReducer.GetNearest(targetFramework, possibleTargetFrameworks);

				if (possibleTargetFramework != null)
				{
					var nugetPackageKeyTargetFramework = nugetPackageKey.TargetFrameworks.NullCheckedFirstOrDefault(framework => (framework.TargetFramework != null) && nuGetFrameworkFullComparer.Equals(possibleTargetFramework, framework.TargetFramework));

					if (nugetPackageKeyTargetFramework != null)
					{
						return nugetPackageKeyTargetFramework;
					}
				}
			}

			return nugetPackageKey.TargetFrameworks.FirstOrDefault(framework => framework.TargetFramework == null);
		}

		public static string GetHintPath(this IEnumerable<NugetPackageKeyTargetFrameworkAssembly> nugetPackageKeyTargetFrameworkAssemblies, string assemblyName)
		{
			return nugetPackageKeyTargetFrameworkAssemblies?.NullCheckedFirstOrDefault(assembly => string.Equals(assembly.AssemblyFileName, assemblyName, StringComparison.InvariantCultureIgnoreCase))?.HintPath;
		}
	}
}
