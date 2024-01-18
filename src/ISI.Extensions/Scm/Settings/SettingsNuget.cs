﻿#region Copyright & License
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
using System.Text;

namespace ISI.Extensions.Scm
{
	public partial class Settings
	{
		public class SettingsNuget
		{
			protected Settings Settings { get; }

			public SettingsNuget(Settings settings)
			{
				Settings = settings;
			}

			public string ApiUrl
			{
				get => Settings.GetValue(Settings.Key.NugetApiUrl);
				set => Settings.SetValue(Settings.Key.NugetApiUrl, value);
			}

			public string RepositoryName
			{
				get => Settings.GetValue(Settings.Key.NugetRepositoryName);
				set => Settings.SetValue(Settings.Key.NugetRepositoryName, value);
			}

			public string RepositoryUrl
			{
				get => Settings.GetValue(Settings.Key.NugetRepositoryUrl);
				set => Settings.SetValue(Settings.Key.NugetRepositoryUrl, value);
			}

			public string ApiKey
			{
				get => Settings.GetValue(Settings.Key.NugetApiKey);
				set => Settings.SetValue(Settings.Key.NugetApiKey, value);
			}

			public string UserName
			{
				get => Settings.GetValue(Settings.Key.NugetUserName);
				set => Settings.SetValue(Settings.Key.NugetUserName, value);
			}

			public string Password
			{
				get => Settings.GetValue(Settings.Key.NugetPassword);
				set => Settings.SetValue(Settings.Key.NugetPassword, value);
			}

			public string NuSpecIconUrl
			{
				get => Settings.GetValue(Settings.Key.NugetNuSpecIconUrl);
				set => Settings.SetValue(Settings.Key.NugetNuSpecIconUrl, value);
			}

			public string NuSpecCopyright
			{
				get => Settings.GetValue(Settings.Key.NugetNuSpecCopyright);
				set => Settings.SetValue(Settings.Key.NugetNuSpecCopyright, value);
			}

			public string NuSpecAuthor
			{
				get => Settings.GetValue(Settings.Key.NugetNuSpecAuthor);
				set => Settings.SetValue(Settings.Key.NugetNuSpecAuthor, value);
			}

			public string NuSpecOwner
			{
				get => Settings.GetValue(Settings.Key.NugetNuSpecOwner);
				set => Settings.SetValue(Settings.Key.NugetNuSpecOwner, value);
			}
		}
	}
}