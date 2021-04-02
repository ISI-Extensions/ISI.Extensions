#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
		public DTOs.GetInfoResponse GetInfo(DTOs.GetInfoRequest request)
		{
			var response = new DTOs.GetInfoResponse();

			var arguments = new List<string>();

			arguments.Append("info");
			arguments.Append(string.Format("\"{0}\"", request.Source.TrimEnd(System.IO.Path.DirectorySeparatorChar)));

			var content = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = Logger,
				ProcessExeFullName = "svn",
				Arguments = arguments.ToArray(),
			}).Output;

			var contentItems = content.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

			var properties = new Dictionary<string, string>();
			foreach (var property in contentItems.Select(contentItem => contentItem.Split(new string[] { ":" }, 2, StringSplitOptions.RemoveEmptyEntries)))
			{
				if ((property.Length >= 2) && !properties.ContainsKey(property[0]))
				{
					properties.Add(property[0], property[1]);
				}
			}

			if (properties.Any())
			{
				response.IsUnderSvn = properties.Count > 1;

				if (properties.TryGetValue("Path", out var path))
				{
					response.Path = path.Trim();
				}
				if (properties.TryGetValue("Working Copy Root Path", out var workingCopyRootPath))
				{
					response.WorkingCopyRootPath = workingCopyRootPath.Trim();
				}
				if (properties.TryGetValue("URL", out var url))
				{
					response.Uri = new Uri(url);
				}
				if (properties.TryGetValue("Repository Root", out var repositoryRoot))
				{
					response.RepositoryRoot = new Uri(repositoryRoot.Trim());
				}
				if (properties.TryGetValue("Revision", out var revision))
				{
					response.Revision = revision.Trim().ToLong();
				}
				if (properties.TryGetValue("Node Kind", out var nodeKind))
				{
					response.NodeKind = ISI.Extensions.Enum<NodeKind>.Parse(nodeKind.Trim());
				}
				if (properties.TryGetValue("Schedule", out var schedule))
				{
					response.Schedule = ISI.Extensions.Enum<Schedule>.Parse(schedule.Trim());
				}
				if (properties.TryGetValue("Last Changed Author", out var lastChangeAuthor))
				{
					response.LastChangeAuthor = lastChangeAuthor.Trim();
				}
				if (properties.TryGetValue("Last Changed Rev", out var lastChangeRevision))
				{
					response.LastChangeRevision = lastChangeRevision.ToLong();
				}
				if (properties.TryGetValue("Last Changed Date", out var lastChangeTime))
				{
					response.LastChangeTime = lastChangeTime.Trim().ToDateTime();
				}
			}

			return response;
		}
	}
}