#region Copyright & License
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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class FtpFileSystem_Tests
	{
		public const string Server = "10.105.100.5";
		public const string UserName = "anonymous";
		public const string Password = "x.com";

		[Test]
		public void GetFileSystemPathFile_Test()
		{
			var directory = "Pizza/Homemade";
			var pathName = "Topping.txt";

			var fullPath = $"{directory}/{pathName}";
			var attributedFullPath = $"ftp://{UserName}:{Password}@{Server}/{fullPath}";

			var fileSystemPathFile = ISI.Extensions.FileSystem.GetFileSystemPathFile(attributedFullPath);

			Assert.That(fileSystemPathFile is ISI.Extensions.FtpFileSystem.FtpFileSystemPathFile);
			Assert.That(string.Equals(fileSystemPathFile.Server, Server, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathFile.UserName, UserName, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathFile.Password, Password, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathFile.Directory, directory, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathFile.PathName, pathName, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathFile.FullPath(), fullPath, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathFile.AttributedFullPath(), attributedFullPath, StringComparison.Ordinal));
		}

		[Test]
		public void GetFileSystemPathDirectory_Test()
		{
			var directory = "Pizza/Homemade";
			var pathName = "Topping";

			var fullPath = $"{directory}/{pathName}";
			var attributedFullPath = $"ftp://{UserName}:{Password}@{Server}/{fullPath}";

			var fileSystemPathDirectory = ISI.Extensions.FileSystem.GetFileSystemPathDirectory(attributedFullPath);

			Assert.That(fileSystemPathDirectory is ISI.Extensions.FtpFileSystem.FtpFileSystemPathDirectory);
			Assert.That(string.Equals(fileSystemPathDirectory.Server, Server, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.UserName, UserName, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.Password, Password, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.Directory, directory, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.PathName, pathName, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.FullPath(), fullPath, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.AttributedFullPath(), attributedFullPath, StringComparison.Ordinal));
		}
		
		[Test]
		public void GetParentFileSystemPathDirectory_Test()
		{
			var rootDirectory = "Pizza";
			var localDirectory = "Homemade";
			var directory = $"{rootDirectory}/{localDirectory}";
			var pathName = "Topping.txt";

			var fullPath = $"{directory}/{pathName}";
			var attributedFullPath = $"ftp://{UserName}:{Password}@{Server}/{fullPath}";

			var fileSystemPathDirectory = ISI.Extensions.FileSystem.GetFileSystemPathFile(attributedFullPath).GetParentFileSystemPathDirectory();

			Assert.That(fileSystemPathDirectory is ISI.Extensions.FtpFileSystem.FtpFileSystemPathDirectory);
			Assert.That(string.Equals(fileSystemPathDirectory.Server, Server, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.UserName, UserName, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.Password, Password, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.Directory, rootDirectory, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.PathName, localDirectory, StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.FullPath(), $"{rootDirectory}/{localDirectory}", StringComparison.Ordinal));
			Assert.That(string.Equals(fileSystemPathDirectory.AttributedFullPath(), $"ftp://{UserName}:{Password}@{Server}/{rootDirectory}/{localDirectory}", StringComparison.Ordinal));
		}

		[Test]
		public void GetDirectoryFileSystemPaths_Test()
		{
			var attributedFullPath = $"ftp://{UserName}:{Password}@{Server}/";

			var fileSystemPaths = ISI.Extensions.FileSystem.GetDirectoryFileSystemPaths(attributedFullPath, false);

			var fileSystemPathsRecursive = ISI.Extensions.FileSystem.GetDirectoryFileSystemPaths(attributedFullPath, true);
		}

		[Test]
		public void FileExists_Test()
		{
			var attributedFullPath = $"ftp://{UserName}:{Password}@{Server}/setup.exe";

			Assert.That(ISI.Extensions.FileSystem.FileExists(attributedFullPath));
		}

		[Test]
		public void DirectoryExists_Test()
		{
			var attributedFullPath = $"ftp://{UserName}:{Password}@{Server}/support";

			Assert.That(ISI.Extensions.FileSystem.DirectoryExists(attributedFullPath));
		}

		[Test]
		public void Directory_Test()
		{
			var attributedFullPath = $"ftp://{UserName}:{Password}@{Server}/testDirectory";

			ISI.Extensions.FileSystem.CreateDirectory(attributedFullPath);

			Assert.That(ISI.Extensions.FileSystem.DirectoryExists(attributedFullPath));

			ISI.Extensions.FileSystem.RemoveDirectory(attributedFullPath);

			Assert.That(!ISI.Extensions.FileSystem.DirectoryExists(attributedFullPath));
		}

		[Test]
		public void File_Test()
		{
			var attributedFullPath = $"ftp://{UserName}:{Password}@{Server}/testFile.txt";

			var content = string.Join("==>", (Enumerable.Range(1, 1000)).Select(i => Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)));

			using (var stream = new System.IO.MemoryStream())
			{
				using (var streamWriter = new System.IO.StreamWriter(stream))
				{
					streamWriter.Write(content);
					streamWriter.Flush();

					stream.Flush();
					stream.Rewind();

					using (var fileSystemStream = ISI.Extensions.FileSystem.OpenWrite(attributedFullPath, true, true))
					{
						stream.CopyTo(fileSystemStream);
						fileSystemStream.Flush();
					}
				}
			}

			Assert.That(ISI.Extensions.FileSystem.FileExists(attributedFullPath));

			using (var stream = new System.IO.MemoryStream())
			{
				using (var fileSystemStream = ISI.Extensions.FileSystem.OpenRead(attributedFullPath))
				{
					fileSystemStream.CopyTo(stream);
					stream.Flush();
				}

				stream.Rewind();

				var readContent = stream.TextReadToEnd();

				Assert.That(string.Equals(content, readContent, StringComparison.Ordinal));
			}

			ISI.Extensions.FileSystem.RemoveFile(attributedFullPath);

			Assert.That(!ISI.Extensions.FileSystem.FileExists(attributedFullPath));
		}
	}
}
