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

namespace ISI.Extensions.Linux
{
	public abstract class ArchiveStreamReader : IDisposable
	{
		private readonly System.IO.Stream _stream;

		private readonly bool _leaveOpen = false;

		private bool _disposed = false;

		public ArchiveStreamReader(System.IO.Stream stream, bool leaveOpen)
		{
			_stream = stream ?? throw new ArgumentNullException(nameof(stream));
			_leaveOpen = leaveOpen;
		}

		public string FileName { get; protected set; }
		public IArchiveHeader FileHeader { get; protected set; }
		public ISI.Extensions.Stream.PocketStream FileStream { get; protected set; }

		protected System.IO.Stream Stream
		{
			get
			{
				CheckIfDisposed();
				return _stream;
			}
		}

		public static int PaddingSize(int multiple, int value)
		{
			if (value % multiple == 0)
			{
				return 0;
			}

			return multiple - value % multiple;
		}


		public System.IO.Stream Open() => FileStream;

		public void Dispose()
		{
			if (!_leaveOpen)
			{
				Stream.Dispose();
			}

			_disposed = true;
		}

		public abstract bool Read();

		public void SkipToEnd() => FileStream.SkipToEnd();
		
		protected void CheckIfDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}

		protected void Align(int alignmentBase)
		{
			var currentIndex = (int)(FileStream != null ? (FileStream.Offset + FileStream.Length) : Stream.Position);

			if (Stream.CanSeek)
			{
				Stream.Seek(currentIndex + PaddingSize(alignmentBase, currentIndex), System.IO.SeekOrigin.Begin);
			}
			else
			{
				var buffer = new byte[PaddingSize(alignmentBase, currentIndex)];

				Stream.Read(buffer, 0, buffer.Length);
			}
		}
	}
}
