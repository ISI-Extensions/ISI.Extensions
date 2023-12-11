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
		public async Task<DTOs.WriteResponse> WriteAsync(DTOs.WriteRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.WriteResponse();
			
			var bucketExistsArgs = new Minio.DataModel.Args.BucketExistsArgs().WithBucket(BucketName);
			
			var found = await MinioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
			
			if (!found)
			{
				var mbArgs = new Minio.DataModel.Args.MakeBucketArgs().WithBucket(BucketName);
				await MinioClient.MakeBucketAsync(mbArgs, cancellationToken).ConfigureAwait(false);
			}
			// Upload a file to bucket.
			//var putObjectArgs = new Minio.DataModel.Args.PutObjectArgs()
			//	.WithBucket(bucketName)
			//	.WithObject(objectName)
			//	.WithFileName(filePath)
			//	.WithContentType(contentType);
			//await MinioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);





			//var writeFile = request.OverWriteExisting;

			//if (!writeFile)
			//{
			//	writeFile = !(await blob.ExistsAsync(cancellationToken)).Value;
			//}

			//if (writeFile)
			//{
			//	request.Stream.Rewind();
			//	await blob.UploadAsync(request.Stream, request.OverWriteExisting, cancellationToken);
			//}

			return response;
		}
	}
}