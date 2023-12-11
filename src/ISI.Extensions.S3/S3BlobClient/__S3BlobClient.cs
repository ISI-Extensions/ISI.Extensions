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
		protected string EndpointAddress { get; }
		protected int EndpointPort { get; }
		protected bool EndpointIsSecure { get; }
		protected string AccessKey { get; }
		protected string SecretKey { get; }
		protected string BucketName { get; }

		private Minio.IMinioClient _minioClient = null;
		protected Minio.IMinioClient MinioClient => _minioClient ??= new Minio.MinioClient()
			.WithEndpoint(EndpointAddress, EndpointPort)
			.WithCredentials(AccessKey, SecretKey)
			.WithSSL(EndpointIsSecure)
			.Build();

		public const string DirectorySeparator = "/";

		public S3BlobClient(
			string endpointUrl,
			string accessKey,
			string secretKey,
			string bucketName)
		{
			var endPointUri = new Uri(endpointUrl);

			EndpointAddress = endPointUri.Host;
			EndpointPort = endPointUri.Port;
			EndpointIsSecure = string.Equals(endPointUri.Scheme, "https", StringComparison.InvariantCultureIgnoreCase);

			AccessKey = accessKey;
			SecretKey = secretKey;
			BucketName = bucketName;
		}
	}
}