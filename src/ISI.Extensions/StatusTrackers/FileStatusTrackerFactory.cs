﻿#region Copyright & License
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
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.StatusTrackers
{
	public partial class FileStatusTrackerFactory
	{
		public const string RunningFileNameExtension = ".Running.txt";
		public const string CaptionFileNameExtension = ".Caption.txt";
		public const string PercentFileNameExtension = ".Percent.txt";
		public const string LogFileNameExtension = ".Log.txt";
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

		public FileStatusTracker CreateStatusTracker(string statusTrackerKey)
		{
			return new(statusTrackerKey, Configuration, Logger, DateTimeStamper, GetFileName);
		}

		protected string GetFileName(string statusTrackerKey, string fileNameExtension)
		{
			return System.IO.Path.Combine(Configuration.FileStatusTrackerDirectory, string.Format("{0}{1}", statusTrackerKey, fileNameExtension));
		}
		
		public bool IsRunning(string statusTrackerKey)
		{
			return System.IO.File.Exists(GetFileName(statusTrackerKey, RunningFileNameExtension));
		}

		public bool IsFinished(string statusTrackerKey)
		{
			return System.IO.File.Exists(GetFileName(statusTrackerKey, FinishedFileNameExtension));
		}

		public bool IsSuccessful(string statusTrackerKey)
		{
			if (!IsFinished(statusTrackerKey))
			{
				return false;
			}

			var fileName = GetFileName(statusTrackerKey, FinishedFileNameExtension);

			var processes = ISI.Extensions.IO.Path.GetLockingProcesses(new[] { fileName });

			if (processes.Any())
			{
				System.Threading.Tasks.Parallel.ForEach(processes, process =>
				{
					process.WaitForExit(60 * 1000);
				});
			}

			var content = System.IO.File.ReadAllText(fileName).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

			return content.FirstOrDefault().Split(new[] { '\t' }, StringSplitOptions.None)[1].ToBoolean();
		}

		public HashSet<string> GetActiveStatusTrackerKeys()
		{
			var statusTrackerKeys = new HashSet<string>();

			foreach (var fileName in System.IO.Directory.GetFiles(Configuration.FileStatusTrackerDirectory, string.Format("*{0}", RunningFileNameExtension)))
			{
				statusTrackerKeys.Add(System.IO.Path.GetFileName(fileName.Substring(0, fileName.Length - RunningFileNameExtension.Length)));
			}

			return statusTrackerKeys;
		}

		public IStatusTrackerSnapshot GetStatusTrackerSnapshot(string statusTrackerKey)
		{
			Func<string, string> readFile = fileName =>
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
			};

			var caption = readFile(GetFileName(statusTrackerKey, CaptionFileNameExtension));
			var percent = readFile(GetFileName(statusTrackerKey, PercentFileNameExtension)).ToInt();
			var logEntries = readFile(GetFileName(statusTrackerKey, LogFileNameExtension))
				.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				.Select(l => l.Split(new[] { '\t' }, 2)).
				Select(logParts => new StatusTrackerLogEntry()
				{
					DateTimeStamp = logParts[0].ToDateTime(),
					Description = System.Web.HttpUtility.UrlDecode(logParts[1]),
				});

			return new StatusTrackerSnapshot(caption, percent, logEntries);
		}
		
		public IStatusTrackerSnapshot[] GetActiveStatusTrackerSnapshots(IEnumerable<string> statusTrackerKeys)
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

		public IStatusTrackerSnapshot[] GetActiveStatusTrackerSnapshots() => GetActiveStatusTrackerSnapshots(GetActiveStatusTrackerKeys());

		public void DeleteStatusTracker(string statusTrackerKey)
		{
			foreach (var fileNameExtension in new [] {RunningFileNameExtension,CaptionFileNameExtension,PercentFileNameExtension,LogFileNameExtension,FinishedFileNameExtension})
			{
				var fileName = GetFileName(statusTrackerKey, fileNameExtension);
				if (System.IO.File.Exists(fileName))
				{
					System.IO.File.Delete(fileName);
				}
			}
		}
	}
}
