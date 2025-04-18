﻿#region Copyright & License
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
	public class DateTimeStamp
	{
		public DateTime DateTimeUtc { get; }

		public DateTimeStamp()
		{
			DateTimeUtc = DateTime.UtcNow;
		}
		public DateTimeStamp(string dateTimeStamp)
		{
			DateTimeUtc = dateTimeStamp.ToDateTimeUtc();
		}
		public DateTimeStamp(DateTime dateTimeUtc)
		{
			DateTimeUtc = dateTimeUtc;
		}

		public override string ToString() => string.Format("{0:yyyyMMdd.HHmmss}", DateTimeUtc);

		public static bool operator ==(DateTimeStamp x, DateTimeStamp y) => (x?.DateTimeUtc == y?.DateTimeUtc);
		public static bool operator !=(DateTimeStamp x, DateTimeStamp y) => !(x == y);

		public override bool Equals(object obj) => ((obj is DateTimeStamp other) && (this == other));

		public static implicit operator DateTimeStamp(string dateTimeStamp) => new(dateTimeStamp);
		public static implicit operator DateTimeStamp(DateTime dateTimeUtc) => new(dateTimeUtc);
		//public static implicit operator DateTimeStamp(DateTimeStampVersion dateTimeVersion) => (dateTimeVersion?.DateTimeStamp == null ? null : new(dateTimeVersion.DateTimeStamp.DateTimeUtc));
	}
}
