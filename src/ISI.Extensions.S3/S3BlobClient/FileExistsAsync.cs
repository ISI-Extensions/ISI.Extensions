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
		public async Task<DTOs.FileExistsResponse> FileExistsAsync(DTOs.FileExistsRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.FileExistsResponse();

			try
			{
				var statObjectArgs = new Minio.DataModel.Args.StatObjectArgs()
					.WithBucket(BucketName)
					.WithObject(request.FullName);

				var statObjectResponse = await MinioClient.StatObjectAsync(statObjectArgs, cancellationToken);

				response.FileExists = ((statObjectResponse?.LastModified ?? DateTime.MinValue) != DateTime.MinValue);
			}
			catch
			{
			}

			return response;
		}
	}
}