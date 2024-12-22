#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.PfSense.DataTransferObjects.PfSenseApi;

namespace ISI.Extensions.PfSense
{
	public partial class PfSenseApi
	{
		public DTOs.ActivateCertificateResponse ActivateCertificate(DTOs.ActivateCertificateRequest request)
		{
			var response = new DTOs.ActivateCertificateResponse();

			var connectionInfo = GetConnectionInfo(request);

			var directoryName = "/home/certificate-installer";
			var scriptFileName = "pfsense-import-certificate.php";
			var keyCertificateFileName = "key.pem";
			var bundleCertificateFileName = "fullchain.pem";

			using (var client = new Renci.SshNet.SshClient(connectionInfo))
			{
				client.Connect();

				using (var cmd = client.CreateCommand("8"))
				{
					var result = cmd.Execute();
				}

				using (var cmd = client.CreateCommand($"rm -r {directoryName}"))
				{
					var result = cmd.Execute();
				}

				using (var cmd = client.CreateCommand($"mkdir -p {directoryName}"))
				{
					var result = cmd.Execute();
				}

				using (var scpClient = new Renci.SshNet.ScpClient(connectionInfo))
				{
					scpClient.Connect();

					using (var stream = ISI.Extensions.PfSense.T4Resources.Content.Getpfsense_import_certificate_phpStream())
					{
						stream.Rewind();
						scpClient.Upload(stream, $"{directoryName}/{scriptFileName}");
					}

					using (var stream = new System.IO.MemoryStream(request.BundleCertificate))
					{
						stream.Rewind();
						scpClient.Upload(stream, $"{directoryName}/{bundleCertificateFileName}");
					}

					using (var stream = new System.IO.MemoryStream(request.KeyCertificate))
					{
						stream.Rewind();
						scpClient.Upload(stream, $"{directoryName}/{keyCertificateFileName}");
					}

					scpClient.Disconnect();
				}

				var command = new StringBuilder();
				command.Append("php");
				command.Append($" {directoryName}/{scriptFileName}");
				command.Append($" {directoryName}/{bundleCertificateFileName}");
				command.Append($" {directoryName}/{keyCertificateFileName}");
				command.Append($" {request.CertificateName}");

				using (var cmd = client.CreateCommand(command.ToString()))
				{
					var result = cmd.Execute();
				}

				using (var cmd = client.CreateCommand($"rm -r {directoryName}"))
				{
					var result = cmd.Execute();
				}
			}

			return response;
		}
	}
}