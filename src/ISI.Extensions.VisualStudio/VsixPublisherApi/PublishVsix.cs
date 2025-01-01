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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.VsixPublisherApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class VsixPublisherApi
	{
		public DTOs.PublishVsixResponse PublishVsix(DTOs.PublishVsixRequest request)
		{
			var response = new DTOs.PublishVsixResponse();

			var logger = new AddToLogLogger(request.AddToLog, Logger);


			//VsixPublisher.exe login -personalAccessToken "{Personal Access Token}" -publisherName "{Publisher Name}"

			//VsixPublisher.exe publish -payload "{path to vsix}" -publishManifest "{path to vs-publish.json}" -ignoreWarnings "VSIXValidatorWarning01,VSIXValidatorWarning02"

			//VsixPublisher.exe logout -publisherName "{Publisher Name}"
			

			//VsixPublisher.exe publish -payload "{Path to vsix file}"  -publishManifest "{path to publishManifest file}"  -personalAccessToken "{Personal Access Token that is used to authenticate the publisher. If not provided, the pat is acquired from the logged-in users.}"


			var vsixPublisherExeFullName = GetVsixPublisherExeFullName(new()).VsixPublisherExeFullName;

			if (string.IsNullOrWhiteSpace(vsixPublisherExeFullName) || !System.IO.File.Exists(vsixPublisherExeFullName))
			{
				throw new Exception("Cannot find or get VsixPublisher.exe");
			}

			var generateVsixPublishManifestResponse = GenerateVsixPublishManifest(new()
			{
				PublisherKey = request.PublisherKey,
				VsixManifestFullName = request.VsixManifestFullName,
				InternalName = request.InternalName,
				Version = request.Version,
				ReadMeFullName = request.ReadMeFullName,
				PriceCategory = request.PriceCategory,
				Categories = request.Categories.ToNullCheckedArray(),
				Public = request.Public,
				QuestionAndAnswer = request.QuestionAndAnswer,
			});


			//var loginResponse = ISI.Extensions.Process.WaitForProcessResponse(vsixPublisherExeFullName, new[] { "login", $"-personalAccessToken \"{request.PersonalAccessToken}\"", $"-publisherName \"{generateVsixPublishManifestResponse.PublisherName}\"" });

			//if (loginResponse.Errored)
			//{
			//	throw new Exception("Cannot login");
			//}

			using (var tempVsixPublishManifestFile = new ISI.Extensions.IO.Path.TempFile())
			{
				System.IO.File.WriteAllText(tempVsixPublishManifestFile.FullName, generateVsixPublishManifestResponse.VsixPublishManifest);

				var publishResponse = ISI.Extensions.Process.WaitForProcessResponse(new Process.ProcessRequest()
				{
					Logger = logger,
					ProcessExeFullName = vsixPublisherExeFullName,
					Arguments =
					[
						"publish",
						$"-payload \"{request.VsixFullName}\"",
						$"-publishManifest \"{tempVsixPublishManifestFile.FullName}\"",
						$"-personalAccessToken \"{request.PersonalAccessToken}\""
					],
				});

				if (publishResponse.Errored)
				{
					throw new Exception("Cannot publish");
				}
			}




			//var logoutResponse = ISI.Extensions.Process.WaitForProcessResponse(vsixPublisherExeFullName, new[] { "logout", $"-publisherName \"{generateVsixPublishManifestResponse.PublisherName}\"" });

			//if (logoutResponse.Errored)
			//{
			//	throw new Exception("Cannot logout");
			//}

			return response;
		}
	}
}