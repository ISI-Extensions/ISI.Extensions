#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
		public DTOs.GetSolutionFullNameResponse GetSolutionFullName(DTOs.GetSolutionFullNameRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.GetSolutionFullNameResponse();

			if (System.IO.Directory.Exists(request.Solution))
			{
				var possibleSolutionFullNames = ISI.Extensions.VisualStudio.Solution.FindSolutionFullNames(request.Solution, System.IO.SearchOption.AllDirectories).ToArray();

				if (possibleSolutionFullNames.Length == 1)
				{
					response.SolutionFullName = possibleSolutionFullNames.First();
				}
				else if (possibleSolutionFullNames.Length > 1)
				{
					var possibleSolutionName = System.IO.Path.GetFileName(request.Solution);

					var possibleSolutionFullName = possibleSolutionFullNames.FirstOrDefault(possibleSolutionFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleSolutionFullName), possibleSolutionName, StringComparison.InvariantCultureIgnoreCase));

					if (string.IsNullOrWhiteSpace(possibleSolutionFullName))
					{
						possibleSolutionFullName = possibleSolutionFullNames.FirstOrDefault(possibleSolutionFullName => string.Equals(System.IO.Path.GetFileNameWithoutExtension(possibleSolutionFullName).Replace(" ", string.Empty).Replace(".", string.Empty), possibleSolutionName, StringComparison.InvariantCultureIgnoreCase));
					}

					if (string.IsNullOrWhiteSpace(possibleSolutionFullName))
					{
						possibleSolutionFullName = possibleSolutionFullNames.OrderBy(possibleSolutionFullName => possibleSolutionFullName.Split(new[] { '/', '\\' }).Length).FirstOrDefault();
					}

					if (!string.IsNullOrWhiteSpace(possibleSolutionFullName))
					{
						response.SolutionFullName = possibleSolutionFullName;
					}
					else
					{
						var message = string.Format("Cannot determine which solution to get details for \"{0}\"", request.Solution);

						logger.LogError(message);

						throw new(message);
					}
				}
				else
				{
					var message = string.Format("Cannot find a solution to get details for \"{0}\"", request.Solution);

					logger.LogError(message);

					if (request.ThrowErrorIfNoSolutionFound)
					{
						throw new(message);
					}
				}
			}

			if (System.IO.File.Exists(request.Solution))
			{
				response.SolutionFullName = request.Solution;
			}

			return response;
		}
	}
}