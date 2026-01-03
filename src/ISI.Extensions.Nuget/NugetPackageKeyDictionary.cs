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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Nuget
{
	public class NugetPackageKeyDictionary : IEnumerable<NugetPackageKey>
	{
		private readonly System.Collections.Concurrent.ConcurrentDictionary<string, NugetPackageKey> _nugetPackageKeys = new(StringComparer.InvariantCultureIgnoreCase);

		public NugetPackageKeyDictionary()
		{

		}
		public NugetPackageKeyDictionary(IEnumerable<NugetPackageKey> nugetPackageKeys)
		{
			foreach (var nugetPackageKey in nugetPackageKeys)
			{
				TryAdd(nugetPackageKey);
			}
		}

		public NugetPackageKey this[string package] => _nugetPackageKeys.TryGetValue(package, out var nugetPackageKey) ? nugetPackageKey : null;

		public bool ContainsKey(string package) => _nugetPackageKeys.ContainsKey(package);

		public bool TryGetValue(string package, out NugetPackageKey nugetPackageKey) => _nugetPackageKeys.TryGetValue(package, out nugetPackageKey);

		public bool TryAdd(NugetPackageKey nugetPackageKey)
		{
			if (nugetPackageKey != null)
			{
				return _nugetPackageKeys.TryAdd(nugetPackageKey.Package, nugetPackageKey);
			}

			return false;
		}

		public bool TryAdd(string package, string version) => TryAdd(new()
		{
			Package = package,
			Version = version,
		});

		public void Merge(IEnumerable<NugetPackageKey> nugetPackageKeys)
		{
			foreach (var nugetPackageKey in nugetPackageKeys)
			{
				if (_nugetPackageKeys.TryGetValue(nugetPackageKey.Package, out var existingNugetPackageKey))
				{
					existingNugetPackageKey.Version = nugetPackageKey.Version;
					if (nugetPackageKey.TargetFrameworks.NullCheckedAny())
					{
						existingNugetPackageKey.TargetFrameworks = nugetPackageKey.TargetFrameworks;
					}
				}
				else
				{
					TryAdd(nugetPackageKey);
				}
			}
		}

		public void Clear()
		{
			_nugetPackageKeys.Clear();
		}

		public IEnumerator<NugetPackageKey> GetEnumerator() => _nugetPackageKeys.Values.GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
