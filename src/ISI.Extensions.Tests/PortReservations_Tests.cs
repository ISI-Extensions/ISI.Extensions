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
using NUnit.Framework;

namespace ISI.Extensions.Tests
{
	[TestFixture]
	public class PortReservations_Tests
	{
		[Test]
		public void ReadLaunchSettings_Test()
		{
			var fullName = @"F:\ISI\Internal Projects\ISI.Telephony.ServiceApplication\src\ISI.Telephony.ServiceApplication\Properties\launchSettings.json";

			var schemaPortReservations = new Dictionary<string, (string Schema, int Port)>(StringComparer.InvariantCultureIgnoreCase);

			var content = System.IO.File.ReadAllText(fullName);

			var launchSettingsJsonNode = System.Text.Json.Nodes.JsonNode.Parse(content);

			var profilesJsonNode = launchSettingsJsonNode["profiles"];

			if (profilesJsonNode is System.Text.Json.Nodes.JsonObject profilesJsonObject)
			{
				foreach (var launchSettingJsonNode in profilesJsonObject)
				{
					Console.WriteLine(launchSettingJsonNode.Key);

					if (launchSettingJsonNode.Value is System.Text.Json.Nodes.JsonObject launchSettingJsonObject)
					{
						foreach (var launchSetting in launchSettingJsonObject)
						{
							Console.WriteLine($"\t{launchSetting.Key}");

							if (string.Equals(launchSetting.Key, "applicationUrl", StringComparison.InvariantCultureIgnoreCase))
							{
								var uri = new UriBuilder(launchSetting.Value.ToString());

								Console.WriteLine($"\t\t{uri.Port}");

								schemaPortReservations.Add(launchSettingJsonNode.Key, (Schema: uri.Scheme, Port: uri.Port));
							}
						}
					}
				}
			}

			foreach (var portReservation in schemaPortReservations)
			{
				profilesJsonNode[portReservation.Key]["applicationUrl"] = $"{portReservation.Value.Schema}://localhost:{portReservation.Value.Port + 1}";
			}

			Console.WriteLine(launchSettingsJsonNode.ToJsonString(new System.Text.Json.JsonSerializerOptions()
			{
				WriteIndented = true,
			}));
		}

		[Test]
		public void CheckProjectPortReservations_Test()
		{
			var fullName = @"F:\ISI\Internal Projects\ISI.Extensions\src\ISI.Extensions.AspNetCore.Tests\ISI.Extensions.AspNetCore.Tests.csproj";

			var projectName = System.IO.Path.GetFileNameWithoutExtension(fullName);

			var launchSettingsJsonFullName = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(fullName), "Properties", "launchSettings.json");

			if (System.IO.File.Exists(launchSettingsJsonFullName))
			{
				var launchSettingsJsonNode = System.Text.Json.Nodes.JsonNode.Parse(System.IO.File.ReadAllText(launchSettingsJsonFullName));

				var profilesJsonNode = launchSettingsJsonNode["profiles"];

				if (profilesJsonNode != null)
				{
					var launchSettingJsonNode = profilesJsonNode[projectName];

					if (launchSettingJsonNode != null)
					{
						var applicationUrlJsonNode = launchSettingJsonNode["applicationUrl"];

						if (applicationUrlJsonNode != null)
						{
							var applicationUris = applicationUrlJsonNode.GetValue<string>().Split(new[] { ';' }).ToNullCheckedArray(applicationUrl => new UriBuilder(applicationUrl));

							var trySetPortReservationsResponse = ISI.Extensions.PortReservations.TrySetPortReservations(projectName, applicationUris.ToNullCheckedArray(applicationUri => applicationUri.Port));

							if (!trySetPortReservationsResponse.Success)
							{
								foreach (var applicationUri in applicationUris.Where(applicationUri => trySetPortReservationsResponse.UsedPorts.Contains(applicationUri.Port)))
								{
									var port = ISI.Extensions.PortReservations.GetNewPortReservation(projectName);

									applicationUri.Port = port;
								}

								launchSettingJsonNode["applicationUrl"] = string.Join(";", applicationUris.Select(applicationUri => applicationUri.Uri.ToString()));

								System.IO.File.WriteAllText(launchSettingsJsonFullName, launchSettingsJsonNode.ToJsonString(new System.Text.Json.JsonSerializerOptions()
								{
									WriteIndented = true,
								}));
							}
						}
					}
				}
			}
		}
	}
}