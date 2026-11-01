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
using DTOs = ISI.Extensions.Debian.DataTransferObjects.DebianPackagingApi;

namespace ISI.Extensions.Debian
{
	public partial class DebianPackagingApi
	{
		private RepositoryDetails DownloadAndParseRepositoryDetails(DistributionConfiguration distribution, DistributionRepositoryConfiguration repository, string architecture, string suite)
		{
			var repositoryDetails = new RepositoryDetails()
			{
				Distribution = distribution.Distribution,
				Branch = distribution.Branch,
				Architecture = architecture,
			};

			var uri = new UriBuilder(repository.Url);
			uri.AddDirectoryToPath("dists");
			uri.AddDirectoryToPath(suite);
			uri.AddFileToPath("Release");

			repositoryDetails.Url = uri.Uri.ToString();

			var packageSources = new HashSet<string>(StringComparer.InvariantCulture);

			using (var archiveStream = new System.IO.MemoryStream())
			{
				ISI.Extensions.WebClient.Download.DownloadFile(repositoryDetails.Url, null, archiveStream);

				archiveStream.Rewind();

				var contentLines = archiveStream.TextReadToEnd().Split(['\r', '\n']).Where(contentLine => !string.IsNullOrWhiteSpace(contentLine));

				var packageDetails = (PackageDetails)null;

				foreach (var contentLine in contentLines)
				{
					var pieces = contentLine.Split([':'], 2);

					switch (pieces[0].ToLower())
					{
						case "origin":
							repositoryDetails.Origin = pieces[1].Trim();
							break;

						case "label":
							repositoryDetails.Label = pieces[1].Trim();
							break;

						case "suite":
							repositoryDetails.Suite = pieces[1].Trim();
							break;

						case "version":
							repositoryDetails.Version = pieces[1].Trim();
							break;

						case "codename":
							repositoryDetails.Codename = pieces[1].Trim();
							break;

						case "date":
							repositoryDetails.Date = pieces[1].ToDateTime();
							break;

						case "architectures":
							repositoryDetails.Architectures = pieces[1].Split(' ');
							break;

						case "components":
							repositoryDetails.Components = pieces[1].Split(' ');
							break;

						case "description":
							repositoryDetails.Description = pieces[1].Trim();
							break;

						default:
							pieces = pieces[0].Replace('\t', ' ').Split([' '], StringSplitOptions.RemoveEmptyEntries);
							if (pieces.Length > 2)
							{
								packageSources.Add(pieces[2]);
							}
							break;
					}
				}
			}

			var packages = new List<PackageDetails>();

			foreach (var component in repository.Components)
			{
				foreach (var packageSource in packageSources.Where(packageSource =>
									 packageSource.StartsWith($"{component}/", StringComparison.InvariantCulture) &&
									 (packageSource.IndexOf($"binary-{architecture}", StringComparison.InvariantCulture) >= 0) &&
									 packageSource.EndsWith(".gz", StringComparison.InvariantCultureIgnoreCase)))
				{
					uri = new UriBuilder(repository.Url);
					uri.AddDirectoryToPath("dists");
					uri.AddDirectoryToPath(suite);
					uri.AddFileToPath(packageSource);

					using (var archiveStream = new System.IO.MemoryStream())
					{
						ISI.Extensions.WebClient.Download.DownloadFile(uri.Uri, null, archiveStream);

						archiveStream.Rewind();

						using (var gzipStreamReader = new ISI.Extensions.Linux.GZipStreamReader(archiveStream, true))
						{
							var contentLines = gzipStreamReader.TextReadToEnd().Split(['\r', '\n']).Where(contentLine => !string.IsNullOrWhiteSpace(contentLine));

							var packageDetails = (PackageDetails)null;

							foreach (var contentLine in contentLines)
							{
								var pieces = contentLine.Split([':'], 2);

								switch (pieces[0].ToLower())
								{
									case "package":
										packageDetails = new()
										{
											Distribution = repositoryDetails.Distribution,
											Branch = repositoryDetails.Branch,
											Architecture = repositoryDetails.Architecture,
											Package = pieces[1].Trim(),
										};
										packages.Add(packageDetails);
										break;

									case "priority":
										packageDetails.Priority = pieces[1].Trim();
										break;

									case "section":
										packageDetails.Section = pieces[1].Trim();
										break;

									case "installed-size":
										packageDetails.InstalledSize = pieces[1].ToLong();
										break;

									case "maintainer":
										packageDetails.Maintainer = pieces[1].Trim();
										break;

									case "original-maintainer":
										packageDetails.OriginalMaintainer = pieces[1].Trim();
										break;

									case "architecture":
										packageDetails.Architecture = pieces[1].Trim();
										break;

									case "version":
										packageDetails.Version = pieces[1].Trim();
										break;

									case "replaces":
										packageDetails.Replaces = pieces[1].Trim();
										break;

									case "enhances":
										packageDetails.Enhances = pieces[1].Trim();
										break;

									case "breaks":
										packageDetails.Breaks = pieces[1].Trim();
										break;

									case "filename":
										packageDetails.FileName = pieces[1].Trim();
										break;

									case "size":
										packageDetails.FileSize = pieces[1].ToLong();
										break;

									case "md5sum":
										packageDetails.Md5sum = pieces[1].Trim();
										break;

									case "sha1":
										packageDetails.SHA1Hash = pieces[1].Trim();
										break;

									case "sha256":
										packageDetails.SHA256Hash = pieces[1].Trim();
										break;

									case "sha512":
										packageDetails.SHA512Hash = pieces[1].Trim();
										break;

									case "homepage":
										packageDetails.Homepage = pieces[1].Trim();
										break;

									case "description":
										packageDetails.Description = pieces[1].Trim();
										break;

									case "description-md5":
										packageDetails.DescriptionMd5sum = pieces[1].Trim();
										break;

									case "multi-arch":
										packageDetails.MultiArch = pieces[1].Trim();
										break;

									case "origin":
										packageDetails.Origin = pieces[1].Trim();
										break;

									case "bugs":
										packageDetails.Bugs = pieces[1].Trim();
										break;

									case "task":
										packageDetails.Tasks = pieces[1].Trim();
										break;
								}
							}
						}
					}
				}
			}

			repositoryDetails.Packages = packages.ToArray();

			return repositoryDetails;
		}
	}
}