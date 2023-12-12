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
			var bucketExists = await MinioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken).ConfigureAwait(false);
			
			if (!bucketExists)
			{
				var makeBucketArgs = new Minio.DataModel.Args.MakeBucketArgs().WithBucket(BucketName);
				await MinioClient.MakeBucketAsync(makeBucketArgs, cancellationToken).ConfigureAwait(false);
			}

			request.Stream.Rewind();

			var putObjectArgs = new Minio.DataModel.Args.PutObjectArgs()
				.WithBucket(BucketName)
				.WithStreamData(request.Stream)
				.WithFileName(request.FullName)
				.WithContentType(ISI.Extensions.MimeType.GetMimeType(request.FullName));

			await MinioClient.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);

			return response;
		}
	}
}