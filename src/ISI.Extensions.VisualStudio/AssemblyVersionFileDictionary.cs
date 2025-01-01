#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using System.Text;

namespace ISI.Extensions.VisualStudio
{
	public class AssemblyVersionFileDictionary : IDictionary<string, AssemblyVersionFile>, IEnumerable<AssemblyVersionFile>
	{
		private readonly IDictionary<string, AssemblyVersionFile> _assemblyVersionFiles = new Dictionary<string, AssemblyVersionFile>(StringComparer.InvariantCultureIgnoreCase);

		public IEnumerator<KeyValuePair<string, AssemblyVersionFile>> GetEnumerator() => _assemblyVersionFiles.GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _assemblyVersionFiles.GetEnumerator();

		IEnumerator<AssemblyVersionFile> IEnumerable<AssemblyVersionFile>.GetEnumerator() => _assemblyVersionFiles.Values.GetEnumerator();

		public void Add(AssemblyVersionFile assemblyVersionFile) => _assemblyVersionFiles.Add(assemblyVersionFile.AssemblyGroupName, assemblyVersionFile);
		public void Add(KeyValuePair<string, AssemblyVersionFile> item) => _assemblyVersionFiles.Add(item);
		public void Add(string key, AssemblyVersionFile value) => _assemblyVersionFiles.Add(key, value);
		
		public bool Remove(KeyValuePair<string, AssemblyVersionFile> item) => _assemblyVersionFiles.Remove(item.Key);
		public bool Remove(string key) => _assemblyVersionFiles.Remove(key);

		public void Clear() => _assemblyVersionFiles.Clear();
		
		public bool Contains(KeyValuePair<string, AssemblyVersionFile> item) => _assemblyVersionFiles.ContainsKey(item.Key);
		public bool ContainsKey(string key) => _assemblyVersionFiles.ContainsKey(key);
		
		public void CopyTo(KeyValuePair<string, AssemblyVersionFile>[] array, int arrayIndex) => _assemblyVersionFiles.CopyTo(array, arrayIndex);
		public bool TryGetValue(string key, out AssemblyVersionFile value) => _assemblyVersionFiles.TryGetValue(key, out value);

		public AssemblyVersionFile this[string key]
		{
			get => _assemblyVersionFiles[key];
			set => _assemblyVersionFiles[key] = value;
		}

		public ICollection<string> Keys => _assemblyVersionFiles.Keys;
		public ICollection<AssemblyVersionFile> Values => _assemblyVersionFiles.Values;
		
		public int Count => _assemblyVersionFiles.Count;
		public bool IsReadOnly => _assemblyVersionFiles.IsReadOnly;
	}
}
