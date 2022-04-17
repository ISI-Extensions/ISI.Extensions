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

namespace ISI.Extensions.SshNet.ScpFileSystem
{
	public class ScpFileSystemStream : FileSystem.AbstractFileSystemStream
	{
		public override bool WriteNeedsSeekableSource => true;

		protected Func<string, string> EncodeFileName { get; }
		protected Func<FileSystem.IFileSystemPathFile, Renci.SshNet.SshClient> GetSshClient { get; }
		protected Func<FileSystem.IFileSystemPathFile, Renci.SshNet.ScpClient> GetTransferClient { get; }

		private Renci.SshNet.ScpClient _client = null;

		private bool _isUpload = false;

		public ScpFileSystemStream(Func<string, string> encodeFileName, Func<FileSystem.IFileSystemPathFile, Renci.SshNet.SshClient> getSshClient, Func<FileSystem.IFileSystemPathFile, Renci.SshNet.ScpClient> getTransferClient)
		{
			EncodeFileName = encodeFileName;
			GetSshClient = getSshClient;
			GetTransferClient = getTransferClient;
		}

		public override void OpenRead(FileSystem.IFileSystemPathFile fileSystemPathFile)
		{
			_client = GetTransferClient(fileSystemPathFile);
			_client.Connect();

			Stream = new ISI.Extensions.Stream.TempFileStream();

			_client.Download(EncodeFileName(fileSystemPathFile.FullPath()), Stream);

			FileSystemPathFile = fileSystemPathFile.Clone() as FileSystem.IFileSystemPathFile;
		}

		public override void OpenWrite(FileSystem.IFileSystemPathFile fileSystemPathFile, bool overWrite = true, long fileSize = 0)
		{
			if (overWrite)
			{
				try
				{
					var fileSystemProvider = new ScpFileSystemProvider();

					fileSystemProvider.RemoveFile(fileSystemPathFile);
				}
				catch
				{

				}
			}

			Stream = new ISI.Extensions.Stream.TempFileStream();

			_isUpload = true;

			FileSystemPathFile = fileSystemPathFile.Clone() as FileSystem.IFileSystemPathFile;
		}

		public override void Close()
		{
			if (Stream != null)
			{
				if (_isUpload)
				{
					using (var client = GetTransferClient(FileSystemPathFile))
					{
						client.Connect();

						Stream.Rewind();

						client.Upload(Stream, EncodeFileName(FileSystemPathFile.FullPath()));

						client.Disconnect();
					}
				}

				Stream.Close();
			}

			base.Close();
			_client?.Disconnect();
			_client?.Dispose();
			_client = null;
		}
	}
}
