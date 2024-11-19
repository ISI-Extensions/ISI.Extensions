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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.SolutionApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class SolutionApi
	{
		private string[] ProjectFileNamesFromSolutionFullName(string solutionFullName, bool returnProjectFullNames)
		{
			var solutionLines = System.IO.File.ReadAllLines(solutionFullName);

			if (returnProjectFullNames)
			{
				return ProjectFileNamesFromSolutionContentLines(solutionLines, System.IO.Path.GetDirectoryName(solutionFullName));
			}

			return ProjectFileNamesFromSolutionContentLines(solutionLines);
		}

		private string[] ProjectFileNamesFromSolutionContentLines(IEnumerable<string> solutionLines)
		{
			return solutionLines
				.Select(solutionLine =>
				{
					if (solutionLine.Trim().StartsWith("Project(", StringComparison.InvariantCultureIgnoreCase))
					{
						var pieces = solutionLine.Split(new[] { '=' }).ToList();

						pieces = pieces[1].Split(new[] { '"' }).Select(piece => piece.Trim()).ToList();

						pieces.RemoveAll(piece => string.Equals(piece, ","));
						pieces.RemoveAll(string.IsNullOrWhiteSpace);

						return pieces[1];

						//return solutionLine.Split(new[] { ',' }, StringSplitOptions.None)[1].Trim(' ', '\"');
					}

					if (solutionLine.Trim().StartsWith("<Project", StringComparison.InvariantCultureIgnoreCase))
					{
						return solutionLine.Split(['\"'], StringSplitOptions.None)[1].Trim(' ', '\"');
					}

					return (string)null;
				})
				.Where(projectFileName => !string.IsNullOrWhiteSpace(projectFileName) && string.Equals(System.IO.Path.GetExtension(projectFileName), ".csproj", StringComparison.InvariantCultureIgnoreCase))
				.ToArray();
		}

		private string[] ProjectFileNamesFromSolutionContentLines(IEnumerable<string> solutionLines, string solutionSourceDirectory)
		{
			return ProjectFileNamesFromSolutionContentLines(solutionLines)
				.Select(projectFileName => System.IO.Path.Combine(solutionSourceDirectory, projectFileName))
				.ToArray();
		}
	}
}