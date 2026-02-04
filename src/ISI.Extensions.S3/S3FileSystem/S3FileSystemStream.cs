#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using ISI.Extensions.S3.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.S3.S3FileSystem
{
	public class S3FileSystemStream : FileSystem.AbstractFileSystemStream
	{
		public override bool WriteNeedsSeekableSource => true;

		protected Func<IS3FileSystemPath, S3BlobClient> GetClient { get; }

		private bool _overWrite = false;
		private bool _isUpload = false;

		public S3FileSystemStream(Func<IS3FileSystemPath, S3BlobClient> getClient)
		{
			GetClient = getClient;
		}

		public override void OpenRead(FileSystem.IFileSystemPathFile fileSystemPathFile)
		{
			if (fileSystemPathFile is IS3FileSystemPathFile s3FileSystemPathFile)
			{
				var client = GetClient(s3FileSystemPathFile);

				Stream = new ISI.Extensions.Stream.TempFileStream();

				client.ReadAsync(new()
				{
					FullName = s3FileSystemPathFile.GetFullName(),
					Stream = Stream,
				}).GetAwaiter().GetResult();

				Stream.Rewind();

				FileSystemPathFile = s3FileSystemPathFile.Clone() as FileSystem.IFileSystemPathFile;
			}
		}

		public override void OpenWrite(FileSystem.IFileSystemPathFile fileSystemPathFile, bool overWrite = true, long fileSize = 0)
		{
			_isUpload = true;
			_overWrite = overWrite;

			Stream = new ISI.Extensions.Stream.TempFileStream();

			FileSystemPathFile = fileSystemPathFile.Clone() as FileSystem.IFileSystemPathFile;
		}

		public override void Close()
		{
			if (Stream != null)
			{
				if (_isUpload)
				{
					if (FileSystemPathFile is IS3FileSystemPathFile s3FileSystemPathFile)
					{
						Stream.Rewind();

						var client = GetClient(s3FileSystemPathFile);

						client.WriteAsync(new ()
						{
							FullName = s3FileSystemPathFile.GetFullName(),
							OverWriteExisting = _overWrite,
							Stream = Stream,
						}).GetAwaiter().GetResult();
					}
				}

				Stream.Close();
				Stream = null;
			}

			base.Close();
		}
	}
}
