#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.StatusTrackers
{
	public partial class FileStatusTrackerFactory
	{
		public delegate string GetStatusTrackerFileNameDelegate(string statusTrackerKey, string fileNameExtension);

		public class FileStatusTracker : IStatusTracker, IDisposable
		{
			public string StatusTrackerKey { get; }
			public event ISI.Extensions.StatusTrackers.OnStatusChange OnStatusChangeEvents;
			public event ISI.Extensions.StatusTrackers.OnLogUpdate OnLogUpdateEvents;
			public event ISI.Extensions.StatusTrackers.OnAddToLog OnAddToLogEvents;

			protected Configuration Configuration { get; }
			protected Microsoft.Extensions.Logging.ILogger Logger { get; }
			protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

			private GetStatusTrackerFileNameDelegate _getStatusTrackerFileName { get; }

			private const int DefaultBufferSize = 4096;

			public int MaxLogSize { get; set; } = 100000;

			protected readonly object SyncLock = new();

			protected System.IO.FileStream RunningFileStream { get; set; }

			protected System.IO.FileStream CaptionFileStream { get; set; }
			protected System.IO.StreamWriter CaptionStreamWriter { get; set; }

			protected System.IO.FileStream PercentFileStream { get; set; }
			protected System.IO.StreamWriter PercentStreamWriter { get; set; }

			protected System.IO.FileStream LogFileStream { get; set; }
			protected System.IO.StreamWriter LogStreamWriter { get; set; }

			protected bool Finished { get; set; }

			private string _caption = string.Empty;
			public string Caption
			{
				get => _caption;
				set
				{
					lock (SyncLock)
					{
						CaptionFileStream.Position = 0;
						CaptionStreamWriter.Write(value);
						CaptionStreamWriter.Flush();

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
					lock (SyncLock)
					{
						if (value < 0)
						{
							value = 0;
						}

						if (value > 100)
						{
							value = 100;
						}

						PercentFileStream.Position = 0;
						PercentStreamWriter.Write(value);
						PercentStreamWriter.Flush();

						_percent = value;
						OnStatusChangeEvents?.Invoke(Caption, Percent);
					}
				}
			}
			
			public void SetCaptionPercent(string caption, int percent)
			{
				lock (SyncLock)
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

			public string GetStatusTrackerFileName(string fileNameExtension) => _getStatusTrackerFileName(StatusTrackerKey, fileNameExtension);

			public FileStatusTracker(
				string statusTrackerKey,
				Configuration configuration,
				Microsoft.Extensions.Logging.ILogger logger,
				ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
				GetStatusTrackerFileNameDelegate getStatusTrackerFileName)
			{
				StatusTrackerKey = statusTrackerKey;
				Configuration = configuration;
				Logger = logger;
				DateTimeStamper = dateTimeStamper;
				_getStatusTrackerFileName = getStatusTrackerFileName;

				{
					var fileName = GetStatusTrackerFileName(RunningFileNameExtension);
					try
					{
						RunningFileStream = new(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read, DefaultBufferSize, System.IO.FileOptions.DeleteOnClose);
						RunningFileStream.WriteByte(255);
						RunningFileStream.Flush();
					}
					catch (Exception exception)
					{
						throw new(string.Format("Cannot create file: \"{0}\"", fileName), exception);
					}
				}

				{
					var fileName = GetStatusTrackerFileName(CaptionFileNameExtension);
					try
					{
						CaptionFileStream = new(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read, DefaultBufferSize, System.IO.FileOptions.RandomAccess);
						CaptionStreamWriter = new(CaptionFileStream);
					}
					catch (Exception exception)
					{
						throw new(string.Format("Cannot create file: \"{0}\"", fileName), exception);
					}
				}

				{
					var fileName = GetStatusTrackerFileName(PercentFileNameExtension);
					try
					{
						PercentFileStream = new(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read, DefaultBufferSize, System.IO.FileOptions.RandomAccess);
						PercentStreamWriter = new(PercentFileStream);
					}
					catch (Exception exception)
					{
						throw new(string.Format("Cannot create file: \"{0}\"", fileName), exception);
					}
				}

				{
					var fileName = GetStatusTrackerFileName(LogFileNameExtension);
					try
					{
						LogFileStream = new(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read, DefaultBufferSize, System.IO.FileOptions.None);
						LogStreamWriter = new(LogFileStream);
					}
					catch (Exception exception)
					{
						throw new(string.Format("Cannot create file: \"{0}\"", fileName), exception);
					}
				}
			}

			private readonly List<IStatusTrackerLogEntry> _logEntries = new();

			public void AddToLog(System.Exception exception)
			{
				AddToLog(string.Empty, exception);
			}

			public void AddToLog(string logEntry, System.Exception exception)
			{
				AddToLog(DateTimeStamper.CurrentDateTime(), string.Format("Error: {1}{0}{2}", Environment.NewLine, logEntry, exception.ErrorMessageFormatted("  ")));
			}

			public void AddToLog(string logEntry)
			{
				AddToLog(DateTimeStamper.CurrentDateTime(), logEntry);
			}

			public void AddToLog(DateTime dateTimeStamp, string logEntry)
			{
				AddToLog(new IStatusTrackerLogEntry[]
				{
					new ISI.Extensions.StatusTrackers.StatusTrackerLogEntry()
					{
						DateTimeStamp = dateTimeStamp,
						Description = logEntry.TrimEnd(' ', '\r', '\n'),
					}
				});
			}

			public void AddToLog(IEnumerable<IStatusTrackerLogEntry> logEntries)
			{
				foreach (var logEntry in logEntries)
				{
					lock (SyncLock)
					{
						while (_logEntries.Count >= MaxLogSize)
						{
							_logEntries.RemoveAt(0);
						}

						_logEntries.Add(logEntry);

						LogStreamWriter.Write("{0}\t{1}\r\n", logEntry.DateTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise), System.Web.HttpUtility.UrlEncode(logEntry.Description));
						LogStreamWriter.Flush();
					}

					OnAddToLogEvents?.Invoke(logEntry);
				}

				OnLogUpdateEvents?.Invoke(_logEntries);
			}

			public void Finish(bool successful)
			{
				if (!Finished)
				{
					Finished = true;
					System.IO.File.WriteAllText(GetStatusTrackerFileName(FinishedFileNameExtension), string.Format("Success:\t{0}", successful.TrueFalse()));
				}
			}

			public IEnumerable<IStatusTrackerLogEntry> GetLogEntries()
			{
				return _logEntries.ToArray();
			}

			public void Dispose()
			{
				Finish(false);

				LogStreamWriter?.Close();
				LogStreamWriter = null;

				LogFileStream?.Close();
				LogFileStream = null;

				PercentStreamWriter?.Close();
				PercentStreamWriter = null;

				PercentFileStream?.Close();
				PercentFileStream = null;

				CaptionStreamWriter?.Close();
				CaptionStreamWriter = null;

				CaptionFileStream?.Close();
				CaptionFileStream = null;

				RunningFileStream?.Close();
				RunningFileStream = null;

				{
					var fileName = GetStatusTrackerFileName(CaptionFileNameExtension);
					if (System.IO.File.Exists(fileName))
					{
						System.IO.File.Delete(fileName);
					}
				}

				{
					var fileName = GetStatusTrackerFileName(PercentFileNameExtension);
					if (System.IO.File.Exists(fileName))
					{
						System.IO.File.Delete(fileName);
					}
				}
			}
		}
	}
}