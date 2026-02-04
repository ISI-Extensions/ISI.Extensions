using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.BackupAgent
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
					else if (string.Equals(backupsExtension, ".lz", StringComparison.InvariantCultureIgnoreCase))
					{
						var compressedFullName = $"{exportFullName}.lz";

						logger.LogInformation($"Compressing backup to {compressedFullName}");

						using (var uncompressedFileStream = System.IO.File.OpenRead(exportFullName))
						{
							using (var compressedFileStream = System.IO.File.Create(compressedFullName))
							{
								var tarWriterOptions = new SharpCompress.Writers.Tar.TarWriterOptions(SharpCompress.Common.CompressionType.LZip, true)
								{
									ArchiveEncoding = new()
									{
										Default = Encoding.UTF8,
									},
								};

								using (var tarWriter = new SharpCompress.Writers.Tar.TarWriter(compressedFileStream, tarWriterOptions))
								{
									tarWriter.Write(System.IO.Path.GetFileName(exportFullName), uncompressedFileStream, null);
								}
							}
						}

						compressedBackups.Add(backupsExtension, compressedFullName);
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