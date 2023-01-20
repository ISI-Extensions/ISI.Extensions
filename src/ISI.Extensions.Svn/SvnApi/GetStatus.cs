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
		public DTOs.GetStatusResponse GetStatus(DTOs.GetStatusRequest request)
		{
			var response = new DTOs.GetStatusResponse();

			var statuses = new List<(string Path, Status LocalContentStatus)>();

			var arguments = new List<string>();

			arguments.Add("status");
			arguments.Add("-u");
			arguments.Add(string.Format("\"{0}\"", request.Source.TrimEnd(System.IO.Path.DirectorySeparatorChar)));
			AddCredentials(arguments, request);

			var content = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = new NullLogger(),
				ProcessExeFullName = "svn",
				Arguments = arguments.ToArray(),
			}).Output;

			var contentItems = content.Split(new string[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);

			foreach (var contentItem in contentItems)
			{
				if (contentItem[9] == ' ')
				{
					var itemInfo = contentItem.Substring(7).Trim().Split(new string[] {" "}, 2, StringSplitOptions.RemoveEmptyEntries);

					var path = (itemInfo.Length > 1 ? itemInfo[1] : contentItem);

					switch (contentItem[0])
					{
						case 'A':
							statuses.Add((Path: path, LocalContentStatus: Status.Added));
							break;
						case 'C':
							statuses.Add((Path: path, LocalContentStatus: Status.Conflicted));
							break;
						case 'D':
							statuses.Add((Path: path, LocalContentStatus: Status.Deleted));
							break;
						case 'I':
							statuses.Add((Path: path, LocalContentStatus: Status.Ignored));
							break;
						case 'M':
							statuses.Add((Path: path, LocalContentStatus: Status.Modified));
							break;
						case 'R':
							statuses.Add((Path: path, LocalContentStatus: Status.Replaced));
							break;
						case 'X':
							statuses.Add((Path: path, LocalContentStatus: Status.External));
							break;
						case '?':
							statuses.Add((Path: path, LocalContentStatus: Status.NotVersioned));
							break;
						case '!':
							statuses.Add((Path: path, LocalContentStatus: Status.Missing));
							break;
						case '~':
							statuses.Add((Path: path, LocalContentStatus: Status.Obstructed));
							break;
					}
				}
			}

			response.Statuses = statuses;

			return response;
		}
	}
}