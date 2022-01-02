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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.VsWhereApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class VsWhereApi
	{
		private static string _vsWhereExeFullName = string.Empty;

		public DTOs.GetVsWhereExeFullNameResponse GetVsWhereExeFullName(DTOs.GetVsWhereExeFullNameRequest request)
		{
			var response = new DTOs.GetVsWhereExeFullNameResponse();
			
			const string vsWhereExeFileName = "vswhere.exe";

			if (string.IsNullOrEmpty(_vsWhereExeFullName))
			{
				if (ISI.Extensions.IO.Path.IsInEnvironmentPath(vsWhereExeFileName))
				{
					_vsWhereExeFullName = vsWhereExeFileName;
				}

				if (string.IsNullOrEmpty(_vsWhereExeFullName))
				{
					var uriCodeBase = new UriBuilder(typeof(ISI.Extensions.Nuget.NugetApi).Assembly.CodeBase);

					var directoryName = System.IO.Path.GetDirectoryName(Uri.UnescapeDataString(uriCodeBase.Path));

					var vsWhereExeFullName = System.IO.Path.Combine(directoryName, vsWhereExeFileName);

					if (!System.IO.File.Exists(vsWhereExeFullName))
					{
						var tempDirectory = System.IO.Path.GetTempFileName();
						System.IO.File.Delete(tempDirectory);
						System.IO.Directory.CreateDirectory(tempDirectory);

						{
							var process = new System.Diagnostics.Process();
							process.StartInfo.UseShellExecute = false;
							process.StartInfo.FileName = NugetApi.GetNugetExeFullName(new ISI.Extensions.Nuget.DataTransferObjects.NugetApi.GetNugetExeFullNameRequest()).NugetExeFullName;
							process.StartInfo.Arguments = string.Format("install vswhere -ExcludeVersion -OutputDirectory \"{0}\"", tempDirectory);
							process.Start();
							process.WaitForExit();
						}

						System.IO.File.Copy(System.IO.Path.Combine(System.IO.Path.Combine(System.IO.Path.Combine(tempDirectory, "vswhere"), "tools"), vsWhereExeFileName), vsWhereExeFullName);

						System.IO.Directory.Delete(tempDirectory, true);
					}

					if (System.IO.File.Exists(vsWhereExeFullName))
					{
						_vsWhereExeFullName = vsWhereExeFullName;
					}
				}
			}

			response.VsWhereExeFullName = _vsWhereExeFullName;

			return response;
		}
	}
}