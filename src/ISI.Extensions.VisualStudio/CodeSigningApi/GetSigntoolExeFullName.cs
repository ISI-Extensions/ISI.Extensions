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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class CodeSigningApi
	{
		private static string _signtoolExeFullName = string.Empty;

		public DTOs.GetSigntoolExeFullNameResponse GetSigntoolExeFullName(DTOs.GetSigntoolExeFullNameRequest request)
		{
			var response = new DTOs.GetSigntoolExeFullNameResponse();

			const string signtoolExeFileName = "signtool.exe";

			if (string.IsNullOrEmpty(_signtoolExeFullName))
			{
				//if (ISI.Extensions.IO.Path.IsInEnvironmentPath(SigntoolExeFileName))
				//{
				//	_signtoolExeFullName = SigntoolExeFileName;
				//}

				if (string.IsNullOrEmpty(_signtoolExeFullName))
				{
					var uriCodeBase = new UriBuilder(typeof(ISI.Extensions.VisualStudio.CodeSigningApi).Assembly.CodeBase);

					var directoryName = System.IO.Path.GetDirectoryName(Uri.UnescapeDataString(uriCodeBase.Path));

					if (request.UseShortPathName)
					{
						directoryName = System.IO.Path.Combine(System.IO.Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)), "Temp");
						System.IO.Directory.CreateDirectory(directoryName);
					}

					var signtoolExeFullName = System.IO.Path.Combine(directoryName, signtoolExeFileName);

					if (!System.IO.File.Exists(signtoolExeFullName))
					{
						const string sdkDirectory = @"C:\Program Files (x86)\Windows Kits\10\bin\";
						var sourceSigntoolExeFullName = System.IO.Directory.EnumerateFiles(sdkDirectory, signtoolExeFileName, System.IO.SearchOption.AllDirectories)
							.Select(fullName =>
							{
								var parts = fullName.Split(['\\'], StringSplitOptions.RemoveEmptyEntries);

								return (Version: new Version(parts[5]), Processor: parts[6], FileName: parts[7], FullName: fullName);
							})
							.Where(file => string.Equals(file.FileName ?? string.Empty, signtoolExeFileName, StringComparison.InvariantCultureIgnoreCase))
							.Where(file => file.Processor.StartsWith("x", StringComparison.InvariantCultureIgnoreCase))
							.OrderByDescending(file => file.Version)
							.ThenBy(file => file.Processor, StringComparer.InvariantCultureIgnoreCase)
							.FirstOrDefault().FullName;

						System.IO.File.Copy(sourceSigntoolExeFullName, signtoolExeFullName);
					}

					if (System.IO.File.Exists(signtoolExeFullName))
					{
						_signtoolExeFullName = signtoolExeFullName;
					}
				}
			}

			response.SigntoolExeFullName = _signtoolExeFullName;

			return response;
		}
	}
}