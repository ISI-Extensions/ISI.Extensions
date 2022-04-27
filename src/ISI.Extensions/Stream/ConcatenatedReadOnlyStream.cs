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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class Stream
	{
		public delegate void ConcatenatedReadOnlyStream_OnNextStream(int streamIndex);

		public class ConcatenatedReadOnlyStream : System.IO.Stream, IEnumerable<System.IO.Stream>, IStreamSourceInformation
		{
			public class StreamInformation : System.IO.Stream
			{
				public System.IO.Stream Stream { get; set; }
				public bool ResponsibleForStream { get; set; }

				public override void Flush()
				{
					Stream.Flush();
				}

				public override long Seek(long offset, SeekOrigin origin)
				{
					return Stream.Seek(offset, origin);
				}

				public override void SetLength(long value)
				{
					Stream.SetLength(value);
				}

				public override int Read(byte[] buffer, int offset, int count)
				{
					return Stream.Read(buffer, offset, count);
				}

				public override void Write(byte[] buffer, int offset, int count)
				{
					Stream.Write(buffer, offset, count);
				}

				public override bool CanRead => Stream.CanRead;
				public override bool CanSeek => Stream.CanSeek;
				public override bool CanWrite => Stream.CanWrite;
				public override long Length => Stream.Length;

				public override long Position
				{
					get => Stream.Position;
					set => Stream.Position = value;
				}
			}

			private List<System.IO.Stream> _streams;

			public int StreamIndex { get; private set; }

			public string FileName => (((StreamIndex >= 0) && (_streams != null) && (StreamIndex < _streams.Count) && (_streams[StreamIndex] is IStreamSourceInformation)) ? ((IStreamSourceInformation)_streams[StreamIndex]).FileName : string.Empty);

			public string SourceFileName => (((StreamIndex >= 0) && (StreamIndex < _streams.Count) && (_streams[StreamIndex] is IStreamSourceInformation)) ? ((IStreamSourceInformation)_streams[StreamIndex]).SourceFileName : string.Empty);

			public ConcatenatedReadOnlyStream_OnNextStream OnNextStream { get; set; }

			public ConcatenatedReadOnlyStream()
			{
				_streams = new List<System.IO.Stream>();
				Reset();
			}

			public ConcatenatedReadOnlyStream(IEnumerable<System.IO.Stream> streams)
			{
				_streams = streams.ToList();
				Reset();
			}

			public void Add(System.IO.Stream stream, bool responsibleForStream)
			{
				stream.Rewind();

				_streams.Add(new StreamInformation()
				{
					Stream = stream,
					ResponsibleForStream = responsibleForStream
				});
			}

			public void Add(System.IO.Stream stream)
			{
				Add(stream, false);
			}

			public void Reset()
			{
				StreamIndex = 0;

				foreach (var stream in _streams)
				{
					stream.Rewind();
				}
			}

			public override bool CanRead
			{
				get { return _streams.All(stream => stream.CanRead); }
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				if (!_streams.Any())
				{
					return 0;
				}

				var totalBytesRead = 0;
				var bytesRead = int.MaxValue;

				while ((totalBytesRead < count) && (bytesRead > 0) && (_streams != null) && (StreamIndex < _streams.Count))
				{
					bytesRead = _streams[StreamIndex].Read(buffer, offset, count - totalBytesRead);

					totalBytesRead += bytesRead;
					offset += bytesRead;

					if (totalBytesRead < count)
					{
						NextStream();
						bytesRead = int.MaxValue;
					}
				}

				return totalBytesRead;
			}

			private void NextStream()
			{
				StreamIndex++;

				OnNextStream?.Invoke(StreamIndex);
			}

			public override bool CanSeek
			{
				get { return _streams.All(stream => stream.CanSeek); }
			}

			public override bool CanWrite => false;

			public override void Flush()
			{
				foreach (var stream in _streams)
				{
					stream.Flush();
				}
			}

			public override long Length
			{
				get { return _streams.Sum(stream => stream.Length); }
			}

			public override long Position
			{
				get
				{
					long position = 0;

					if (StreamIndex > 0)
					{
						position += _streams.Take(StreamIndex).Sum(stream => stream.Length);
					}

					return position + _streams[StreamIndex].Position;
				}
				set
				{
					if (!CanSeek)
					{
						throw new NotImplementedException();
					}

					if ((value < 0) || (value > Length))
					{
						throw new IndexOutOfRangeException();
					}

					foreach (var stream in _streams)
					{
						stream.Rewind();
					}

					long position = 0;

					var streamIndex = StreamIndex;

					StreamIndex = 0;

					while ((position < value) && (_streams != null) && (StreamIndex < _streams.Count))
					{
						if ((position + _streams[StreamIndex].Length) < value)
						{
							position += _streams[StreamIndex].Length;
							StreamIndex++;
						}
						else
						{
							_streams[StreamIndex].Position = value - position;
						}
					}

					if ((OnNextStream != null) && (streamIndex != StreamIndex))
					{
						OnNextStream(StreamIndex);
					}
				}
			}

			public override long Seek(long offset, System.IO.SeekOrigin origin)
			{
				long position = 0;

				switch (origin)
				{
					case System.IO.SeekOrigin.Begin:
						position = offset;
						break;
					case System.IO.SeekOrigin.Current:
						position = Position + offset;
						break;
					case System.IO.SeekOrigin.End:
						position = Length + offset;
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
				}

				Position = position;

				return position;
			}

			public override void SetLength(long value)
			{
				throw new NotImplementedException();
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotImplementedException();
			}

			protected override void Dispose(bool disposing)
			{
				if (!disposing)
				{
					foreach (var stream in _streams)
					{
						if ((stream is StreamInformation streamInformation) && streamInformation.ResponsibleForStream)
						{
							streamInformation.Stream?.Dispose();
							streamInformation.Stream = null;
						}
					}
				}

				base.Dispose(disposing);
			}

			IEnumerator<System.IO.Stream> IEnumerable<System.IO.Stream>.GetEnumerator()
			{
				return _streams.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _streams.GetEnumerator();
			}
		}
	}
}