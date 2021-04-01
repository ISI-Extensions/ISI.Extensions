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
using DTOs = ISI.Extensions.SCM.DataTransferObjects.FileStoreApi;

namespace ISI.Extensions.SCM
{
	public partial class FileStoreApi
	{
		public DTOs.UploadFileResponse UploadFile(DTOs.UploadFileRequest request)
		{
			var response = new DTOs.UploadFileResponse();
			
			var fileSegments = new List<byte[]>();

			using (var ms = new System.IO.MemoryStream())
			{
				using (System.IO.Stream iStream = System.IO.File.OpenRead(request.FileName))
				{
					int chunkSize = 2048;
					byte[] buffer = new byte[chunkSize];
					int readBlocks = 0;

					readBlocks = iStream.Read(buffer, 0, chunkSize);

					while (readBlocks > 0)
					{
						ms.Write(buffer, 0, readBlocks);
						if (readBlocks >= chunkSize)
						{
							readBlocks = iStream.Read(buffer, 0, chunkSize);
						}
						else
						{
							readBlocks = 0;
						}

						ms.Flush();

						if ((ms.Position > 0) && ((readBlocks == 0) || (ms.Position + chunkSize > request.MaxFileSegmentSize)))
						{
							ms.Position = 0;
							fileSegments.Add(ms.ToArray());
							ms.SetLength(0);
						}
					}
				}
			}
			
			using (var fileStoreClient = ISI.Extensions.SCM.ServiceReferences.FileStore.FileStoreClient.GetClient(request.FileStoreUrl))
			{
				int jFileSegment = fileSegments.Count;
				for (int iFileSegment = 1; iFileSegment <= jFileSegment; iFileSegment++)
				{
					fileStoreClient.UploadFileSegmentAsync(request.UserName, request.Password, request.FileStoreUuid, request.Version, (iFileSegment == jFileSegment), fileSegments[iFileSegment - 1]);
				}
			}

			return response;
		}
	}
}