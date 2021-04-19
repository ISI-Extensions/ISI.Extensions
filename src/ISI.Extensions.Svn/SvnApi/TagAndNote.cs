#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Svn.DataTransferObjects.SvnApi;

namespace ISI.Extensions.Svn
{
	public partial class SvnApi
	{
		public DTOs.TagAndNoteResponse TagAndNote(DTOs.TagAndNoteRequest request)
		{
			var response = new DTOs.TagAndNoteResponse();

			var info = GetInfo(new DTOs.GetInfoRequest()
			{
				Source = request.WorkingCopyDirectory,
			});

			if (info.IsUnderSvn)
			{
				var trunkUrl = GetTrunkUrl(info.Uri);

				if (!string.IsNullOrEmpty(trunkUrl))
				{
					Logger.LogInformation(string.Format("  trunkUrl=\"{0}\"", trunkUrl));

					var tagsUrl = GetTagsUrl(trunkUrl, request.Version, request.DateTimeStamp, request.DateTimeMask);

					if (!string.IsNullOrEmpty(tagsUrl))
					{
						Logger.LogInformation(string.Format("  tagsUrl=\"{0}\"", tagsUrl));

						var note = string.Format("Version: {0}\nDateTimeStamp: {1}", request.Version, request.DateTimeStamp.Formatted(DateTimeExtensions.DateTimeFormat.DateTimePrecise));

						Logger.LogInformation("  svn tag start");

						RemoteCopy(new DTOs.RemoteCopyRequest()
						{
							SourceUrl = trunkUrl,
							TargetUrl = tagsUrl,
							LogMessage = note,
							CreateParents = true,
						});

						Logger.LogInformation("  svn tag done");
					}
					else
					{
						Logger.LogInformation("Missing tagsUrl");
					}
				}
				else
				{
					Logger.LogInformation("Missing trunkUrl");
				}
			}
			else
			{
				Logger.LogInformation("Not under source Control");
			}

			return response;
		}
	}
}