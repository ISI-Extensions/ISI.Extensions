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
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.XmlTransformApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class XmlTransformApi
	{
		public DTOs.XmlTransformConfigsInProjectResponse XmlTransformConfigsInProject(DTOs.XmlTransformConfigsInProjectRequest request)
		{
			var response = new DTOs.XmlTransformConfigsInProjectResponse();
			
			var logger = new AddToLogLogger(request.AddToLog, request.Logger ?? Logger);

			var filesToTransform = new Dictionary<string, string>();

			var csProjXml = System.Xml.Linq.XElement.Load(request.ProjectFullName);

			//cakeContext.Log.Write(global::Cake.Core.Diagnostics.Verbosity.Normal, global::Cake.Core.Diagnostics.LogLevel.Information, "{0}", csProjXml.ToString());

			foreach (var itemGroup in csProjXml.Elements().Where(e => string.Equals(e.Name.LocalName, "ItemGroup", StringComparison.InvariantCultureIgnoreCase)))
			{
				//cakeContext.Log.Write(global::Cake.Core.Diagnostics.Verbosity.Normal, global::Cake.Core.Diagnostics.LogLevel.Information, "itemGroup");

				var contents = itemGroup.Elements().Where(e => (string.Equals(e.Name.LocalName, "Content", StringComparison.InvariantCultureIgnoreCase) || string.Equals(e.Name.LocalName, "None", StringComparison.InvariantCultureIgnoreCase)));

				foreach (var content in contents)
				{
					//cakeContext.Log.Write(global::Cake.Core.Diagnostics.Verbosity.Normal, global::Cake.Core.Diagnostics.LogLevel.Information, "content");

					var fileName = content.Attributes("Include").FirstOrDefault()?.Value;
					//cakeContext.Log.Write(global::Cake.Core.Diagnostics.Verbosity.Normal, global::Cake.Core.Diagnostics.LogLevel.Information, "fileName: {0}", fileName);
					if (!string.IsNullOrWhiteSpace(fileName) && fileName.EndsWith(".config", StringComparison.InvariantCultureIgnoreCase))
					{
						var directoryName = System.IO.Path.GetDirectoryName(fileName);
						//cakeContext.Log.Write(global::Cake.Core.Diagnostics.Verbosity.Normal, global::Cake.Core.Diagnostics.LogLevel.Information, "directoryName: {0}", directoryName);

						var dependentUpons = content.Elements().Where(e => string.Equals(e.Name.LocalName, "DependentUpon", StringComparison.InvariantCultureIgnoreCase));
						if (dependentUpons != null)
						{
							foreach (var dependentUpon in dependentUpons)
							{
								var dependentUponFileName = System.IO.Path.Combine(directoryName, dependentUpon.Value);

								//cakeContext.Log.Write(global::Cake.Core.Diagnostics.Verbosity.Normal, global::Cake.Core.Diagnostics.LogLevel.Information, "dependentUponFileName: {0}", dependentUponFileName);

								filesToTransform.Add(fileName, dependentUponFileName);
							}
						}
					}
				}
			}

			if (filesToTransform.NullCheckedAny())
			{
				var projectDirectory = System.IO.Path.GetDirectoryName(request.ProjectFullName);

				foreach (var fileToTransform in filesToTransform)
				{
					var transformFileName = fileToTransform.Key;
					var sourceFileName = fileToTransform.Value;

					var targetFileName = transformFileName;

					//cakeContext.Log.Write(global::Cake.Core.Diagnostics.Verbosity.Normal, global::Cake.Core.Diagnostics.LogLevel.Information, "transformFileName: {0}", transformFileName);
					//cakeContext.Log.Write(global::Cake.Core.Diagnostics.Verbosity.Normal, global::Cake.Core.Diagnostics.LogLevel.Information, "sourceFileName: {0}", sourceFileName);
					//cakeContext.Log.Write(global::Cake.Core.Diagnostics.Verbosity.Normal, global::Cake.Core.Diagnostics.LogLevel.Information, "targetFileName: {0}", targetFileName);

					if (request.MoveConfigurationKey)
					{
						var targetDirectory = System.IO.Path.GetDirectoryName(targetFileName);
						var targetFileNameSuffix = System.IO.Path.GetExtension(targetFileName);
						targetFileName = System.IO.Path.GetFileNameWithoutExtension(targetFileName);
						var targetFileNameConfiguration = System.IO.Path.GetExtension(targetFileName);
						targetFileName = System.IO.Path.GetFileNameWithoutExtension(targetFileName);
						targetFileName = string.Format("{0}{1}{2}", targetFileName, targetFileNameSuffix, targetFileNameConfiguration);
						targetFileName = System.IO.Path.Combine(targetDirectory, targetFileName);
					}

					sourceFileName = System.IO.Path.Combine(projectDirectory, sourceFileName);
					transformFileName = System.IO.Path.Combine(projectDirectory, transformFileName);
					targetFileName = System.IO.Path.Combine(request.DestinationDirectory, targetFileName);

					var destinationDirectory = System.IO.Path.GetDirectoryName(targetFileName);
					if (!System.IO.Directory.Exists(destinationDirectory))
					{
						System.IO.Directory.CreateDirectory(destinationDirectory);
					}

					var xmlTransformableDocument = new Microsoft.Web.XmlTransform.XmlTransformableDocument();
					xmlTransformableDocument.PreserveWhitespace = true;
					xmlTransformableDocument.Load(sourceFileName);

					var xmlTransformation = new Microsoft.Web.XmlTransform.XmlTransformation(transformFileName, true, new XmlTransformationLogger(logger));

					var success = xmlTransformation.Apply(xmlTransformableDocument);

					if (success)
					{
						using (var stream = new System.IO.FileStream(string.Format("{0}{1}", targetFileName, (string.IsNullOrWhiteSpace(request.TransformedFileSuffix) ? string.Empty : request.TransformedFileSuffix)), System.IO.FileMode.OpenOrCreate))
						{
							xmlTransformableDocument.Save(stream);
						}
					}

					if (!string.IsNullOrWhiteSpace(request.TransformedFileSuffix) && System.IO.File.Exists(targetFileName))
					{
						System.IO.File.Delete(targetFileName);
					}
				}
			}

			return response;
		}
	}
}