#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	public class ConsoleLogger : Microsoft.Extensions.Logging.ILogger
	{
		public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			formatter ??= (formatterState, formatterException) =>
			{
				if (formatterState is string stringValue)
				{
					return stringValue;
				}

				if (formatterException != null)
				{
					return formatterException.ErrorMessageFormatted();
				}

				return formatterState.ToString();
			};

			var foregroundColor = System.Console.ForegroundColor;

			switch (logLevel)
			{
				case Microsoft.Extensions.Logging.LogLevel.None:
				case Microsoft.Extensions.Logging.LogLevel.Trace:
				case Microsoft.Extensions.Logging.LogLevel.Debug:
				case Microsoft.Extensions.Logging.LogLevel.Information:
					break;
				case Microsoft.Extensions.Logging.LogLevel.Warning:
				case Microsoft.Extensions.Logging.LogLevel.Error:
				case Microsoft.Extensions.Logging.LogLevel.Critical:
					System.Console.ForegroundColor = ConsoleColor.Red;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
			}

			System.Console.WriteLine(formatter(state, exception));

			System.Console.ForegroundColor = foregroundColor;

		}

		public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
		{
			return true;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return new ConsoleLoggerScope<TState>(state);
		}

		public class ConsoleLoggerScope<TState> : IDisposable
		{
			protected TState State { get; }

			public ConsoleLoggerScope(TState state)
			{
				State = state;
			}

			public void Dispose()
			{
			}
		}
	}
}
