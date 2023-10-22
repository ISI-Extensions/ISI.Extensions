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
using DTOs = ISI.Extensions.Svn.DataTransferObjects.SvnApi;
using SourceControlClientApiDTOs = ISI.Extensions.Scm.DataTransferObjects.SourceControlClientApi;

namespace ISI.Extensions.Svn
{
	public partial class SvnApi
	{
		public DTOs.GetRevisionInfoResponse GetRevisionInfo(DTOs.GetRevisionInfoRequest request)
		{
			var response = new DTOs.GetRevisionInfoResponse();

			if (SvnIsInstalled)
			{
				string getProperty(string propertyName)
				{
					var arguments = new List<string>();

					arguments.Add(propertyName);
					arguments.Add(string.Format("-r {0}", request.Revision));
					arguments.Add(string.Format("\"{0}\"", request.RepositoryPath));

					return ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
					{
						ProcessExeFullName = "svnlook",
						Arguments = arguments.ToArray(),
					}).Output;
				}

				var pathsChanged = new List<(SvnCommitAction CommitAction, string Path, long? SourceRevision, string SourcePath)>();

				var historyLines = getProperty("diff").Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				var lastHistoryLine = (string)null;
				foreach (var historyLine in historyLines)
				{
					if (historyLine.StartsWith("===================================================================", StringComparison.InvariantCultureIgnoreCase) && !string.IsNullOrWhiteSpace(lastHistoryLine))
					{
						var pieces = lastHistoryLine.Split(new[] { ':' }, 2);
						if (pieces.Length > 1)
						{
							if (ISI.Extensions.Enum<SvnCommitAction>.TryParse(pieces[0], out var svnCommitAction))
							{
								var path = pieces[1].Trim();
								var sourceRevision = (long?)null;
								var sourcePath = (string)null;

								if (svnCommitAction == SvnCommitAction.Copied)
								{
									var sourceIndex = path.IndexOf("(from rev", StringComparison.InvariantCultureIgnoreCase);
									if (sourceIndex > 0)
									{
										sourcePath = path.Substring(sourceIndex).TrimStart("(from rev", StringComparison.InvariantCultureIgnoreCase).Trim();
										path = path.Substring(0, sourceIndex);

										pieces = sourcePath.Split(new[] { ',' }, 2);
										sourceRevision = pieces[0].ToLong();
										sourcePath = pieces[1].Trim().TrimEnd(')', ' ');
									}
								}

								pathsChanged.Add((CommitAction: svnCommitAction, Path: path, SourceRevision: sourceRevision, SourcePath: sourcePath));
							}
						}
					}

					lastHistoryLine = historyLine;
				}

				response.RevisionInfo = new RevisionInfo()
				{
					RevisionDateTime = getProperty("date").Trim('\n', '\r', ' ').Split(new[] { '(' }).First().Trim().ToDateTime(),
					Author = getProperty("author").Trim('\n', '\r', ' '),
					Log = getProperty("log").Trim('\n', '\r', ' '),
					DirectoriesChanged = getProperty("dirs-changed").Trim('\n', '\r', ' ').Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries),
					PathsChanged = pathsChanged.ToArray(),
				};
			}

			return response;
		}
	}
}