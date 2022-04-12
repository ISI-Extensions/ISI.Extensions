#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
	public partial class FileSystem
	{
		public abstract class AbstractFileSystemInfo : IFileSystemInfo
		{
			private static readonly System.Text.RegularExpressions.Regex[] FileInformationRegexes = new System.Text.RegularExpressions.Regex[]
			{
				//Windows
				new(@"^(?<datetime>\d{2}-\d{2}-\d{2}\s+\d{2}:\d{2}(A|P)M)(?<type>\s+\<type\>)?\s+(?<size>\d*)\s+(?<filename>.+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase),
				//Unix
				new(@"^(?<type>[\-dbclps])(?<ownerpermissions>[-rwxsStT]{3})(?<grouppermissions>[-rwxsStT]{3})(?<otherpermissions>[-rwxsStT]{3})\s+\d*\s+(?<owner>[\w\-\.]+)\s+(?<group>[\w\-\.]+)\s+(?<size>\d+)\s+(?<datetime>\d{2,4}\-\d{2}\-\d{2,4}\s\d{2}\:\d{2}\:\d{2}\.\d{9}\s[-+\s]\d{4})\s+(?<filename>.+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase),
				new(@"^(?<type>[\-dbclps])(?<ownerpermissions>[-rwxsStT]{3})(?<grouppermissions>[-rwxsStT]{3})(?<otherpermissions>[-rwxsStT]{3})\s+\d*\s+(?<owner>[\w\-\.]+)\s+(?<group>[\w\-\.]+)\s+(?<size>\d+)\s+(?<datetime>((\w+\s+\d+)|(\d{2,4}[/\-]\d{2}[/\-]\d{2,4}))\s+\d{1,2}:\d{2}(\s*[AP]M)?)\s(?<filename>.+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase),
			};

			#region Parse
			public static IFileSystemInfo Parse(string attributedFilePath, string value)
			{
				IFileSystemInfo result = null;

				System.Text.RegularExpressions.Match match = null;

				if (!attributedFilePath.EndsWith("\\") && !attributedFilePath.EndsWith("/"))
				{
					attributedFilePath += (attributedFilePath.IndexOf("/") >= 0 ? "/" : "\\");
				}

				for (var regexIndex = 0; (((match == null) || !match.Success) && (regexIndex < FileInformationRegexes.Length)); regexIndex++)
				{
					match = FileInformationRegexes[regexIndex].Match(value);
				}

				if (match.Success)
				{
					switch (match.Groups["type"].Value.Trim())
					{
						case "d":
						case "<DIR>":
							result = new FileSystemInfoDirectory();
							break;
						case "l":
							result = (value.EndsWith("/") ? ((IFileSystemInfo)new FileSystemInfoSymbolicLinkDirectory()) : ((IFileSystemInfo)new FileSystemInfoSymbolicLinkFile()));
							break;
						default:
							result = new FileSystemInfoFile();
							break;
					}

					var fileName = match.Groups["filename"].Value;
					result.FileName = fileName.Split(new[] { "->" }, StringSplitOptions.None).FirstOrDefault().Trim().TrimEnd("/", "\\");

					if (result is IFileSystemInfoSymbolicLink symbolicLinkInfo)
					{
						symbolicLinkInfo.LinkedTo = fileName.Split(new[] { "->" }, StringSplitOptions.None).LastOrDefault().Trim();
					}

					{
						var dateTimeValue = match.Groups["datetime"].Value;

						if (!DateTime.TryParseExact(dateTimeValue, "MMM dd HH:mm", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out var modifyDateTime))
						{
							modifyDateTime = dateTimeValue.ToDateTime();
						}

						result.ModifiedDateTime = modifyDateTime;
					}

					result.Size = match.Groups["size"].Value.ToInt();

					result.FullName = attributedFilePath + result.FileName;
					result.AttributedFullName = attributedFilePath + result.FileName;
				}

				return result;
			}
			public static IFileSystemInfo Parse(System.IO.FileSystemInfo value)
			{
				IFileSystemInfo fileSystemInfo = (value is System.IO.DirectoryInfo) ? new FileSystemInfoDirectory() : new FileSystemInfoFile();

				fileSystemInfo.AttributedFullName = value.FullName;
				fileSystemInfo.FullName = value.FullName;
				fileSystemInfo.FileName = value.Name;
				fileSystemInfo.Size = (value as System.IO.FileInfo)?.Length;
				fileSystemInfo.CreatedDateTime = value.CreationTime;
				fileSystemInfo.ModifiedDateTime = value.LastWriteTime;

				return fileSystemInfo;
			}
			#endregion

			private string _attributedFullName = null;
			public virtual string AttributedFullName
			{
				get => _attributedFullName;
				set
				{
					_attributedFullName = value;

					if (string.IsNullOrEmpty(_fullName))
					{
						var pathInfo = FileSystem.GetFileSystemHandler(value).GetFileSystemPathInfo(value);

						_fullName = pathInfo.FullPathName;
					}

					if (string.IsNullOrEmpty(FileName))
					{
						FileName = FileSystem.GetFileName(value);
					}
				}
			}

			private string _fullName = null;
			public virtual string FullName
			{
				get => _fullName;
				set
				{
					_fullName = value;

					if (string.IsNullOrEmpty(FileName))
					{
						FileName = FileSystem.GetFileName(value);
					}
				}
			}

			public virtual string FileName { get; set; }

			public virtual DateTime? CreatedDateTime { get; set; }

			public virtual DateTime? ModifiedDateTime { get; set; }

			public virtual long? Size { get; set; }

			protected AbstractFileSystemInfo()
			{
				AttributedFullName = string.Empty;
				FullName = string.Empty;
				FileName = string.Empty;
				CreatedDateTime = null;
				ModifiedDateTime = null;
				Size = null;
			}

			protected AbstractFileSystemInfo(IFileSystemInfo fileSystemInfo)
			{
				AttributedFullName = fileSystemInfo.AttributedFullName;
				FullName = fileSystemInfo.FullName;
				FileName = fileSystemInfo.FileName;
				CreatedDateTime = fileSystemInfo.CreatedDateTime;
				ModifiedDateTime = fileSystemInfo.ModifiedDateTime;
				Size = fileSystemInfo.Size;
			}

			public override string ToString() => (string.IsNullOrEmpty(AttributedFullName) ? (string.IsNullOrEmpty(FullName) ? FileName : FullName) : AttributedFullName);
		}
	}
}
