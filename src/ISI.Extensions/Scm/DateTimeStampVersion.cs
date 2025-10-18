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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Scm
{
	public class DateTimeStampVersion
	{
		public DateTimeStamp DateTimeStamp { get; set; }
		public Version Version { get; set; }

		public DateTimeStampVersion()
		{

		}
		public DateTimeStampVersion(string dateTimeStampVersion)
		{
			Value = dateTimeStampVersion;
		}
		public DateTimeStampVersion(string dateTimeStamp, Version version)
		{
			DateTimeStamp = new(dateTimeStamp);
			Version = version;
		}
		public DateTimeStampVersion(string dateTimeStamp, string version)
		{
			DateTimeStamp = new(dateTimeStamp);
			Version = string.IsNullOrWhiteSpace(version) ? null : new Version(version);
		}
		public DateTimeStampVersion(DateTime dateTimeUtc)
		{
			DateTimeStamp = new(dateTimeUtc);
		}
		public DateTimeStampVersion(DateTimeStamp dateTimeStamp)
		{
			DateTimeStamp = dateTimeStamp;
		}
		public DateTimeStampVersion(DateTimeStamp dateTimeStamp, Version version)
		{
			DateTimeStamp = dateTimeStamp;
			Version = version;
		}
		public DateTimeStampVersion(DateTimeStamp dateTimeStamp, string version)
		{
			DateTimeStamp = dateTimeStamp;
			Version = string.IsNullOrWhiteSpace(version) ? null : new Version(version);
		}

		public string Value
		{
			get => $"{DateTimeStamp}|{Version ?? new Version()}";
			set
			{
				if (string.IsNullOrWhiteSpace(value))
				{
					DateTimeStamp = null;
					Version = null;
				}
				else if (value.IndexOf("(") >= 0)
				{
					var pieces = $"{value}(((-(-".Split(['(', ')', ' ', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries, p => p.Trim('-'));
					DateTimeStamp = new(pieces[1]);
					Version = string.IsNullOrWhiteSpace(pieces[0]) ? null : new Version(pieces[0]);
				}
				else
				{
					var pieces = $"{value}||||-|-".Split(['|', '\r', '\n'], StringSplitOptions.RemoveEmptyEntries, p => p.Trim('-'));
					DateTimeStamp = new(pieces[0]);
					Version = string.IsNullOrWhiteSpace(pieces[1]) ? null : new Version(pieces[1]);
				}
			}
		}

		public bool HasValue => ((DateTimeStamp != null) || (Version != null));

		public override string ToString() => Value;
		//public override string ToString() => string.Format("{0} ({1})", Version, DateTimeStamp);
		public string Formatted() => $"{Version} ({DateTimeStamp})";
		
		public static bool operator ==(DateTimeStampVersion x, DateTimeStampVersion y) => string.Equals(x?.ToString() ?? string.Empty, y?.ToString() ?? string.Empty, StringComparison.InvariantCultureIgnoreCase);
		public static bool operator !=(DateTimeStampVersion x, DateTimeStampVersion y) => !(x == y);

		public override bool Equals(object obj) => ((obj is DateTimeStampVersion other) && (this == other));

		public static implicit operator DateTimeStampVersion(string dateTimeStampVersion) => new(dateTimeStampVersion);
		public static implicit operator DateTimeStampVersion(DateTimeStamp dateTimeStamp) => new(dateTimeStamp);
	}
}
