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

namespace ISI.Extensions.UnixFileSystem
{
	public abstract class UnixFileSystemProvider<TUnixFileSystemPathFile, TUnixFileSystemPathDirectory, TUnixFileSystemPathSymbolicLinkFile, TUnixFileSystemPathSymbolicLinkDirectory> : FileSystem.AbstractFileSystemProvider
		where TUnixFileSystemPathFile : IUnixFileSystemPathFile, new()
		where TUnixFileSystemPathDirectory : IUnixFileSystemPathDirectory, new()
		where TUnixFileSystemPathSymbolicLinkFile : IUnixFileSystemPathSymbolicLinkFile, new()
		where TUnixFileSystemPathSymbolicLinkDirectory : IUnixFileSystemPathSymbolicLinkDirectory, new()
	{
		internal static string _directorySeparator => "/";
		protected override string DirectorySeparator => _directorySeparator;

		private static readonly System.Text.RegularExpressions.Regex[] FileSystemRecordRegexs = new System.Text.RegularExpressions.Regex[]
		{
			//Windows
			new(@"^(?<datetime>\d{2}-\d{2}-\d{2}\s+\d{2}:\d{2}(A|P)M)(?:\s+)(?<type>(?:\<)(?:\w+)(?:\>))?\s+(?<size>\d*)\s+(?<filename>.+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase),
			//Unix
			new(@"^(?<type>[\-dbclps])(?<ownerpermissions>[-rwxsStT]{3})(?<grouppermissions>[-rwxsStT]{3})(?<otherpermissions>[-rwxsStT]{3})\s+\d*\s+(?<owner>[\w\-\.]+)\s+(?<group>[\w\-\.]+)\s+(?<size>\d+)\s+(?<datetime>\d{2,4}\-\d{2}\-\d{2,4}\s\d{2}\:\d{2}\:\d{2}\.\d{9}\s[-+\s]\d{4})\s+(?<filename>.+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase),
			new(@"^(?<type>[\-dbclps])(?<ownerpermissions>[-rwxsStT]{3})(?<grouppermissions>[-rwxsStT]{3})(?<otherpermissions>[-rwxsStT]{3})\s+\d*\s+(?<owner>[\w\-\.]+)\s+(?<group>[\w\-\.]+)\s+(?<size>\d+)\s+(?<datetime>((\w+\s+\d+)|(\d{2,4}[/\-]\d{2}[/\-]\d{2,4}))\s+\d{1,2}:\d{2}(\s*[AP]M)?)\s(?<filename>.+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase),
		};

		public FileSystem.IFileSystemPath ParseFileSystemRecord(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, string fileSystemRecord)
		{
			FileSystem.IFileSystemPath fileSystemPath = null;

			System.Text.RegularExpressions.Match match = null;

			var directory = fileSystemPathDirectory.FullPath();

			if (!string.IsNullOrWhiteSpace(directory) && !directory.EndsWith(DirectorySeparator))
			{
				directory += DirectorySeparator;
			}

			for (var regexIndex = 0; (((match == null) || !match.Success) && (regexIndex < FileSystemRecordRegexs.Length)); regexIndex++)
			{
				match = FileSystemRecordRegexs[regexIndex].Match(fileSystemRecord);
			}

			var fileName = match.Groups["filename"].Value;

			if (match.Success)
			{
				switch (match.Groups["type"].Value.Trim())
				{
					case "<DIR>":
					case "d":
						fileSystemPath = new TUnixFileSystemPathDirectory();
						break;

					case "<SYMLINKD>":
					case "<SYMLINK>":
					case "l":
						fileSystemPath = (fileSystemRecord.EndsWith(DirectorySeparator) ? ((FileSystem.IFileSystemPath)new TUnixFileSystemPathSymbolicLinkDirectory()) : ((FileSystem.IFileSystemPath)new TUnixFileSystemPathSymbolicLinkFile()));
						break;

					default:
						fileSystemPath = new TUnixFileSystemPathFile();
						break;
				}

				var pathName = fileName.Split(new[] { "->" }, StringSplitOptions.None).FirstOrDefault().NullCheckedTrim().NullCheckedTrimEnd(DirectorySeparator);

				var linkedTo = fileName.Split(new[] { "->" }, StringSplitOptions.None).LastOrDefault().NullCheckedTrim();

				var dateTimeValue = match.Groups["datetime"].Value;

				if (!DateTime.TryParseExact(dateTimeValue, "MMM dd HH:mm", null, System.Globalization.DateTimeStyles.NoCurrentDateDefault, out var modifiedDateTime))
				{
					modifiedDateTime = dateTimeValue.ToDateTime();
				}

				var size = match.Groups["size"].Value.ToIntNullable();

				switch (fileSystemPath)
				{
					case IUnixFileSystemPathSymbolicLinkDirectory unixFileSystemPathSymbolicLinkDirectory:
						unixFileSystemPathSymbolicLinkDirectory.SetValues(fileSystemPathDirectory.Server, fileSystemPathDirectory.UserName, fileSystemPathDirectory.Password, directory, pathName, linkedTo);
						break;

					case IUnixFileSystemPathSymbolicLinkFile unixFileSystemPathSymbolicLinkFile:
						unixFileSystemPathSymbolicLinkFile.SetValues(fileSystemPathDirectory.Server, fileSystemPathDirectory.UserName, fileSystemPathDirectory.Password, directory, pathName, linkedTo, modifiedDateTime, size);
						break;

					case IUnixFileSystemPathDirectory unixFileSystemPathDirectory:
						unixFileSystemPathDirectory.SetValues(fileSystemPathDirectory.Server, fileSystemPathDirectory.UserName, fileSystemPathDirectory.Password, directory, pathName);
						break;

					case IUnixFileSystemPathFile unixFileSystemPathFile:
						unixFileSystemPathFile.SetValues(fileSystemPathDirectory.Server, fileSystemPathDirectory.UserName, fileSystemPathDirectory.Password, directory, pathName, modifiedDateTime, size);
						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(fileSystemPath));
				}
			}

			return fileSystemPath;
		}

		protected virtual (string Server, string UserName, string Password, string Directory, string PathName) ParseAttributedFullName(string attributedFullName)
		{
			var fileSystemPath = (Server: string.Empty, UserName: string.Empty, Password: string.Empty, Directory: string.Empty, PathName: string.Empty);

			attributedFullName = attributedFullName.Trim();

			while (attributedFullName.EndsWith(DirectorySeparator))
			{
				attributedFullName = attributedFullName.Substring(0, attributedFullName.Length - DirectorySeparator.Length).Trim();
			}

			var match = AttributedPathRegex.Match(attributedFullName);

			if (match.Success)
			{
				if (match.Groups.TryGetValue("server", out var serverGroup))
				{
					fileSystemPath.Server = serverGroup.Value;
				}
				if (match.Groups.TryGetValue("user", out var userNameGroup))
				{
					fileSystemPath.UserName = userNameGroup.Value;
				}
				if (match.Groups.TryGetValue("password", out var passwordGroup))
				{
					fileSystemPath.Password = passwordGroup.Value;
				}

				var fullPath = match.Groups["file"].Value.Trim();

				while (fullPath.StartsWith(DirectorySeparator))
				{
					fullPath = fullPath.TrimStart(DirectorySeparator).Trim();
				}

				while (fullPath.EndsWith(DirectorySeparator))
				{
					fullPath = fullPath.TrimEnd(DirectorySeparator).Trim();
				}

				var lastDirectorySeparatorIndex = fullPath.LastIndexOf(DirectorySeparator, StringComparison.InvariantCultureIgnoreCase);
				if (lastDirectorySeparatorIndex > 0)
				{
					fileSystemPath.Directory = fullPath.Substring(0, lastDirectorySeparatorIndex);
					fileSystemPath.PathName = fullPath.Substring(lastDirectorySeparatorIndex + DirectorySeparator.Length);
				}
				else
				{
					fileSystemPath.PathName = fullPath;
				}
			}

			return fileSystemPath;
		}

		public override FileSystem.IFileSystemPathFile GetFileSystemPathFile(string attributedFullName)
		{
			var parseAttributedFullName = ParseAttributedFullName(attributedFullName);

			IUnixFileSystemPathFile fileSystemPathFile = new TUnixFileSystemPathFile();

			fileSystemPathFile.SetValues(parseAttributedFullName.Server, parseAttributedFullName.UserName, parseAttributedFullName.Password, parseAttributedFullName.Directory, parseAttributedFullName.PathName, null, null);

			return fileSystemPathFile;
		}

		public virtual FileSystem.IFileSystemPathFile GetFileSystemPathFile(System.IO.FileInfo fileInfo)
		{
			var fileSystemPathFile = GetFileSystemPathFile(fileInfo.FullName);

			if (fileSystemPathFile is UnixFileSystemPathFile smbFileSystemPathFile)
			{
				smbFileSystemPathFile.Size = (fileInfo as System.IO.FileInfo)?.Length;
				smbFileSystemPathFile.ModifiedDateTime = fileInfo.LastWriteTime;
			}

			return fileSystemPathFile;
		}

		public override FileSystem.IFileSystemPathDirectory GetFileSystemPathDirectory(string attributedFullName)
		{
			var parseAttributedFullName = ParseAttributedFullName(attributedFullName);

			IUnixFileSystemPathDirectory fileSystemPathDirectory = new TUnixFileSystemPathDirectory();

			fileSystemPathDirectory.SetValues(parseAttributedFullName.Server, parseAttributedFullName.UserName, parseAttributedFullName.Password, parseAttributedFullName.Directory, parseAttributedFullName.PathName);

			return fileSystemPathDirectory;
		}
	}
}
