#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Xcp.DataTransferObjects.XcpApi;

namespace ISI.Extensions.Xcp
{
	public partial class XcpApi
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

			var nowUtc = DateTimeStamper.CurrentDateTimeUtc();

			var certificateFullName = $"/etc/xensource/xapi-ssl-{nowUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeSortable)}.cert";
			var caBundleCertificateFullName = $"/etc/xensource/xapi-ssl-{nowUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeSortable)}.pem";
			var keyCertificateFullName = $"/etc/xensource/xapi-ssl-{nowUtc.Formatted(DateTimeExtensions.DateTimeFormat.DateTimeSortable)}.key";

			using (var client = new Renci.SshNet.SshClient(connectionInfo.ConnectionInfo))
			{
				client.Connect();

				using (var scpClient = new Renci.SshNet.ScpClient(connectionInfo.ConnectionInfo))
				{
					scpClient.Connect();

					using (var shellStream = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024))
					{
						//shellStream.Sudo(connectionInfo.Password);

						using (var stream = new System.IO.MemoryStream(request.KeyCertificate))
						{
							stream.Rewind();
							var command = $"cat > {keyCertificateFullName} <<EOF\n{stream.TextReadToEnd()}\nEOF\n";
							addLog(command);
							shellStream.SendCommandToShellStream(command);
							addLog($"uploaded: {keyCertificateFullName}");
						}

						using (var stream = new System.IO.MemoryStream(request.Certificate))
						{
							stream.Rewind();
							var command = $"cat > {certificateFullName} <<EOF\n{stream.TextReadToEnd()}\nEOF\n";
							addLog(command);
							shellStream.SendCommandToShellStream(command);
							addLog($"uploaded: {certificateFullName}");
						}

						if (request.CaBundleCertificate.NullCheckedAny())
						{
							using (var stream = new System.IO.MemoryStream(request.CaBundleCertificate))
							{
								stream.Rewind();
								var command = $"cat > {caBundleCertificateFullName} <<EOF\n{stream.TextReadToEnd()}\nEOF\n";
								addLog(command);
								shellStream.SendCommandToShellStream(command);
								addLog($"uploaded: {caBundleCertificateFullName}");
							}
						}

						if (request.CaBundleCertificate.NullCheckedAny())
						{
							shellStream.SendCommandToShellStream($"xe host-server-certificate-install certificate={certificateFullName} private-key={keyCertificateFullName} certificate-chain={caBundleCertificateFullName}");
						}
						else
						{
							shellStream.SendCommandToShellStream($"xe host-server-certificate-install certificate={certificateFullName} private-key={keyCertificateFullName}");
						}
						addLog("host-server-certificate-install");
					}

					scpClient.Disconnect();
				}
			}

			response.LogEntries = logEntries.ToArray();

			return response;
		}
	}
}