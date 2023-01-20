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

namespace ISI.Extensions.Svn
{
	public partial class SvnApi
	{
		public DTOs.GetWorkingCopyInfosResponse GetWorkingCopyInfos(DTOs.GetWorkingCopyInfosRequest request)
		{
			var response = new DTOs.GetWorkingCopyInfosResponse();

			var arguments = new List<string>();

			arguments.Add("info");
			arguments.Add(string.Format("\"{0}\"", request.Source.TrimEnd(System.IO.Path.DirectorySeparatorChar)));

			switch (request.Depth)
			{
				case Depth.Empty:
					arguments.Add("--depth empty");
					break;
				case Depth.Files:
					arguments.Add("--depth files");
					break;
				case Depth.Children:
					arguments.Add("--depth immediates");
					break;
				case Depth.Infinity:
					arguments.Add("--depth infinity");
					break;
			}

			AddCredentials(arguments, request);

			var content = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = new NullLogger(),
				ProcessExeFullName = "svn",
				Arguments = arguments.ToArray(),
			}).Output;

			var contentItems = content.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

			var infos = new List<DTOs.WorkingCopyInfo>();

			DTOs.WorkingCopyInfo info = null;

			foreach (var contentItem in contentItems)
			{
				if (!string.IsNullOrWhiteSpace(contentItem))
				{
					var property = contentItem.Split(new string[] { ":" }, 2, StringSplitOptions.RemoveEmptyEntries);

					if (property.Length >= 2)
					{
						var propertyKey = property[0];
						var propertyValue = property[1];

						switch (propertyKey)
						{
							case "Path":
								info = new();
								infos.Add(info);
								info.Path = propertyValue.Trim();
								break;

							case "Working Copy Root Path":
								info.WorkingCopyRootPath = propertyValue.Trim();
								break;

							case "URL":
								info.Uri = new(propertyValue.Trim());
								break;

							case "Repository Root":
								info.RepositoryRoot = new(propertyValue.Trim());
								break;

							case "Revision":
								info.Revision = propertyValue.Trim().ToLong();
								break;

							case "Node Kind":
								info.NodeKind = ISI.Extensions.Enum<NodeKind>.Parse(propertyValue.Trim());
								break;

							case "Schedule":
								info.Schedule = ISI.Extensions.Enum<Schedule>.Parse(propertyValue.Trim());
								break;

							case "Last Changed Author":
								info.LastChangeAuthor = propertyValue.Trim();
								break;

							case "Last Changed Rev":
								info.LastChangeRevision = propertyValue.Trim().ToLong();
								break;

							case "Last Changed Date":
								info.LastChangeTime = propertyValue.Trim().ToDateTime();
								break;
						}
					}
				}
			}

			response.Infos = infos;

			return response;
		}
	}
}