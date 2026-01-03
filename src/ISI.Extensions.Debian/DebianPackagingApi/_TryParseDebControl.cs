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
using DTOs = ISI.Extensions.Debian.DataTransferObjects.DebianPackagingApi;

namespace ISI.Extensions.Debian
{
	public partial class DebianPackagingApi
	{
		private bool TryParseDebControl(string content, out DebControl debControl)
		{
			debControl = null;

			var keyValues = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

			{
				var key = string.Empty;
				var value = string.Empty;
				foreach (var line in content.Split(['\n', '\r']))
				{
					if (line.Length > 0)
					{
						if ((line[0] != ' ') && (line[0] != '\t'))
						{
							var lineParts = line.Split([':'], 2);
							key = lineParts[0];
							value = (lineParts.Length > 1) ? lineParts[1].Trim() : string.Empty;

							if (keyValues.ContainsKey(key))
							{
								keyValues[key] = $"{keyValues[key]}\n{value}";
							}
							else
							{
								keyValues.Add(key, value);
							}
						}
						else if (!value.Trim().StartsWith("#") && !string.IsNullOrWhiteSpace(key))
						{
							keyValues[key] = $"{keyValues[key]}\n{value.Trim()}";
						}
					}
				}
			}

			{
				if (keyValues.TryGetValue("Package", out var value))
				{
					debControl ??= new DebControl();

					debControl.Package = value;
				}
			}

			{
				if (keyValues.TryGetValue("Source", out var value))
				{
					debControl ??= new DebControl();

					debControl.Source = value;
				}
			}

			{
				if (keyValues.TryGetValue("Version", out var value))
				{
					if (Version.TryParse(value, out var version))
					{
						debControl ??= new DebControl();

						debControl.Version = version;
					}
				}
			}

			{
				if (keyValues.TryGetValue("Architecture", out var value))
				{
					debControl ??= new DebControl();

					debControl.Architecture = value;
				}
			}

			{
				if (keyValues.TryGetValue("Depends", out var values))
				{
					debControl ??= new DebControl();

					debControl.Depends = values.Split([',','\n','\r']).Select(value => value.Trim()).Where(value => !string.IsNullOrWhiteSpace(value)).ToArray();
				}
			}

			{
				if (keyValues.TryGetValue("Pre-Depends", out var values))
				{
					debControl ??= new DebControl();

					debControl.PreDepends = values.Split([',','\n','\r']).Select(value => value.Trim()).Where(value => !string.IsNullOrWhiteSpace(value)).ToArray();
				}
			}

			{
				if (keyValues.TryGetValue("Recommends", out var values))
				{
					debControl ??= new DebControl();

					debControl.Recommends = values.Split([',','\n','\r']).Select(value => value.Trim()).Where(value => !string.IsNullOrWhiteSpace(value)).ToArray();
				}
			}

			{
				if (keyValues.TryGetValue("Suggests", out var values))
				{
					debControl ??= new DebControl();

					debControl.Suggests = values.Split([',','\n','\r']).Select(value => value.Trim()).Where(value => !string.IsNullOrWhiteSpace(value)).ToArray();
				}
			}

			{
				if (keyValues.TryGetValue("Enhances", out var values))
				{
					debControl ??= new DebControl();

					debControl.Enhances = values.Split([',','\n','\r']).Select(value => value.Trim()).Where(value => !string.IsNullOrWhiteSpace(value)).ToArray();
				}
			}

			{
				if (keyValues.TryGetValue("Breaks ", out var values))
				{
					debControl ??= new DebControl();

					debControl.Breaks  = values.Split([',','\n','\r']).Select(value => value.Trim()).Where(value => !string.IsNullOrWhiteSpace(value)).ToArray();
				}
			}

			{
				if (keyValues.TryGetValue("Conflicts", out var values))
				{
					debControl ??= new DebControl();

					debControl.Conflicts = values.Split([',','\n','\r']).Select(value => value.Trim()).Where(value => !string.IsNullOrWhiteSpace(value)).ToArray();
				}
			}

			{
				if (keyValues.TryGetValue("Installed-Size", out var value))
				{
					debControl ??= new DebControl();

					debControl.InstalledSize = value.ToLongNullable();
				}
			}

			{
				if (keyValues.TryGetValue("Maintainer", out var value))
				{
					debControl ??= new DebControl();

					debControl.Maintainer = value;
				}
			}

			{
				if (keyValues.TryGetValue("Homepage", out var value))
				{
					debControl ??= new DebControl();

					debControl.Homepage = value;
				}
			}

			{
				if (keyValues.TryGetValue("Description", out var value))
				{
					debControl ??= new DebControl();

					debControl.Description = value;
				}
			}

			return (debControl != null);
		}
	}
}