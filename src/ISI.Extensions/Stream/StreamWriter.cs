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

namespace ISI.Extensions
{
	public partial class Stream
	{
		public class StreamWriter : IDisposable
		{
			protected System.IO.Stream _stream = null;
			protected Encoding _encoding = null;

			public StreamWriter(System.IO.Stream stream)
			{
				Initialize(stream, EncodingType.Unicode);
			}

			public StreamWriter(System.IO.Stream stream, EncodingType encoding)
			{
				Initialize(stream, encoding);
			}

			private void Initialize(System.IO.Stream stream, EncodingType encoding)
			{
				_stream = stream;
				switch (encoding)
				{
					case EncodingType.ASCII:
						_encoding = new ASCIIEncoding();
						break;

					case EncodingType.UTF8:
						_encoding = new UTF8Encoding();
						break;

					default:
						_encoding = new UnicodeEncoding();
						break;
				}
			}

			public void Write(string format, params object[] args)
			{
				var buffer = _encoding.GetBytes(string.Format(format, args));
				_stream.Write(buffer, 0, buffer.Length);
			}

			public void Write(string message)
			{
				var buffer = _encoding.GetBytes(message);
				_stream.Write(buffer, 0, buffer.Length);
			}

			public void WriteLine(string format, params object[] args)
			{
				Write(string.Format(format, args) + "\n");
			}

			public void WriteLine(string message)
			{
				Write($"{message}\n");
			}

			public override string ToString()
			{
				var result = new StringBuilder();

				_stream.Rewind();

				using (var stream = new System.IO.StreamReader(_stream))
				{
					result.Append(stream.ReadToEnd());
				}

				return result.ToString();
			}

			public void Dispose()
			{
				_stream?.Dispose();
				_stream = null;
				_encoding = null;
			}
		}
	}
}