#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		public DTOs.GetProjectDetailsResponse GetProjectDetails(DTOs.GetProjectDetailsRequest request)
		{
			var response = new DTOs.GetProjectDetailsResponse();

			response.ProjectDetails = new();

			if (System.IO.Directory.Exists(request.Project))
			{
				var possibleProjectFullNames = System.IO.Directory.GetFiles(request.Project, "*.csproj", System.IO.SearchOption.AllDirectories);

				if (possibleProjectFullNames.Length == 1)
				{
					response.ProjectDetails.ProjectFullName = possibleProjectFullNames.First();
				}
				else if (possibleProjectFullNames.Length > 1)
				{
					var possibleProjectName = System.IO.Path.GetFileName(request.Project);

					var possibleProjectFullName = possibleProjectFullNames.FirstOrDefault(possibleProjectFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleProjectFullName), possibleProjectName, StringComparison.InvariantCultureIgnoreCase));

					if (!string.IsNullOrWhiteSpace(possibleProjectFullName))
					{
						response.ProjectDetails.ProjectName = possibleProjectFullName;
					}
					else
					{
						throw new(string.Format("Cannot determine which project to update \"{0}\"", request.Project));
					}
				}
				else
				{
					throw new(string.Format("Cannot find a project to update \"{0}\"", request.Project));
				}
			}

			if (System.IO.File.Exists(request.Project))
			{
				response.ProjectDetails.ProjectFullName = request.Project;
			}

			if (string.IsNullOrWhiteSpace(response.ProjectDetails.ProjectFullName))
			{
				return null;
			}

			response.ProjectDetails.ProjectName = System.IO.Path.GetFileNameWithoutExtension(response.ProjectDetails.ProjectFullName);
			response.ProjectDetails.ProjectDirectory = System.IO.Path.GetDirectoryName(response.ProjectDetails.ProjectFullName);

			return response;
		}
	}
}