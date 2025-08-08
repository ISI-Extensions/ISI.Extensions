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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Debian.DataTransferObjects.DebianPackagingApi;

namespace ISI.Extensions.Debian
{
	public partial class DebianPackagingApi
	{
		public DTOs.CreateDebFileResponse CreateDebFile(DTOs.ICreateDebFileRequest request)
		{
			var response = new DTOs.CreateDebFileResponse();

			var md5sums = new StringBuilder();
			var installedSize = (long)0;
			void addWriteEntryResponse((string FileName, long FileLength, string Md5Hash) writeEntryResponse)
			{
				installedSize += writeEntryResponse.FileLength;
				md5sums.AppendLine($"{writeEntryResponse.Md5Hash}  {writeEntryResponse.FileName}");
			}

			var dataEntryDirectories = new HashSet<string>(StringComparer.InvariantCulture);
			var dataEntries = new Dictionary<string, ISI.Extensions.Linux.ArchiveEntry>(StringComparer.InvariantCulture);
			void addDataFile(DataFile dataFile)
			{
				var dataFileDirectory = System.IO.Path.GetDirectoryName(dataFile.TargetPath);
				if (!dataEntryDirectories.Contains(dataFileDirectory))
				{
					var dataFileDirectoryQueue = new Queue<string>(dataFileDirectory.Split(['/']));
					dataFileDirectory = string.Empty;
					while (dataFileDirectoryQueue.Any())
					{
						dataFileDirectory = $"{dataFileDirectory}/{dataFileDirectoryQueue.Dequeue()}/".TrimStart('/');
						if (!dataEntryDirectories.Contains(dataFileDirectory))
						{
							dataEntries[dataFileDirectory] = new DataDirectory(dataFileDirectory);
						}
					}
				}

				dataEntries[dataFile.TargetPath] = dataFile;
			}

			foreach (var requestDataFile in request.DataEntries ?? [])
			{
				switch (requestDataFile)
				{
					case DTOs.ICreateDebFileRequestDataEntryFile createDebFileRequestDataFileFile:
						if (createDebFileRequestDataFileFile.IsAscii)
						{
							using (var stream = (requestDataFile as DTOs.CreateDebFileRequestEntryStream)?.SourceStream ?? System.IO.File.OpenRead((requestDataFile as DTOs.CreateDebFileRequestEntryFile).SourceFullName))
							{
								var content = stream.TextReadToEnd().Replace("\r\n", "\n").Replace("\r", "\n");

								var modifiedDateTime = DateTimeOffset.UtcNow;
								if (requestDataFile is DTOs.CreateDebFileRequestEntryFile createDebFileRequestDataFile)
								{
									modifiedDateTime = (new System.IO.FileInfo(createDebFileRequestDataFile.SourceFullName)).LastWriteTimeUtc;
								}

								addDataFile(new DataFile(createDebFileRequestDataFileFile.TargetPath.TrimStart('/'), createDebFileRequestDataFileFile.IsExecutable, createDebFileRequestDataFileFile.DoNotRemove, modifiedDateTime, () =>
								{
									var dataStream = new System.IO.MemoryStream();

									dataStream.TextWrite(content, System.Text.Encoding.UTF8);

									dataStream.Flush();

									dataStream.Rewind();

									return dataStream;
								}));
							}
						}
						else if (requestDataFile is DTOs.CreateDebFileRequestEntryStream createDebFileRequestDataStream)
						{
							addDataFile(new DataFile(createDebFileRequestDataFileFile.TargetPath.TrimStart('/'), createDebFileRequestDataFileFile.IsExecutable, createDebFileRequestDataFileFile.DoNotRemove, DateTimeOffset.UtcNow, () => createDebFileRequestDataStream.SourceStream));
						}
						else if (requestDataFile is DTOs.CreateDebFileRequestEntryFile createDebFileRequestDataFile)
						{
							var fileInfo = new System.IO.FileInfo(createDebFileRequestDataFile.SourceFullName);

							addDataFile(new DataFile(createDebFileRequestDataFileFile.TargetPath.TrimStart('/'), createDebFileRequestDataFileFile.IsExecutable, createDebFileRequestDataFileFile.DoNotRemove, fileInfo.LastWriteTimeUtc, () => System.IO.File.OpenRead(createDebFileRequestDataFile.SourceFullName)));
						}
						else
						{
							throw new ArgumentOutOfRangeException(nameof(requestDataFile));
						}
						break;

					case DTOs.CreateDebFileRequestEntryFileWildCard createDebFileRequestDataFileWildCard:
						{
							var sourceDirectory = createDebFileRequestDataFileWildCard.SourceDirectory;

							var allDirectories = true;
							var wildcard = System.IO.Path.GetFileName(sourceDirectory);
							if (string.Equals(wildcard, "*", StringComparison.InvariantCultureIgnoreCase))
							{
								allDirectories = false;
								sourceDirectory = System.IO.Path.GetDirectoryName(sourceDirectory);
							}
							else if (string.Equals(wildcard, "**", StringComparison.InvariantCultureIgnoreCase))
							{
								sourceDirectory = System.IO.Path.GetDirectoryName(sourceDirectory);
							}

							var sourceFileNames = System.IO.Directory.GetFiles(sourceDirectory, "*", (allDirectories ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly));

							var targetPathDirectory = $"{createDebFileRequestDataFileWildCard.TargetPathDirectory.Trim('/')}/";

							foreach (var sourceFileName in sourceFileNames)
							{
								var relativePath = ISI.Extensions.IO.Path.GetRelativePath(sourceDirectory, sourceFileName).Replace("\\", "/");

								var dataFullName = sourceFileName;

								var fileInfo = new System.IO.FileInfo(sourceFileName);

								addDataFile(new DataFile($"{targetPathDirectory}{relativePath}", false, false, fileInfo.LastWriteTimeUtc, () => System.IO.File.OpenRead(dataFullName)));
							}
						}
						break;

					default:
						throw new ArgumentOutOfRangeException(nameof(requestDataFile));
				}
			}

			request.DebControl.InstalledSize ??= installedSize;

			using (var debSpecVersionStream = new System.IO.MemoryStream())
			{
				debSpecVersionStream.TextWrite(request.DebSpecVersion.ToString());
				debSpecVersionStream.Flush();
				debSpecVersionStream.Rewind();

				using (var dataStream = new ISI.Extensions.Stream.TempFileStream())
				{
					using (var tarSteamWriter = new ISI.Extensions.Linux.TarStreamWriter(dataStream))
					{
						foreach (var dataEntry in dataEntries.Values)
						{
							switch (dataEntry)
							{
								case DataDirectory dataDirectory:
									tarSteamWriter.WriteEntry(dataDirectory, null);
									break;
								case DataFile dataFile:
									addWriteEntryResponse(tarSteamWriter.WriteEntry(dataFile, () => dataFile.GetStream()));
									break;
								default:
									throw new ArgumentOutOfRangeException(nameof(dataEntry));
							}
						}
					}
					dataStream.Flush();
					dataStream.Rewind();

					using (var md5sumsStream = new System.IO.MemoryStream())
					{
						md5sumsStream.TextWrite(md5sums.ToString());
						md5sumsStream.Flush();
						md5sumsStream.Rewind();

						using (var controlStream = new ISI.Extensions.Stream.TempFileStream())
						{
							using (var gzStreamWriter = new System.IO.Compression.GZipStream(controlStream, System.IO.Compression.CompressionMode.Compress, true))
							{
								using (var tarSteamWriter = new ISI.Extensions.Linux.TarStreamWriter(gzStreamWriter))
								{
									using (var debSpecStream = new System.IO.MemoryStream())
									{
										debSpecStream.TextWrite(request.DebSpecVersion.ToString());
										debSpecStream.Flush();
										debSpecStream.Rewind();

										tarSteamWriter.WriteEntry(new ISI.Extensions.Linux.ArchiveEntry()
										{
											TargetPath = "/",
										}, null);

										tarSteamWriter.WriteEntry(new ISI.Extensions.Linux.ArchiveEntry()
										{
											TargetPath = "/control",
										}, () => debSpecStream, true);

										tarSteamWriter.WriteEntry(new ISI.Extensions.Linux.ArchiveEntry()
										{
											TargetPath = "/md5sums",
										}, () => md5sumsStream, true);

										if (!string.IsNullOrWhiteSpace(request.PreInstallScript))
										{
											tarSteamWriter.WriteEntry(new ISI.Extensions.Linux.ArchiveEntry()
											{
												TargetPath = "/preinst",
												LinuxFileMode = ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite | ISI.Extensions.IO.LinuxFileMode.UserCanExecute |
																				ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite | ISI.Extensions.IO.LinuxFileMode.GroupCanExecute |
																				ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite | ISI.Extensions.IO.LinuxFileMode.OthersCanExecute,
											}, () =>
											{
												var stream = new System.IO.MemoryStream();
												stream.TextWrite(request.PreInstallScript);
												stream.Flush();
												stream.Rewind();
												return stream;
											}, false);
										}

										if (!string.IsNullOrWhiteSpace(request.PostInstallScript))
										{
											tarSteamWriter.WriteEntry(new ISI.Extensions.Linux.ArchiveEntry()
											{
												TargetPath = "/postinst",
												LinuxFileMode = ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite | ISI.Extensions.IO.LinuxFileMode.UserCanExecute |
																				ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite | ISI.Extensions.IO.LinuxFileMode.GroupCanExecute |
																				ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite | ISI.Extensions.IO.LinuxFileMode.OthersCanExecute,
											}, () =>
											{
												var stream = new System.IO.MemoryStream();
												stream.TextWrite(request.PostInstallScript);
												stream.Flush();
												stream.Rewind();
												return stream;
											}, false);
										}

										if (!string.IsNullOrWhiteSpace(request.PreRemovalScript))
										{
											tarSteamWriter.WriteEntry(new ISI.Extensions.Linux.ArchiveEntry()
											{
												TargetPath = "/prerm",
												LinuxFileMode = ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite | ISI.Extensions.IO.LinuxFileMode.UserCanExecute |
																				ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite | ISI.Extensions.IO.LinuxFileMode.GroupCanExecute |
																				ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite | ISI.Extensions.IO.LinuxFileMode.OthersCanExecute,
											}, () =>
											{
												var stream = new System.IO.MemoryStream();
												stream.TextWrite(request.PreRemovalScript);
												stream.Flush();
												stream.Rewind();
												return stream;
											}, false);
										}

										if (!string.IsNullOrWhiteSpace(request.PostRemovalScript))
										{
											tarSteamWriter.WriteEntry(new ISI.Extensions.Linux.ArchiveEntry()
											{
												TargetPath = "/postrm",
												LinuxFileMode = ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite | ISI.Extensions.IO.LinuxFileMode.UserCanExecute |
																				ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite | ISI.Extensions.IO.LinuxFileMode.GroupCanExecute |
																				ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite | ISI.Extensions.IO.LinuxFileMode.OthersCanExecute,
											}, () =>
											{
												var stream = new System.IO.MemoryStream();
												stream.TextWrite(request.PostRemovalScript);
												stream.Flush();
												stream.Rewind();
												return stream;
											}, false);
										}

									}

								}
							}
							controlStream.Flush();
							controlStream.Rewind();

							var debStream = (System.IO.Stream)null;

							if (request is DTOs.CreateDebFileRequestWithDebFullName createDebFileRequestWithDebFullName)
							{
								debStream = System.IO.File.OpenWrite(createDebFileRequestWithDebFullName.DebFullName);
							}
							else if (request is DTOs.CreateDebFileRequestWithDebStream createDebFileRequestWithDebStream)
							{
								debStream = createDebFileRequestWithDebStream.DebStream;
							}

							using (var archiverStreamWriter = new ISI.Extensions.Linux.ArchiverStreamWriter(debStream))
							{
								archiverStreamWriter.WriteEntry("debian-binary", ISI.Extensions.IO.LinuxFileMode.File |
																																 ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite |
																																 ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite |
																																 ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite, debSpecVersionStream);
								archiverStreamWriter.WriteEntry("control.tar.gz", ISI.Extensions.IO.LinuxFileMode.File |
																																 ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite |
																																 ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite |
																																 ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite, controlStream);
								archiverStreamWriter.WriteEntry("data.tar.gz", ISI.Extensions.IO.LinuxFileMode.File |
																																 ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite |
																																 ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite |
																																 ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite, dataStream);
							}

							debStream.Flush();
							debStream.Rewind();

							if (request is DTOs.CreateDebFileRequestWithDebFullName)
							{
								debStream.Dispose();
								debStream = null;
							}
						}
					}
				}
			}



			return response;
		}

