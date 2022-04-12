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
		public interface IFileSystemStreamProvider : IDisposable
		{
			bool WriteNeedsSeekableSource();

			void OpenRead(IFileSystemPathInfo fileSystemPathInfo, bool mustBeSeekable);
			void OpenRead(IFileSystemPathInfo fileSystemPathInfo);
			void OpenWrite(IFileSystemPathInfo fileSystemPathInfo, bool overWrite = true, long fileSize = 0);
			
			bool CanRead { get; }
			bool CanWrite { get; }

			bool CanSeek { get; }
			bool CanTimeout { get; }

			long Length { get; }
			long Position { get; set; }

			int ReadTimeout { get; set; }
			int WriteTimeout { get; set; }
			
			long Seek(long offset, System.IO.SeekOrigin origin);
			void SetLength(long value);

			int Read(byte[] buffer, int offset, int count);
			int ReadByte();
			
			void Write(byte[] buffer, int offset, int count);
			void WriteByte(byte value);

			IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state);
			IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state);

			int EndRead(IAsyncResult asyncResult);
			void EndWrite(IAsyncResult asyncResult);
			
			void Flush();

			void Close();
		}
	}
}