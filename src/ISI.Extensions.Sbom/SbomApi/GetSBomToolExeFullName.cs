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
using DTOs = ISI.Extensions.Sbom.DataTransferObjects.SbomApi;

namespace ISI.Extensions.Sbom
{
	public partial class SbomApi
	{
		private static string _sBomToolExeFileName = string.Empty;

		public DTOs.GetSBomToolExeFullNameResponse GetSBomToolExeFullName(DTOs.GetSBomToolExeFullNameRequest request)
		{
			var response = new DTOs.GetSBomToolExeFullNameResponse();

			const string sBomToolExeFileName = "sbom-tool-win-x64.exe.exe";

			if (string.IsNullOrEmpty(_sBomToolExeFileName))
			{
				if (ISI.Extensions.IO.Path.IsInEnvironmentPath(sBomToolExeFileName))
				{
					_sBomToolExeFileName = sBomToolExeFileName;
				}

				if (string.IsNullOrEmpty(_sBomToolExeFileName))
				{
					var sBomToolExeFullName = Configuration.SBomToolExeFullName;
					var sBomToolExeLastCheckFullName = $"{Configuration.SBomToolExeFullName}.lastchecked";

					var lastChecked = (System.IO.File.Exists(sBomToolExeLastCheckFullName) ? System.IO.File.ReadAllText(sBomToolExeLastCheckFullName).ToDateTimeUtcNullable() : null) ?? DateTime.MinValue;
					if (lastChecked.AddDays(1) < DateTimeStamper.CurrentDateTimeUtc())
					{
						ISI.Extensions.Locks.FileLock.Lock(sBomToolExeFullName, () =>
						{
							lastChecked = (System.IO.File.Exists(sBomToolExeLastCheckFullName) ? System.IO.File.ReadAllText(sBomToolExeLastCheckFullName).ToDateTimeUtcNullable() : null) ?? DateTime.MinValue;
							if (lastChecked.AddDays(1) < DateTimeStamper.CurrentDateTimeUtc())
							{
								using (var stream = System.IO.File.OpenWrite(sBomToolExeFullName))
								{
									ISI.Extensions.WebClient.Download.DownloadFile(@"https://github.com/microsoft/sbom-tool/releases/latest/download/sbom-tool-win-x64.exe", null, stream);
								}

								System.IO.File.WriteAllText(sBomToolExeLastCheckFullName, DateTimeStamper.CurrentDateTimeUtc().Formatted(DateTimeExtensions.DateTimeFormat.DateTime));
							}
						});
					}

					if (System.IO.File.Exists(sBomToolExeFullName))
					{
						_sBomToolExeFileName = sBomToolExeFullName;
					}
				}
			}

			response.SBomToolExeFullName = _sBomToolExeFileName;

			return response;
		}
	}
}