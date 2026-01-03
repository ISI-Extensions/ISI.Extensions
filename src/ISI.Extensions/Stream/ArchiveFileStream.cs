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

namespace ISI.Extensions
{
	public partial class Stream
	{
		public class ArchiveFileStream : FileStream, IArchiveFileStream, IStreamSourceInformation
		{
			public string ArchiveFileName { get; set; }

			private string _relativeFileName = null;
			public string RelativeFileName
			{
				get => _relativeFileName;
				set
				{
					if (string.IsNullOrWhiteSpace(FileName))
					{
						FileName = value;
					}

					_relativeFileName = value;
				}
			}

			public ArchiveFileStream(string relativeFullName, string archiveFullName, System.IO.Stream stream = null, bool responsibleForStream = false)
				: base(relativeFullName, null, stream, responsibleForStream)
			{
				ArchiveFileName = archiveFullName;
			}

			string IStreamSourceInformation.SourceFileName => ArchiveFileName;
		}

		public class ArchiveFileStream<TStream> : FileStream<TStream>, IArchiveFileStream, IStreamSourceInformation
			where TStream : System.IO.Stream, new()
		{
			public string ArchiveFileName { get; set; }
			public string RelativeFileName { get; set; }

			public ArchiveFileStream(string fileName, string archiveFileName, TStream stream = null, bool responsibleForStream = false)
				: base(fileName, null, stream, responsibleForStream)
			{
				ArchiveFileName = archiveFileName;
			}

			string IStreamSourceInformation.SourceFileName => ArchiveFileName;
		}
	}
}