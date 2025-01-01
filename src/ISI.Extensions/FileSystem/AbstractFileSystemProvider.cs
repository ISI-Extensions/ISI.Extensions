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
	public partial class FileSystem
	{
		public abstract class AbstractFileSystemProvider : IFileSystemProvider
		{
			public virtual int Priority => 0;

			public abstract Type GetFileSystemPathType { get; }

			protected abstract string Schema { get; }
			protected abstract string DirectorySeparator { get; }
			protected abstract System.Text.RegularExpressions.Regex AttributedPathRegex { get; }

			public virtual bool CanParsePath(string attributedFullName) => AttributedPathRegex.Match(attributedFullName).Success;

			public abstract string Combine(string attributedFullName, string path2);

			public abstract IFileSystemPathFile GetFileSystemPathFile(string attributedFullName);
			public abstract IFileSystemPathDirectory GetFileSystemPathDirectory(string attributedFullName);

			public virtual IEnumerable<IFileSystemPath> GetDirectoryFileSystemPaths(string directoryName, bool doRecursive) => GetDirectoryFileSystemPaths(GetFileSystemPathDirectory(directoryName), doRecursive);
			public abstract IEnumerable<IFileSystemPath> GetDirectoryFileSystemPaths(IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive);

			public virtual bool FileExists(string attributedFullName) => FileExists(GetFileSystemPathFile(attributedFullName));
			public virtual bool FileExists(IFileSystemPathFile fileSystemPathFile)
			{
				foreach (var fileSystemPath in GetDirectoryFileSystemPaths(fileSystemPathFile.GetParentFileSystemPathDirectory(), false))
				{
					if ((fileSystemPath is IFileSystemPathFile) && string.Equals(fileSystemPath.PathName, fileSystemPathFile.PathName, StringComparison.CurrentCulture))
					{
						return true;
					}
				}

				return false;
			}

			public virtual bool DirectoryExists(string attributedFullName) => DirectoryExists(GetFileSystemPathDirectory(attributedFullName));
			public virtual bool DirectoryExists(IFileSystemPathDirectory fileSystemPathDirectory)
			{
				var parentFileSystemPathDirectory = fileSystemPathDirectory.GetParentFileSystemPathDirectory();

				if (parentFileSystemPathDirectory == null)
				{
					return true;
				}

				foreach (var fileSystemPath in GetDirectoryFileSystemPaths(parentFileSystemPathDirectory, false))
				{
					if ((fileSystemPath is IFileSystemPathDirectory) && string.Equals(fileSystemPath.PathName, fileSystemPathDirectory.PathName, StringComparison.CurrentCulture))
					{
						return true;
					}
				}

				return false;
			}

			public virtual void CreateDirectory(string attributedFullName) => CreateDirectory(GetFileSystemPathDirectory(attributedFullName));
			public abstract void CreateDirectory(IFileSystemPathDirectory fileSystemPathDirectory);

			public virtual void RemoveDirectory(string attributedFullName, bool doRecursive = true) => RemoveDirectory(GetFileSystemPathDirectory(attributedFullName), doRecursive);
			public abstract void RemoveDirectory(IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive = true);
			public virtual void RemoveFile(string attributedFullName) => RemoveFile(GetFileSystemPathFile(attributedFullName));
			public abstract void RemoveFile(IFileSystemPathFile fileSystemPathFile);

			public virtual System.IO.Stream OpenRead(string attributedFullName, bool mustBeSeekable = false) => OpenRead(GetFileSystemPathFile(attributedFullName), mustBeSeekable);
			public abstract System.IO.Stream OpenRead(IFileSystemPathFile fileSystemPathFile, bool mustBeSeekable = false);
			public virtual System.IO.Stream OpenWrite(string attributedFullName, bool createDirectories, bool overwrite, long fileSize = 0) => OpenWrite(GetFileSystemPathFile(attributedFullName), createDirectories, overwrite, fileSize);
			public abstract System.IO.Stream OpenWrite(IFileSystemPathFile fileSystemPathFile, bool createDirectories, bool overwrite, long fileSize = 0);
		}
	}
}