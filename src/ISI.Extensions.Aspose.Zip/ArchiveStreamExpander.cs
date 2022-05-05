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
using System.Collections.Generic;
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Aspose.Zip
{
	[ISI.Extensions.Stream.ArchiveStreamExpander]
	public class ArchiveStreamExpander : ISI.Extensions.Stream.IArchiveStreamExpander
	{
		protected delegate IEnumerable<(string RelativeFileName, Func<System.IO.Stream> GetStream)> ExpandStreamDelegate(System.IO.Stream archiveStream, ISI.Extensions.Stream.FileStreamFilter archiveFileFilter);

		private readonly IDictionary<string, ExpandStreamDelegate> _fileNameExtensionExpanderLookup = new Dictionary<string, ExpandStreamDelegate>(StringComparer.InvariantCultureIgnoreCase)
		{
			{ ".rar", ExpandRarStream },
			{ ".zip", ExpandZipStream },
			{ ".tar", ExpandTarStream },
			{ ".7z", ExpandSevenZipStream },
			{ ".gz", ExpandGzipStream },
		};

		public bool Handles(string fileName)
		{
			return _fileNameExtensionExpanderLookup.ContainsKey(System.IO.Path.GetExtension(fileName));
		}

		public ISI.Extensions.Stream.FileStreamCollection Expand<TStream>(string fileName, System.IO.Stream archiveStream, ISI.Extensions.Stream.FileStreamFilter archiveFileFilter = null)
			where TStream : System.IO.Stream, new()
		{
			archiveFileFilter ??= (_ => true);

			var fileStreams = new ISI.Extensions.Stream.FileStreamCollection();

			if (_fileNameExtensionExpanderLookup.TryGetValue(System.IO.Path.GetExtension(fileName), out var expandStream))
			{
				archiveStream.Rewind();

				foreach (var archiveEntry in expandStream(archiveStream, archiveFileFilter))
				{
					var relativeFileName = archiveEntry.RelativeFileName;
					if (string.IsNullOrWhiteSpace(relativeFileName))
					{
						relativeFileName = System.IO.Path.GetFileName(System.IO.Path.GetFileNameWithoutExtension(fileName));
					}

					if (archiveFileFilter(relativeFileName))
					{
						var archiveFileStream = new ISI.Extensions.Stream.ArchiveFileStream<TStream>(relativeFileName, fileName)
						{
							RelativeFileName = relativeFileName,
						};

						using (var stream = archiveEntry.GetStream())
						{
							stream.Rewind();

							stream.CopyTo(archiveFileStream.Stream);

							archiveFileStream.Stream.Rewind();
						}

						fileStreams.Add(archiveFileStream);
					}
				}
			}

			return fileStreams;
		}

		private static IEnumerable<(string RelativeFileName, Func<System.IO.Stream> GetStream)> ExpandRarStream(System.IO.Stream archiveStream, ISI.Extensions.Stream.FileStreamFilter archiveFileFilter)
		{
			using (var archive = new global::Aspose.Zip.Rar.RarArchive(archiveStream))
			{
				foreach (var archiveEntry in archive.Entries)
				{
					if (!archiveEntry.IsDirectory)
					{
						yield return (RelativeFileName: archiveEntry.Name, GetStream: () => archiveEntry.Open());
					}
				}
			}
		}

		private static IEnumerable<(string RelativeFileName, Func<System.IO.Stream> GetStream)> ExpandZipStream(System.IO.Stream archiveStream, ISI.Extensions.Stream.FileStreamFilter archiveFileFilter)
		{
			using (var archive = new global::Aspose.Zip.Archive(archiveStream))
			{
				foreach (var archiveEntry in archive.Entries)
				{
					if (!archiveEntry.IsDirectory)
					{
						yield return (RelativeFileName: archiveEntry.Name, GetStream: () => archiveEntry.Open());
					}
				}
			}
		}

		private static IEnumerable<(string RelativeFileName, Func<System.IO.Stream> GetStream)> ExpandTarStream(System.IO.Stream archiveStream, ISI.Extensions.Stream.FileStreamFilter archiveFileFilter)
		{
			using (var archive = new global::Aspose.Zip.Tar.TarArchive(archiveStream))
			{
				foreach (var archiveEntry in archive.Entries)
				{
					if (!archiveEntry.IsDirectory)
					{
						yield return (RelativeFileName: archiveEntry.Name, GetStream: () => archiveEntry.Open());
					}
				}
			}
		}

		private static IEnumerable<(string RelativeFileName, Func<System.IO.Stream> GetStream)> ExpandSevenZipStream(System.IO.Stream archiveStream, ISI.Extensions.Stream.FileStreamFilter archiveFileFilter)
		{
			using (var tempDirectory = new ISI.Extensions.IO.Path.TempDirectory())
			{
				using (var archive = new global::Aspose.Zip.SevenZip.SevenZipArchive(archiveStream))
				{
					archive.ExtractToDirectory(tempDirectory.FullName);
				}

				foreach (var fileName in System.IO.Directory.EnumerateFiles(tempDirectory.FullName, "*", System.IO.SearchOption.AllDirectories))
				{
					yield return (RelativeFileName: fileName.Substring(tempDirectory.FullName.Length + 1), GetStream: () => System.IO.File.Open(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read));
				}
			}
		}

		private static IEnumerable<(string RelativeFileName, Func<System.IO.Stream> GetStream)> ExpandGzipStream(System.IO.Stream archiveStream, ISI.Extensions.Stream.FileStreamFilter archiveFileFilter)
		{
			using (var archive = new global::Aspose.Zip.Gzip.GzipArchive(archiveStream))
			{
				yield return (RelativeFileName: archive.Name, GetStream: () =>
				{
					var stream = new Stream.TempFileStream();
					
					archive.Extract(stream);

					return stream;
				});
			}
		}
	}
}
