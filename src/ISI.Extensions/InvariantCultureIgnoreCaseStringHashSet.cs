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
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions
{
	public class InvariantCultureIgnoreCaseStringHashSet : ICollection<string>, IEnumerable<string>, System.Collections.IEnumerable, IReadOnlyCollection<string>, ISet<string>, System.Runtime.Serialization.IDeserializationCallback, System.Runtime.Serialization.ISerializable
	{
		private readonly HashSet<string> _hashSet = new(StringComparer.InvariantCultureIgnoreCase);

		public InvariantCultureIgnoreCaseStringHashSet()
		{

		}

		public InvariantCultureIgnoreCaseStringHashSet(IEnumerable<string> values)
		{
			if (values != null)
			{
				_hashSet.UnionWith(values);
			}
		}

		public IEqualityComparer<string> Comparer => _hashSet.Comparer;
		public int Count => _hashSet.Count;

		bool ICollection<string>.IsReadOnly => (_hashSet as ICollection<string>).IsReadOnly;

		public bool Add(string item) => _hashSet.Add(item);

		public void Clear() => _hashSet.Clear();

		public bool Contains(string item) => _hashSet.Contains(item);

		public void CopyTo(string[] array) => _hashSet.CopyTo(array);
		public void CopyTo(string[] array, int arrayIndex) => _hashSet.CopyTo(array, arrayIndex);
		public void CopyTo(string[] array, int arrayIndex, int count) => _hashSet.CopyTo(array, arrayIndex, count);
		
		public void ExceptWith(IEnumerable<string> other) => _hashSet.ExceptWith(other);

		public HashSet<string>.Enumerator GetEnumerator() => _hashSet.GetEnumerator();

		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) => _hashSet.GetObjectData(info, context);

		public void IntersectWith(IEnumerable<string> other) => _hashSet.IntersectWith(other);

		public bool IsProperSubsetOf(IEnumerable<string> other) => _hashSet.IsProperSubsetOf(other);

		public bool IsProperSupersetOf(IEnumerable<string> other)=>_hashSet.IsProperSupersetOf(other);

		public bool IsSubsetOf(IEnumerable<string> other) => _hashSet.IsSubsetOf(other);

		public bool IsSupersetOf(IEnumerable<string> other) => _hashSet.IsSupersetOf(other);

		public virtual void OnDeserialization(object sender) => _hashSet.OnDeserialization(sender);

		public bool Overlaps(IEnumerable<string> other) => _hashSet.Overlaps(other);

		public bool Remove(string item) => _hashSet.Remove(item);

		public int RemoveWhere(Predicate<string> match) => _hashSet.RemoveWhere(match);

		public bool SetEquals(IEnumerable<string> other) => _hashSet.SetEquals(other);

		public void SymmetricExceptWith(IEnumerable<string> other)=>_hashSet.SymmetricExceptWith(other);

		void ICollection<string>.Add(string item) => _hashSet.Add(item);

		IEnumerator<string> IEnumerable<string>.GetEnumerator() => _hashSet.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => _hashSet.GetEnumerator();

		public void TrimExcess() => _hashSet.TrimExcess();

		public void UnionWith(IEnumerable<string> other) => _hashSet.UnionWith(other ?? Array.Empty<string>());

		public static implicit operator InvariantCultureIgnoreCaseStringHashSet(HashSet<string> values) => new(values);
		public static implicit operator InvariantCultureIgnoreCaseStringHashSet(string[] values) => new(values);
		public static implicit operator InvariantCultureIgnoreCaseStringHashSet(string value) => new(new [] { value });
	}
}
