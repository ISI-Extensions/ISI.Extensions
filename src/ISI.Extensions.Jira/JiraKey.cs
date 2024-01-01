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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Jira.Extensions
{
	public static class JiraKeyExtensions
	{
		public static bool HasValue(this JiraKey jiraKey) => ((jiraKey?.IsId ?? false) || (jiraKey?.IsKey ?? false));
	}
}

namespace ISI.Extensions.Jira
{
	public class JiraKeyCollection : IEnumerable<JiraKey>
	{
		private JiraKey[] _jiraKeys;

		public JiraKeyCollection(IEnumerable<string> values)
		{
			_jiraKeys = values.ToNullCheckedArray(value => new JiraKey(value));
		}
		public JiraKeyCollection(IEnumerable<long> values)
		{
			_jiraKeys = values.ToNullCheckedArray(value => new JiraKey(value));
		}

		public IEnumerator<JiraKey> GetEnumerator() => (new List<JiraKey>(_jiraKeys ?? Array.Empty<JiraKey>())).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public static implicit operator JiraKeyCollection(List<string> values) => new(values);
		public static implicit operator JiraKeyCollection(string[] values) => new(values);
		public static implicit operator JiraKeyCollection(List<long> values) => new(values);
		public static implicit operator JiraKeyCollection(long[] values) => new(values);
	}

	public class JiraKey
	{
		public long? Id { get; } = null;
		public string Key { get; } = null;

		public JiraKey(string key)
		{
			Key = key;
		}
		public JiraKey(long id)
		{
			Id = id;
		}

		public bool IsId => Id.HasValue;
		public bool IsKey => !string.IsNullOrWhiteSpace(Key);

		public override string ToString() => (IsId ? $"{Id}" : Key);

		public static bool operator ==(JiraKey x, JiraKey y)
		{
			if (x.IsId && y.IsId && (x.IsId == y.IsId))
			{
				return true;
			}

			if (x.IsKey && y.IsKey && string.Equals(x.Key, y.Key, StringComparison.InvariantCulture))
			{
				return true;
			}

			return false;
		}
		public static bool operator !=(JiraKey x, JiraKey y) => !(x == y);

		public override bool Equals(object obj) => ((obj is JiraKey other) && (this == other));

		public static implicit operator JiraKey(string value) => new(value);
		public static implicit operator JiraKey(long value) => new(value);
	}
}
