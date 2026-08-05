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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Syslog
{
	public class Message
	{
		public static System.Text.RegularExpressions.Regex Rfc3164Regex = new("^<(?<pri>\\d{1,3})>(?<timestamp>[A-Z][a-z]{2}\\s+\\d{1,2}\\s+\\d{2}:\\d{2}:\\d{2})\\s(?<hostname>\\S+)\\s(?<appname>[^\\[:]+)(?:\\[(?<procid>\\d+)\\])?:\\s(?<message>.*)$\n");
		public static System.Text.RegularExpressions.Regex Rfc5424Regex = new("^<(?<pri>\\d{1,3})>(?<version>\\d{1,2})\\s(?<timestamp>-|\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}(?:\\.\\d+)?(?:Z|[+-]\\d{2}:\\d{2}))\\s(?<hostname>-|\\S+)\\s(?<appname>-|\\S+)\\s(?<procid>-|\\S+)\\s(?<msgid>-|\\S+)\\s(?<structureddata>-|(?:\\[.+?\\])+)(?:\\s(?<message>.*))?$\n");

		public Facility Facility { get; set; }

		public LogLevel LogLevel { get; set; }

		public int? Version { get; set; }

		public DateTime? DateTime { get; set; }

		public string HostName { get; set; }

		public string AppName { get; set; }

		public int? ProcessId { get; set; }

		public string MessageId { get; set; }

		public string StructuredData { get; set; }

		public string Text { get; set; }

		public int Priority
		{
			get => (int)Facility * 8 + (int)LogLevel;
			set
			{
				Facility = (Facility)(value >> 3);
				LogLevel = (LogLevel)(value & 0x7);
			}
		}

		public static bool TryParseRfc3146(string value, out Message message)
		{
			var parsed = Rfc3164Regex.Match(value ?? string.Empty);

			if (parsed.Success)
			{
				message = new()
				{
					Priority = parsed.Groups["pri"].Value.ToInt(),
					DateTime = parsed.Groups["timestamp"].Value.ToDateTimeNullable(),
					HostName = parsed.Groups["hostname"].Value,
					AppName = parsed.Groups["appname"].Value,
					ProcessId = parsed.Groups["procid"].Value.ToIntNullable(),
					Text = parsed.Groups["message"].Value,
				};

				return true;
			}

			message = null;
			return false;
		}

		public static bool TryParseRfc5425(string value, out Message message)
		{
			var parsed = Rfc5424Regex.Match(value ?? string.Empty);

			if (parsed.Success)
			{
				message = new()
				{
					Priority = parsed.Groups["pri"].Value.ToInt(),
					Version = parsed.Groups["version"].Value.ToIntNullable(),
					DateTime = parsed.Groups["timestamp"].Value.ToDateTimeNullable(),
					HostName = parsed.Groups["hostname"].Value,
					AppName = parsed.Groups["appname"].Value,
					ProcessId = parsed.Groups["procid"].Value.ToIntNullable(),
					MessageId = parsed.Groups["msgid"].Value,
					StructuredData = (parsed.Groups["structureddata"].Value ?? string.Empty).TrimStart('[').TrimEnd(']'),
					Text = parsed.Groups["message"].Value,
				};

				return true;
			}

			message = null;
			return false;
		}
	}
}
