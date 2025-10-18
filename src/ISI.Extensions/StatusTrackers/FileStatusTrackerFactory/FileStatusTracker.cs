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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ISI.Extensions.StatusTrackers
{
	public partial class FileStatusTrackerFactory
	{
		public delegate string GetStatusTrackerFileNameDelegate(string statusTrackerKey, string fileNameExtension);
		public delegate string GetStatusTrackerKeyValueFileNameDelegate(string statusTrackerKey, string key);

		public class FileStatusTracker : IStatusTracker
		{
			public string StatusTrackerKey { get; }
			public event ISI.Extensions.StatusTrackers.OnStatusChange OnStatusChangeEvents;
			public event ISI.Extensions.StatusTrackers.OnLogUpdate OnLogUpdateEvents;
			public event ISI.Extensions.StatusTrackers.OnAddToLog OnAddToLogEvents;
			public event ISI.Extensions.StatusTrackers.OnFinished OnFinishedEvents;

			protected Configuration Configuration { get; }
			protected Microsoft.Extensions.Logging.ILogger Logger { get; }
			protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

			protected GetStatusTrackerFileNameDelegate GetStatusTrackerFileName { get; }
			protected GetStatusTrackerKeyValueFileNameDelegate GetStatusTrackerKeyValueFileName { get; }

			private const int DefaultBufferSize = 4096;

			public int MaxLogSize { get; set; } = 100000;

			protected System.IO.FileStream RunningFileStream { get; set; }

			protected string LockFullName { get; set; }
			protected string RunningFullName { get; set; }
			protected string CaptionFullName { get; set; }
			protected string PercentFullName { get; set; }
			protected string LogFullName { get; set; }
			protected string FinishedFullName { get; set; }

			public bool Successful { get; protected set; }
			public bool Finished { get; protected set; }

			private string _caption = string.Empty;
			public string Caption
			{
				get => _caption;
				set
				{
					ISI.Extensions.Locks.FileLock.Lock(LockFullName, () =>
					{
						if (System.IO.File.Exists(CaptionFullName))
						{
							System.IO.File.Delete(CaptionFullName);
						}

						System.IO.File.WriteAllText(CaptionFullName, value);

						_caption = value;
						OnStatusChangeEvents?.Invoke(Caption, Percent);
					});
				}
			}

			private int _percent = 0;
			public int Percent
			{
				get => _percent;
				set
				{
					ISI.Extensions.Locks.FileLock.Lock(LockFullName, () =>
					{
						if (value < 0)
						{
							value = 0;
						}

						if (value > 100)
						{
							value = 100;
						}

						if (System.IO.File.Exists(PercentFullName))
						{
							System.IO.File.Delete(PercentFullName);
						}

						System.IO.File.WriteAllText(PercentFullName, $"{value}");

						_percent = value;
						OnStatusChangeEvents?.Invoke(Caption, Percent);
					});
				}
			}

			public void SetCaptionPercent(string caption, int percent)
			{
				ISI.Extensions.Locks.FileLock.Lock(LockFullName, () =>
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
				});
			}

			public FileStatusTracker(
				string statusTrackerKey,
				Configuration configuration,
				Microsoft.Extensions.Logging.ILogger logger,
				ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
				GetStatusTrackerFileNameDelegate getStatusTrackerFileName,
				GetStatusTrackerKeyValueFileNameDelegate getStatusTrackerKeyValueFileName)
			{
				StatusTrackerKey = statusTrackerKey;
				Configuration = configuration;
				Logger = logger;
				DateTimeStamper = dateTimeStamper;
				GetStatusTrackerFileName = getStatusTrackerFileName;
				GetStatusTrackerKeyValueFileName = getStatusTrackerKeyValueFileName;

				LockFullName = GetStatusTrackerFileName(StatusTrackerKey, LockFileNameExtension);

				RunningFullName = GetStatusTrackerFileName(StatusTrackerKey, RunningFileNameExtension);
				FinishedFullName = GetStatusTrackerFileName(StatusTrackerKey, FinishedFileNameExtension);
				if (!System.IO.File.Exists(RunningFullName) && !System.IO.File.Exists(FinishedFullName))
				{
					try
					{
						RunningFileStream = new(RunningFullName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read, DefaultBufferSize, System.IO.FileOptions.DeleteOnClose);
						RunningFileStream.WriteByte(255);
						RunningFileStream.Flush();
					}
					catch (Exception exception)
					{
						throw new($"Cannot create file: \"{RunningFullName}\"", exception);
					}
				}

				CaptionFullName = GetStatusTrackerFileName(StatusTrackerKey, CaptionFileNameExtension);
				var caption = string.Empty;
				if (System.IO.File.Exists(CaptionFullName))
				{
					caption = System.IO.File.ReadAllText(CaptionFullName);
				}

				if (!string.IsNullOrWhiteSpace(caption))
				{
					_caption = caption;
				}

				PercentFullName = GetStatusTrackerFileName(StatusTrackerKey, PercentFileNameExtension);
				var percent = 0;
				if (System.IO.File.Exists(PercentFullName))
				{
					percent = ((System.IO.File.ReadAllLines(PercentFullName) ?? []).FirstOrDefault() ?? string.Empty).ToInt();
				}

				if (percent > 0)
				{
					_percent = percent;
				}

				LogFullName = GetStatusTrackerFileName(StatusTrackerKey, LogFileNameExtension);
				var logEntries = (IEnumerable<IStatusTrackerLogEntry>)null;

				if (System.IO.File.Exists(LogFullName))
				{
					logEntries = System.IO.File.ReadAllText(LogFullName)
						.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
						.Select(l => l.Split(['\t'], 2)).
						Select(logParts => new StatusTrackerLogEntry()
						{
							DateTimeStamp = logParts[0].ToDateTime(),
							Description = System.Web.HttpUtility.UrlDecode(logParts[1]),
						});
				}
				if (logEntries.NullCheckedAny())
				{
					ISI.Extensions.Locks.FileLock.Lock(LockFullName, () =>
					{
						_logEntries.AddRange(logEntries);
					});
				}
			}

			private readonly List<IStatusTrackerLogEntry> _logEntries = [];

			public void AddToLog(System.Exception exception)
			{
				AddToLog(string.Empty, exception);
			}

			public void AddToLog(string logEntry, System.Exception exception)
			{
				AddToLog(DateTimeStamper.CurrentDateTime(), LogEntryLevel.Error, string.Format("Error: {1}{0}{2}", Environment.NewLine, logEntry, exception.ErrorMessageFormatted("  ")));
			}

			public void AddToLog(string logEntry)
			{
				AddToLog(DateTimeStamper.CurrentDateTime(), logEntry);
			}

			public void AddToLog(LogEntryLevel logEntryLevel, string logEntry)
			{
				AddToLog(DateTimeStamper.CurrentDateTime(), logEntryLevel, logEntry);
			}

			public void AddToLog(DateTime dateTimeStamp, string logEntry)
			{
				AddToLog(dateTimeStamp, LogEntryLevel.Information, logEntry);
			}

			public void AddToLog(DateTime dateTimeStamp, LogEntryLevel logEntryLevel, string logEntry)
			{
				AddToLog([
					new ISI.Extensions.StatusTrackers.StatusTrackerLogEntry()
					{
						DateTimeStamp = dateTimeStamp,
						LogEntryLevel = logEntryLevel,
						Description = logEntry.TrimEnd(' ', '\r', '\n'),
					}
				]);
			}

			public void AddToLog(IEnumerable<IStatusTrackerLogEntry> logEntries)
			{
				foreach (var logEntry in logEntries)
				{
					ISI.Extensions.Locks.FileLock.Lock(LockFullName, () =>
					{
						_logEntries.Add(logEntry);

						System.IO.File.AppendAllText(LogFullName, $"{logEntry.DateTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise)}\t{System.Web.HttpUtility.UrlEncode(logEntry.Description)}{Environment.NewLine}");
					});

					OnAddToLogEvents?.Invoke(logEntry);
				}

				OnLogUpdateEvents?.Invoke(_logEntries);
			}

			public void SetOnFinished(ISI.Extensions.StatusTrackers.OnFinished onFinished)
			{
				OnFinishedEvents += onFinished;
			}

			public void Finish(bool successful)
			{
				if (!Finished)
				{
					Successful = successful;
					Finished = true;

					System.IO.File.WriteAllText($"{FinishedFullName}.tmp", $"Success:\t{successful.TrueFalse()}");

					System.IO.File.Move($"{FinishedFullName}.tmp", FinishedFullName);


					RunningFileStream?.Close();
					RunningFileStream = null;

					if (System.IO.File.Exists(CaptionFullName))
					{
						System.IO.File.Delete(CaptionFullName);
					}

					if (System.IO.File.Exists(PercentFullName))
					{
						System.IO.File.Delete(PercentFullName);
					}
				}

				OnFinishedEvents?.Invoke(successful);
			}

			public IEnumerable<IStatusTrackerLogEntry> GetLogEntries()
			{
				if (_logEntries.Count > MaxLogSize)
				{
					return _logEntries.Skip(_logEntries.Count - MaxLogSize).ToArray();
				}

				return _logEntries.ToArray();
			}

			private IDictionary<string, string> _keyValues = null;
			public IDictionary<string, string> KeyValues => _keyValues ??= new FileStatusTrackerDictionary(StatusTrackerKey, GetStatusTrackerKeyValueFileName);

			public void Dispose()
			{
				if (RunningFileStream != null)
				{
					Finish(false);
				}
			}
		}

		internal class FileStatusTrackerDictionary : IDictionary<string, string>
		{
			protected string StatusTrackerKey { get; }
			protected GetStatusTrackerKeyValueFileNameDelegate GetStatusTrackerKeyValueFileName { get; }

			internal FileStatusTrackerDictionary(
				string statusTrackerKey,
				GetStatusTrackerKeyValueFileNameDelegate getStatusTrackerKeyValueFileName)
			{
				StatusTrackerKey = statusTrackerKey;
				GetStatusTrackerKeyValueFileName = getStatusTrackerKeyValueFileName;
			}

			protected string[] GetKeys()
			{
				var fullNamePrefix = GetStatusTrackerKeyValueFileName(StatusTrackerKey, string.Empty).TrimEnd(FileStatusTrackerFactory.KeyValueFileNameExtension, StringComparison.InvariantCultureIgnoreCase);
				var directory = System.IO.Path.GetDirectoryName(fullNamePrefix);
				var fileNamePrefix = System.IO.Path.GetFileName(fullNamePrefix);

				var fullNames = System.IO.Directory.GetFiles(directory, $"*{FileStatusTrackerFactory.KeyValueFileNameExtension}", System.IO.SearchOption.TopDirectoryOnly);

				return fullNames.ToNullCheckedArray(fullName => System.IO.Path.GetFileName(fullName).TrimStart(fileNamePrefix, StringComparison.InvariantCultureIgnoreCase).TrimEnd(FileStatusTrackerFactory.KeyValueFileNameExtension, StringComparison.InvariantCultureIgnoreCase), NullCheckCollectionResult.Empty);
			}

			protected string GetValue(string key)
			{
				var fullName = GetStatusTrackerKeyValueFileName(StatusTrackerKey, key);

				if (System.IO.File.Exists(fullName))
				{
					return System.IO.File.ReadAllText(fullName);
				}

				return null;
			}

			protected IDictionary<string, string> GetKeyValues()
			{
				var keys = GetKeys();
				var keyValues = new Dictionary<string, string>();

				foreach (var key in keys)
				{
					keyValues.Add(key, GetValue(key));
				}

				return keyValues;
			}

			public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => GetKeyValues().GetEnumerator();
			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

			public void Add(KeyValuePair<string, string> item) => Add(item.Key, item.Value);
			public void Add(string key, string value)
			{
				var fullName = GetStatusTrackerKeyValueFileName(StatusTrackerKey, key);

				if (System.IO.File.Exists(fullName))
				{
					throw new System.ArgumentException("An element with the same key already exists");
				}

				System.IO.File.WriteAllText(fullName, value);
			}

			public bool Contains(KeyValuePair<string, string> item) => ContainsKey(item.Key);
			public bool ContainsKey(string key)
			{
				var fullName = GetStatusTrackerKeyValueFileName(StatusTrackerKey, key);

				if (System.IO.File.Exists(fullName))
				{
					return true;
				}

				return false;
			}

			public bool TryGetValue(string key, out string value)
			{
				var fullName = GetStatusTrackerKeyValueFileName(StatusTrackerKey, key);

				if (System.IO.File.Exists(fullName))
				{
					value = System.IO.File.ReadAllText(fullName);

					return true;
				}

				value = null;
				return false;
			}

			public string this[string key]
			{
				get
				{
					if (TryGetValue(key, out var value))
					{
						return value;
					}

					throw new System.Collections.Generic.KeyNotFoundException("key");
				}
				set
				{
					var fullName = GetStatusTrackerKeyValueFileName(StatusTrackerKey, key);

					System.IO.File.WriteAllText(fullName, value);
				}
			}

			public ICollection<string> Keys => GetKeys();

			public ICollection<string> Values => GetKeyValues().Values;

			public int Count => GetKeys().Length;

			public bool IsReadOnly => false;

			public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => GetKeyValues().CopyTo(array, arrayIndex);

			public bool Remove(KeyValuePair<string, string> item) => Remove(item.Key);
			public bool Remove(string key)
			{
				var fullName = GetStatusTrackerKeyValueFileName(StatusTrackerKey, key);

				if (System.IO.File.Exists(fullName))
				{
					System.IO.File.Delete(fullName);

					return true;
				}

				return false;
			}

			public void Clear()
			{
				var fullNamePrefix = GetStatusTrackerKeyValueFileName(StatusTrackerKey, string.Empty).TrimEnd(FileStatusTrackerFactory.KeyValueFileNameExtension, StringComparison.InvariantCultureIgnoreCase);
				var directory = System.IO.Path.GetDirectoryName(fullNamePrefix);
				var fileNamePrefix = System.IO.Path.GetFileName(fullNamePrefix);

				var fullNames = System.IO.Directory.GetFiles(directory, $"*{FileStatusTrackerFactory.KeyValueFileNameExtension}", System.IO.SearchOption.TopDirectoryOnly);

				foreach (var fullName in fullNames)
				{
					System.IO.File.Delete(fullName);
				}
			}
		}
	}
}