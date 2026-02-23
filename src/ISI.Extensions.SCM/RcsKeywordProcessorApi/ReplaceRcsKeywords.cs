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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.RcsKeywordProcessorApi;

namespace ISI.Extensions.Scm
{
	public partial class RcsKeywordProcessorApi
	{
		public delegate ISI.Extensions.Scm.WorkingCopyCommitInformation GetWorkingCopyCommitInformationDelegate();
		public delegate string UpdateContentDelegate(string content, GetWorkingCopyCommitInformationDelegate getWorkingCopyCommitInformation);

		public DTOs.ReplaceRcsKeywordsResponse ReplaceRcsKeywords(DTOs.ReplaceRcsKeywordsRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.ReplaceRcsKeywordsResponse();

			var keywords = new Dictionary<string, UpdateContentDelegate>();
			keywords.Add("$Author$", (content, getWorkingCopyCommitInformation) =>
			{
				var workingCopyCommitInformation = getWorkingCopyCommitInformation();

				return content.Replace("$Author$", $"$Author: {workingCopyCommitInformation.Author} Exp $");
			});
			keywords.Add("$Date$", (content, getWorkingCopyCommitInformation) =>
			{
				var workingCopyCommitInformation = getWorkingCopyCommitInformation();

				return content.Replace("$Date$", $"$Date: {workingCopyCommitInformation.CommitDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTime)} Exp $");
			});
			keywords.Add("$Header$", (content, getWorkingCopyCommitInformation) =>
			{
				var workingCopyCommitInformation = getWorkingCopyCommitInformation();

				var version = request.DateTimeStampVersion?.Version?.ToString();

				return content.Replace("$Header$", $"$Header: {workingCopyCommitInformation.Path.Replace("\\", "/")} {workingCopyCommitInformation.CommitDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTime)} {version} {workingCopyCommitInformation.Author} Exp $");
			});
			keywords.Add("$Id$", (content, getWorkingCopyCommitInformation) =>
			{
				var workingCopyCommitInformation = getWorkingCopyCommitInformation();

				var version = request.DateTimeStampVersion?.Version?.ToString();

				return content.Replace("$Id$", $"$Id: {System.IO.Path.GetFileName(workingCopyCommitInformation.Path)} {workingCopyCommitInformation.CommitDateTimeUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTime)} {version} {workingCopyCommitInformation.Author} Exp $");
			});
			keywords.Add("$Log$", (content, getWorkingCopyCommitInformation) =>
			{
				var workingCopyCommitInformation = getWorkingCopyCommitInformation();

				return content.Replace("$Log$", $"$Log: {workingCopyCommitInformation.Message} Exp $");
			});
			keywords.Add("$Revision$", (content, getWorkingCopyCommitInformation) =>
			{
				var workingCopyCommitInformation = getWorkingCopyCommitInformation();

				return content.Replace("$Revision$", $"$Revision: {workingCopyCommitInformation.CommitKey} Exp $");
			});
			keywords.Add("$Version$", (content, getWorkingCopyCommitInformation) => content.Replace("$Version$", $"$Version: {request.DateTimeStampVersion?.Version} Exp $"));


			var modifiedFiles = new List<DTOs.ReplaceRcsKeywordsFile>();

			var fullNames = System.IO.Directory.GetFiles(request.SourceDirectory, request.FileNameExtension, (request.IncludeChildDirectories ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly));

			foreach (var fullName in fullNames)
			{
				var isDirty = false;
				var content = System.IO.File.ReadAllText(fullName);

				var modifiedFile = new DTOs.ReplaceRcsKeywordsFile()
				{
					FullName = fullName,
					OriginalContent = content,
				};

				var workingCopyCommitInformation = (ISI.Extensions.Scm.WorkingCopyCommitInformation)null;
				ISI.Extensions.Scm.WorkingCopyCommitInformation getWorkingCopyCommitInformation()
				{
					workingCopyCommitInformation ??= SourceControlClientApi.GetWorkingCopyCommitInformation(new()
					{
						FullName = modifiedFile.FullName,
					}).WorkingCopyCommitInformation;

					return workingCopyCommitInformation;
				}

				foreach (var keyword in keywords)
				{
					if (content.IndexOf(keyword.Key, StringComparison.InvariantCultureIgnoreCase) >= 0)
					{
						if (!isDirty)
						{
							logger.LogInformation($"Processing {System.IO.Path.GetFileName(modifiedFile.FullName)}");
						}

						content = keyword.Value(content, getWorkingCopyCommitInformation);
						isDirty = true;
					}
				}

				if (isDirty)
				{
					modifiedFiles.Add(modifiedFile);
					System.IO.File.Delete(modifiedFile.FullName);
					System.IO.File.WriteAllText(modifiedFile.FullName, content);
				}
			}

			response.ModifiedFiles = modifiedFiles.ToArray();
			
			return response;
		}
	}
}