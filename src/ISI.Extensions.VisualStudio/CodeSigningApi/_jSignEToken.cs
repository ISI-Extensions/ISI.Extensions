using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using Microsoft.Extensions.Logging;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.CodeSigningApi;

namespace ISI.Extensions.VisualStudio
{
	public partial class CodeSigningApi
	{
		private bool jSignEToken(Microsoft.Extensions.Logging.ILogger logger, DTOs.ISignRequest request, IEnumerable<string> fileNames)
		{
			var arguments = new List<string>();

			arguments.Add("--storetype ETOKEN");

			if (!string.IsNullOrWhiteSpace(request.CertificatePassword))
			{
				arguments.Add($"--storepass \"{request.CertificatePassword}\"");
			}

			if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha256)
			{
				arguments.Add("--alg SHA-256");
			}
			else if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha384)
			{
				arguments.Add("--alg SHA-384");
			}
			else if (request.DigestAlgorithm == DTOs.CodeSigningDigestAlgorithm.Sha512)
			{
				arguments.Add("--alg SHA-512");
			}

			if (request.TimeStampUri != null)
			{
				arguments.Add($"--tsaurl \"{request.TimeStampUri}\"");
			}

			arguments.AddRange(fileNames.Select(fileName => $"\"{fileName}\""));

			var waitForProcessResponse = ISI.Extensions.Process.WaitForProcessResponse(new ISI.Extensions.Process.ProcessRequest()
			{
				ProcessExeFullName = "jsign",
				Arguments = arguments,
				Logger = (fileNames.NullCheckedCount() == 1 ? null : logger),
			});

			if (waitForProcessResponse.Errored)
			{
				Logger.LogError(waitForProcessResponse.Output);

				return false;
			}

			logger.LogInformation($"Signed files: {string.Join(", ", fileNames.Select(System.IO.Path.GetFileName))}");

			return true;
		}
	}
}
