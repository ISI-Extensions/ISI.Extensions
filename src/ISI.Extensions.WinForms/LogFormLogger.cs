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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.WinForms
{
	public class LogFormLogger : Microsoft.Extensions.Logging.ILogger, IDisposable
	{
		public ISI.Extensions.WinForms.LogForm LogForm { get; protected set; } = null;

		public bool IsFormVisible { get; protected set; }

		public ISI.Extensions.StatusTrackers.AddToLog AddToLog { get; protected set; }

		public LogFormLogger(ApplyFormSizeDelegate applyFormSize, RecordFormSizeDelegate recordFormSize, Action onClose)
		{
			LogForm = new LogForm(applyFormSize, recordFormSize, addToLog =>
			{
				AddToLog = addToLog;
			}, onClose)
			{
				ShowDoneButtonOnStartUp = false,
			};

			IsFormVisible = false;
		}

		public void Log<TState>(Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsFormVisible)
			{
				LogForm.Invoke((System.Windows.Forms.MethodInvoker)delegate
				{
					LogForm.Show();
					System.Windows.Forms.Application.DoEvents();
				});
			}

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

			AddToLog(logLevel.ToLogEntryLevel(), formatter(state, exception));
		}

		public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
		{
			return true;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return new LogFormLoggerScope<TState>(state);
		}

		public void Dispose()
		{
			LogForm.Close();
			LogForm?.Dispose();
			LogForm = null;
		}

		public class LogFormLoggerScope<TState> : IDisposable
		{
			protected TState State { get; }

			public LogFormLoggerScope(TState state)
			{
				State = state;
			}

			public void Dispose()
			{
			}
		}
	}
}
