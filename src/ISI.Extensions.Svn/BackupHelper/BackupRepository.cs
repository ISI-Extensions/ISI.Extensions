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
using DTOs = ISI.Extensions.Svn.DataTransferObjects.BackupHelper;

namespace ISI.Extensions.Svn
{
	public partial class BackupHelper
	{
		public virtual DTOs.BackupRepositoryResponse BackupRepository(DTOs.BackupRepositoryRequest request)
		{
			var response = new DTOs.BackupRepositoryResponse();

			request.StatusTracker ??= new ISI.Extensions.StatusTracker();
			request.ExecutedDateTimeUtc ??= DateTimeStamper.CurrentDateTime();

			var doBackup = !request.ModifiedRepositoriesOnly;

			var currentRevisionKey = GetRevisionKey(new()
			{
				StatusTracker = request.StatusTracker,
				RepositoryKey = request.RepositoryKey,
			}).RevisionKey;

			var lastBackupRevisionKeyFileName = System.IO.Path.Combine(RepositoriesPath, request.RepositoryKey, "lastBackupRevisionKey.txt");

			if (request.ModifiedRepositoriesOnly)
			{
				var lastBackupRevisionKey = string.Empty;

				if (System.IO.File.Exists(lastBackupRevisionKeyFileName))
				{
					lastBackupRevisionKey = System.IO.File.ReadAllText(lastBackupRevisionKeyFileName);

					doBackup = !string.Equals(lastBackupRevisionKey, currentRevisionKey);
				}
				else
				{
					doBackup = true;
				}

				if (doBackup)
				{
					request.StatusTracker.AddToLog(string.Format("  Backing up Svn: {0}", request.RepositoryKey));

					if (string.IsNullOrWhiteSpace(request.BackupDirectoryFullName))
					{
						request.BackupDirectoryFullName = System.IO.Path.GetTempFileName();

						System.IO.File.Delete(request.BackupDirectoryFullName);
					}

					System.IO.Directory.CreateDirectory(request.BackupDirectoryFullName);

					var dumpFullName = System.IO.Path.Combine(request.BackupDirectoryFullName, string.Format("{0}.svndump", request.RepositoryKey));

					{
						var dumpDirectory = System.IO.Path.GetDirectoryName(dumpFullName);

						System.IO.Directory.CreateDirectory(dumpDirectory);
					}

					request.StatusTracker.AddToLog("    Taking Dump");

					DumpRepository(new DTOs.DumpRepositoryFileNameRequest()
					{
						StatusTracker = request.StatusTracker,
						RepositoryKey = request.RepositoryKey,
						DumpFullName = dumpFullName,
					});

					response.BackupDirectoryFullName = request.BackupDirectoryFullName;
					response.RepositoryKey = request.RepositoryKey;
					response.BackupFullName = dumpFullName;
				}

				if (request.ModifiedRepositoriesOnly && doBackup)
				{
					System.IO.File.WriteAllText(lastBackupRevisionKeyFileName, currentRevisionKey);
				}

				System.IO.File.WriteAllText(System.IO.Path.Combine(RepositoriesPath, request.RepositoryKey, "lastBackupCheck.txt"), request.ExecutedDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise));
			}

			return response;
		}
	}
}