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

namespace ISI.Extensions.FtpFileSystem
{
	[FileSystem.FileSystemProvider]
	public class FtpFileSystemProvider : FtpFileSystemProvider<FtpFileSystemPathFile, FtpFileSystemPathDirectory, FtpFileSystemPathSymbolicLinkFile, FtpFileSystemPathSymbolicLinkDirectory>
	{
		internal static bool _enableSsl => false;
		protected override bool EnableSsl => _enableSsl;

		internal static string _schema => "ftp://";
		protected override string Schema => _schema;

		internal static readonly System.Text.RegularExpressions.Regex _attributedPathRegex = new(@"^" + _schema + @"((?<user>.+?)(:(?<password>.+))?@)?(?<server>.+?)(/(?<file>.*))?$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		protected override System.Text.RegularExpressions.Regex AttributedPathRegex => _attributedPathRegex;

		public override Type GetFileSystemPathType => typeof(IFtpFileSystemPath);
	}

	public abstract class FtpFileSystemProvider<TFtpFileSystemPathFile, TFtpFileSystemPathDirectory, TFtpFileSystemPathSymbolicLinkFile, TFtpFileSystemPathSymbolicLinkDirectory> : UnixFileSystem.UnixFileSystemProvider<TFtpFileSystemPathFile, TFtpFileSystemPathDirectory, TFtpFileSystemPathSymbolicLinkFile, TFtpFileSystemPathSymbolicLinkDirectory>, IFtpFileSystemProvider
		where TFtpFileSystemPathFile : FtpFileSystemPathFile, new()
		where TFtpFileSystemPathDirectory : FtpFileSystemPathDirectory, new()
		where TFtpFileSystemPathSymbolicLinkFile : FtpFileSystemPathSymbolicLinkFile, new()
		where TFtpFileSystemPathSymbolicLinkDirectory : FtpFileSystemPathSymbolicLinkDirectory, new()
	{
		protected abstract bool EnableSsl { get; }

		private static readonly System.Text.RegularExpressions.Regex EncodePathStartingSlashRegex = new(@"^(?<slash>/)");
		private static readonly System.Text.RegularExpressions.Regex EncodePathDoubleDotRegex = new(@"(?<begin>(^/?)|/)(?<dots>\.\.)(?=(/|$))");
		private static readonly System.Text.RegularExpressions.Regex EncodePathSingleDotRegex = new(@"(?<begin>(^/?)|/)(?<dot>\.)(?=(/|$))");

		internal static string EncodeFileName(string fileName)
		{
			// http://blogs.msdn.com/mariya/archive/2006/03/06/544523.aspx
			var encodedFileName = fileName;

			encodedFileName = EncodePathDoubleDotRegex.Replace(encodedFileName, @"${begin}%2e%2e");
			encodedFileName = EncodePathSingleDotRegex.Replace(encodedFileName, @"${begin}%2e");
			encodedFileName = EncodePathStartingSlashRegex.Replace(encodedFileName, @"%2F");

			return encodedFileName;
		}

		internal string ServerWithPort(string server)
		{
			if (EnableSsl && !server.Contains(":"))
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

				var ftpRequest = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(string.Format(@"{0}{1}//{2}", Schema, server, EncodeFileName(directorySystemPathInfo.FullPath())));
				if (EnableSsl)
				{
					System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
				}
				ftpRequest.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails;
				ftpRequest.UseBinary = false;
				ftpRequest.UsePassive = true;
				ftpRequest.EnableSsl = EnableSsl;
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

		public override void CreateDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory)
		{
			if (fileSystemPathDirectory != null)
			{
				var server = fileSystemPathDirectory.Server;
				var userName = fileSystemPathDirectory.UserName;
				var password = fileSystemPathDirectory.Password;

				if (!DirectoryExists(fileSystemPathDirectory))
				{
					var fileSystemPathDirectories = new Stack<FileSystem.IFileSystemPathDirectory>();

					while (fileSystemPathDirectory != null)
					{
						if (!string.IsNullOrWhiteSpace(fileSystemPathDirectory.Directory) || !string.IsNullOrWhiteSpace(fileSystemPathDirectory.PathName))
						{
							fileSystemPathDirectories.Push(fileSystemPathDirectory);
						}

						fileSystemPathDirectory = fileSystemPathDirectory.GetParentFileSystemPathDirectory();
					}

					while (fileSystemPathDirectories.NullCheckedAny())
					{
						fileSystemPathDirectory = fileSystemPathDirectories.Pop();

						if (!DirectoryExists(fileSystemPathDirectory))
						{
							var request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(string.Format(@"{0}{1}//{2}", Schema, server, EncodeFileName(fileSystemPathDirectory.FullPath())));
							request.Method = System.Net.WebRequestMethods.Ftp.MakeDirectory;
							request.EnableSsl = EnableSsl;
							request.Credentials = new System.Net.NetworkCredential(userName, password);

							try
							{
								using (var response = (System.Net.FtpWebResponse)request.GetResponse())
								{
									using (var responseStream = response.GetResponseStream())
									{
										responseStream?.Flush();
									}

									//if ((response.StatusCode != System.Net.FtpStatusCode.PathnameCreated) && (response.StatusCode != System.Net.FtpStatusCode.FileActionOK) && (response.StatusCode != System.Net.FtpStatusCode.CommandOK))
									//{
									//	result = false;
									//}
								}
							}
							catch (Exception exception)
							{
								// Catch 550 here as it's possible we tried to create a dir that already exists if we do not have permission to list dir above it
								if (!exception.Message.Contains("550"))
								{
									throw exception;
								}
							}
						}
					}
				}
			}
		}

		protected virtual void RemoveFile(string server, string userName, string password, string pathName)
		{
			var request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(string.Format(@"{0}{1}//{2}", Schema, server, EncodeFileName(pathName)));
			request.Method = System.Net.WebRequestMethods.Ftp.DeleteFile;
			request.EnableSsl = EnableSsl;
			request.Credentials = new System.Net.NetworkCredential(userName, password);

			using (var response = (System.Net.FtpWebResponse)request.GetResponse())
			{
				using (var responseStream = response.GetResponseStream())
				{
					responseStream?.Flush();
				}
			}
		}

		protected virtual void RemoveDirectory(string server, string userName, string password, string pathName)
		{
			var request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(string.Format(@"{0}{1}//{2}", Schema, server, EncodeFileName(pathName)));
			request.Method = System.Net.WebRequestMethods.Ftp.RemoveDirectory;
			request.EnableSsl = EnableSsl;
			request.Credentials = new System.Net.NetworkCredential(userName, password);

			using (var response = (System.Net.FtpWebResponse)request.GetResponse())
			{
				using (var responseStream = response.GetResponseStream())
				{
					responseStream?.Flush();
				}
			}
		}

		public override void RemoveDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive = true)
		{
			var server = fileSystemPathDirectory.Server;
			var userName = fileSystemPathDirectory.UserName;
			var password = fileSystemPathDirectory.Password;

			void removeDirectory(FileSystem.IFileSystemPathDirectory removeFileSystemPathDirectory)
			{
				if (doRecursive)
				{
					var fileSystemPaths = GetDirectoryFileSystemPaths(removeFileSystemPathDirectory, true);

					foreach (var subFileSystemPathDirectory in fileSystemPaths.Cast<FileSystem.IFileSystemPathDirectory>())
					{
						removeDirectory(subFileSystemPathDirectory);
					}

					foreach (var subFileSystemPathFile in fileSystemPaths.Cast<FileSystem.IFileSystemPathFile>())
					{
						RemoveFile(server, userName, password, subFileSystemPathFile.FullPath());
					}
				}

				RemoveDirectory(server, userName, password, removeFileSystemPathDirectory.FullPath());
			}

			if (DirectoryExists(fileSystemPathDirectory))
			{
				removeDirectory(fileSystemPathDirectory);
			}
		}

		public override void RemoveFile(FileSystem.IFileSystemPathFile fileSystemPathFile)
		{
			var server = fileSystemPathFile.Server;
			var userName = fileSystemPathFile.UserName;
			var password = fileSystemPathFile.Password;
			var pathName = fileSystemPathFile.FullPath();

			RemoveFile(server, userName, password, pathName);
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
				CreateDirectory(fileSystemPathFile.GetParentFileSystemPathDirectory());
			}

			var stream = new FtpFileSystemStream();

			stream.OpenWrite(fileSystemPathFile, overwrite, fileSize);

			return stream;
		}
	}
}
