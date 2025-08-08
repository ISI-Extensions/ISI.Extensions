#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
	public partial class Stream
	{
		public class PocketStream : System.IO.Stream
		{
			private readonly System.IO.Stream _stream;
			private long _pocketStreamOffset;
			private long _pocketStreamLength;
			private readonly bool _leaveParentOpen;
			private readonly bool _readOnly;
			private long _position;

			public PocketStream(System.IO.Stream stream, long offset, long length, bool leaveParentOpen = false, bool readOnly = false)
			{
				_stream = stream;
				_pocketStreamOffset = offset;
				_pocketStreamLength = length;
				_leaveParentOpen = leaveParentOpen;
				_readOnly = readOnly;
				_position = 0;

				if (_stream.CanSeek)
				{
					Seek(0, System.IO.SeekOrigin.Begin);
				}
			}

			public override bool CanRead => _stream.CanRead;

			public override bool CanSeek => _stream.CanSeek;

			public override bool CanWrite => !_readOnly && _stream.CanWrite;

			public override long Length => _pocketStreamLength;

			public override long Position
			{
				get => _position;

				set
				{
					lock (_stream)
					{
						_stream.Position = value + Offset;
						_position = value;
					}
				}
			}

			public System.IO.Stream Stream => _stream;

			public long Offset => _pocketStreamOffset;

			public override void Flush()
			{
				lock (_stream)
				{
					_stream.Flush();
				}
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				lock (_stream)
				{
					EnsurePosition();

					// Make sure we don't pass the size of the substream
					var bytesRemaining = Length - Position;
					var bytesToRead = Math.Min(count, bytesRemaining);

					if (bytesToRead < 0)
					{
						bytesToRead = 0;
					}

					var read = _stream.Read(buffer, offset, (int)bytesToRead);
					_position += read;
					return read;
				}
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				if (_readOnly)
				{
					throw new NotSupportedException();
				}

				lock (_stream)
				{
					EnsurePosition();

					if (Position + offset + count > Length || Position < 0)
					{
						throw new InvalidOperationException("This write operation would exceed the current length of the pocket stream.");
					}

					_stream.Write(buffer, offset, count);
					_position += count;
				}
			}

			public override void WriteByte(byte value)
			{
				if (_readOnly)
				{
					throw new NotSupportedException();
				}

				lock (_stream)
				{
					EnsurePosition();

					if (Position > Length || Position < 0)
					{
						throw new InvalidOperationException("This write operation would exceed the current length of the pocket stream.");
					}

					_stream.WriteByte(value);
					_position++;
				}
			}

			public override long Seek(long offset, System.IO.SeekOrigin origin)
			{
				lock (_stream)
				{
					switch (origin)
					{
						case System.IO.SeekOrigin.Begin:
							offset += _pocketStreamOffset;
							break;

						case System.IO.SeekOrigin.End:
							var enddelta = _pocketStreamOffset + _pocketStreamLength - _stream.Length;
							offset += enddelta;
							break;

						case System.IO.SeekOrigin.Current:
							// Nothing to do, because we'll pass SeekOrigin.Current to the
							// parent stream.
							break;
					}

					// If we're doing an absolute seek, we don't care about the position,
					// but if the seek is relative, make sure we start from the correct position
					if (origin == System.IO.SeekOrigin.Current)
					{
						EnsurePosition();
					}

					var parentPosition = _stream.Seek(offset, origin);
					_position = parentPosition - Offset;
					return _position;
				}
			}

			public override void SetLength(long value)
			{
				if (_readOnly)
				{
					throw new NotSupportedException();
				}

				_pocketStreamLength = value;
			}

			public void UpdateWindow(long offset, long length)
			{
				_pocketStreamOffset = offset;
				_pocketStreamLength = length;
			}

			protected override void Dispose(bool disposing)
			{
				if (!_leaveParentOpen)
				{
					_stream.Dispose();
				}

				base.Dispose(disposing);
			}

			public void EnsurePosition()
			{
				if (_stream.Position != _position + Offset)
				{
					_stream.Position = _position + Offset;
				}
			}
		}
	}
}