		public delegate System.IO.Stream GetStreamDelegate();

		public class DataFile : ISI.Extensions.Linux.ArchiveEntry
		{
			private readonly GetStreamDelegate _getStream;

			public DataFile(string targetPath, bool isExecutable, bool doNotRemove, DateTimeOffset modifiedDateTime, GetStreamDelegate getStream)
			{
				TargetPath = targetPath;
				Owner = "root";
				Group = "root";
				LinuxFileMode = ISI.Extensions.IO.LinuxFileMode.File | (isExecutable ? ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite | ISI.Extensions.IO.LinuxFileMode.UserCanExecute |
																																							 ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite | ISI.Extensions.IO.LinuxFileMode.GroupCanExecute |
																																							 ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite | ISI.Extensions.IO.LinuxFileMode.OthersCanExecute :
																																							 ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite |
																																							 ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite |
																																							 ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite);
				DoNotRemove = doNotRemove;
				ModifiedDateTime = modifiedDateTime;
				_getStream = getStream;
			}

			public System.IO.Stream GetStream() => _getStream();
		}

		public class DataDirectory : ISI.Extensions.Linux.ArchiveEntry
		{
			public DataDirectory(string targetPath)
			{
				TargetPath = targetPath;
				Owner = "root";
				Group = "root";
				LinuxFileMode = ISI.Extensions.IO.LinuxFileMode.Directory |
												ISI.Extensions.IO.LinuxFileMode.UserCanRead | ISI.Extensions.IO.LinuxFileMode.UserCanWrite |
												ISI.Extensions.IO.LinuxFileMode.GroupCanRead | ISI.Extensions.IO.LinuxFileMode.GroupCanWrite |
												ISI.Extensions.IO.LinuxFileMode.OthersCanRead | ISI.Extensions.IO.LinuxFileMode.OthersCanWrite;
			}
		}
	}
}