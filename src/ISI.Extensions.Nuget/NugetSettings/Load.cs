﻿#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
using System.Text;

namespace ISI.Extensions.Nuget
{
	public partial class NugetSettings
	{
		public ISI.Extensions.Nuget.SerializableModels.NugetSettings Load()
		{
			if (!System.IO.File.Exists(SettingsFileName))
			{
				return new();
			}

			using (new ISI.Extensions.Locks.FileLock(SettingsFileName))
			{
				using (var stream = System.IO.File.OpenRead(SettingsFileName))
				{
					var settings = Serialization.Deserialize<ISI.Extensions.Nuget.SerializableModels.NugetSettings>(stream, ISI.Extensions.Serialization.SerializationFormat.Json);

					settings.UpdateNugetPackages ??= new()
					{
						NugetSettingsNugetPackageKeys = new[]
						{
							new ISI.Extensions.Nuget.SerializableModels.NugetSettingsNugetPackageKey()
							{
								PackageId = "StackifyLib",
								PackageVersion = "2.2.6",
							},
							new ISI.Extensions.Nuget.SerializableModels.NugetSettingsNugetPackageKey()
							{
								PackageId = "Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv",
								PackageVersion = "2.2.0",
							},
							new ISI.Extensions.Nuget.SerializableModels.NugetSettingsNugetPackageKey()
							{
								PackageId = "Microsoft.ClearScript",
								PackageVersion = "6.0.2",
							},
						},
						IgnorePackageIds = new[]
						{
							"ISI.CMS.T4CMS",
							"ISI.CMS.T4CMS.MSSQL",
							"ISI.CMS.T4CMS.FileSystem",
							"ISI.CMS.T4CMS.SqlServer",
							"ISI.Extensions.T4LocalContent",
							"ISI.Extensions.T4LocalContent.Embedded",
							"ISI.Extensions.T4LocalContent.RazorEngine",
							"ISI.Extensions.T4LocalContent.Resources",
							"ISI.Extensions.T4LocalContent.VirtualFiles",
							"ISI.Extensions.T4LocalContent.Web",
							"ISI.Extensions.T4LocalContent.WebPortableArea",
							"ISI.Extensions.T4LocalContent",
							"ISI.Extensions.T4LocalContent.Embedded",
							"ISI.Extensions.T4LocalContent.Resources",
							"ISI.Extensions.T4LocalContent.VirtualFiles",
							"ISI.Extensions.T4LocalContent.Web",
							"ISI.Extensions.T4LocalContent.WebPortableArea",
							"ISI.Libraries.T4LocalContent",
							"ISI.Libraries.T4LocalContent.Embedded",
							"ISI.Libraries.T4LocalContent.RazorEngine",
							"ISI.Libraries.T4LocalContent.Resources",
							"ISI.Libraries.T4LocalContent.VirtualFiles",
							"ISI.Libraries.T4LocalContent.Web",
							"ISI.Libraries.T4LocalContent.WebPortableArea",
							"ISI.Libraries.T4LocalContent",
							"ISI.Libraries.T4LocalContent.Embedded",
							"ISI.Libraries.T4LocalContent.Resources",
							"ISI.Libraries.T4LocalContent.VirtualFiles",
							"ISI.Libraries.T4LocalContent.Web",
							"ISI.Libraries.T4LocalContent.WebPortableArea",
							"Microsoft.ClearScript",
							"jQuery",
							"AccumailGoldConnections.NETToolkit",
							"nsoftware.InPay",
							"nsoftware.InPtech",
							"nsoftware.InShip",
							"nsoftware.IPWorksSSH",
						},
					};

					return settings;
				}
			}
		}
	}
}