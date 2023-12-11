using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.S3.DataTransferObjects.S3BlobClient;

namespace ISI.Extensions.S3
{
	public partial class S3BlobClient
	{
		public async Task<DTOs.DeleteFileIfExistsResponse> DeleteFileIfExistsAsync(DTOs.DeleteFileIfExistsRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.DeleteFileIfExistsResponse();
			
			//var blob = MinioClient.GetBlobClient(request.FullName);

			//await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken).ContinueWith(task => response.FileExisted = task.Result.Value, cancellationToken);

			return response;
		}
	}
}