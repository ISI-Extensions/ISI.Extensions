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
using DTOs = ISI.Extensions.GoDrive.DataTransferObjects.GoDriveApi;

namespace ISI.Extensions.GoDrive.GoDriveFileSystem
{
	[FileSystem.FileSystemProvider]
	public class GoDriveFileSystemProvider : GoDriveFileSystemProvider<GoDriveFileSystemPathFile, GoDriveFileSystemPathDirectory>
	{
		internal static string _schema => "godrive://";
		protected override string Schema => _schema;

		internal static readonly System.Text.RegularExpressions.Regex _attributedPathRegex = new(@"^" + _schema + @"(?<server>.+?)(/(?<file>.*))?$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		protected override System.Text.RegularExpressions.Regex AttributedPathRegex => _attributedPathRegex;

		public override Type GetFileSystemPathType => typeof(IGoDriveFileSystemPath);
	}

	public abstract class GoDriveFileSystemProvider<TGoDriveFileSystemPathFile, TGoDriveFileSystemPathDirectory> : FileSystem.AbstractFileSystemProvider, IGoDriveFileSystemProvider
		where TGoDriveFileSystemPathFile : GoDriveFileSystemPathFile, new()
		where TGoDriveFileSystemPathDirectory : GoDriveFileSystemPathDirectory, new()
	{
		internal static string _directorySeparator => "/";
		protected override string DirectorySeparator => _directorySeparator;

		protected override System.Text.RegularExpressions.Regex AttributedPathRegex => null;

		protected virtual (string Server, string Directory, string PathName) ParseAttributedFullName(string attributedFullName)
		{
			var fileSystemPath = (Server: string.Empty, Directory: string.Empty, PathName: string.Empty);

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

			var fileSystemPathFile = new TGoDriveFileSystemPathFile();

			fileSystemPathFile.SetValues(parseAttributedFullName.Server, parseAttributedFullName.Directory, parseAttributedFullName.PathName);

			return fileSystemPathFile;
		}

		public override FileSystem.IFileSystemPathDirectory GetFileSystemPathDirectory(string attributedFullName)
		{
			var parseAttributedFullName = ParseAttributedFullName(attributedFullName);

			var fileSystemPathDirectory = new TGoDriveFileSystemPathDirectory();

			fileSystemPathDirectory.SetValues(parseAttributedFullName.Server, parseAttributedFullName.Directory, parseAttributedFullName.PathName);

			return fileSystemPathDirectory;
		}

		public override IEnumerable<FileSystem.IFileSystemPath> GetDirectoryFileSystemPaths(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive)
		{
			var goDriveApi = new GoDriveApi();

			var listFilesResponse = goDriveApi.ListFiles(new DTOs.ListFilesRequest()
			{
				DirectoryUrl = fileSystemPathDirectory.AttributedFullPath(),
			});

			return listFilesResponse.FileNames.NullCheckedSelect(fileName => GetFileSystemPathFile(fileName.FullName), NullCheckCollectionResult.Empty);
		}

		public override void CreateDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory)
		{
			throw new NotImplementedException();
		}

		public override void RemoveDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive = true)
		{
			throw new NotImplementedException();
		}

		public override void RemoveFile(FileSystem.IFileSystemPathFile fileSystemPathFile)
		{
			throw new NotImplementedException();
		}

		public override System.IO.Stream OpenRead(FileSystem.IFileSystemPathFile fileSystemPathFile, bool mustBeSeekable = false)
		{
			var stream = new GoDriveFileSystemStream();

			stream.OpenRead(fileSystemPathFile, mustBeSeekable);

			return stream;
		}

		public override System.IO.Stream OpenWrite(FileSystem.IFileSystemPathFile fileSystemPathFile, bool createDirectories, bool overwrite, long fileSize = 0)
		{
			throw new NotImplementedException();
		}
	}
}
