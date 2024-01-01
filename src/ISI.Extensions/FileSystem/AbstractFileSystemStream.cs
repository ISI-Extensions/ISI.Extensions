#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
		public abstract class AbstractFileSystemStream : System.IO.Stream, IFileSystemStream
		{
			public abstract bool WriteNeedsSeekableSource { get; }

			protected System.IO.Stream Stream;
			protected string TempFileName = null;

			public virtual IFileSystemPathFile FileSystemPathFile { get; protected set; }

			protected AbstractFileSystemStream(System.IO.Stream stream)
				: this()
			{
				Stream = stream;
			}

			protected AbstractFileSystemStream() { }

			#region OpenRead
			public virtual void OpenRead(IFileSystemPathFile fileSystemPathFile, bool mustBeSeekable)
			{
				OpenRead(fileSystemPathFile);

				if (mustBeSeekable && !CanSeek)
				{
					TempFileName = System.IO.Path.GetTempFileName();

					using (System.IO.Stream oStream = System.IO.File.OpenWrite(TempFileName))
					{
						Stream.CopyTo(oStream);
					}

					Stream.Close();

					Stream = System.IO.File.OpenRead(TempFileName);
				}
			}
			public abstract void OpenRead(IFileSystemPathFile fileSystemPathFile);
			#endregion

			public abstract void OpenWrite(IFileSystemPathFile fileSystemPathFile, bool overWrite = true, long fileSize = 0);

			#region Stream
			public override bool CanRead => Stream.CanRead;
			public override bool CanSeek => Stream.CanSeek;

			public override bool CanTimeout => Stream.CanTimeout;

			public override bool CanWrite => Stream.CanWrite;
			public override long Length => Stream.Length;
			public override long Position { get => Stream.Position; set => Stream.Position = value; }
			public override int ReadTimeout { get => Stream.ReadTimeout; set => Stream.ReadTimeout = value; }
			public override int WriteTimeout { get => Stream.WriteTimeout; set => Stream.WriteTimeout = value; }
			public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state) { return Stream.BeginRead(buffer, offset, count, callback, state); }
			public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state) { return Stream.BeginWrite(buffer, offset, count, callback, state); }

			public override void Close()
			{
				Stream?.Close();
				Stream?.Dispose();
				if (TempFileName != null)
				{
					System.IO.File.Delete(TempFileName);
				}
			}

			public override int EndRead(IAsyncResult asyncResult) => Stream.EndRead(asyncResult);
			public override void EndWrite(IAsyncResult asyncResult) => Stream.EndWrite(asyncResult);
			public override void Flush() => Stream.Flush();
			public override int Read(byte[] buffer, int offset, int count) => Stream.Read(buffer, offset, count);
			public override int ReadByte() => Stream.ReadByte();
			public override long Seek(long offset, System.IO.SeekOrigin origin) => Stream.Seek(offset, origin);
			public override void SetLength(long value) => Stream.SetLength(value);
			public override void Write(byte[] buffer, int offset, int count) => Stream.Write(buffer, offset, count);
			public override void WriteByte(byte value) => Stream.WriteByte(value);
			#endregion
		}
	}
}