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
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.VisualStudioCode.DataTransferObjects.SolutionApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudioCode
{
	public partial class SolutionApi
	{
		public DTOs.GetSolutionDetailsResponse GetSolutionDetails(DTOs.GetSolutionDetailsRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.GetSolutionDetailsResponse();

			var solutionFullName = GetSolutionFullName(new()
			{
				Solution = request.Solution,
				AddToLog = request.AddToLog,
			}).SolutionFullName;

			if (!string.IsNullOrWhiteSpace(solutionFullName))
			{
				var solutionPackage = (SolutionPackage)null;

				try
				{
					solutionPackage = JsonSerializer.Deserialize<ISI.Extensions.VisualStudioCode.SerializableModels.SolutionPackage>(System.IO.File.ReadAllText(solutionFullName))?.Export();
				}
				catch (Exception exception)
				{
					//Console.WriteLine(exception);
					//throw;
				}

				var solutionDirectory = System.IO.Path.GetDirectoryName(solutionFullName);

				var rootSourceDirectory = SourceControlClientApi.GetRootDirectory(new()
				{
					FullName = solutionDirectory,
				}).FullName;

				var solutionName = solutionPackage?.Description;
				if (string.IsNullOrWhiteSpace(solutionName))
				{
					solutionName = solutionPackage?.Name;
				}
				if (string.IsNullOrWhiteSpace(solutionName))
				{
					solutionName = System.IO.Path.GetFileName(rootSourceDirectory);
				}

				response.SolutionDetails = new()
				{
					SolutionName = solutionName,
					SolutionFullName = solutionFullName,
					SolutionDirectory = solutionDirectory,
					RootSourceDirectory = rootSourceDirectory,
				};
			}

			return response;
		}
	}
}