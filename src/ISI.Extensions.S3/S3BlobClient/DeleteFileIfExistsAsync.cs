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

			var fileExistsResponse = await FileExistsAsync(new()
			{
				FullName = request.FullName,
			}, cancellationToken).ConfigureAwait(false);

			if (fileExistsResponse.FileExisted)
			{
				var args = new Minio.DataModel.Args.RemoveObjectArgs()
					.WithBucket(BucketName)
					.WithObject(request.FullName);

				await MinioClient.RemoveObjectAsync(args, cancellationToken).ConfigureAwait(false);

				response.FileExisted = true;
			}

			return response;
		}
	}
}