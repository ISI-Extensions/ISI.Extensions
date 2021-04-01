using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.SCM.DataTransferObjects.FileStoreApi;

namespace ISI.Extensions.SCM
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