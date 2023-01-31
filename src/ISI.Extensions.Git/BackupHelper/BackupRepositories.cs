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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.StatusTrackers.Extensions;
using DTOs = ISI.Extensions.Git.DataTransferObjects.BackupHelper;

namespace ISI.Extensions.Git
{
	public partial class BackupHelper
	{
		public virtual DTOs.BackupRepositoriesResponse BackupRepositories(DTOs.BackupRepositoriesRequest request)
		{
			var response = new DTOs.BackupRepositoriesResponse();
			
			request.StatusTracker ??= new ISI.Extensions.StatusTracker();
			request.ExecutedDateTimeUtc ??= DateTimeStamper.CurrentDateTime();

			var repositoryKeys = new List<string>();

			foreach (var repositoryOwnerDirectoryFullName in System.IO.Directory.EnumerateDirectories(RepositoriesPath))
			{
				foreach (var repositoryDirectoryFullName in System.IO.Directory.EnumerateDirectories(repositoryOwnerDirectoryFullName))
				{
					repositoryKeys.Add(string.Format("{0}+{1}", repositoryOwnerDirectoryFullName, System.IO.Path.GetFileName(repositoryDirectoryFullName)));
				}
			}

			if (string.IsNullOrWhiteSpace(request.BackupDirectoryFullName))
			{
				request.BackupDirectoryFullName = System.IO.Path.GetTempFileName();

				System.IO.File.Delete(request.BackupDirectoryFullName);
			}

			System.IO.Directory.CreateDirectory(request.BackupDirectoryFullName);

			response.BackupDirectoryFullName = request.BackupDirectoryFullName;

			var backups = new List<(string RepositoryKey, string BackupFullName)>();

			var index = 0;
			foreach (var repositoryKey in repositoryKeys)
			{
				request.StatusTracker.SetCaptionPercent(string.Format("Backing up {0}", repositoryKey), 5, 90, index++, repositoryKeys.Count);

				var backupRepositoryResponse = BackupRepository(new()
				{
					StatusTracker = request.StatusTracker,
					ExecutedDateTimeUtc = request.ExecutedDateTimeUtc,
					RepositoryKey = repositoryKey,
					BackupDirectoryFullName = request.BackupDirectoryFullName,
					ModifiedRepositoriesOnly = request.ModifiedRepositoriesOnly,
				});

				if (!string.IsNullOrWhiteSpace(backupRepositoryResponse.BackupFullName))
				{
					backups.Add((RepositoryKey: repositoryKey, BackupFullName: backupRepositoryResponse.BackupFullName));

					request.StatusTracker.AddToLog(string.Format("Backed up Git: {0} to \"{1}\"", repositoryKey, backupRepositoryResponse.BackupFullName));
				}
				else
				{
					request.StatusTracker.AddToLog(string.Format("DID NOT Backup Git: {0}", repositoryKey));
				}
			}

			response.Backups = backups.ToArray();

			return response;
		}
	}
}