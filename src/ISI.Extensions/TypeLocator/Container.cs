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
using System.Reflection;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.TypeLocator
{
	public class Container : IContainer
	{
		private static object _localContainerLock { get; } = new object();
		private static IContainer _localContainer { get; set; } = null;
		public static IContainer LocalContainer => _localContainer ??= BuildLocalContainer();

		private static IContainer BuildLocalContainer()
		{
			lock (_localContainerLock)
			{
				if (_localContainer == null)
				{
					var localContainer = new Container();
					localContainer.Build();
					return localContainer;
				}
			}

			return _localContainer;
		}

		protected Dictionary<Type, List<Type>> ImplementationTypesLookup { get; } = new Dictionary<Type, List<Type>>();

		public IContainer Build(IEnumerable<string> excludeAssemblyFileNames = null, bool includeExes = false)
		{
			var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

			var executingDirectoryName = System.IO.Path.GetDirectoryName(executingAssembly.Location);

			var assemblyFileNames = new HashSet<string>(System.IO.Directory.GetFiles(executingDirectoryName, "*.dll", System.IO.SearchOption.TopDirectoryOnly).Select(fileName => System.IO.Path.Combine(executingDirectoryName, fileName)), StringComparer.CurrentCultureIgnoreCase);
			if (includeExes)
			{
				assemblyFileNames.UnionWith(System.IO.Directory.GetFiles(executingDirectoryName, "*.exe", System.IO.SearchOption.TopDirectoryOnly).Select(fileName => System.IO.Path.Combine(executingDirectoryName, fileName)));
			}

			if (excludeAssemblyFileNames.NullCheckedAny())
			{
				excludeAssemblyFileNames = new HashSet<string>(excludeAssemblyFileNames, StringComparer.CurrentCultureIgnoreCase);

				assemblyFileNames.RemoveWhere(excludeAssemblyFileNames.Contains);
			}

			var types = new List<Type>();

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic))
			{
				try
				{
					var assemblyFileName = assembly.Location;

					if (assemblyFileNames.Contains(assemblyFileName))
					{
						types.AddRange(assembly.GetTypes());
						assemblyFileNames.Remove(assemblyFileName);
					}
				}
				catch
				{
				}
			}

			var assemblies = new List<Assembly>();

			foreach (var assemblyFileName in assemblyFileNames)
			{
				try
				{
					var assemblyName = AssemblyName.GetAssemblyName(assemblyFileName);

					var assembly = AppDomain.CurrentDomain.Load(assemblyName);

					assemblies.Add(assembly);
				}
				catch
				{
				}
			}

			foreach (var assembly in assemblies)
			{
				try
				{
					types.AddRange(assembly.DefinedTypes.Select(dt => dt.AsType()));
				}
				catch
				{
				}
			}

			foreach (var type in types)
			{
				if (type.GetCustomAttribute(typeof(TypeLocatorAttribute)) is TypeLocatorAttribute attribute)
				{
					foreach (var attributeType in attribute.Types)
					{
						if (!ImplementationTypesLookup.TryGetValue(attributeType, out var implementationTypes))
						{
							implementationTypes = new List<Type>();
							ImplementationTypesLookup.Add(attributeType, implementationTypes);
						}

						implementationTypes.Add(type);
					}
				}
			}

			return this;
		}

		public Type[] GetImplementationTypes(Type type)
		{
			if (ImplementationTypesLookup.TryGetValue(type, out var implementationTypes))
			{
				return implementationTypes.ToArray();
			}

			return new Type[0];
		}
	}
}
