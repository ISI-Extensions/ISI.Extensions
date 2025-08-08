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
	public class TarStreamReader : ArchiveStreamReader
	{
		private TarHeader _entryHeader;

		public TarStreamReader(System.IO.Stream stream, bool leaveOpen)
			: base(stream, leaveOpen)
		{
		}

		public string LinkName { get; private set; }

		public override bool Read()
		{
			Align(512);

			FileStream?.Dispose();
			FileStream = null;

			_entryHeader = Stream.ReadStruct<TarHeader>();
			FileHeader = _entryHeader;
			FileName = _entryHeader.FileName;
			LinkName = _entryHeader.LinkName;

			// There are two empty blocks at the end of the file.
			if (string.IsNullOrEmpty(_entryHeader.Magic))
			{
				return false;
			}

			if (!string.Equals(_entryHeader.Magic, TarHeader.UsTarMagic, StringComparison.InvariantCultureIgnoreCase))
			{
				throw new System.IO.InvalidDataException("The magic for the file entry is invalid");
			}

			Align(512);

			// TODO: Validate Checksum
			FileStream = new ISI.Extensions.Stream.PocketStream(Stream, Stream.Position, _entryHeader.FileSize, leaveParentOpen: true);

			if (_entryHeader.TypeFlag == TarType.LongName)
			{
				using (var stream = Open())
				{
					var longFileName = stream.TextReadToEnd();
					var read = Read();
					FileName = longFileName;
					return read;
				}
			}

			if (_entryHeader.TypeFlag == TarType.LongLink)
			{
				using (var stream = Open())
				{
					var longLinkName = stream.TextReadToEnd();
					var read = Read();
					LinkName = longLinkName;
					return read;
				}
			}

			return true;
		}
	}
}
