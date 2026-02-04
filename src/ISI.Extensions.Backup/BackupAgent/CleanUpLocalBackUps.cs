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
		public void CleanUpLocalBackUps(Microsoft.Extensions.Logging.ILogger logger, string backupRepositoryDirectory, string backupFileNamesPrefix, string backupFileNamesSuffix, DateTime fileNameDateTimeUtc, TimeSpan backupDirectoryRetention)
		{
			foreach (var backupFileNamesFullName in System.IO.Directory.GetFiles(backupRepositoryDirectory, $"{backupFileNamesPrefix}.*.{backupFileNamesSuffix}"))
			{
				var backupFileNamesDateTime = System.IO.Path.GetFileName(backupFileNamesFullName).TrimStart($"{backupFileNamesPrefix}.").TrimEnd(backupFileNamesSuffix).ToDateTimeUtcNullable();

				if (backupFileNamesDateTime.HasValue && (backupFileNamesDateTime <= fileNameDateTimeUtc - backupDirectoryRetention))
				{
					var backupFileNames = System.IO.File.ReadAllLines(backupFileNamesFullName);

					foreach (var backupFileName in backupFileNames)
					{
						var backupFullName = System.IO.Path.Combine(backupRepositoryDirectory, backupFileName);

						if (System.IO.File.Exists(backupFullName))
						{
							System.IO.File.Delete(backupFullName);
							logger.LogInformation($"Deleted backup {backupFullName}");
						}
					}

					System.IO.File.Delete(backupFileNamesFullName);
				}
			}
		}
	}
}