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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.Backup
{
	public partial class BackupAgent
	{
		public void PushBackupsToRemotes(Microsoft.Extensions.Logging.ILogger logger, string exportFullName, string[] remoteBackupFullNames, Func<DateTime?> getDateTimeStamp, IEnumerable<ISI.Extensions.IO.FileNameMask> additionalMasks, string backupFileNamesFullName)
		{
			var backupFullNames = new List<string>();
			backupFullNames.Add(exportFullName);

			if (remoteBackupFullNames.NullCheckedAny())
			{
				var backupsExtensions = new HashSet<string>(remoteBackupFullNames.ToNullCheckedArray(System.IO.Path.GetExtension), StringComparer.InvariantCultureIgnoreCase);
				var compressedBackups = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

				foreach (var backupsExtension in backupsExtensions)
				{
					if (string.Equals(backupsExtension, ".gz", StringComparison.InvariantCultureIgnoreCase))
					{
						if (exportFullName.EndsWith(".gz", StringComparison.InvariantCultureIgnoreCase))
						{
							compressedBackups.Add(backupsExtension, exportFullName);
						}
						else
						{
							var compressedFullName = $"{exportFullName}.gz";

							logger.LogInformation($"Compressing backup to {compressedFullName}");

							using (var uncompressedFileStream = System.IO.File.OpenRead(exportFullName))
							{
								using (var compressedFileStream = System.IO.File.Create(compressedFullName))
								{
									using (var compressionStream = new System.IO.Compression.GZipStream(compressedFileStream, System.IO.Compression.CompressionMode.Compress))
									{
										uncompressedFileStream.CopyTo(compressionStream);
									}
								}
							}

							compressedBackups.Add(backupsExtension, compressedFullName);
						}
					}
					else if (string.Equals(backupsExtension, ".lz", StringComparison.InvariantCultureIgnoreCase))
					{
						if (exportFullName.EndsWith(".lz", StringComparison.InvariantCultureIgnoreCase))
						{
							compressedBackups.Add(backupsExtension, exportFullName);
						}
						else
						{
							var compressedFullName = $"{exportFullName}.lz";

							logger.LogInformation($"Compressing backup to {compressedFullName}");

							using (var uncompressedFileStream = System.IO.File.OpenRead(exportFullName))
							{
								using (var compressedFileStream = System.IO.File.Create(compressedFullName))
								{
									var tarWriterOptions = new SharpCompress.Writers.Tar.TarWriterOptions(SharpCompress.Common.CompressionType.LZip, true);
									//{
									//	ArchiveEncoding = new SharpCompress.Common.ArchiveEncoding()
									//	{
									//		Default = Encoding.UTF8,
									//	},
									//};

									using (var tarWriter = new SharpCompress.Writers.Tar.TarWriter(compressedFileStream, tarWriterOptions))
									{
										tarWriter.Write(System.IO.Path.GetFileName(exportFullName), uncompressedFileStream, null);
									}
								}
							}

							compressedBackups.Add(backupsExtension, compressedFullName);
						}
					}
				}

				backupFullNames.AddRange(compressedBackups.Values);

				foreach (var remoteBackupFullName in remoteBackupFullNames.NullCheckedSelect(remoteBackupFullName => ISI.Extensions.IO.Path.GetFileNameDeMasked(remoteBackupFullName, getDateTimeStamp, additionalMasks), NullCheckCollectionResult.Empty))
				{
					var backupFullName = exportFullName;

					var extension = System.IO.Path.GetExtension(remoteBackupFullName);

					if (compressedBackups.TryGetValue(extension, out var compressedFullName))
					{
						backupFullName = compressedFullName;
					}

					using (var sourceStream = ISI.Extensions.FileSystem.OpenRead(backupFullName))
					{
						using (var remoteStream = ISI.Extensions.FileSystem.OpenWrite(remoteBackupFullName, true, true))
						{
							sourceStream.CopyTo(remoteStream);
							remoteStream.Flush();
						}

						logger.LogInformation($"Copied backup to {ISI.Extensions.FileSystem.GetObfuscatedAttributedFullPath(remoteBackupFullName)}");
					}
				}
			}

			System.IO.File.WriteAllText(backupFileNamesFullName, string.Join(Environment.NewLine, backupFullNames.Select(System.IO.Path.GetFileName)));
		}
	}
}