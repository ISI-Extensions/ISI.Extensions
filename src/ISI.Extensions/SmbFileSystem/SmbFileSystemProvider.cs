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
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.SmbFileSystem
{
	[FileSystem.FileSystemProvider]
	public class SmbFileSystemProvider : FileSystem.AbstractFileSystemProvider
	{
		internal static string _schema => "smb:\\\\";
		protected override string Schema => _schema;

		internal static string _directorySeparator => "\\";
		protected override string DirectorySeparator => _directorySeparator;

		internal static readonly System.Text.RegularExpressions.Regex _attributedPathRegex = new(@"^(?:smb:\\\\(?:(?<user>.+?)(?::(?<password>.+))?@)?((?<server>.+?)\\)|(?:\\\\(?<server>.+?)\\)|(?:(?<drive>[A-Z]:)\\)|(?![A-Z]+://))(?<fileName>.*)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		protected override System.Text.RegularExpressions.Regex AttributedPathRegex => _attributedPathRegex;

		internal static readonly System.Text.RegularExpressions.Regex DriveLetterRegex = new(@"^[A-Z]:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

		public override Type GetFileSystemPathType => typeof(ISmbFileSystemPath);

		protected virtual (string Drive, string Server, string UserName, string Password, string Directory, string PathName) ParseAttributedFullName(string attributedFullName)
		{
			var fileSystemPath = (Drive: string.Empty, Server: string.Empty, UserName: string.Empty, Password: string.Empty, Directory: string.Empty, PathName: string.Empty);

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
				if (match.Groups.TryGetValue("drive", out var driveGroup))
				{
					fileSystemPath.Drive = driveGroup.Value;
				}

				var fullPath = match.Groups["fileName"].Value.Trim();

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

			ISmbFileSystemPathFile fileSystemPathFile = new SmbFileSystemPathFile();

			fileSystemPathFile.SetValues(parseAttributedFullName.Drive, parseAttributedFullName.Server, parseAttributedFullName.UserName, parseAttributedFullName.Password, parseAttributedFullName.Directory, parseAttributedFullName.PathName, null, null);

			return fileSystemPathFile;
		}

		public virtual FileSystem.IFileSystemPathFile GetFileSystemPathFile(System.IO.FileInfo fileInfo)
		{
			var fileSystemPathFile = GetFileSystemPathFile(fileInfo.FullName);

			if (fileSystemPathFile is SmbFileSystemPathFile smbFileSystemPathFile)
			{
				smbFileSystemPathFile.Size = (fileInfo as System.IO.FileInfo)?.Length;
				smbFileSystemPathFile.ModifiedDateTime = fileInfo.LastWriteTime;
			}

			return fileSystemPathFile;
		}

		public override FileSystem.IFileSystemPathDirectory GetFileSystemPathDirectory(string attributedFullName)
		{
			var parseAttributedFullName = ParseAttributedFullName(attributedFullName);

			ISmbFileSystemPathDirectory fileSystemPathDirectory = new SmbFileSystemPathDirectory();

			fileSystemPathDirectory.SetValues(parseAttributedFullName.Drive, parseAttributedFullName.Server, parseAttributedFullName.UserName, parseAttributedFullName.Password, parseAttributedFullName.Directory, parseAttributedFullName.PathName);

			return fileSystemPathDirectory;
		}

		public override IEnumerable<FileSystem.IFileSystemPath> GetDirectoryFileSystemPaths(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive)
		{
			IEnumerable<FileSystem.IFileSystemPath> response = null;

			IEnumerable<FileSystem.IFileSystemPath> getDirectoryFileSystemInfos(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive)
			{
				var fileSystemPaths = new List<FileSystem.IFileSystemPath>();

				var directory = new System.IO.DirectoryInfo(fileSystemPathDirectory.FullPath());

				if ((directory?.Exists).GetValueOrDefault())
				{
					foreach (var directoryInfo in directory.GetDirectories())
					{
						var newFileSystemDirectory = GetFileSystemPathDirectory(directoryInfo.FullName);

						fileSystemPaths.Add(newFileSystemDirectory);

						if (doRecursive)
						{
							fileSystemPaths.AddRange(getDirectoryFileSystemInfos(newFileSystemDirectory, doRecursive));
						}
					}

					foreach (var fileInfo in directory.GetFiles())
					{
						fileSystemPaths.Add(GetFileSystemPathFile(fileInfo));
					}
				}

				return fileSystemPaths;
			}

			ImpersonateWindowsUser.RunImpersonated(fileSystemPathDirectory.UserName, fileSystemPathDirectory.Password, true, () =>
			{
				response = getDirectoryFileSystemInfos(fileSystemPathDirectory, doRecursive);
			});

			return response;
		}

		public override bool FileExists(FileSystem.IFileSystemPathFile fileSystemPathFile)
		{
			try
			{
				var fileExists = false;

				ImpersonateWindowsUser.RunImpersonated(fileSystemPathFile.UserName, fileSystemPathFile.Password, true, () =>
				{
					fileExists = System.IO.File.Exists(fileSystemPathFile.FullPath());
				});

				return fileExists;
			}
			catch (Exception exception)
			{
				throw new("attributedFileName = " + fileSystemPathFile.AttributedFullPath(), exception);
			}
		}

		public override bool DirectoryExists(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory)
		{
			try
			{
				var directoryExists = false;

				ImpersonateWindowsUser.RunImpersonated(fileSystemPathDirectory.UserName, fileSystemPathDirectory.Password, true, () =>
				{
					directoryExists = System.IO.Directory.Exists(fileSystemPathDirectory.FullPath());
				});

				return directoryExists;
			}
			catch (Exception exception)
			{
				throw new("attributedFileName = " + fileSystemPathDirectory.AttributedFullPath(), exception);
			}
		}

		public override void CreateDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory)
		{
			try
			{
				ImpersonateWindowsUser.RunImpersonated(fileSystemPathDirectory.UserName, fileSystemPathDirectory.Password, true, () =>
				{
					var fullPath = fileSystemPathDirectory.FullPath();

					if (!System.IO.Directory.Exists(fullPath))
					{
						System.IO.Directory.CreateDirectory(fullPath);
					}
				});
			}
			catch (Exception exception)
			{
				throw new("attributedFileName = " + fileSystemPathDirectory.AttributedFullPath(), exception);
			}
		}

		public override void RemoveDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive = true)
		{
			try
			{
				ImpersonateWindowsUser.RunImpersonated(fileSystemPathDirectory.UserName, fileSystemPathDirectory.Password, true, () =>
				{
					var fullPath = fileSystemPathDirectory.FullPath();

					if (System.IO.Directory.Exists(fullPath))
					{
						System.IO.Directory.Delete(fullPath, doRecursive);
					}
				});
			}
			catch (Exception exception)
			{
				throw new("attributedFileName = " + fileSystemPathDirectory.AttributedFullPath(), exception);
			}
		}

		public override void RemoveFile(FileSystem.IFileSystemPathFile fileSystemPathFile)
		{
			try
			{
				ImpersonateWindowsUser.RunImpersonated(fileSystemPathFile.UserName, fileSystemPathFile.Password, true, () =>
				{
					var fullPath = fileSystemPathFile.FullPath();

					if (System.IO.File.Exists(fullPath))
					{
						System.IO.File.Delete(fullPath);
					}
				});
			}
			catch (Exception exception)
			{
				throw new("attributedFileName = " + fileSystemPathFile.AttributedFullPath(), exception);
			}
		}

		public override System.IO.Stream OpenRead(FileSystem.IFileSystemPathFile fileSystemPathFile, bool mustBeSeekable = false)
		{
			var stream = new SmbFileSystemStream();

			stream.OpenRead(fileSystemPathFile, mustBeSeekable);

			return stream;
		}

		public override System.IO.Stream OpenWrite(FileSystem.IFileSystemPathFile fileSystemPathFile, bool createDirectories, bool overwrite, long fileSize = 0)
		{
			if (createDirectories)
			{
				//CreateDirectory(fileSystemPathFile);
			}

			var stream = new SmbFileSystemStream();

			stream.OpenWrite(fileSystemPathFile, overwrite, fileSize);

			return stream;
		}
	}
}
