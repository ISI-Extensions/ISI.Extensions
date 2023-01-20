#region Copyright & License
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class Stream
	{
		public class TempFileStream : System.IO.FileStream, IDisposable
		{
			public const int DefaultBufferSize = 4096;

			public Action<System.IO.Stream> OnClose { get; set; }

			public TempFileStream()
				: this(System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read, DefaultBufferSize)
			{
			}

			public TempFileStream(System.IO.FileAccess access)
				: this(access, System.IO.FileShare.Read, DefaultBufferSize)
			{
			}

			public TempFileStream(System.IO.FileAccess access, System.IO.FileShare share)
				: this(access, share, DefaultBufferSize)
			{
			}

			public TempFileStream(System.IO.FileAccess access, System.IO.FileShare share, int bufferSize)
				: base(System.IO.Path.GetTempFileName(), System.IO.FileMode.Create, access, share, bufferSize, System.IO.FileOptions.DeleteOnClose)
			{
			}

			public TempFileStream(System.IO.Stream stream)
				: this(stream, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read, DefaultBufferSize)
			{
			}

			public TempFileStream(System.IO.Stream stream, System.IO.FileAccess access)
				: this(stream, access, System.IO.FileShare.Read, DefaultBufferSize)
			{
			}

			public TempFileStream(System.IO.Stream stream, System.IO.FileAccess access, System.IO.FileShare share)
				: this(stream, access, share, DefaultBufferSize)
			{
			}

			public TempFileStream(System.IO.Stream stream, System.IO.FileAccess access, System.IO.FileShare share, int bufferSize)
				: base(System.IO.Path.GetTempFileName(), System.IO.FileMode.Create, access, share, bufferSize, System.IO.FileOptions.DeleteOnClose)
			{
				Import(stream);
			}

			private void Import(System.IO.Stream stream, bool disposeStream = true)
			{
				if (stream != null)
				{
					stream.Rewind();
					stream.CopyTo(this);
					if (disposeStream)
					{
						stream.Dispose();
					}
				}
			}

			public TempFileStream(byte[] data)
				: this(data, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Read, DefaultBufferSize)
			{
			}

			public TempFileStream(byte[] data, System.IO.FileAccess access)
				: this(data, access, System.IO.FileShare.Read, DefaultBufferSize)
			{
			}

			public TempFileStream(byte[] data, System.IO.FileAccess access, System.IO.FileShare share)
				: this(data, access, share, DefaultBufferSize)
			{
			}

			public TempFileStream(byte[] data, System.IO.FileAccess access, System.IO.FileShare share, int bufferSize)
				: base(System.IO.Path.GetTempFileName(), System.IO.FileMode.Create, access, share, bufferSize, System.IO.FileOptions.DeleteOnClose)
			{
				Import(data);
			}

			private void Import(byte[] data)
			{
				if (data != null)
				{
					this.Write(data, 0, data.Length);
					this.Rewind();
				}
			}

			public override void Close()
			{
				if (OnClose != null)
				{
					var onClose = OnClose;
					OnClose = null;
					onClose(this);
				}

				base.Close();
			}
		}
	}
}