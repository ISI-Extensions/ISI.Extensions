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

namespace ISI.Extensions
{
	public partial class IO
	{
		public partial class Path
		{
			private static IEnumerable<FileNameMask> DefaultFileNameMasks =>
			[
				new($"{{{FileNameMask.FilePrefix}", FileNameMask.FileNameMaskType.KeyValue, "file", "file retrieval"),
				new($"{{{FileNameMask.FirstExistingDirectoryPrefix}", FileNameMask.FileNameMaskType.FirstExistingDirectory, "firstExistingDirectory", "First Existing Directory"),
				new("{YYYYMMDD}", FileNameMask.FileNameMaskType.DateTimeMask, "yyyyMMdd", "Date Stamp: Year Month Day"),
				new("{YYYYMMDD.HHmmssfff}", FileNameMask.FileNameMaskType.DateTimeMask, "yyyyMMdd.HHmmssfff", "Date Time Stamp: Year Month Day.24Hour minute second millisecond"),
				new("{Now}", FileNameMask.FileNameMaskType.DateTimeMask, "yyyyMMdd.HHmmssfff", "Date Time Stamp: Year Month Day.24Hour minute second millisecond"),
				new("{YYYY}", FileNameMask.FileNameMaskType.DateTimeMask, "yyyy", "Date Stamp: Year"),
				new("{YY}", FileNameMask.FileNameMaskType.DateTimeMask, "yy", "Date Stamp: Year last two digits"),
				new("{MM}", FileNameMask.FileNameMaskType.DateTimeMask, "MM", "Date Stamp: Month"),
				new("{WW}", FileNameMask.FileNameMaskType.DateTimeMask, "ww", "Date Stamp: Week"),
				new("{DD}", FileNameMask.FileNameMaskType.DateTimeMask, "dd", "Date Stamp: Day"),
				new("{HH}", FileNameMask.FileNameMaskType.DateTimeMask, "HH", "Time Stamp: 24Hour"),
				new("{hh}", FileNameMask.FileNameMaskType.DateTimeMask, "hh", "Time Stamp: Hour"),
				new("{mm}", FileNameMask.FileNameMaskType.DateTimeMask, "mm", "Time Stamp: Minute"),
				new("{ss}", FileNameMask.FileNameMaskType.DateTimeMask, "ss", "Time Stamp: Second"),
				new("{fff}", FileNameMask.FileNameMaskType.DateTimeMask, "fff", "Time Stamp: Millisecond"),
				new("{TempDirectory}", FileNameMask.FileNameMaskType.StringReplacement, System.IO.Path.GetTempPath, "Temp Directory"),
				new("{PathRoot}", FileNameMask.FileNameMaskType.StringReplacement, ISI.Extensions.IO.Path.PathRoot, "PathRoot Directory"),
				new("{DataRoot}", FileNameMask.FileNameMaskType.StringReplacement, ISI.Extensions.IO.Path.DataRoot, "DataRoot Directory"),
				new("{MachineName}", FileNameMask.FileNameMaskType.StringReplacement, System.Environment.MachineName.ToLower(), "MachineName"),
				new("{LocalApplicationData}", FileNameMask.FileNameMaskType.StringReplacement, () => System.Environment.GetEnvironmentVariable("LOCALAPPDATA"), "LocalApplicationData"),
				new("{ApplicationData}", FileNameMask.FileNameMaskType.StringReplacement, () => System.Environment.GetEnvironmentVariable("APPDATA"), "ApplicationData"),
				new("{ApplicationName}", FileNameMask.FileNameMaskType.StringReplacement, () => System.Reflection.Assembly.GetEntryAssembly().FullName.Split([',' ]).First(), "Application Name"),
			];

			public static string GetFileNameDeMasked(string fileName, ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper = null)
			{
				return GetFileNameDeMasked(fileName, () => (dateTimeStamper ?? new ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper()).CurrentDateTimeUtc(), null);
			}

			public static string GetFileNameDeMasked(string fileName, IEnumerable<FileNameMask> additionalMasks, ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper = null)
			{
				return GetFileNameDeMasked(fileName, () => (dateTimeStamper ?? new ISI.Extensions.DateTimeStamper.LocalMachineDateTimeStamper()).CurrentDateTimeUtc(), additionalMasks);
			}

			public static string GetFileNameDeMasked(string fileName, DateTime? dateTimeStamp, IEnumerable<FileNameMask> additionalMasks = null)
			{
				return GetFileNameDeMasked(fileName, () => dateTimeStamp, additionalMasks);
			}

			public static string GetFileNameDeMasked(string fileName, Func<DateTime?> getDateTimeStamp, IEnumerable<FileNameMask> additionalMasks)
			{
				var masks = new Dictionary<string, FileNameMask>();

				foreach (var fileNameMask in DefaultFileNameMasks)
				{
					if (masks.ContainsKey(fileNameMask.Key))
					{
						masks.Remove(fileNameMask.Key);
					}

					if ((getDateTimeStamp != null) || (fileNameMask.MaskType != FileNameMask.FileNameMaskType.DateTimeMask))
					{
						masks.Add(fileNameMask.Key, fileNameMask);
					}
				}

				if (additionalMasks != null)
				{
					foreach (var fileNameMask in additionalMasks)
					{
						var key = fileNameMask.Key;

						if (!key.StartsWith("{"))
						{
							key = "{" + key;
						}

						if (!key.EndsWith("}"))
						{
							key += "}";
						}

						if (masks.ContainsKey(key))
						{
							masks.Remove(key);
						}

						masks.Add(key, fileNameMask);
					}
				}

				DateTime? dateTimeStamp = null;

				var fileNameDemasked = masks.Aggregate(fileName, (current, fileNameMask) => fileNameMask.Value.Process(current, () =>
				{
					if (!dateTimeStamp.HasValue && (getDateTimeStamp != null))
					{
						dateTimeStamp = getDateTimeStamp();
					}

					return dateTimeStamp;
				}));

				var indexOfStartOfEnvironmentMask = fileNameDemasked.IndexOf("{env:", StringComparison.InvariantCultureIgnoreCase);
				while (indexOfStartOfEnvironmentMask >= 0)
				{
					var indexOfEndOfEnvironmentMask = fileNameDemasked.IndexOf("}", indexOfStartOfEnvironmentMask, StringComparison.InvariantCultureIgnoreCase);

					var environmentMask = fileNameDemasked.Substring(indexOfStartOfEnvironmentMask, indexOfEndOfEnvironmentMask - indexOfStartOfEnvironmentMask + 1);

					var environmentKey = environmentMask.TrimStart("{env:", StringComparison.InvariantCultureIgnoreCase).TrimEnd("}").Trim();

					var environmentValue = string.Empty;
					try
					{
						environmentValue = Environment.GetEnvironmentVariable(environmentKey);
					}
					catch (Exception exception)
					{
					}

					fileNameDemasked = fileNameDemasked.Replace(environmentMask, environmentValue);

					indexOfStartOfEnvironmentMask = fileNameDemasked.IndexOf("{env:", StringComparison.InvariantCultureIgnoreCase);
				}
				
				return fileNameDemasked;
			}
		}
	}
}