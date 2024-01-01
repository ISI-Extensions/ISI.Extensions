#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Git.DataTransferObjects.BackupHelper;

namespace ISI.Extensions.Git
{
	public partial class BackupHelper
	{
		public virtual DTOs.DumpRepositoryResponse DumpRepository(DTOs.DumpRepositoryRequest request)
		{
			var response = new DTOs.DumpRepositoryResponse();

			request.StatusTracker ??= new ISI.Extensions.StatusTracker();
			request.ExecutedDateTimeUtc ??= DateTimeStamper.CurrentDateTime();

			var processStartInfo = new System.Diagnostics.ProcessStartInfo();

			processStartInfo.CreateNoWindow = true;
			//processStartInfo.FileName = "git";
			//processStartInfo.Arguments = string.Format("bundle create \"{0}\" --all", request.DumpFullName);
			//7z a -r E:\gogs-backups\isi.extensions.cake.git.zip E:\gogs-repositories\isi\isi.extensions.cake.git\*
			processStartInfo.FileName = "7z";
			processStartInfo.Arguments = string.Format("a -r \"{0}\" .\\*", request.DumpFullName);
			processStartInfo.WorkingDirectory = System.IO.Path.Combine(RepositoriesPath, request.RepositoryKey.Replace("+", "\\"));

			processStartInfo.RedirectStandardOutput = true;
			processStartInfo.RedirectStandardError = true;
			processStartInfo.UseShellExecute = false;
			processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

			//request.statusTracker.AddToLog(string.Format("    Working Directory: \"{0}\"", processStartInfo.WorkingDirectory));
			//request.statusTracker.AddToLog(string.Format("    Command: {0} {1}", processStartInfo.FileName, processStartInfo.Arguments));

			using (var process = System.Diagnostics.Process.Start(processStartInfo))
			{
				process.OutputDataReceived += (sender, args) =>
				{
					if (!string.IsNullOrWhiteSpace(args.Data))
					{
						request.StatusTracker.AddToLog(args.Data);
					}
				};

				process.BeginOutputReadLine();

				process.ErrorDataReceived += (sender, args) =>
				{
					if (!string.IsNullOrWhiteSpace(args.Data))
					{
						request.StatusTracker.AddToLog(string.Format("Error: {0}", args.Data));
					}
				};

				process.BeginErrorReadLine();

				process.WaitForExit();
			}

			return response;
		}
	}
}