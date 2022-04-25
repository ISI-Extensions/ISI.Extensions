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

namespace ISI.Extensions.SshNet.ScpFileSystem
{
	[FileSystem.FileSystemProvider]
	public class ScpFileSystemProvider : UnixFileSystem.UnixFileSystemProvider<ScpFileSystemPathFile, ScpFileSystemPathDirectory, ScpFileSystemPathSymbolicLinkFile, ScpFileSystemPathSymbolicLinkDirectory>
	{
		internal static string _schema => "scp://";
		protected override string Schema => _schema;

		internal static readonly System.Text.RegularExpressions.Regex _attributedPathRegex = new(@"^" + _schema + @"((?<user>.+?)(:(?<password>.+))?@)?(?<server>.+?)(?<file>/.*)?$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		protected override System.Text.RegularExpressions.Regex AttributedPathRegex => _attributedPathRegex;

		public override Type GetFileSystemPathType => typeof(IScpFileSystemPath);

		protected virtual string EncodeFileName(string fileName)
		{
			if (fileName.StartsWith("/~"))
			{
				return string.Format("${{HOME}}{0}", fileName.Substring(2));
			}

			if (fileName.StartsWith("/"))
			{
				return fileName.Substring(1);
			}

			return fileName;
		}

		protected Renci.SshNet.SshClient GetSshClient(FileSystem.IFileSystemPath fileSystemPath)
		{
			return new Renci.SshNet.SshClient(ConnectionManager.GetConnectionInfo(fileSystemPath.Server, fileSystemPath.UserName, fileSystemPath.Password));
		}
		protected Renci.SshNet.ScpClient GetTransferClient(FileSystem.IFileSystemPath fileSystemPath)
		{
			return new Renci.SshNet.ScpClient(ConnectionManager.GetConnectionInfo(fileSystemPath.Server, fileSystemPath.UserName, fileSystemPath.Password));
		}

		protected virtual FileSystem.IFileSystemStream GetFileSystemStream()
		{
			return new ScpFileSystemStream(EncodeFileName, GetSshClient, GetTransferClient);
		}

		public override IEnumerable<FileSystem.IFileSystemPath> GetDirectoryFileSystemPaths(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive)
		{
			try
			{
				using (var client = GetSshClient(fileSystemPathDirectory))
				{
					client.Connect();

					IEnumerable<FileSystem.IFileSystemPath> getDirectoryFileSystemInfos(FileSystem.IFileSystemPathDirectory directorySystemPathInfo, bool doRecursive)
					{
						var fileSystemInfos = new List<FileSystem.IFileSystemPath>();
						var fileSystemInfoDirectories = new List<FileSystem.IFileSystemPathDirectory>();

						using (var cmd = client.CreateCommand(string.Format("ls -lkF --full-time {0}\n", directorySystemPathInfo.FullPath().TrimStart(DirectorySeparator))))
						{
							cmd.Execute();

							var items = cmd.Result.Replace("\r", "\n").Replace("\n\n", "\n").Split('\n').Where(item => !string.IsNullOrEmpty(item) && !item.StartsWith("total", StringComparison.InvariantCultureIgnoreCase));

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
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		public override void CreateDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory)
		{
			if (!string.IsNullOrWhiteSpace(fileSystemPathDirectory.PathName))
			{
				using (var client = GetSshClient(fileSystemPathDirectory))
				{
					client.Connect();

					using (var cmd = client.CreateCommand(string.Format("mkdir {0}", EncodeFileName(fileSystemPathDirectory.FullPath()))))
					{
						cmd.Execute();
					}

					client.Disconnect();
				}
			}
		}

		public override void RemoveDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive = true)
		{
			if (DirectoryExists(fileSystemPathDirectory))
			{
				using (var client = GetSshClient(fileSystemPathDirectory))
				{
					client.Connect();

					using (var cmd = client.CreateCommand(string.Format("{0} {1}", (doRecursive ? "rm -r" : "rmdir"), EncodeFileName(fileSystemPathDirectory.FullPath()))))
					{
						cmd.Execute();
					}

					client.Disconnect();
				}
			}
		}

		public override void RemoveFile(FileSystem.IFileSystemPathFile fileSystemPathFile)
		{
			if (FileExists(fileSystemPathFile))
			{
				try
				{
					using (var client = GetSshClient(fileSystemPathFile))
					{
						client.Connect();

						using (var cmd = client.CreateCommand(string.Format("rm {0}", EncodeFileName(fileSystemPathFile.FullPath()))))
						{
							cmd.Execute();
						}

						client.Disconnect();
					}
				}
				catch (Exception exception)
				{
					throw exception;
				}
			}
		}

		public override System.IO.Stream OpenRead(FileSystem.IFileSystemPathFile fileSystemPathFile, bool mustBeSeekable = false)
		{
			var stream = GetFileSystemStream();

			stream.OpenRead(fileSystemPathFile, mustBeSeekable);

			return stream as System.IO.Stream;
		}

		public override System.IO.Stream OpenWrite(FileSystem.IFileSystemPathFile fileSystemPathFile, bool createDirectories, bool overwrite, long fileSize = 0)
		{
			if (createDirectories)
			{
				CreateDirectory(fileSystemPathFile.GetParentFileSystemPathDirectory());
			}

			var stream = GetFileSystemStream();

			stream.OpenWrite(fileSystemPathFile, overwrite, fileSize);

			return stream as System.IO.Stream;
		}
	}
}
