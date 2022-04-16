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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.FtpFileSystem
{
	[FileSystem.FileSystemProvider]
	public class FtpFileSystemProvider : UnixFileSystem.UnixFileSystemProvider<FtpFileSystemPathFile, FtpFileSystemPathDirectory, FtpFileSystemPathSymbolicLinkFile, FtpFileSystemPathSymbolicLinkDirectory>
	{
		protected virtual bool EnableSsl() => false;

		internal static string _schema => "ftp://";
		protected override string Schema => _schema;


		internal static readonly System.Text.RegularExpressions.Regex _attributedPathRegex = new(@"^" + _schema + @"((?<user>.+?)(:(?<password>.+))?@)?(?<server>.+?)(/(?<file>.*))?$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		protected override System.Text.RegularExpressions.Regex AttributedPathRegex => _attributedPathRegex;

		public override Type GetFileSystemPathType => typeof(IFtpFileSystemPath);

		private static readonly System.Text.RegularExpressions.Regex EncodePathStartingSlashRegex = new(@"^(?<slash>/)");
		private static readonly System.Text.RegularExpressions.Regex EncodePathDoubleDotRegex = new(@"(?<begin>(^/?)|/)(?<dots>\.\.)(?=(/|$))");
		private static readonly System.Text.RegularExpressions.Regex EncodePathSingleDotRegex = new(@"(?<begin>(^/?)|/)(?<dot>\.)(?=(/|$))");

		internal static string EncodeFileName(string fileName)
		{
			// http://blogs.msdn.com/mariya/archive/2006/03/06/544523.aspx
			var result = fileName;

			result = EncodePathDoubleDotRegex.Replace(result, @"${begin}%2e%2e");
			result = EncodePathSingleDotRegex.Replace(result, @"${begin}%2e");
			result = EncodePathStartingSlashRegex.Replace(result, @"%2F");

			return result;
		}

		internal string ServerWithPort(string server)
		{
			if (EnableSsl() && !server.Contains(":"))
			{
				server = string.Format("{0}:990", server);
			}

			return server;
		}

		public override IEnumerable<FileSystem.IFileSystemPath> GetDirectoryFileSystemPaths(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive)
		{
			var server = ServerWithPort(fileSystemPathDirectory.Server);

			IEnumerable<FileSystem.IFileSystemPath> getDirectoryFileSystemInfos(FileSystem.IFileSystemPathDirectory directorySystemPathInfo, bool doRecursive)
			{
				var fileSystemInfos = new List<FileSystem.IFileSystemPath>();
				var fileSystemInfoDirectories = new List<FileSystem.IFileSystemPathDirectory>();

				var ftpRequest = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(string.Format(@"ftp://{0}//{1}", server, EncodeFileName(directorySystemPathInfo.FullPath())));
				if (EnableSsl())
				{
					System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
				}
				ftpRequest.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;
				ftpRequest.UseBinary = false;
				ftpRequest.UsePassive = true;
				ftpRequest.EnableSsl = EnableSsl();
				ftpRequest.Credentials = new System.Net.NetworkCredential(directorySystemPathInfo.UserName, directorySystemPathInfo.Password);
				
				using (var ftpResponse = (System.Net.FtpWebResponse)ftpRequest.GetResponse())
				{
					using (var streamReader = new System.IO.StreamReader(ftpResponse.GetResponseStream(), System.Text.Encoding.ASCII))
					{
						var items = streamReader.ReadToEnd().Replace("\r", "\n").Replace("\n\n", "\n").Split('\n');

						foreach (var item in items)
						{
							var fileItem = ParseFileSystemRecord(directorySystemPathInfo, item);

							if (fileItem is FileSystem.IFileSystemPathDirectory subFileSystemPathDirectory)
							{
								fileSystemInfoDirectories.Add(subFileSystemPathDirectory);
							}
							
							if ((fileItem != null) && !string.IsNullOrEmpty(fileItem.PathName))
							{
								fileSystemInfos.Add(fileItem);
							}
						}
					}
				}

				if (doRecursive)
				{
					foreach (var fileSystemInfoDirectory in fileSystemInfoDirectories)
					{
						fileSystemInfos.AddRange(getDirectoryFileSystemInfos(fileSystemInfoDirectory, doRecursive));
					}
				}

				return fileSystemInfos;
			}

			return getDirectoryFileSystemInfos(fileSystemPathDirectory, doRecursive);
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
				throw new Exception("attributedFileName = " + fileSystemPathFile.AttributedFullPath(), exception);
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
				throw new Exception("attributedFileName = " + fileSystemPathDirectory.AttributedFullPath(), exception);
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
				throw new Exception("attributedFileName = " + fileSystemPathDirectory.AttributedFullPath(), exception);
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
				throw new Exception("attributedFileName = " + fileSystemPathDirectory.AttributedFullPath(), exception);
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
				throw new Exception("attributedFileName = " + fileSystemPathFile.AttributedFullPath(), exception);
			}
		}

		public override System.IO.Stream OpenRead(FileSystem.IFileSystemPathFile fileSystemPathFile, bool mustBeSeekable = false)
		{
			var stream = new FtpFileSystemStream();

			stream.OpenRead(fileSystemPathFile, mustBeSeekable);

			return stream;
		}

		public override System.IO.Stream OpenWrite(FileSystem.IFileSystemPathFile fileSystemPathFile, bool createDirectories, bool overwrite, long fileSize = 0)
		{
			if (createDirectories)
			{
				//CreateDirectory(fileSystemPathFile);
			}

			var stream = new FtpFileSystemStream();

			stream.OpenWrite(fileSystemPathFile, overwrite, fileSize);

			return stream;
		}
	}
}
