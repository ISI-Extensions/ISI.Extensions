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
using System.Threading.Tasks;

namespace ISI.Extensions.Stream
{
	public class ProgressStream : System.IO.Stream
	{
		public delegate void OnReadDelegate(int offset, int count);

		public delegate void OnWriteDelegate(int offset, int count);

		protected System.IO.Stream WrappedStream { get; set; }

		public override bool CanRead => WrappedStream.CanRead;
		public override bool CanSeek => WrappedStream.CanSeek;
		public override bool CanWrite => WrappedStream.CanWrite;
		public override long Length => WrappedStream.Length;

		public override long Position
		{
			get => WrappedStream.Position;
			set => WrappedStream.Position = value;
		}

		public event OnReadDelegate OnRead = null;
		public event OnWriteDelegate OnWrite = null;

		public ProgressStream(System.IO.Stream wrappedStream)
		{
			WrappedStream = wrappedStream;
		}

		public override void Flush() => WrappedStream.Flush();
		public override long Seek(long offset, System.IO.SeekOrigin origin) => WrappedStream.Seek(offset, origin);
		public override void SetLength(long value) => WrappedStream.SetLength(value);

		public override int Read(byte[] buffer, int offset, int count)
		{
			OnRead?.Invoke(offset, count);

			return WrappedStream.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			OnWrite?.Invoke(offset, count);

			WrappedStream.Write(buffer, offset, count);
		}

		protected override void Dispose(bool disposing) => WrappedStream.Dispose();
	}
}