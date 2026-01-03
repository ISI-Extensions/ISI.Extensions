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
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.VsixPublisherApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class VsixPublisherApi
	{
		public DTOs.GenerateVsixPublishManifestResponse GenerateVsixPublishManifest(DTOs.GenerateVsixPublishManifestRequest request)
		{
			var response = new DTOs.GenerateVsixPublishManifestResponse();


			var vsixManifestXml = System.Xml.Linq.XElement.Load(request.VsixManifestFullName);
			var vsixManifestMetadataXml = vsixManifestXml.GetElementByLocalName("Metadata");
			var vsixManifestMetadataIdentityXml = vsixManifestMetadataXml.GetElementByLocalName("Identity");
			var vsixManifestInstallationXml = vsixManifestXml.GetElementByLocalName("Installation");

			var overview = (string)null;
			var assetFiles = new List<ISI.Extensions.VisualStudio.SerializableModels.VsixPublishManifestAssetFile>();

			if (!string.IsNullOrWhiteSpace(request.ReadMeFullName) && System.IO.File.Exists(request.ReadMeFullName))
			{
				overview = System.IO.File.ReadAllText(request.ReadMeFullName);
				var readMeDirectory = System.IO.Path.GetDirectoryName(request.ReadMeFullName);

				var imageRegex = new System.Text.RegularExpressions.Regex(@"(?:(?:\!\[)(?:\w*)?(?:\]\()(?<path>.*)(?:\)))");

				var imageMatches = imageRegex.Matches(overview);
				for (var i = 0; i < imageMatches.Count; i++)
				{
					var imageMatch = imageMatches[i];

					var targetPath = imageMatch.Groups["path"].Value;
					var assetFileName = readMeDirectory;
					foreach (var path in targetPath.Split(new[] { '\\', '/' }))
					{
						assetFileName = System.IO.Path.Combine(assetFileName, path);
					}

					if (!assetFiles.Any(assetFile => string.Equals(assetFile.TargetPath, targetPath, StringComparison.InvariantCultureIgnoreCase)))
					{
						assetFiles.Add(new()
						{
							TargetPath = targetPath,
							AssetFileName = assetFileName,
						});
					}
				}
			}

			response.PublisherName = vsixManifestMetadataIdentityXml.GetAttributeByLocalName("Publisher")?.Value;

			var vsixPublishManifest = new ISI.Extensions.VisualStudio.SerializableModels.VsixPublishManifest()
			{
				Categories = request.Categories.ToNullCheckedArray(),
				Identity = new()
				{
					//Description = vsixManifestMetadataXml.GetElementByLocalName("Description")?.Value,
					//DisplayName = vsixManifestMetadataXml.GetElementByLocalName("DisplayName")?.Value,
					//IconFileName = vsixManifestMetadataXml.GetElementByLocalName("Icon")?.Value,

					//InstallTargets = vsixManifestInstallationXml.GetElementsByLocalName("InstallationTarget")?.ToNullCheckedArray(installationTargetXml => new ISI.Extensions.VisualStudio.SerializableModels.VsixPublishManifestIdentityInstallTarget()
					//{
					//	SKU = installationTargetXml.GetAttributeByLocalName("Id")?.Value,
					//	Version = installationTargetXml.GetAttributeByLocalName("Version")?.Value,
					//}),
					//VsixId = vsixManifestMetadataIdentityXml.GetAttributeByLocalName("Id")?.Value,
					//Version = request.Version,
					InternalName = request.InternalName,
					//Language = vsixManifestMetadataIdentityXml.GetAttributeByLocalName("Language")?.Value,
					//Tags = request.Tags.ToNullCheckedArray(),
				},

				Overview = request.ReadMeFullName,
				AssetFiles = (assetFiles.Any() ? assetFiles.ToNullCheckedArray() : null),
				PriceCategory = request.PriceCategory.GetDescription(),

				Publisher = request.PublisherKey,
				Private = !request.Public,
				QuestionAndAnswer = request.QuestionAndAnswer,
				RepositoryUrl = vsixManifestMetadataXml.GetElementByLocalName("MoreInfo")?.Value,
			};

			response.VsixPublishManifest = JsonSerializer.Serialize(vsixPublishManifest, true);

			return response;
		}
	}
}