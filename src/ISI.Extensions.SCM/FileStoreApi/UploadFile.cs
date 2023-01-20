#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Scm.DataTransferObjects.FileStoreApi;

namespace ISI.Extensions.Scm
{
	public partial class FileStoreApi
	{
		public DTOs.UploadFileResponse UploadFile(DTOs.UploadFileRequest request)
		{
			var response = new DTOs.UploadFileResponse();
			
			Logger.LogInformation("UploadFile");
			Logger.LogInformation(string.Format("  FileStoreUrl: {0}", request.FileStoreUrl));
			Logger.LogInformation(string.Format("  FileName: {0}", request.FileName));
			Logger.LogInformation(string.Format("  FileStoreUuid: {0}", request.FileStoreUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens)));

			var fileSegments = new List<byte[]>();

			using (var memoryStream = new System.IO.MemoryStream())
			{
				using (System.IO.Stream fileStream = System.IO.File.OpenRead(request.FileName))
				{
					var chunkSize = 2048;
					var buffer = new byte[chunkSize];
					var readBlocks = 0;

					readBlocks = fileStream.Read(buffer, 0, chunkSize);

					while (readBlocks > 0)
					{
						memoryStream.Write(buffer, 0, readBlocks);
						readBlocks = readBlocks >= chunkSize ? fileStream.Read(buffer, 0, chunkSize) : 0;

						memoryStream.Flush();

						if ((memoryStream.Position > 0) && ((readBlocks == 0) || (memoryStream.Position + chunkSize > request.MaxFileSegmentSize)))
						{
							memoryStream.Position = 0;
							fileSegments.Add(memoryStream.ToArray());

							Logger.LogInformation(string.Format("  Created FileSegment: {0} Size: {1}", fileSegments.Count, memoryStream.Length));

							memoryStream.SetLength(0);
						}
					}
				}
			}
			
			using (var fileStoreClient = ISI.Extensions.Scm.ServiceReferences.FileStore.FileStoreClient.GetClient(request.FileStoreUrl))
			{
				var fileSegmentsCount = fileSegments.Count;
				for (var fileSegmentIndex = 1; fileSegmentIndex <= fileSegments.Count; fileSegmentIndex++)
				{
					Logger.LogInformation(string.Format("  Uploading FileSegment: {0} of {1}", fileSegmentIndex, fileSegmentsCount));
					fileStoreClient.UploadFileSegmentAsync(request.UserName, request.Password, request.FileStoreUuid, request.Version, (fileSegmentIndex == fileSegmentsCount), fileSegments[fileSegmentIndex - 1]).Wait();
				}
			}

			return response;
		}
	}
}