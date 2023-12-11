using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Azure.DataTransferObjects.AzureBlobClient
{
	public class DeleteFileIfExistsRequest
	{
		public string FullName { get; set; }
	}
}