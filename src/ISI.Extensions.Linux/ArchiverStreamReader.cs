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
	public class ArchiverStreamReader : ArchiveStreamReader
	{
		private bool _MagicRead;

		private ArchiverHeader _archiverHeader;

		public ArchiverStreamReader(System.IO.Stream stream, bool leaveOpen = false)
			: base(stream, leaveOpen)
		{
		}

		public override bool Read()
		{
			if (!_MagicRead)
			{
				var buffer = new byte[ArchiverHeader.ArchiverMagic.Length];

				Stream.Read(buffer, 0, buffer.Length);

				var header = Encoding.ASCII.GetString(buffer);

				if (!string.Equals(header, ArchiverHeader.ArchiverMagic, StringComparison.Ordinal))
				{
					throw new System.IO.InvalidDataException("The .ar file did not start with the expected header");
				}

				_MagicRead = true;
			}

			if (FileStream != null)
			{
				FileStream.Dispose();
			}

			Align(2);

			if (Stream.Position < Stream.Length)
			{
				_archiverHeader = Stream.ReadStruct<ArchiverHeader>();
				FileHeader = _archiverHeader;
				FileName = _archiverHeader.FileName;

				if (_archiverHeader.EndChar != "`\n")
				{
					throw new System.IO.InvalidDataException("The magic for the file entry is invalid");
				}
				
				FileStream = new ISI.Extensions.Stream.PocketStream(Stream, Stream.Position, _archiverHeader.FileSize, leaveParentOpen: true);

				return true;
			}

			return false;
		}
	}
}
