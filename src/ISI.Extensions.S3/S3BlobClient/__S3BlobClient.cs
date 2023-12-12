using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Minio;
using DTOs = ISI.Extensions.S3.DataTransferObjects.S3BlobClient;

namespace ISI.Extensions.S3
{
	public partial class S3BlobClient
	{
		protected string BucketName { get; }

		protected Minio.IMinioClient MinioClient { get; }

		public const string DirectorySeparator = "/";

		public S3BlobClient(
			string endpointUrl,
			string accessKey,
			string secretKey,
			string bucketName)
		{
			var endPointUri = new Uri(endpointUrl);

			MinioClient = new Minio.MinioClient()
				.WithEndpoint(endPointUri.Host, endPointUri.Port)
				.WithCredentials(accessKey, secretKey)
				.WithSSL(string.Equals(endPointUri.Scheme, "https", StringComparison.InvariantCultureIgnoreCase))
				.Build();

			BucketName = bucketName;
		}
	}
}