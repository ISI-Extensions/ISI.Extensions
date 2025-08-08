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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Linux
{
	public class ArchiverStreamWriter : IDisposable
	{
		private readonly System.IO.Stream _stream;

		public ArchiverStreamWriter(System.IO.Stream stream)
		{
			_stream = stream;

			using (var streamWriter = new System.IO.StreamWriter(_stream, Encoding.UTF8, -1, true))
			{
				streamWriter.Write(ArchiverHeader.ArchiverMagic);
				streamWriter.Flush();
			}
		}

		public void WriteEntry(string fileName, ISI.Extensions.IO.LinuxFileMode linuxFileMode, System.IO.Stream data)
		{
			var archiverHeader = new ArchiverHeader
			{
				EndChar = "`\n",
				LinuxFileMode = linuxFileMode,
				FileName = fileName,
				FileSize = (uint)data.Length,
				GroupId = 0,
				OwnerId = 0,
				LastModified = DateTimeOffset.UtcNow
			};

			WriteEntry(archiverHeader, data);
		}

		public void WriteEntry(ArchiverHeader header, System.IO.Stream data)
		{
			_stream.WriteStruct(header);

			data.CopyTo(_stream);
			
			if (_stream.Position % 2 != 0)
			{
				_stream.WriteByte(0);
			}
		}

		public void Dispose()
		{
			_stream?.Dispose();
		}
	}
}
