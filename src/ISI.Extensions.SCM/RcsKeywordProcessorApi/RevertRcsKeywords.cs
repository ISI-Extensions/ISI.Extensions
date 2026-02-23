using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.Scm.DataTransferObjects.RcsKeywordProcessorApi;

namespace ISI.Extensions.Scm
{
	public partial class RcsKeywordProcessorApi
	{
		public DTOs.RevertRcsKeywordsResponse RevertRcsKeywords(DTOs.RevertRcsKeywordsRequest request)
		{
			var logger = new AddToLogLogger(request.AddToLog, Logger);

			var response = new DTOs.RevertRcsKeywordsResponse();

			foreach (var modifiedFile in request.ModifiedFiles ?? [])
			{
				logger.LogInformation($"Processing {System.IO.Path.GetFileName(modifiedFile.FullName)}");

				System.IO.File.Delete(modifiedFile.FullName);
				System.IO.File.WriteAllText(modifiedFile.FullName, modifiedFile.OriginalContent);
			}

			return response;
		}
	}
}