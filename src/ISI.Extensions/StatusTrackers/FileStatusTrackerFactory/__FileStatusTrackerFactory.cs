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
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.StatusTrackers
{
	public partial class FileStatusTrackerFactory : IStatusTrackerFactory
	{
		public const string LockFileNameExtension = ".StatusTracker";
		public const string RunningFileNameExtension = ".Running.txt";
		public const string CaptionFileNameExtension = ".Caption.txt";
		public const string PercentFileNameExtension = ".Percent.txt";
		public const string LogFileNameExtension = ".Log.txt";
		public const string KeyValueFileNameExtension = ".KeyValue.txt";
		public const string FinishedFileNameExtension = ".Finished.txt";

		protected Configuration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }

		public FileStatusTrackerFactory(
			Configuration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;

			if (!System.IO.Directory.Exists(Configuration.FileStatusTrackerDirectory))
			{
				System.IO.Directory.CreateDirectory(Configuration.FileStatusTrackerDirectory);
			}
		}

		public string GetStatusTrackerStorageDirectoryName(string statusTrackerKey)
		{
			if (Guid.TryParse(statusTrackerKey, out var statusTrackerUuid))
			{
				statusTrackerKey = statusTrackerUuid.Formatted(GuidExtensions.GuidFormat.NoFormatting);
			}

			var subDirectories = statusTrackerKey.SplitIntoChunks(2, 5, false).ToList();
			subDirectories.Insert(0, Configuration.FileStatusTrackerDirectory);

			var directoryName = System.IO.Path.Combine(subDirectories.ToArray());

			System.IO.Directory.CreateDirectory(directoryName);

			return directoryName;
		}

		public string GetStatusTrackerFileName(string statusTrackerKey, string fileNameExtension)
		{
			var directory = GetStatusTrackerStorageDirectoryName(statusTrackerKey);

			return System.IO.Path.Combine(directory, $"{statusTrackerKey}{fileNameExtension}");
		}

		public void MigrateStatusTrackers()
		{
			var fileExtensions =
				new[]
				{
					LockFileNameExtension,
					RunningFileNameExtension,
					CaptionFileNameExtension,
					PercentFileNameExtension,
					LogFileNameExtension,
					KeyValueFileNameExtension,
					FinishedFileNameExtension,
				};

			{
				var fullNames = System.IO.Directory.GetFiles(Configuration.FileStatusTrackerDirectory, "*.txt", System.IO.SearchOption.AllDirectories)
					.Where(fullName =>
					{
						var pieces = System.IO.Path.GetDirectoryName(fullName).TrimStart(Configuration.FileStatusTrackerDirectory, StringComparison.InvariantCultureIgnoreCase).Split(['\\', '/']);

						return pieces.Any(piece => piece.IndexOf('-', 0) >= 0);
					});

				var directories = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
				
				foreach (var fullName in fullNames)
				{
					directories.Add(System.IO.Path.GetDirectoryName(fullName));
					
					var fileName = System.IO.Path.GetFileName(fullName);

					var statusTrackerKey = fileName;
					foreach (var fileExtension in fileExtensions)
					{
						statusTrackerKey = statusTrackerKey.TrimEnd(fileExtension, StringComparison.InvariantCultureIgnoreCase);
					}

					Logger.LogInformation($"Migrate statusTrackerKey: {statusTrackerKey} File: {fileName}");

					var statusTrackerStorageDirectoryName = GetStatusTrackerStorageDirectoryName(statusTrackerKey);

					Logger.LogInformation($"               Directory: {statusTrackerStorageDirectoryName}");

					System.IO.File.Move(fullName, System.IO.Path.Combine(statusTrackerStorageDirectoryName, fileName));
				}

				foreach (var directory in directories)
				{
					System.IO.Directory.Delete(directory);
				}
			}

			{
				var fullNames = System.IO.Directory.GetFiles(Configuration.FileStatusTrackerDirectory, "*.txt", System.IO.SearchOption.TopDirectoryOnly);

				foreach (var fullName in fullNames)
				{
					var fileName = System.IO.Path.GetFileName(fullName);

					var statusTrackerKey = fileName;
					foreach (var fileExtension in fileExtensions)
					{
						statusTrackerKey = statusTrackerKey.TrimEnd(fileExtension, StringComparison.InvariantCultureIgnoreCase);
					}

					Logger.LogInformation($"Migrate statusTrackerKey: {statusTrackerKey} File: {fileName}");

					var statusTrackerStorageDirectoryName = GetStatusTrackerStorageDirectoryName(statusTrackerKey);

					Logger.LogInformation($"               Directory: {statusTrackerStorageDirectoryName}");

					System.IO.File.Move(fullName, System.IO.Path.Combine(statusTrackerStorageDirectoryName, fileName));
				}
			}
		}

		public bool TryGetStatusTracker(string statusTrackerKey, out IStatusTracker statusTracker)
		{
			var logFullName = GetStatusTrackerFileName(statusTrackerKey, LogFileNameExtension);

			if (System.IO.File.Exists(logFullName))
			{
				statusTracker = new FileStatusTracker(statusTrackerKey, Configuration, Logger, DateTimeStamper, GetStatusTrackerFileName, GetStatusTrackerKeyValueFileName, true);
				return true;
			}

			statusTracker = null;
			return false;
		}

		public IStatusTracker TryGetOrAddStatusTracker(string statusTrackerKey, bool leaveRunning = false)
		{
			if (TryGetStatusTracker(statusTrackerKey, out var statusTracker))
			{
				return statusTracker;
			}

			return CreateStatusTracker(statusTrackerKey, leaveRunning);
		}

		public virtual IStatusTracker CreateStatusTracker(string statusTrackerKey, string jobKey, string description, string lastExecutionUrl, bool leaveRunning = false) => CreateStatusTracker(statusTrackerKey, leaveRunning);
		public IStatusTracker CreateStatusTracker(string statusTrackerKey, bool leaveRunning = false)
		{
			return new FileStatusTracker(statusTrackerKey, Configuration, Logger, DateTimeStamper, GetStatusTrackerFileName, GetStatusTrackerKeyValueFileName, leaveRunning);
		}

		public string GetStatusTrackerKeyValueFileName(string statusTrackerKey, string key)
		{
			return GetStatusTrackerFileName(statusTrackerKey, $"-{key}{FileStatusTrackerFactory.KeyValueFileNameExtension}");
		}

		public bool IsRunning(string statusTrackerKey)
		{
			return System.IO.File.Exists(GetStatusTrackerFileName(statusTrackerKey, RunningFileNameExtension));
		}

		public bool IsFinished(string statusTrackerKey)
		{
			return System.IO.File.Exists(GetStatusTrackerFileName(statusTrackerKey, FinishedFileNameExtension));
		}

		public bool IsSuccessful(string statusTrackerKey)
		{
			if (!IsFinished(statusTrackerKey))
			{
				return false;
			}

			var fileName = GetStatusTrackerFileName(statusTrackerKey, FinishedFileNameExtension);

			var lockFullName = GetStatusTrackerFileName(statusTrackerKey, LockFileNameExtension);

			var isSuccessful = false;

			ISI.Extensions.Locks.FileLock.Lock(lockFullName, () =>
			{
				var content = System.IO.File.ReadAllText(fileName).Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);

				isSuccessful = content.FirstOrDefault().Split(['\t'], StringSplitOptions.None)[1].ToBoolean();
			});

			return isSuccessful;
		}

		public bool TryStatusTrackerGetKeyValue(string statusTrackerKey, string key, out string value)
		{
			var fullName = GetStatusTrackerKeyValueFileName(statusTrackerKey, key);

			if (System.IO.File.Exists(fullName))
			{
				var lockFullName = GetStatusTrackerFileName(statusTrackerKey, LockFileNameExtension);

				var content = (string)null;

				ISI.Extensions.Locks.FileLock.Lock(lockFullName, () =>
				{
					content = System.IO.File.ReadAllText(fullName);
				});

				value = content;
				return true;
			}

			value = null;
			return false;
		}

		public IEnumerable<string> GetActiveStatusTrackerKeys()
		{
			var statusTrackerKeys = new HashSet<string>();

			foreach (var fileName in System.IO.Directory.GetFiles(Configuration.FileStatusTrackerDirectory, $"*{RunningFileNameExtension}", System.IO.SearchOption.TopDirectoryOnly))
			{
				statusTrackerKeys.Add(System.IO.Path.GetFileName(fileName.Substring(0, fileName.Length - RunningFileNameExtension.Length)));
			}

			return statusTrackerKeys;
		}

		public IStatusTrackerSnapshot GetStatusTrackerSnapshot(string statusTrackerKey)
		{
			string ReadFile(string fileName)
			{
				if (System.IO.File.Exists(fileName))
				{
					try
					{
						using (var fileStream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite))
						{
							using (var stream = new System.IO.StreamReader(fileStream))
							{
								return stream.ReadToEnd();
							}
						}
					}
#pragma warning disable CS0168 // Variable is declared but never used
					catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
					{
					}
				}

				return string.Empty;
			}

			var lockFullName = GetStatusTrackerFileName(statusTrackerKey, LockFileNameExtension);

			var statusTrackerSnapshot = (IStatusTrackerSnapshot)null;

			ISI.Extensions.Locks.FileLock.Lock(lockFullName, () =>
			{
				var caption = ReadFile(GetStatusTrackerFileName(statusTrackerKey, CaptionFileNameExtension));
				var percent = ReadFile(GetStatusTrackerFileName(statusTrackerKey, PercentFileNameExtension)).ToInt();
				var logEntries = ReadFile(GetStatusTrackerFileName(statusTrackerKey, LogFileNameExtension))
					.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
					.Select(l => l.Split(['\t'], 2)).Select(logParts => new StatusTrackerLogEntry()
					{
						DateTimeStamp = logParts[0].ToDateTime(),
						Description = System.Web.HttpUtility.UrlDecode(logParts[1]),
					});

				statusTrackerSnapshot = new StatusTrackerSnapshot(caption, percent, logEntries);
			});

			return statusTrackerSnapshot;
		}

		public IStatusTrackerSnapshot[] GetStatusTrackerSnapshots(IEnumerable<string> statusTrackerKeys)
		{
			var statusTrackers = new List<IStatusTrackerSnapshot>();

			foreach (var statusTrackerKey in statusTrackerKeys)
			{
				var statusTracker = GetStatusTrackerSnapshot(statusTrackerKey);

				if (statusTracker != null)
				{
					statusTrackers.Add(statusTracker);
				}
			}

			return statusTrackers.ToArray();
		}

		public IStatusTrackerSnapshot[] GetActiveStatusTrackerSnapshots() => GetStatusTrackerSnapshots(GetActiveStatusTrackerKeys());

		public void DeleteStatusTracker(string statusTrackerKey)
		{
			foreach (var fileNameExtension in new[] { RunningFileNameExtension, CaptionFileNameExtension, PercentFileNameExtension, LogFileNameExtension, FinishedFileNameExtension })
			{
				var fileName = GetStatusTrackerFileName(statusTrackerKey, fileNameExtension);
				if (System.IO.File.Exists(fileName))
				{
					System.IO.File.Delete(fileName);
				}
			}
		}
	}
}
