﻿#region Copyright & License
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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Scm
{
	public partial class Settings
	{
		public class SettingsCodeSigningToken
		{
			protected Settings Settings { get; }

			public SettingsCodeSigningToken(Settings settings)
			{
				Settings = settings;
			}

			public string CertificateFileName
			{
				get => Settings.GetValue(Settings.Key.CodeSigningCertificateTokenCertificateFileName);
				set => Settings.SetValue(Settings.Key.CodeSigningCertificateTokenCertificateFileName, value);
			}

			public string CryptographicProvider
			{
				get => Settings.GetValue(Settings.Key.CodeSigningCertificateTokenCryptographicProvider);
				set => Settings.SetValue(Settings.Key.CodeSigningCertificateTokenCryptographicProvider, value);
			}

			public string ContainerName
			{
				get => Settings.GetValue(Settings.Key.CodeSigningCertificateTokenContainerName);
				set => Settings.SetValue(Settings.Key.CodeSigningCertificateTokenContainerName, value);
			}

			public string Password
			{
				get => Settings.GetValue(Settings.Key.CodeSigningCertificateTokenPassword);
				set => Settings.SetValue(Settings.Key.CodeSigningCertificateTokenPassword, value);
			}

			public string RevocationPassword
			{
				get => Settings.GetValue(Settings.Key.CodeSigningCertificateTokenRevocationPassword);
				set => Settings.SetValue(Settings.Key.CodeSigningCertificateTokenRevocationPassword, value);
			}
		}
	}
}