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
		/*
svn checkout <url_of_big_dir> <target> --depth empty
cd <target>
svn up <file_you_want>
		*/
		public DTOs.CheckOutSingleFileResponse CheckOutSingleFile(DTOs.CheckOutSingleFileRequest request)
		{
			var response = new DTOs.CheckOutSingleFileResponse();

			if (!System.IO.Directory.Exists(request.TargetFullName))
			{
				System.IO.Directory.CreateDirectory(request.TargetFullName);
			}

			var lastPathSeparatorIndex = request.SourceUrl.LastIndexOf("/", StringComparison.InvariantCultureIgnoreCase);
			if (lastPathSeparatorIndex < 0)
			{
				lastPathSeparatorIndex = request.SourceUrl.LastIndexOf("\\", StringComparison.InvariantCultureIgnoreCase);
			}

			var sourceUrl = request.SourceUrl.Substring(0, lastPathSeparatorIndex);
			var fileName = request.SourceUrl.Substring(lastPathSeparatorIndex + 1);

			var targetFullName = request.TargetFullName.TrimEnd(fileName, StringComparison.InvariantCultureIgnoreCase);

			var arguments = new List<string>();

			arguments.Add("checkout");
			arguments.Add(string.Format("\"{0}\"", sourceUrl));
			arguments.Add(string.Format("\"{0}\"", targetFullName));
			arguments.Add("--depth empty");
			AddCredentials(arguments, request);

			response.Success = !ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				Logger = new AddToLogLogger(request.AddToLog, Logger),
				ProcessExeFullName = "svn",
				Arguments = arguments.ToArray(),
			}).Errored;

			if (response.Success)
			{
				arguments.Clear();

				arguments.Add("update");
				arguments.Add(string.Format("\"{0}\"", fileName));
				AddCredentials(arguments, request);

				response.Success = !ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
				{
					Logger = new AddToLogLogger(request.AddToLog, Logger),
					ProcessExeFullName = "svn",
					Arguments = arguments.ToArray(),
					WorkingDirectory = targetFullName,
				}).Errored;
			}
			
			return response;
		}
	}
}