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
using System.Linq;
using System.Text;

namespace ISI.Extensions
{
	public class EpochDateTime
	{
		internal static readonly DateTime EpochStartOfDateTimeUtc = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		private DateTime _valueUtc;

		public EpochDateTime(DateTime dateTimeUtc)
		{
			Value = dateTimeUtc;
		}

		public EpochDateTime(long timeStampUtc)
		{
			TimeStamp = timeStampUtc;
		}

		public long TimeStamp
		{
			get => (long)(_valueUtc - EpochStartOfDateTimeUtc).TotalSeconds;
			set => _valueUtc = EpochStartOfDateTimeUtc.AddSeconds(value);
		}

		public long Ticks
		{
			get => (long)(_valueUtc - EpochStartOfDateTimeUtc).Ticks;
			set => _valueUtc = EpochStartOfDateTimeUtc.AddTicks(value);
		}

		public double Milliseconds
		{
			get => (double)(_valueUtc - EpochStartOfDateTimeUtc).TotalMilliseconds;
			set => _valueUtc = EpochStartOfDateTimeUtc.AddMilliseconds(value);
		}

		public double Seconds
		{
			get => (long)(_valueUtc - EpochStartOfDateTimeUtc).TotalSeconds;
			set => _valueUtc = EpochStartOfDateTimeUtc.AddSeconds(value);
		}

		public DateTime Value
		{
			get => _valueUtc;
			set => _valueUtc = value;
		}

		public static EpochDateTime FromDateTimeUtc(DateTime dateTimeUtc) => new(dateTimeUtc);
		public static EpochDateTime FromDateTimeUtc(DateTime? dateTimeUtc) => (dateTimeUtc.HasValue ? new EpochDateTime(dateTimeUtc.Value) : null);
		public static EpochDateTime FromTicks(long ticks) => new(EpochStartOfDateTimeUtc.AddTicks(ticks));
		public static EpochDateTime FromTicks(long? ticks) => (ticks.HasValue ? new EpochDateTime(EpochStartOfDateTimeUtc.AddTicks(ticks.Value)) : null);
		public static EpochDateTime FromMilliseconds(double milliseconds) => new(EpochStartOfDateTimeUtc.AddMilliseconds(milliseconds));
		public static EpochDateTime FromMilliseconds(double? milliseconds) => (milliseconds.HasValue ? new EpochDateTime(EpochStartOfDateTimeUtc.AddMilliseconds(milliseconds.Value)) : null);
		public static EpochDateTime FromTotalSeconds(long seconds) => new(EpochStartOfDateTimeUtc.AddSeconds(seconds));
		public static EpochDateTime FromTotalSeconds(long? seconds) => (seconds.HasValue ? new EpochDateTime(EpochStartOfDateTimeUtc.AddSeconds(seconds.Value)) : null);
	}
}