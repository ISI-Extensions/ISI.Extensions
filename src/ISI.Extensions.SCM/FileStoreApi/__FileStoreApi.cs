using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.FileStoreApi;

namespace ISI.Extensions.Scm
{
	public partial class FileStoreApi 
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public FileStoreApi(
			Microsoft.Extensions.Logging.ILogger logger = null)
		{
			Logger = logger ?? new ConsoleLogger();
		}
	}
}