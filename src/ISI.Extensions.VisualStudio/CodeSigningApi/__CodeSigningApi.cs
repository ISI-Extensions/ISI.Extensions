using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class CodeSigningApi 
	{
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }

		public CodeSigningApi(
			Microsoft.Extensions.Logging.ILogger logger)
		{
			Logger = logger;
		}
	}
}