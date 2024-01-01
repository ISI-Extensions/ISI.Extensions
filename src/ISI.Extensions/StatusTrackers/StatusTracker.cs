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
using Microsoft.Extensions.DependencyInjection;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public class StatusTracker : IStatusTracker
	{
		private static ISI.Extensions.DateTimeStamper.IDateTimeStamper _dateTimeStamper = null;
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper => _dateTimeStamper ??= (ISI.Extensions.ServiceLocator.Current?.GetService<ISI.Extensions.DateTimeStamper.IDateTimeStamper>() ?? new ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper());

		public event ISI.Extensions.StatusTrackers.OnStatusChange OnStatusChangeEvents;
		public event ISI.Extensions.StatusTrackers.OnLogUpdate OnLogUpdateEvents;
		public event ISI.Extensions.StatusTrackers.OnAddToLog OnAddToLogEvents;

		public int MaxLogSize { get; set; } = 100000;

		private string _caption = string.Empty;
		public string Caption
		{
			get => _caption;
			set
			{
				lock (_syncLock)
				{
					_caption = value;
					OnStatusChangeEvents?.Invoke(Caption, Percent);
				}
			}
		}

		private int _percent = 0;
		public int Percent
		{
			get => _percent;
			set
			{
				lock (_syncLock)
				{
					if (value < 0)
					{
						value = 0;
					}
					if (value > 100)
					{
						value = 100;
					}

					_percent = value;
					OnStatusChangeEvents?.Invoke(Caption, Percent);
				}
			}
		}

		public void SetCaptionPercent(string caption, int percent)
		{
			lock (_syncLock)
			{
				_caption = caption;

				if (percent < 0)
				{
					percent = 0;
				}
				if (percent > 100)
				{
					percent = 100;
				}
				_percent = percent;

				OnStatusChangeEvents?.Invoke(Caption, Percent);
			}
		}

		private readonly List<IStatusTrackerLogEntry> _logEntries = new();

		private readonly object _syncLock = new();

		public void AddToLog(System.Exception exception)
		{
			AddToLog(string.Empty, exception);
		}
		public void AddToLog(string logEntry, System.Exception exception)
		{
			AddToLog(DateTimeStamper.CurrentDateTime(), ISI.Extensions.StatusTrackers.LogEntryLevel.Error, string.Format("Error: {1}{0}{2}", Environment.NewLine, logEntry, exception.ErrorMessageFormatted("  ")));
		}
		public void AddToLog(string logEntry)
		{
			AddToLog(DateTimeStamper.CurrentDateTime(), logEntry);
		}
		public void AddToLog(ISI.Extensions.StatusTrackers.LogEntryLevel logEntryLevel, string logEntry)
		{
			AddToLog(DateTimeStamper.CurrentDateTime(), logEntryLevel, logEntry);
		}
		public void AddToLog(DateTime dateTimeStamp, string logEntry)
		{
			AddToLog(dateTimeStamp, ISI.Extensions.StatusTrackers.LogEntryLevel.Information, logEntry);
		}
		public void AddToLog(DateTime dateTimeStamp, ISI.Extensions.StatusTrackers.LogEntryLevel logEntryLevel, string logEntry)
		{
			AddToLog(new IStatusTrackerLogEntry[]
			{
				new ISI.Extensions.StatusTrackers.StatusTrackerLogEntry()
				{
					DateTimeStamp = dateTimeStamp,
					LogEntryLevel = logEntryLevel,
					Description = logEntry,
				}
			});
		}
		public void AddToLog(IEnumerable<IStatusTrackerLogEntry> logEntries)
		{
			lock (_syncLock)
			{
				foreach (var logEntry in logEntries)
				{
					while (_logEntries.Count >= MaxLogSize)
					{
						_logEntries.RemoveAt(0);
					}

					_logEntries.Add(logEntry);
					OnLogUpdateEvents?.Invoke(_logEntries);
					OnAddToLogEvents?.Invoke(logEntry);
				}
			}
		}
		
		public IEnumerable<IStatusTrackerLogEntry> GetLogEntries()
		{
			return _logEntries.ToArray();
		}
	}
}
