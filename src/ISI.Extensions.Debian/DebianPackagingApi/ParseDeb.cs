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
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Debian.DataTransferObjects.DebianPackagingApi;

namespace ISI.Extensions.Debian
{
	public partial class DebianPackagingApi
	{
		public DTOs.ParseDebResponse ParseDeb(DTOs.IParseDebRequest request)
		{
			var response = new DTOs.ParseDebResponse();

			var debStream = (request as DTOs.ParseDebRequestWithDebStream)?.DebStream ?? System.IO.File.OpenRead((request as DTOs.ParseDebRequestWithDebFullName).DebFullName);

			using (var archiverStreamReader = new ISI.Extensions.Linux.ArchiverStreamReader(debStream, true))
			{
				while (archiverStreamReader.Read())
				{
					if (string.Equals(archiverStreamReader.FileName, "debian-binary", StringComparison.InvariantCultureIgnoreCase))
					{
						using (var archiveStream = archiverStreamReader.Open())
						{
							response.DebSpecVersion = new Version(archiveStream.TextReadToEnd());
						}
					}
					else if (string.Equals(archiverStreamReader.FileName, "control.tar.gz", StringComparison.InvariantCultureIgnoreCase))
					{
						using (var archiveStream = archiverStreamReader.Open())
						{
							using (var gzipStreamReader = new ISI.Extensions.Linux.GZipStreamReader(archiveStream, true))
							{
								using (var tarStreamReader = new ISI.Extensions.Linux.TarStreamReader(gzipStreamReader, true))
								{
									while (tarStreamReader.Read())
									{
										var fileName = tarStreamReader.FileName.TrimStart("./");

										if (string.Equals(fileName, "control", StringComparison.InvariantCultureIgnoreCase))
										{
											using (var contentStream = tarStreamReader.Open())
											{
												if (TryParseDebControl(contentStream.ReadAsStringToEnd(), out var debControl))
												{
													response.DebControl = debControl;
												}
											}
										}
										else if (string.Equals(fileName, "md5sums", StringComparison.InvariantCultureIgnoreCase))
										{
											using (var contentStream = tarStreamReader.Open())
											{
												var md5Sums = new ISI.Extensions.InvariantCultureIgnoreCaseStringDictionary<string>();

												foreach (var line in contentStream.TextReadToEnd().Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
												{
													var lineParts = line.Split(["  "], 2, StringSplitOptions.None);

													md5Sums[lineParts[1].Trim()] = lineParts[0].Trim();
												}

												if (md5Sums.Any())
												{
													response.Md5Sums = md5Sums;
												}
											}
										}
										else if (string.Equals(fileName, "preinst", StringComparison.InvariantCultureIgnoreCase))
										{
											using (var contentStream = tarStreamReader.Open())
											{
												response.PreInstallScript = contentStream.ReadAsStringToEnd();
											}
										}
										else if (string.Equals(fileName, "postinst", StringComparison.InvariantCultureIgnoreCase))
										{
											using (var contentStream = tarStreamReader.Open())
											{
												response.PostInstallScript = contentStream.ReadAsStringToEnd();
											}
										}
										else if (string.Equals(fileName, "prerm", StringComparison.InvariantCultureIgnoreCase))
										{
											using (var contentStream = tarStreamReader.Open())
											{
												response.PreRemovalScript = contentStream.ReadAsStringToEnd();
											}
										}
										else if (string.Equals(fileName, "postrm", StringComparison.InvariantCultureIgnoreCase))
										{
											using (var contentStream = tarStreamReader.Open())
											{
												response.PostRemovalScript = contentStream.ReadAsStringToEnd();
											}
										}
										else
										{
											using (var contentStream = tarStreamReader.Open())
											{
												var file = new DTOs.ParseDebFileResponseFile()
												{
													FileName = fileName,
													LinuxFileMode = tarStreamReader.FileHeader.LinuxFileMode,
													Content = contentStream.ReadAsStringToEnd(),
												};

												if (!string.IsNullOrWhiteSpace(file.FileName))
												{
													response.ExtraFiles ??= new();
													response.ExtraFiles[fileName] = file;
												}
											}
										}
									}
								}
							}
						}
					}
					else
					{
						var fileName = archiverStreamReader.FileName.TrimStart("./");

						if (string.Equals(fileName, "data.tar.gz", StringComparison.InvariantCultureIgnoreCase))
						{
							using (var archiveStream = archiverStreamReader.Open())
							{
								if (request.ParseDataFiles)
								{
									var dataFiles = new ISI.Extensions.InvariantCultureIgnoreCaseStringHashSet();

									using (var gzipStreamReader = new ISI.Extensions.Linux.GZipStreamReader(archiveStream, true))
									{
										using (var tarStreamReader = new ISI.Extensions.Linux.TarStreamReader(gzipStreamReader, true))
										{
											while (tarStreamReader.Read())
											{
												dataFiles.Add(tarStreamReader.FileName.TrimStart("./"));

												tarStreamReader.SkipToEnd();
											}
										}
									}

									if (dataFiles.Any())
									{
										response.DataFiles = dataFiles.ToArray();
									}
								}
								else
								{
									archiveStream.SkipToEnd();
								}
							}
						}
						else if (string.Equals(fileName, "data.tar", StringComparison.InvariantCultureIgnoreCase))
						{
							var dataFiles = new ISI.Extensions.InvariantCultureIgnoreCaseStringHashSet();

							using (var archiveStream = archiverStreamReader.Open())
							{
								if (request.ParseDataFiles)
								{
									using (var tarStreamReader = new ISI.Extensions.Linux.TarStreamReader(archiveStream, true))
									{
										while (tarStreamReader.Read())
										{
											dataFiles.Add(tarStreamReader.FileName.TrimStart("./"));

											tarStreamReader.SkipToEnd();
										}
									}

									if (dataFiles.Any())
									{
										response.DataFiles = dataFiles.ToArray();
									}
								}
								else
								{
									archiveStream.SkipToEnd();
								}
							}
						}
						else
						{
							Logger.LogInformation($"Unknown file found: {archiverStreamReader.FileName}");
						}
					}
				}
			}

			if (request is DTOs.ParseDebRequestWithDebFullName)
			{
				debStream.Dispose();
				debStream = null;
			}

			return response;
		}
	}
}