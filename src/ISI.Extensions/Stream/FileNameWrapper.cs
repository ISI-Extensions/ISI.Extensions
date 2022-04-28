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
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class Stream
	{
		public class FileNameWrapper : System.IO.Stream, IStreamSourceInformation
		{
			private readonly System.IO.Stream _stream;
			public string FileName { get; }
			public string SourceFileName { get; }

			public FileNameWrapper(System.IO.Stream stream, string fileName)
				: this(stream, fileName, fileName)
			{
			}

			public FileNameWrapper(System.IO.Stream stream, string fileName, string sourceFileName)
			{
				_stream = stream;
				FileName = fileName;
				SourceFileName = sourceFileName;
			}

			public override void Flush()
			{
				_stream.Flush();
			}

			public override long Seek(long offset, System.IO.SeekOrigin origin)
			{
				return _stream.Seek(offset, origin);
			}

			public override void SetLength(long value)
			{
				_stream.SetLength(value);
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				return _stream.Read(buffer, offset, count);
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				_stream.Write(buffer, offset, count);
			}

			public override bool CanRead => _stream.CanRead;
			public override bool CanSeek => _stream.CanSeek;
			public override bool CanWrite => _stream.CanWrite;
			public override long Length => _stream.Length;

			public override long Position
			{
				get => _stream.Position;
				set => _stream.Position = value;
			}
		}
	}
}