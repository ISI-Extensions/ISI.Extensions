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
using System.Text;

namespace ISI.Extensions
{
	public partial class FileSystem
	{
		public interface IFileSystemProvider
		{
			int Priority { get; }

			IFileSystemPathInfo GetFileSystemPathInfo(string attributedFullName);

			IEnumerable<IFileSystemInfo> GetDirectoryFileSystemInfos(string directoryName, bool doRecursive);
			IEnumerable<IFileSystemInfo> GetDirectoryFileSystemInfos(IFileSystemPathInfo directoryNameInfo, bool doRecursive);

			IFileSystemInfo GetFileSystemInfo(string fileName);
			IFileSystemInfo GetFileSystemInfo(IFileSystemPathInfo fileNameInfo);

			bool FileExists(string fileName);
			bool FileExists(IFileSystemPathInfo fileNameInfo);
			bool DirectoryExists(string directoryName);
			bool DirectoryExists(IFileSystemPathInfo directoryNameInfo);

			bool CreateDirectory(string directoryName);
			bool CreateDirectory(IFileSystemPathInfo directoryNameInfo);

			System.IO.Stream OpenRead(string fileName, bool mustBeSeekable = false);
			System.IO.Stream OpenRead(IFileSystemPathInfo fileNameInfo, bool mustBeSeekable = false);
			System.IO.Stream OpenWrite(string fileName, bool createDirectories, bool overwrite, bool doDeMasked, long fileSize = 0);
			System.IO.Stream OpenWrite(IFileSystemPathInfo fileNameInfo, bool createDirectories, bool overwrite, bool doDeMasked, long fileSize = 0);

			bool RemoveDirectory(string directoryName, bool doRecursive = true);
			bool RemoveDirectory(IFileSystemPathInfo directoryNameInfo, bool doRecursive = true);
			bool RemoveFile(string fileName);
			bool RemoveFile(IFileSystemPathInfo fileNameInfo);
		}
	}
}