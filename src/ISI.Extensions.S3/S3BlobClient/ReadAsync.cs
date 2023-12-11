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
		public async Task<DTOs.ReadResponse> ReadAsync(DTOs.ReadRequest request, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.ReadResponse();
			
			//var blob = Container.GetBlobClient(request.FullName);

			//await blob.DownloadToAsync(request.Stream, cancellationToken);

			return response;
		}
	}
}