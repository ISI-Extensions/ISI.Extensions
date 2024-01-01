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
 
using ISI.Extensions.Extensions;
using ISI.Extensions.Ngrok.Extensions;
using System;
using System.Collections.Generic;
using DTOs = ISI.Extensions.Ngrok.DataTransferObjects.NGrokLocalServiceApi;

namespace ISI.Extensions.Ngrok
{
	public partial class NGrokLocalServiceApi
	{
		public DTOs.CreateReplacementUrlsResponse CreateReplacementUrls(DTOs.CreateReplacementUrlsRequest request)
		{
			var response = new DTOs.CreateReplacementUrlsResponse()
			{
				UsingNGrok = false,
			};

			if (Configuration.UseNGrok)
			{
				StartLocalService();

				response.UsingNGrok = true;

				var existingTunnelLookupByPort = new Dictionary<int, Tunnel>();
				var existingTunnelLookupByExternalUrl = new Dictionary<string, Tunnel>(StringComparer.InvariantCultureIgnoreCase);

				void RefreshTunnels()
				{
					var existingTunnels = NGrokClientApi.GetTunnels().Tunnels ?? Array.Empty<Tunnel>();

					foreach (var existingTunnel in existingTunnels)
					{
						var localUri = new UriBuilder(existingTunnel.Configuration.LocalUrl);
						var externalUri = new UriBuilder(existingTunnel.ExternalUrl);

						if (!existingTunnelLookupByPort.ContainsKey(localUri.Port))
						{
							existingTunnelLookupByPort.Add(localUri.Port, existingTunnel);
						}

						if (!existingTunnelLookupByExternalUrl.ContainsKey(externalUri.Uri.ToString()))
						{
							existingTunnelLookupByExternalUrl.Add(externalUri.Uri.ToString(), existingTunnel);
						}
					}
				}

				RefreshTunnels();

				var tunnels = new List<DTOs.Tunnel>();

				foreach (var localUrl in request.LocalUrls)
				{
					var localUri = new UriBuilder(localUrl);

					if (existingTunnelLookupByPort.TryGetValue(localUri.Port, out var existingTunnel))
					{
						var externalUri = new UriBuilder(existingTunnel.ExternalUrl);

						if (string.Equals(localUri.Scheme, externalUri.Scheme, StringComparison.InvariantCultureIgnoreCase))
						{
							tunnels.Add(new()
							{
								TunnelName = existingTunnel.TunnelName,
								LocalUrl = localUrl,
								ExternalUrl = externalUri.ToString(),
								NewTunnel = false,
							});
						}
						else
						{
							throw new(string.Format("Existing Tunnel for port: {0} isn't of the correct scheme", localUri.Port));
						}
					}
					else
					{
						var ngrokRequest = new ISI.Extensions.Ngrok.DataTransferObjects.NGrokClientApi.StartTunnelRequest()
						{
							LocalAddress = (string.Equals(localUri.Scheme, "https") ? string.Format("https://localhost:{0}", localUri.Port) : string.Format("{0}", localUri.Port)),
						};

						var tunnelConfiguration = Configuration.Tunnels.NullCheckedFirstOrDefault(tc =>
						{
							var tunnelConfigurationLocalUri = new UriBuilder(tc.LocalUrl);

							if (string.Equals(tunnelConfigurationLocalUri.Uri.ToString(), localUri.Uri.ToString(), StringComparison.InvariantCultureIgnoreCase))
							{
								return true;
							}

							if (string.Equals(tunnelConfigurationLocalUri.Host, "localhost", StringComparison.InvariantCultureIgnoreCase))
							{
								tunnelConfigurationLocalUri.Host = "127.0.0.1";
							}

							return string.Equals(tunnelConfigurationLocalUri.Uri.ToString(), localUri.Uri.ToString(), StringComparison.InvariantCultureIgnoreCase);
						});

						if (!string.IsNullOrWhiteSpace(tunnelConfiguration?.ExternalUrl))
						{
							var externalUri = new UriBuilder(tunnelConfiguration.ExternalUrl);

							if (string.Equals(externalUri.Scheme, "http"))
							{
								ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
								ngrokRequest.Subdomain = tunnelConfiguration.Subdomain;
								ngrokRequest.HostHeader = tunnelConfiguration.HostHeader;
								ngrokRequest.UseTls = UseTls.HttpOnly;
							}
							else if (string.Equals(externalUri.Scheme, "https"))
							{
								ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
								ngrokRequest.Subdomain = tunnelConfiguration.Subdomain;
								ngrokRequest.HostHeader = tunnelConfiguration.HostHeader;
								ngrokRequest.UseTls = UseTls.HttpsOnly;
							}
							else if (string.Equals(externalUri.Scheme, "tcp"))
							{
								if (existingTunnelLookupByExternalUrl.TryGetValue(externalUri.Uri.ToString(), out existingTunnel))
								{
									tunnels.Add(new()
									{
										TunnelName = existingTunnel.TunnelName,
										LocalUrl = localUrl,
										ExternalUrl = externalUri.ToString(),
										NewTunnel = false,
									});
									ngrokRequest = null;
								}
								else
								{
									ngrokRequest.TunnelProtocol = TunnelProtocol.Tcp;
									ngrokRequest.RemoteAddress = string.Format("{0}:{1}", externalUri.Host, externalUri.Port);
									ngrokRequest.Inspect = false;
								}
							}
							else
							{
								throw new ArgumentOutOfRangeException(string.Format("Unknown ExternalUrl Scheme: \"{0}\"", externalUri.Scheme));
							}
						}
						else if (!string.IsNullOrWhiteSpace(tunnelConfiguration?.Subdomain))
						{
							ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
							if (string.Equals(localUri.Scheme, "http"))
							{
								ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
								ngrokRequest.UseTls = UseTls.HttpOnly;
								ngrokRequest.Subdomain = tunnelConfiguration.Subdomain;
							}
							else if (string.Equals(localUri.Scheme, "https"))
							{
								ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
								ngrokRequest.UseTls = UseTls.HttpsOnly;
								ngrokRequest.Subdomain = tunnelConfiguration.Subdomain;
							}
							else
							{
								throw new ArgumentOutOfRangeException(string.Format("Unknown LocalUrl Scheme: \"{0}\"", localUri.Scheme));
							}
						}
						else if (!string.IsNullOrWhiteSpace(tunnelConfiguration?.HostHeader))
						{
							ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
							if (string.Equals(localUri.Scheme, "http"))
							{
								ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
								ngrokRequest.UseTls = UseTls.HttpOnly;
								ngrokRequest.HostHeader = tunnelConfiguration.HostHeader;
							}
							else if (string.Equals(localUri.Scheme, "https"))
							{
								ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
								ngrokRequest.UseTls = UseTls.HttpsOnly;
								ngrokRequest.HostHeader = tunnelConfiguration.HostHeader;
							}
							else
							{
								throw new ArgumentOutOfRangeException(string.Format("Unknown LocalUrl Scheme: \"{0}\"", localUri.Scheme));
							}
						}
						else
						{
							ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
							if (string.Equals(localUri.Scheme, "http"))
							{
								ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
								ngrokRequest.UseTls = UseTls.HttpOnly;
							}
							else if (string.Equals(localUri.Scheme, "https"))
							{
								ngrokRequest.TunnelProtocol = TunnelProtocol.Http;
								ngrokRequest.UseTls = UseTls.HttpsOnly;
							}
							else
							{
								throw new ArgumentOutOfRangeException(string.Format("Unknown LocalUrl Scheme: \"{0}\"", localUri.Scheme));
							}
						}

						if (ngrokRequest != null)
						{
							var tunnel = NGrokClientApi.StartTunnel(ngrokRequest).Tunnel;

							if (tunnel != null)
							{
								tunnels.Add(new()
								{
									TunnelName = tunnel.TunnelName,
									LocalUrl = localUrl,
									ExternalUrl = tunnel.ExternalUrl,
									NewTunnel = true,
								});
							}

							System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

							RefreshTunnels();
						}
					}
				}

				response.Tunnels = tunnels.ToArray();
			}

			return response;
		}
	}
}
