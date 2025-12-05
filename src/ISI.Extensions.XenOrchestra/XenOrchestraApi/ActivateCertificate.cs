#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using ISI.Extensions.SshNet.Extensions;
using DTOs = ISI.Extensions.XenOrchestra.DataTransferObjects.XenOrchestraApi;

namespace ISI.Extensions.XenOrchestra
{
	public partial class XenOrchestraApi
	{
		public DTOs.ActivateCertificateResponse ActivateCertificate(DTOs.ActivateCertificateRequest request)
		{
			var response = new DTOs.ActivateCertificateResponse();

			var connectionInfo = GetConnectionInfo(request);

			var logEntries = new List<DTOs.LogEntry>();
			void addLog(string description)
			{
				logEntries.Add(new DTOs.LogEntry()
				{
					DateTimeStampUtc = DateTimeStamper.CurrentDateTimeUtc(),
					Description = description,
				});
			}

			var bundleCertificateFullName = (string)null;
			var keyCertificateFullName = (string)null;

			using (var client = new Renci.SshNet.SshClient(connectionInfo.ConnectionInfo))
			{
				client.Connect();

				using (var scpClient = new Renci.SshNet.ScpClient(connectionInfo.ConnectionInfo))
				{
					scpClient.Connect();

					using (var stream = new System.IO.MemoryStream())
					{
						scpClient.Download("/etc/xo-server/config.toml", stream);
						stream.Rewind();

						var config = stream.TextReadToEnd();
						foreach (var configLine in config.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
						{
							var pieces = configLine.Split([' '], 2);
							if (pieces.NullCheckedCount() > 1)
							{
								string getValue()
								{
									var valuePieces = pieces[1].Trim().Trim('=').Trim().Split(['\''], 3);

									return (valuePieces.Length >= 2 ? valuePieces[1] : null);
								}

								if (string.Equals(pieces[0], "cert", StringComparison.InvariantCultureIgnoreCase))
								{
									bundleCertificateFullName = getValue();
								}

								if (string.Equals(pieces[0], "key", StringComparison.InvariantCultureIgnoreCase))
								{
									keyCertificateFullName = getValue();
								}
							}
						}
					}

					addLog(string.IsNullOrWhiteSpace(bundleCertificateFullName) ? "bundleCertificateFullName not found" : $"bundleCertificateFullName = {bundleCertificateFullName}");
					addLog(string.IsNullOrWhiteSpace(keyCertificateFullName) ? "keyCertificateFullName not found" : $"keyCertificateFullName = {keyCertificateFullName}");

					if (!string.IsNullOrWhiteSpace(bundleCertificateFullName) && !string.IsNullOrWhiteSpace(keyCertificateFullName))
					{
						using (var shellStream = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024))
						{
							shellStream.Sudo(connectionInfo.Password);

							var nowUtc = DateTimeStamper.CurrentDateTimeUtc();

							using (var stream = new System.IO.MemoryStream(request.BundleCertificate))
							{
								stream.Rewind();
								var oldBundleCertificateFullName = $"{bundleCertificateFullName}-{nowUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeSortable)}";
								shellStream.SendCommandToShellStream($"mv {bundleCertificateFullName} {oldBundleCertificateFullName}");
								addLog($"renamed old bundleCertificate to: {oldBundleCertificateFullName}");
								shellStream.SendCommandToShellStream($"cat > {bundleCertificateFullName} <<EOF\n{stream.TextReadToEnd()}\nEOF\n");
								addLog($"uploaded: {bundleCertificateFullName}");
							}

							using (var stream = new System.IO.MemoryStream(request.KeyCertificate))
							{
								stream.Rewind();
								var oldKeyCertificateFullName = $"{keyCertificateFullName}-{nowUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeSortable)}";
								shellStream.SendCommandToShellStream($"mv {keyCertificateFullName} {oldKeyCertificateFullName}");
								addLog($"renamed old keyCertificate to: {oldKeyCertificateFullName}");
								shellStream.SendCommandToShellStream($"cat > {keyCertificateFullName} <<EOF\n{stream.TextReadToEnd()}\nEOF\n");
								addLog($"uploaded: {keyCertificateFullName}");
							}

							addLog($"restart xoa: {shellStream.SendCommandToShellStream("systemctl restart xo-server")}");
						}
					}

					scpClient.Disconnect();
				}
			}

			response.LogEntries = logEntries.ToArray();

			return response;
		}
	}
}