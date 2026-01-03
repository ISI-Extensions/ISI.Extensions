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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		public DTOs.OpenSolutionResponse OpenSolution(DTOs.OpenSolutionRequest request)
		{
			var response = new DTOs.OpenSolutionResponse();

			var solutionFileName = string.Empty;

			if (!string.IsNullOrWhiteSpace(request.SolutionFilter))
			{
				if (request.SolutionFilter.EndsWith(".slnf", StringComparison.InvariantCultureIgnoreCase))
				{
					solutionFileName = request.SolutionFilter;
				}
				else
				{
					var solutionSourceDirectory = GetSolutionDetails(new()
					{
						Solution = request.Solution,
					}).SolutionDetails.SolutionDirectory;

					if (System.IO.Directory.Exists(solutionSourceDirectory))
					{
						if (!System.IO.Directory.GetFiles(solutionSourceDirectory, "*.slnf").Any())
						{
							solutionSourceDirectory = System.IO.Path.Combine(solutionSourceDirectory, "src");
						}

						if (System.IO.Directory.Exists(solutionSourceDirectory))
						{
							solutionFileName = System.IO.Directory.GetFiles(solutionSourceDirectory, "*.slnf").NullCheckedFirstOrDefault(solutionFilterFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(solutionFilterFullName), request.SolutionFilter, StringComparison.InvariantCultureIgnoreCase));
						}
					}
				}
			}
			else if (request.Solution.EndsWith(ISI.Extensions.VisualStudio.Solution.SolutionExtension, StringComparison.InvariantCultureIgnoreCase))
			{
				solutionFileName = request.Solution;
			}
			else
			{
				var solutionSourceDirectory = GetSolutionDetails(new()
				{
					Solution = request.Solution,
				}).SolutionDetails.SolutionDirectory;

				if (System.IO.Directory.Exists(solutionSourceDirectory))
				{
					if (!ISI.Extensions.VisualStudio.Solution.FindSolutionFullNames(solutionSourceDirectory).NullCheckedAny())
					{
						solutionSourceDirectory = System.IO.Path.Combine(solutionSourceDirectory, "src");
					}

					if (System.IO.Directory.Exists(solutionSourceDirectory))
					{
						solutionFileName = ISI.Extensions.VisualStudio.Solution.FindSolutionFullNames(solutionSourceDirectory).FirstOrDefault();
					}
				}
			}

			if (!string.IsNullOrEmpty(solutionFileName))
			{
				ISI.Extensions.IO.Path.FileOpen(solutionFileName, ISI.Extensions.IO.FileOpenAction.Open);
			}

			return response;
		}
	}
}