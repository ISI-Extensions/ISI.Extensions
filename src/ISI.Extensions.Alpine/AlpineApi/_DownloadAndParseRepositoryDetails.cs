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
using DTOs = ISI.Extensions.Alpine.DataTransferObjects.AlpineApi;

namespace ISI.Extensions.Alpine
{
	public partial class AlpineApi
	{
		private RepositoryDetails DownloadAndParseRepositoryDetails(string branch, string repository, string architecture)
		{
			var repositoryDetails = new RepositoryDetails()
			{
				Branch = branch,
				Repository = repository,
				Architecture = architecture,
			};

			var uri = new UriBuilder(Configuration.Url);
			uri.AddDirectoryToPath(branch);
			uri.AddDirectoryToPath(repository);
			uri.AddDirectoryToPath(architecture);
			uri.AddFileToPath("APKINDEX.tar.gz");

			repositoryDetails.Url = uri.Uri.ToString();

			var packages = new List<PackageDetails>();

			using (var archiveStream = new System.IO.MemoryStream())
			{
				ISI.Extensions.WebClient.Download.DownloadFile(repositoryDetails.Url, null, archiveStream);

				archiveStream.Rewind();

				using (var gzipStreamReader = new ISI.Extensions.Linux.GZipStreamReader(archiveStream, true))
				{
					using (var tarStreamReader = new ISI.Extensions.Linux.TarStreamReader(gzipStreamReader, true))
					{
						while ((repositoryDetails.Packages == null) && tarStreamReader.Read())
						{
							var fileName = tarStreamReader.FileName.TrimStart("./");

							if (string.Equals(fileName, "DESCRIPTION", StringComparison.InvariantCultureIgnoreCase))
							{
								using (var contentStream = tarStreamReader.Open())
								{
									repositoryDetails.FullVersion = contentStream.ReadAsStringToEnd().Replace('\r', '\n').Split('\n').First();
									repositoryDetails.Version = repositoryDetails.FullVersion.Trim('v').Split('-').First();
								}
							}
							else if (string.Equals(fileName, "APKINDEX", StringComparison.InvariantCultureIgnoreCase))
							{
								using (var contentStream = tarStreamReader.Open())
								{
									var contentLines = contentStream.TextReadToEnd().Split(['\r', '\n']).Where(contentLine => !string.IsNullOrWhiteSpace(contentLine));

									var packageDetails = (PackageDetails)null;

									foreach (var contentLine in contentLines)
									{
										var pieces = contentLine.Split([':'], 2);

										switch (pieces[0])
										{
											case "C":
												packageDetails = new PackageDetails()
												{
													Branch = repositoryDetails.Branch,
													Repository = repositoryDetails.Repository,
													Architecture = repositoryDetails.Architecture,
												};
												packages.Add(packageDetails);
												break;

											case "P":
												packageDetails.Package = pieces[1];
												break;

											case "V":
												packageDetails.Version = pieces[1];
												break;

											case "T":
												packageDetails.Description = pieces[1];
												break;

											case "A":
												packageDetails.Architecture = pieces[1];
												break;

											case "L":
												packageDetails.License = pieces[1];
												break;

											case "o":
												packageDetails.Origin = pieces[1];
												break;

											case "U":
												packageDetails.Url = pieces[1];
												break;

											case "S":
												packageDetails.FileSize = pieces[1].ToLongNullable();
												break;

										}
									}
								}

								repositoryDetails.Packages = packages.ToArray();
							}
							else
							{
								using (var contentStream = tarStreamReader.Open())
								{
									using (var stream = new System.IO.MemoryStream())
									{
										contentStream.CopyTo(stream);
										stream.Flush();
									}
								}
							}
						}
					}
				}
			}

			return repositoryDetails;
		}
	}
}