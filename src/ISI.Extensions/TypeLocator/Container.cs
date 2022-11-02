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
using System.Reflection;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.TypeLocator
{
	public class Container : IContainer
	{
		private static object _localContainerLock { get; } = new();
		private static IContainer _localContainer { get; set; } = null;
		public static IContainer LocalContainer => _localContainer ??= BuildLocalContainer();

		private static IContainer BuildLocalContainer()
		{
			lock (_localContainerLock)
			{
				if (_localContainer == null)
				{
					var localContainer = new Container();
					localContainer.Build(ISI.Extensions.Assemblies.Container.LocalContainer);

					return localContainer;
				}
			}

			return _localContainer;
		}

		protected Dictionary<Type, List<Type>> ImplementationTypesLookup { get; } = new();

		public IContainer Build(IEnumerable<string> excludeAssemblyFileNames = null, bool includeExes = false)
		{
			var assembliesContainer = new ISI.Extensions.Assemblies.Container();
			assembliesContainer.Build(excludeAssemblyFileNames, includeExes);

			return Build(assembliesContainer);
		}

		private IContainer Build(ISI.Extensions.Assemblies.IContainer assembliesContainer)
		{
			foreach (var type in assembliesContainer.Types)
			{
				try
				{
					if (type.GetCustomAttribute(typeof(TypeLocatorAttribute)) is TypeLocatorAttribute attribute)
					{
						foreach (var attributeType in attribute.Types)
						{
							if (!ImplementationTypesLookup.TryGetValue(attributeType, out var implementationTypes))
							{
								implementationTypes = new();
								ImplementationTypesLookup.Add(attributeType, implementationTypes);
							}

							implementationTypes.Add(type);
						}
					}
				}
				catch
				{
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

			return Type.EmptyTypes;
		}
	}
}
