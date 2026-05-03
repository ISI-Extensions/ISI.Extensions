using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using ISI.Extensions.JsonSerialization.Extensions;
using DTOs = ISI.Extensions.Nginx.DataTransferObjects.NginxApi;
using SerializableDTOs = ISI.Extensions.Nginx.SerializableModels;

namespace ISI.Extensions.Nginx
{
	public partial class NginxApi
	{
		public DTOs.ParseNginxConfigResponse ParseNginxConfig(DTOs.ParseNginxConfigRequest request)
		{
			var response = new DTOs.ParseNginxConfigResponse();

			response.ParsedNginxConfig = new()
			{
				FileName = request.FileName,
				Content = request.Content,
			};

			var servers = new List<ParsedNginxConfigServer>();

			var serverRegex = new System.Text.RegularExpressions.Regex(@"(?:\s*)(?<server>server)(?:\s+)(?:\{?)(?:\s*)");
			var listenRegex = new System.Text.RegularExpressions.Regex(@"(?:\s*listen\s+(?<port>\d+)(?:\s*(?<scheme>[a-zA-Z]+))?)");
			var serverNameRegex = new System.Text.RegularExpressions.Regex(@"(?:\s*)(?<server_name>server_name)(?:\s+)(?<host>[a-zA-Z0-9\-\.]+)(?:\s*)");
			var locationRegex = new System.Text.RegularExpressions.Regex(@"(?:\s*)(?:location)(?:\s+)(?<directory>[^ ]+)(?:\s*)");
			var proxyPassRegex = new System.Text.RegularExpressions.Regex(@"(?:\s*)(?:proxy_pass)(?:\s+)(?<url>[^ ;]+)(?:\s*)");
			var nginxManagerAgentNginxInstanceUuidRegex = new System.Text.RegularExpressions.Regex(@"(?:\s*)(?:#NginxManagerAgentNginxInstanceUuid\:)(?:\s+)(?<nginxManagerAgentNginxInstanceUuid>[a-zA-Z0-9\-]+)(?:\s*)");
			var dnsAccountUuidsRegex = new System.Text.RegularExpressions.Regex(@"(?:\s*)(?:#DnsAccountUuid)(?:s?)(?:\:)(?:\s+)(?<dnsAccountUuids>[a-zA-Z0-9\-, -[=\>]]+)(?:\s+)(?<recordType>[a-zA-Z]+)(?:\s+)(?:=\>)(?:\s+)(?<data>[a-zA-Z0-9\-\.]+)(?:\s*)");

			{
				var server = (ParsedNginxConfigServer)null;
				foreach (var line in request.Content.Replace("\r\n", "\n").Split('\n'))
				{
					if (serverRegex.Match(line).Success && !serverNameRegex.Match(line).Success)
					{
						server = new ParsedNginxConfigServer();
						servers.Add(server);
					}

					var nginxManagerAgentNginxInstanceUuidMatch = nginxManagerAgentNginxInstanceUuidRegex.Match(line);

					if (nginxManagerAgentNginxInstanceUuidMatch.Success)
					{
						response.ParsedNginxConfig.NginxManagerAgentNginxInstanceUuid = (nginxManagerAgentNginxInstanceUuidMatch.Groups["nginxManagerAgentNginxInstanceUuid"]?.Value ?? string.Empty).ToGuid();
					}

					server?.Content = $"{server.Content}\n{line}";
				}

				response.ParsedNginxConfig.Servers = servers.ToArray();
			}

			foreach (var server in servers)
			{
				server.Content = server.Content.Trim([' ', '\n']);

				var locations = new List<ParsedNginxConfigServerLocation>();

				var location = (ParsedNginxConfigServerLocation)null;
				var scheme = (string)null;
				var dnsAccounts = new List<ParsedNginxConfigServerDnsAccount>();
				foreach (var line in server.Content.Split('\n'))
				{
					if (location == null)
					{
						var listenMatch = listenRegex.Match(line);

						if (listenMatch.Success)
						{
							server.Ssl = (string.Equals(listenMatch.Groups["scheme"]?.Value ?? string.Empty, "ssl", StringComparison.InvariantCultureIgnoreCase));
							scheme = (string.Equals(listenMatch.Groups["scheme"]?.Value ?? string.Empty, "ssl", StringComparison.InvariantCultureIgnoreCase) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp);

							server.Port = (listenMatch.Groups["port"]?.Value ?? string.Empty).ToIntNullable() ?? (string.Equals(scheme, Uri.UriSchemeHttps, StringComparison.InvariantCultureIgnoreCase) ? 443 : 80);
						}

						if (string.IsNullOrWhiteSpace(scheme) && (line.IndexOf("ssl-cert.inc", StringComparison.InvariantCultureIgnoreCase) >= 0))
						{
							server.Port = 443;
						}

						var serverNameMatch = serverNameRegex.Match(line);

						if (serverNameMatch.Success)
						{
							server.Host = serverNameMatch.Groups["host"]?.Value ?? string.Empty;
						}

						var dnsAccountUuidsMatch = dnsAccountUuidsRegex.Match(line);

						if (dnsAccountUuidsMatch.Success)
						{
							var dnsAccountUuids = new HashSet<Guid>();
							foreach (var dnsAccountUuid in (dnsAccountUuidsMatch.Groups["dnsAccountUuids"]?.Value ?? string.Empty).Split(',').Select(dnsAccountUuid => dnsAccountUuid.ToGuidNullable()).Where(dnsAccountUuid => dnsAccountUuid.HasValue))
							{
								dnsAccountUuids.Add(dnsAccountUuid.Value);
							}

							dnsAccounts.Add(new ()
							{
								DnsAccountUuids = dnsAccountUuids.ToArray(),
								DnsRecordType = ISI.Extensions.Enum<ISI.Extensions.Dns.RecordType?>.Parse(dnsAccountUuidsMatch.Groups["recordType"]?.Value ?? string.Empty),
								DnsRecordData = dnsAccountUuidsMatch.Groups["data"]?.Value ?? string.Empty,
							});
						}
					}

					var locationMatch = locationRegex.Match(line);

					if (locationMatch.Success)
					{
						location = new ParsedNginxConfigServerLocation();
						locations.Add(location);

						location.Location = locationMatch.Groups["directory"]?.Value ?? string.Empty;
					}

					if (location != null)
					{
						var proxyPassMatch = proxyPassRegex.Match(line);

						if (proxyPassMatch.Success)
						{
							location.ProxyPassUrl = proxyPassMatch.Groups["url"]?.Value ?? string.Empty;
						}

						location?.Content = $"{location.Content}\n{line}";
					}
				}

				foreach (var serverLocation in locations)
				{
					serverLocation.Content = serverLocation.Content.Trim([' ', '\n']);
				}

				server.DnsAccounts = dnsAccounts.ToArray();
				server.Locations = locations.ToArray();
			}

			return response;
		}
	}
}