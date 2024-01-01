#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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

namespace ISI.Extensions.SharpCompress
{
	//[ISI.Extensions.Stream.ArchiveStreamExpander]
	public class ArchiveStreamExpander : ISI.Extensions.Stream.IArchiveStreamExpander
	{
		private readonly IDictionary<string, global::SharpCompress.Common.ArchiveType> _fileNameExtensionsLookup = new Dictionary<string, global::SharpCompress.Common.ArchiveType>(StringComparer.InvariantCultureIgnoreCase)
		{
			{".rar", global::SharpCompress.Common.ArchiveType.Rar},
			{".zip", global::SharpCompress.Common.ArchiveType.Zip},
			{".tar", global::SharpCompress.Common.ArchiveType.Tar},
			{".7z", global::SharpCompress.Common.ArchiveType.SevenZip},
			{".gz", global::SharpCompress.Common.ArchiveType.GZip},
		};

		public bool Handles(string fileName)
		{
			return _fileNameExtensionsLookup.ContainsKey(System.IO.Path.GetExtension(fileName));
		}

		public ISI.Extensions.Stream.FileStreamCollection Expand<TStream>(string fileName, System.IO.Stream archiveStream, ISI.Extensions.Stream.FileStreamFilter archiveFileFilter = null)
			where TStream : System.IO.Stream, new()
		{
			var result = new ISI.Extensions.Stream.FileStreamCollection();

			archiveFileFilter ??= (_ => true);

			var archiveFileName = System.IO.Path.GetFileNameWithoutExtension(fileName);

			if (archiveFileFilter(archiveFileName))
			{
				archiveStream.Rewind();
				using (var reader = global::SharpCompress.Readers.ReaderFactory.Open(archiveStream))
				{
					while (reader.MoveToNextEntry())
					{
						if (!reader.Entry.IsDirectory)
						{
							var archiveFileStream = new ISI.Extensions.Stream.ArchiveFileStream<TStream>(reader.Entry.Key, fileName)
							{
								RelativeFileName = reader.Entry.Key,
							};

							reader.WriteEntryTo(archiveFileStream.Stream);

							result.Add(archiveFileStream);
						}
					}
				}
			}

			return result;
		}
	}
}
