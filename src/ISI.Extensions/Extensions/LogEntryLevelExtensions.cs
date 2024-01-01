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
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.Extensions
{
	public static class LogEntryLevelExtensions
	{
		public static ISI.Extensions.StatusTrackers.LogEntryLevel ToLogEntryLevel(this Microsoft.Extensions.Logging.LogLevel logLevel)
		{
			switch (logLevel)
			{
				case Microsoft.Extensions.Logging.LogLevel.Trace:
				case Microsoft.Extensions.Logging.LogLevel.Debug:
				case Microsoft.Extensions.Logging.LogLevel.Information:
				case Microsoft.Extensions.Logging.LogLevel.None:
					return ISI.Extensions.StatusTrackers.LogEntryLevel.Information;

				case Microsoft.Extensions.Logging.LogLevel.Warning:
					return ISI.Extensions.StatusTrackers.LogEntryLevel.Warning;

				case Microsoft.Extensions.Logging.LogLevel.Error:
				case Microsoft.Extensions.Logging.LogLevel.Critical:
					return ISI.Extensions.StatusTrackers.LogEntryLevel.Error;

				default:
					throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
			}
		}
	}
}
