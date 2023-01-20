#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
		public class FileStream : IFileStream, IStreamSourceInformation
		{
			public static Comparison<FileStream> SourceFileNameComparer
			{
				get { return (xFileStream, yFileStream) => StringComparer.CurrentCultureIgnoreCase.Compare(((ISI.Extensions.Stream.IStreamSourceInformation)xFileStream).SourceFileName, ((ISI.Extensions.Stream.IStreamSourceInformation)yFileStream).SourceFileName); }
			}

			public static Comparison<FileStream> FileNameComparer
			{
				get { return (xFileStream, yFileStream) => StringComparer.CurrentCultureIgnoreCase.Compare(xFileStream.FileName, yFileStream.FileName); }
			}

			public string FileName { get; protected set; }
			public string SourceFileName { get; protected set; }

			public bool ResponsibleForStream { get; protected set; }

			public FileStream(string fileName, string sourceFileName, System.IO.Stream stream = null, bool responsibleForStream = false)
			{
				FileName = fileName;
				SourceFileName = sourceFileName;
				_stream = stream;
				ResponsibleForStream = responsibleForStream;
			}

			protected System.IO.Stream _stream = null;
			public virtual System.IO.Stream Stream
			{
				get => _stream ??= CreateStream();
				set
				{
					ResponsibleForStream = false;
					_stream = value;
				}
			}

			protected virtual System.IO.Stream CreateStream()
			{
				ResponsibleForStream = true;
				return new System.IO.MemoryStream();
			}

			public void Dispose()
			{
				if (ResponsibleForStream && (_stream != null))
				{
					_stream.Dispose();
					_stream = null;
				}
			}
		}

		public class FileStream<TStream> : FileStream
			where TStream : System.IO.Stream, new()
		{
			public FileStream(string fileName, string sourceFileName, TStream stream = null, bool responsibleForStream = false)
				: base(fileName, sourceFileName, stream, responsibleForStream)
			{
			}

			protected override System.IO.Stream CreateStream()
			{
				ResponsibleForStream = true;
				return new TStream();
			}
		}

		internal class SourceFileStream : FileStream
		{
			public SourceFileStream(string fileName)
				: base(fileName, null, null, false)
			{

			}

			protected override System.IO.Stream CreateStream()
			{
				ResponsibleForStream = true;
				return ISI.Extensions.FileSystem.OpenRead(FileName);
			}
		}
	}
}