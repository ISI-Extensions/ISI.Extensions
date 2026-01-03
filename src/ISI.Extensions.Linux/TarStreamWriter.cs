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
	public class TarStreamWriter : IDisposable
	{
		private readonly System.IO.Stream _stream;
		private readonly System.Security.Cryptography.MD5 _md5;

		public TarStreamWriter(System.IO.Stream stream)
		{
			_stream = stream;
			_md5 = System.Security.Cryptography.MD5.Create();
		}

		public void WriteTrailer()
		{
			// The stream should already be aligned; as it is aligned after every entry.
			// As a safety measure, for streams which can report on .Position, align the
			// stream again.
			if (_stream.CanSeek)
			{
				Align();
			}

			var trailer = new byte[1024];

			_stream.Write(trailer, 0, trailer.Length);
		}

		public (string FileName, long FileLength, string Md5Hash) WriteEntry(TarHeader header, System.IO.Stream stream)
		{
			header.Checksum = header.ComputeChecksum();

			var bytesWritten = _stream.WriteStruct(header);

			Align(bytesWritten);

			stream.CopyTo(_stream);

			Align(stream.Length);

			if (stream.CanRead)
			{
				stream.Rewind();
			}

			var md5Hash = (stream.CanRead ? BitConverter.ToString(_md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant() : string.Empty);

			return (FileName: header.FileName, FileLength: stream.Length, Md5Hash: md5Hash);
		}

		public (string FileName, long FileLength, string Md5Hash) WriteEntry(ArchiveEntry entry, Func<System.IO.Stream> getStream, bool leaveOpen = false)
		{
			var targetPath = entry.TargetPath;
			if (!targetPath.StartsWith("."))
			{
				targetPath = "." + targetPath;
			}

			// Handle long file names (> 99 characters). If this is the case, add a "././@LongLink" pseudo-entry
			// which contains the full name.
			if (targetPath.Length > 99)
			{
				// Must include a trailing \0
				var nameLength = Encoding.UTF8.GetByteCount(targetPath);
				var entryName = new byte[nameLength + 1];

				Encoding.UTF8.GetBytes(targetPath, 0, targetPath.Length, entryName, 0);

				var nameEntry = new ArchiveEntry()
				{
					LinuxFileMode = entry.LinuxFileMode,
					ModifiedDateTime = entry.ModifiedDateTime,
					TargetPath = "././@LongLink",
					Owner = entry.Owner,
					Group = entry.Group
				};

				WriteEntry(nameEntry, () => new System.IO.MemoryStream(entryName));

				targetPath = targetPath.Substring(0, 99);
			}

			var isDirectory = entry.LinuxFileMode.HasFlag(ISI.Extensions.IO.LinuxFileMode.Directory);
			var isLink = !isDirectory && !string.IsNullOrWhiteSpace(entry.LinkTo);
			var isFile = !isDirectory && !isLink;

			TarType tarType;

			if (entry.TargetPath == "././@LongLink")
			{
				tarType = TarType.LongName;
			}
			else if (isFile)
			{
				tarType = TarType.RegularFile;
			}
			else if (isDirectory)
			{
				tarType = TarType.Directory;
			}
			else
			{
				tarType = TarType.SymbolicLink;
			}

			getStream ??= () => new System.IO.MemoryStream();

			var stream = getStream();

			try
			{
				var tarHeader = new TarHeader()
				{
					LinuxFileMode = entry.LinuxFileMode & ISI.Extensions.IO.LinuxFileMode.LinuxPermissionsMask,
					DevMajor = null,
					DevMinor = null,
					FileName = targetPath,
					FileSize = (uint)stream.Length,
					GroupId = 0,
					GroupName = entry.Group,
					UserId = 0,
					UserName = entry.Owner,
					LinkName = isLink ? entry.LinkTo : string.Empty,
					Prefix = string.Empty,
					TarType = tarType,
					Version = null,
					LastModified = entry.ModifiedDateTime,
					Magic = TarHeader.UsTarMagic,
				};

				return WriteEntry(tarHeader, stream);
			}
			finally
			{
				if (!leaveOpen)
				{
					stream.Dispose();
				}
			}
		}

		private void Align()
		{
			Align(_stream.Position);
		}

		private void Align(long position)
		{
			position %= 512;

			if (position != 0)
			{
				var align = new byte[512 - position];

				_stream.Write(align, 0, align.Length);
			}
		}

		public void Dispose()
		{
			WriteTrailer();
			_md5.Dispose();
		}
	}
}
