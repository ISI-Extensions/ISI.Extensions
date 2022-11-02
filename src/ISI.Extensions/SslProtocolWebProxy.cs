#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	//https://stackoverflow.com/questions/3791629/set-the-securityprotocol-ssl3-or-tls-on-the-net-httpwebrequest-per-request
	public class SslProtocolWebProxy : IDisposable
	{
		private string Host { get; }
		private int Port { get; }
		private System.Net.Sockets.TcpListener Listener { get; }
		private System.Security.Authentication.SslProtocols SslProtocols { get; }
		private System.Security.Cryptography.X509Certificates.X509CertificateCollection ClientCertificates { get; }
		private bool _disposed = false;

		public System.Net.WebProxy Proxy { get; }
		public Uri Uri { get; }

		public SslProtocolWebProxy(Uri uri, System.Security.Authentication.SslProtocols protocols, System.Security.Cryptography.X509Certificates.X509CertificateCollection clientCertificates)
		{
			var uriBuilder = new UriBuilder(uri);
			uriBuilder.Scheme = Uri.UriSchemeHttp;

			Host = uriBuilder.Host;
			Port = uriBuilder.Port;
			SslProtocols = protocols;
			ClientCertificates = clientCertificates ?? new System.Security.Cryptography.X509Certificates.X509CertificateCollection();

			Listener = new(System.Net.IPAddress.Loopback, 0);
			Listener.Start();
			Listener.BeginAcceptTcpClient(OnAcceptTcpClient, null);

			Proxy = new("localhost", ((System.Net.IPEndPoint)(Listener.LocalEndpoint)).Port);

			uriBuilder.Port = 80;
			Uri = uriBuilder.Uri;
		}

		private class StreamBuffer
		{
			public System.Net.Sockets.TcpClient SendingTcpClient { get; set; }
			public System.Net.Sockets.TcpClient ReceivingTcpClient { get; set; }
			public System.IO.Stream SendingStream { get; set; }
			public System.IO.Stream ReceivingStream { get; set; }
			public byte[] Buffer { get; set; }
			public StreamBuffer ReverseStreamBuffer { get; set; }
		}

		private void OnAcceptTcpClient(IAsyncResult asyncResult)
		{
			if (_disposed)
			{
				return;
			}

			var requestorTcpClient = Listener.EndAcceptTcpClient(asyncResult);
			System.Net.Sockets.TcpClient responderTcpClient = null;

			try
			{
				Listener.BeginAcceptTcpClient(OnAcceptTcpClient, null);
				var requestorStream = requestorTcpClient.GetStream();

				responderTcpClient = new(Host, Port);
				var responderStream = responderTcpClient.GetStream() as System.IO.Stream;

				if ((SslProtocols != System.Security.Authentication.SslProtocols.None) || (ClientCertificates != null))
				{
					var sslStream = new System.Net.Security.SslStream(responderStream, true);
					sslStream.AuthenticateAsClient(Host, ClientCertificates, SslProtocols, false);
					responderStream = sslStream;
				}

				var requestorStreamBuffer = new StreamBuffer()
				{
					SendingTcpClient = requestorTcpClient,
					SendingStream = requestorStream,
					ReceivingTcpClient = responderTcpClient,
					ReceivingStream = responderStream,
					Buffer = new byte[requestorTcpClient.ReceiveBufferSize]
				};

				var responderStreamBuffer = new StreamBuffer()
				{
					SendingTcpClient = responderTcpClient,
					SendingStream = responderStream,
					ReceivingTcpClient = requestorTcpClient,
					ReceivingStream = requestorStream,
					Buffer = new byte[responderTcpClient.ReceiveBufferSize]
				};

				requestorStreamBuffer.ReverseStreamBuffer = responderStreamBuffer;
				responderStreamBuffer.ReverseStreamBuffer = requestorStreamBuffer;

				requestorStream.BeginRead(requestorStreamBuffer.Buffer, 0, requestorStreamBuffer.Buffer.Length, OnReceive, requestorStreamBuffer);
				responderStream.BeginRead(responderStreamBuffer.Buffer, 0, responderStreamBuffer.Buffer.Length, OnReceive, responderStreamBuffer);
			}
			catch
			{
				requestorTcpClient.Close();
				responderTcpClient?.Close();

				throw;
			}
		}

		private void Close(StreamBuffer streamBuffer)
		{
			var reverseStreamBuffer = streamBuffer.ReverseStreamBuffer;
			if (reverseStreamBuffer != null)
			{
				reverseStreamBuffer.ReverseStreamBuffer = streamBuffer.ReverseStreamBuffer = null;
				streamBuffer.SendingStream.Dispose();
				streamBuffer.ReceivingStream.Dispose();
			}
		}

		private void OnReceive(IAsyncResult asyncResult)
		{
			var asyncStateStreamBuffer = asyncResult.AsyncState as StreamBuffer;

			try
			{
				if (!((asyncStateStreamBuffer?.ReverseStreamBuffer != null) && asyncStateStreamBuffer.SendingTcpClient.Connected && asyncStateStreamBuffer.SendingStream.CanRead && !_disposed))
				{
					Close(asyncStateStreamBuffer);
					return;
				}

				var byteCount = asyncStateStreamBuffer.SendingStream.EndRead(asyncResult);

				if (!(byteCount > 0 && asyncStateStreamBuffer.ReceivingTcpClient.Connected && asyncStateStreamBuffer.ReceivingStream.CanWrite))
				{
					Close(asyncStateStreamBuffer);
					return;
				}

				asyncStateStreamBuffer.ReceivingStream.Write(asyncStateStreamBuffer.Buffer, 0, byteCount);

				if (!(asyncStateStreamBuffer.SendingTcpClient.Connected && asyncStateStreamBuffer.ReceivingTcpClient.Connected && asyncStateStreamBuffer.SendingStream.CanRead && asyncStateStreamBuffer.ReceivingStream.CanWrite))
				{
					Close(asyncStateStreamBuffer);
					return;
				}

				asyncStateStreamBuffer.SendingStream.BeginRead(asyncStateStreamBuffer.Buffer, 0, asyncStateStreamBuffer.Buffer.Length, OnReceive, asyncStateStreamBuffer);
			}
			catch
			{
				Close(asyncStateStreamBuffer);

				throw;
			}
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;
				Listener.Stop();
			}
		}
	}
}
