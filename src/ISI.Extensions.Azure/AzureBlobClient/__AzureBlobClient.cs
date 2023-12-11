using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Azure.DataTransferObjects.AzureBlobClient;

namespace ISI.Extensions.Azure
{
	public partial class AzureBlobClient
	{
		protected string ConnectionString { get; }
		protected string BlobContainerName { get; }

		private global::Azure.Storage.Blobs.BlobContainerClient _container = null;
		protected global::Azure.Storage.Blobs.BlobContainerClient Container => _container ??= new global::Azure.Storage.Blobs.BlobContainerClient(ConnectionString, BlobContainerName);

		public const string DirectorySeparator = "/";

		public AzureBlobClient(
			string connectionString,
			string blobContainerName)
		{
			ConnectionString = connectionString;
			BlobContainerName = blobContainerName;
		}
	}
}