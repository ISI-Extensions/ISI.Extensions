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
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs = ISI.Extensions.Ngrok.DataTransferObjects.NGrokLocalServiceApi;

namespace ISI.Extensions.Ngrok
{
	public partial class NGrokLocalServiceApi
	{
		public DTOs.StartLocalServiceResponse StartLocalService()
		{
			var response = new DTOs.StartLocalServiceResponse();

			var runningLocalService = GetRunningLocalService();

			if (!runningLocalService.IsRunning)
			{
				var ngrokServiceFileName = ISI.Extensions.IO.Path.GetFileNameDeMasked(Configuration.NGrokServiceFileName);

				if (!System.IO.File.Exists(ngrokServiceFileName))
				{
					var downloadLinkRegex = (Environment.Is64BitOperatingSystem ? new System.Text.RegularExpressions.Regex("id=\"dl-windows-amd64\"\\shref=\"(?<url>http[s]?:\\/\\/[^\"]*?)\"") : new System.Text.RegularExpressions.Regex("id=\"dl-windows-386\"\\shref=\"(?<url>http[s]?:\\/\\/[^\"]*?)\""));

					var downloadHtml = ISI.Extensions.WebClient.Rest.ExecuteTextGet(@"https://ngrok.com/download", null, true);

					var match = downloadLinkRegex.Match(downloadHtml);

					if (match.Success)
					{
						var downloadUrl = match.Groups["url"].Value;

						var downloadFileResponse = ISI.Extensions.WebClient.Download.DownloadFile<System.IO.MemoryStream>(downloadUrl, null);

						using (var zipArchive = new System.IO.Compression.ZipArchive(downloadFileResponse.Stream, System.IO.Compression.ZipArchiveMode.Read))
						{
							var archiveEntry = zipArchive.Entries.FirstOrDefault(file => file.Name.IndexOf("ngrok.exe", StringComparison.InvariantCultureIgnoreCase) >= 0);

							System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(ngrokServiceFileName));

							archiveEntry?.ExtractToFile(ngrokServiceFileName);
						}
					}
				}

				//var processStartInfo = new System.Diagnostics.ProcessStartInfo();

				//processStartInfo.CreateNoWindow = false;
				//processStartInfo.FileName = ngrokServiceFileName;
				//processStartInfo.Arguments = "start --none";

				//processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

				//System.Diagnostics.Process.Start(processStartInfo);
				System.Diagnostics.Process.Start(ngrokServiceFileName, "start --none");
			}

			//using (var process = System.Diagnostics.Process.Start(ngrokServiceFileName, "start --none"))
			//{
			//	process.WaitForExit();
			//}

			return response;
		}
	}
}
