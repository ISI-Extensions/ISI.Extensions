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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public class ReferenceKey
	{
		public string Value { get; }

		public ReferenceKey()
		{
		}
		public ReferenceKey(string value)
		{
			Value = value;
		}
		public ReferenceKey(int value)
		{
			Value = $"{value}";
		}
		public ReferenceKey(int? value)
		{
			Value = $"{value}";
		}
		public ReferenceKey(long value)
		{
			Value = $"{value}";
		}
		public ReferenceKey(long? value)
		{
			Value = $"{value}";
		}
		public ReferenceKey(Guid value)
		{
			Value = value.Formatted(GuidExtensions.GuidFormat.WithHyphens);
		}
		public ReferenceKey(Guid? value)
		{
			Value = value.Formatted(GuidExtensions.GuidFormat.WithHyphens);
		}

		public bool TryGetInt(out int value)
		{
			return int.TryParse(Value ?? string.Empty, out value);
		}
		public bool TryGetLong(out long value)
		{
			return long.TryParse(Value ?? string.Empty, out value);
		}
		public bool TryGetGuid(out Guid value)
		{
			if (!string.IsNullOrWhiteSpace(Value) && (Guid.TryParse(Value, out var parsedValue) || GuidExtensions.TryParseGuidBase36(Value, out parsedValue)))
			{
				value = parsedValue;

				return true;
			}

			return false;
		}

		public override string ToString() => Value;

		public static bool operator ==(ReferenceKey x, ReferenceKey y) => string.Equals(x?.Value ?? string.Empty, y?.Value ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);
		public static bool operator !=(ReferenceKey x, ReferenceKey y) => !(x == y);

		public override bool Equals(object obj) => ((obj is ReferenceKey other) && (this == other));

		public static implicit operator ReferenceKey(string value) => new(value);
		public static implicit operator ReferenceKey(int value) => new(value);
		public static implicit operator ReferenceKey(int? value) => new(value);
		public static implicit operator ReferenceKey(long value) => new(value);
		public static implicit operator ReferenceKey(long? value) => new(value);
		public static implicit operator ReferenceKey(Guid value) => new(value);
		public static implicit operator ReferenceKey(Guid? value) => new(value);
	}
}
