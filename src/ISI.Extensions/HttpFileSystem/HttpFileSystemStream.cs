﻿#region Copyright & License
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
using System.Text;

namespace ISI.Extensions.HttpFileSystem
{
	public class HttpFileSystemStream : HttpFileSystemStream<HttpFileSystemProvider>
	{
		protected override string Schema => HttpFileSystemProvider._schema;
	}

	public abstract class HttpFileSystemStream<THttpFileSystemProvider> : FileSystem.AbstractFileSystemStream
		where THttpFileSystemProvider : IHttpFileSystemProvider, new()
	{
		protected abstract string Schema { get; }

		public override bool WriteNeedsSeekableSource => false;

		private System.Net.HttpWebRequest _request = null;
		private System.Net.HttpWebResponse _response = null;

		public override void Close()
		{
			base.Close();
			_response?.Close();
			_request = null;
		}

		public override void OpenRead(FileSystem.IFileSystemPathFile fileSystemPathFile)
		{
			var server = fileSystemPathFile.Server;

			_request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(string.Format(@"{0}{1}//{2}", Schema, server, fileSystemPathFile.FullPath()));
			_request.Method = System.Net.WebRequestMethods.Http.Get;
			if (!string.IsNullOrEmpty(fileSystemPathFile.UserName) || !string.IsNullOrEmpty(fileSystemPathFile.Password))
			{
				if ((fileSystemPathFile.UserName ?? string.Empty).Equals("~~DefaultCredentials", StringComparison.InvariantCultureIgnoreCase))
				{
					_request.Credentials = System.Net.CredentialCache.DefaultCredentials;
				}
				else
				{
					_request.Credentials = new System.Net.NetworkCredential(fileSystemPathFile.UserName, fileSystemPathFile.Password);
				}
			}

			_response = (System.Net.HttpWebResponse)_request.GetResponse();
			Stream = _response.GetResponseStream();

			FileSystemPathFile = fileSystemPathFile.Clone() as FileSystem.IFileSystemPathFile;
		}

		public override void OpenWrite(FileSystem.IFileSystemPathFile fileSystemPathFile, bool overWrite = true, long fileSize = 0)
		{
			throw new NotImplementedException();
		}
	}
}
