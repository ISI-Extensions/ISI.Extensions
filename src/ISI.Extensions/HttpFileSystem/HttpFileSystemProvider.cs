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

namespace ISI.Extensions.HttpFileSystem
{
	[FileSystem.FileSystemProvider]
	public class HttpFileSystemProvider : HttpFileSystemProvider<HttpFileSystemPathFile, HttpFileSystemPathDirectory, HttpFileSystemPathSymbolicLinkFile, HttpFileSystemPathSymbolicLinkDirectory>
	{
		internal static string _schema => "http://";
		protected override string Schema => _schema;

		internal static System.Text.RegularExpressions.Regex _attributedPathRegex = new(@"^" + _schema + @"((?<user>.+?)(:(?<password>.+))?@)?(?<server>.+?)(/(?<file>.*))?$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		protected override System.Text.RegularExpressions.Regex AttributedPathRegex => _attributedPathRegex;

		public override Type GetFileSystemPathType => typeof(IHttpFileSystemPath);
	}

	public abstract class HttpFileSystemProvider<THttpFileSystemPathFile, THttpFileSystemPathDirectory, THttpFileSystemPathSymbolicLinkFile, THttpFileSystemPathSymbolicLinkDirectory> : UnixFileSystem.UnixFileSystemProvider<THttpFileSystemPathFile, THttpFileSystemPathDirectory, THttpFileSystemPathSymbolicLinkFile, THttpFileSystemPathSymbolicLinkDirectory>, IHttpFileSystemProvider
		where THttpFileSystemPathFile : HttpFileSystemPathFile, new()
		where THttpFileSystemPathDirectory : HttpFileSystemPathDirectory, new()
		where THttpFileSystemPathSymbolicLinkFile : HttpFileSystemPathSymbolicLinkFile, new()
		where THttpFileSystemPathSymbolicLinkDirectory : HttpFileSystemPathSymbolicLinkDirectory, new()
	{
		protected override System.Text.RegularExpressions.Regex AttributedPathRegex => null;

		public override IEnumerable<FileSystem.IFileSystemPath> GetDirectoryFileSystemPaths(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive)
		{
			throw new NotImplementedException();
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
			var stream = new HttpFileSystemStream();

			stream.OpenRead(fileSystemPathFile, mustBeSeekable);

			return stream;
		}

		public override System.IO.Stream OpenWrite(FileSystem.IFileSystemPathFile fileSystemPathFile, bool createDirectories, bool overwrite, long fileSize = 0)
		{
			throw new NotImplementedException();
		}
	}
}
