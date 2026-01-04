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
using System.Reflection;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Assemblies
{
	public class AssembliesContainer : IAssembliesContainer
	{
		private static object _localContainerLock { get; } = new();
		private static IAssembliesContainer _localContainer { get; set; } = null;
		public static IAssembliesContainer LocalContainer => _localContainer ??= BuildLocalContainer();

		private static IAssembliesContainer BuildLocalContainer()
		{
			lock (_localContainerLock)
			{
				if (_localContainer == null)
				{
					var localContainer = new AssembliesContainer();
					localContainer.Build();

					return localContainer;
				}
			}

			return _localContainer;
		}

		public Assembly[] Assemblies { get; private set; }
		public Type[] Types { get; private set; }

		public IAssembliesContainer Build(IEnumerable<string> excludeAssemblyFileNames = null, bool includeExes = false)
		{
			var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

			var executingDirectoryName = System.IO.Path.GetDirectoryName(executingAssembly.Location);

			var assemblyFileNames = new HashSet<string>(System.IO.Directory.GetFiles(executingDirectoryName, "*.dll", System.IO.SearchOption.TopDirectoryOnly).Select(fileName => System.IO.Path.Combine(executingDirectoryName, fileName)), StringComparer.CurrentCultureIgnoreCase);
			if (includeExes)
			{
				assemblyFileNames.UnionWith(System.IO.Directory.GetFiles(executingDirectoryName, "*.exe", System.IO.SearchOption.TopDirectoryOnly).Select(fileName => System.IO.Path.Combine(executingDirectoryName, fileName)));
			}

			var excludeAssemblyNames = new HashSet<string>((excludeAssemblyFileNames.NullCheckedSelect(System.IO.Path.GetFileNameWithoutExtension) ?? []), StringComparer.InvariantCultureIgnoreCase);
			excludeAssemblyNames.Add("Microsoft.IdentityModel.Protocols.OpenIdConnect");
			excludeAssemblyNames.Add("Microsoft.IdentityModel.Tokens");

			var assemblies = new Dictionary<string, Assembly>(StringComparer.CurrentCultureIgnoreCase);

			var types = new List<Type>();

			foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.IsDynamic && !(excludeAssemblyNames.Contains(System.IO.Path.GetFileNameWithoutExtension(assembly.Location)))))
			{
				try
				{
					var assemblyFileName = assembly.Location;

					if (!assemblies.ContainsKey(assemblyFileName))
					{

						if (assemblyFileNames.Contains(assemblyFileName))
						{
							assemblyFileNames.Remove(assemblyFileName);
						}

						types.AddRange(assembly.GetTypes());

						assemblies.Add(assemblyFileName, assembly);
					}
				}
				catch
				{
					Console.WriteLine($"Could not load assembly: {assembly.FullName}");
				}
			}

			var notLoadedAssemblies = new List<Assembly>();

			foreach (var assemblyFileName in assemblyFileNames.Where(assemblyFileName => !(excludeAssemblyNames.Contains(System.IO.Path.GetFileNameWithoutExtension(assemblyFileName)))))
			{
				try
				{
					if (!assemblies.ContainsKey(assemblyFileName))
					{

						var assemblyName = AssemblyName.GetAssemblyName(assemblyFileName);

						var assembly = System.AppDomain.CurrentDomain.Load(assemblyName);

						notLoadedAssemblies.Add(assembly);

						assemblies.Add(assemblyFileName, assembly);
					}
				}
				catch
				{
					Console.WriteLine($"Could not load assembly: {assemblyFileName} (possibly 2nd attempt)");
				}
			}

			foreach (var assembly in notLoadedAssemblies)
			{
				try
				{
					types.AddRange(assembly.DefinedTypes.Select(dt => dt.AsType()));
				}
				catch
				{
				}
			}

			Assemblies = assemblies.Values.ToArray();

			Types = types.ToArray();

			return this;
		}
	}
}
