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
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions
{
	public partial class Stream
	{
		public delegate bool FileStreamFilter(string fileName);

		public class FileStreamCollection : IEnumerable<FileStream>, IDisposable
		{
			private static IArchiveStreamExpander[] _archiveStreamExpanders = null;
			protected IArchiveStreamExpander[] ArchiveStreamExpanders => _archiveStreamExpanders ??= ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementations<IArchiveStreamExpander>(ISI.Extensions.ServiceLocator.Current);

			public static FileStreamFilter GetFilter(System.Text.RegularExpressions.Regex regEx)
			{
				if (regEx == null)
				{
					return fileName => true;
				}

				return regEx.IsMatch;
			}

			public static FileStreamFilter GetFilter(string regEx)
			{
				if (string.IsNullOrEmpty(regEx))
				{
					return fileName => true;
				}

				var regex = new System.Text.RegularExpressions.Regex(regEx);

				return fileName => regex.IsMatch(fileName);
			}

			private List<FileStream> _fileStreams = null;
			protected List<FileStream> FileStreams => _fileStreams ??= new();

			public void Add(string fileName)
			{
				AddRange(new[] { fileName });
			}

			public void Add(string fileName, bool processArchives)
			{
				Add(fileName, (System.Text.RegularExpressions.Regex)null, processArchives, (System.Text.RegularExpressions.Regex)null);
			}

			public void Add(string fileName, System.Text.RegularExpressions.Regex fileFilterRegEx, bool processArchives, System.Text.RegularExpressions.Regex archiveFileFilterRegEx)
			{
				AddRange(new[] { fileName }, fileFilterRegEx, processArchives, archiveFileFilterRegEx);
			}

			public void Add(FileStream fileStream)
			{
				Add(fileStream, null, false, null);
			}

			public void Add(FileStream fileStream, bool processArchives)
			{
				Add(fileStream, null, processArchives, null);
			}

			public void Add(string fileName, System.IO.Stream stream, bool processArchives, FileStreamFilter archiveFileFilter)
			{
				var sourceStream = new SourceFileStream(fileName)
				{
					Stream = stream,
				};

				Add(sourceStream, null, processArchives, archiveFileFilter);
			}

			public void Add(FileStream fileStream, FileStreamFilter fileFilter, bool processArchives, FileStreamFilter archiveFileFilter)
			{
				fileFilter ??= (fileInfo => true);
				archiveFileFilter ??= (fileInfo => true);

				if (fileFilter(fileStream.FileName))
				{
					var processed = false;

					if (processArchives)
					{
						foreach (var archiveStreamProcessor in ArchiveStreamExpanders)
						{
							if (!processed)
							{
								if (archiveStreamProcessor.Handles(fileStream.FileName))
								{
									var archives = archiveStreamProcessor.Expand<TempFileStream>(fileStream.FileName, fileStream.Stream, archiveFileFilter);

									foreach (var archive in archives)
									{
										if (archive is ArchiveFileStream archiveFileStream)
										{
											if (fileStream is ArchiveFileStream sourceArchiveFileStream)
											{
												archiveFileStream.ArchiveFileName = sourceArchiveFileStream.ArchiveFileName;
											}
											else
											{
												archiveFileStream.ArchiveFileName = fileStream.FileName;
											}
										}

										Add(archive, null, true, archiveFileFilter);
									}

									processed = true;
								}
							}
						}
					}

					if (processed)
					{
						fileStream.Dispose();
					}
					else
					{
						fileStream.Stream.Rewind();
						FileStreams.Add(fileStream);
					}
				}
			}



			public void AddDirectoryFiles(string directoryName, bool doRecursive, string fileFilterRegEx, bool processArchives, string archiveFileFilterRegEx)
			{
				AddDirectoryFiles(directoryName, doRecursive, GetFilter(fileFilterRegEx), processArchives, GetFilter(archiveFileFilterRegEx));
			}

			public void AddDirectoryFiles(string directoryName, bool doRecursive, System.Text.RegularExpressions.Regex fileFilterRegEx, bool processArchives, System.Text.RegularExpressions.Regex archiveFileFilterRegEx)
			{
				AddDirectoryFiles(directoryName, doRecursive, GetFilter(fileFilterRegEx), processArchives, GetFilter(archiveFileFilterRegEx));
			}

			public void AddDirectoryFiles(string directoryName, bool doRecursive, FileStreamFilter fileFilter, bool processArchives, FileStreamFilter archiveFileFilter)
			{
				fileFilter ??= (fileName => true);

				foreach (var fileSystemPath in ISI.Extensions.FileSystem.GetDirectoryFileSystemPaths(directoryName, doRecursive))
				{
					if (fileFilter(fileSystemPath.FullPath()))
					{
						Add(new SourceFileStream(fileSystemPath.FullPath()), null, processArchives, archiveFileFilter);
					}
				}
			}








			public void AddRange(IEnumerable<FileStream> fileStreams)
			{
				AddRange(fileStreams, (FileStreamFilter)null, false, (FileStreamFilter)null);
			}

			public void AddRange(IEnumerable<FileStream> fileStreams, bool processArchives)
			{
				AddRange(fileStreams, (FileStreamFilter)null, processArchives, (FileStreamFilter)null);
			}

			public void AddRange(IEnumerable<FileStream> fileStreams, string fileFilterRegEx, bool processArchives, string archiveFileFilterRegEx)
			{
				AddRange(fileStreams, GetFilter(fileFilterRegEx), processArchives, GetFilter(archiveFileFilterRegEx));
			}

			public void AddRange(IEnumerable<FileStream> fileStreams, System.Text.RegularExpressions.Regex fileFilterRegEx, bool processArchives, System.Text.RegularExpressions.Regex archiveFileFilterRegEx)
			{
				AddRange(fileStreams, GetFilter(fileFilterRegEx), processArchives, GetFilter(archiveFileFilterRegEx));
			}

			public void AddRange(IEnumerable<FileStream> fileStreams, FileStreamFilter fileFilter, bool processArchives, FileStreamFilter archiveFileFilter)
			{
				fileFilter ??= (fileName => true);

				foreach (var fileStream in fileStreams)
				{
					if (fileFilter(fileStream.FileName))
					{
						Add(fileStream, fileFilter, processArchives, archiveFileFilter);
					}
				}
			}

			public void AddRange(IEnumerable<string> fileNames)
			{
				AddRange(fileNames, (FileStreamFilter)null, false, (FileStreamFilter)null);
			}

			public void AddRange(IEnumerable<string> fileNames, string fileFilterRegEx, bool processArchives, string archiveFileFilterRegEx)
			{
				AddRange(fileNames, GetFilter(fileFilterRegEx), processArchives, GetFilter(archiveFileFilterRegEx));
			}

			public void AddRange(IEnumerable<string> fileNames, System.Text.RegularExpressions.Regex fileFilterRegEx, bool processArchives, System.Text.RegularExpressions.Regex archiveFileFilterRegEx)
			{
				AddRange(fileNames, GetFilter(fileFilterRegEx), processArchives, GetFilter(archiveFileFilterRegEx));
			}

			public void AddRange(IEnumerable<string> fileNames, FileStreamFilter fileFilter, bool processArchives, FileStreamFilter archiveFileFilter)
			{
				fileFilter ??= (fileInfo => true);

				foreach (var fileName in fileNames)
				{
					if (fileFilter(fileName))
					{
						Add(new SourceFileStream(fileName), null, processArchives, archiveFileFilter);
					}
				}
			}

			public bool TryGetValue(string fileName, out FileStream fileStream)
			{
				fileStream = this[fileName];

				return (fileStream != null);
			}

			public FileStream this[string fileName]
			{
				get { return FileStreams.FirstOrDefault(fileStream => string.Equals(fileStream.FileName ?? string.Empty, fileName, StringComparison.InvariantCultureIgnoreCase)); }
			}

			public FileStream this[int index] => FileStreams[index];

			public void Sort(bool useSourceFileNames = false)
			{
				FileStreams.Sort(useSourceFileNames ? FileStream.SourceFileNameComparer : FileStream.FileNameComparer);
			}

			public IEnumerable<IGrouping<string, FileStream>> GroupBySourceFileName()
			{
				return FileStreams.GroupBy(fileStream => ((ISI.Extensions.Stream.IStreamSourceInformation)fileStream).SourceFileName).OrderBy(fileStreamGroup => fileStreamGroup.Key, StringComparer.InvariantCultureIgnoreCase);
			}

			public void RemoveAll(Predicate<FileStream> match)
			{
				FileStreams.RemoveAll(match);
			}

			public void Dispose()
			{
				foreach (var fileStream in this)
				{
					fileStream.Dispose();
				}
			}

			IEnumerator<FileStream> IEnumerable<FileStream>.GetEnumerator()
			{
				return FileStreams.GetEnumerator();
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return FileStreams.GetEnumerator();
			}

			public string[] GetFileNames(bool useSourceFileNames = false)
			{
				return (new HashSet<string>(useSourceFileNames ? this.Select(fileStream => ((IStreamSourceInformation)fileStream).SourceFileName) : this.Select(fileStream => fileStream.FileName))).ToArray();
			}

			public void CloseSourceFiles()
			{
				foreach (var fileStream in this)
				{
					if (fileStream is SourceFileStream)
					{
						fileStream.Dispose();
					}
				}
			}
		}
	}
}