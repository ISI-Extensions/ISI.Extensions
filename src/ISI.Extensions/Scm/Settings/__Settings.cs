#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Scm
{
	public partial class Settings
	{
		public delegate bool GetSettings_TryGetValue(string key, out string value);

		public class Key
		{
			public const string ActiveDirectoryDomain = nameof(ActiveDirectoryDomain);
			public const string ActiveDirectoryUserName = nameof(ActiveDirectoryUserName);
			public const string ActiveDirectoryPassword = nameof(ActiveDirectoryPassword);

			public const string ScmWebServiceUrl = nameof(ScmWebServiceUrl);

			public const string CodeSigningTimeStampUrl = nameof(CodeSigningTimeStampUrl);
			public const string CodeSigningCertificateFileName = nameof(CodeSigningCertificateFileName);
			public const string CodeSigningCertificatePassword = nameof(CodeSigningCertificatePassword);

			public const string NugetRepositoryName = nameof(NugetRepositoryName);
			public const string NugetRepositoryUrl = nameof(NugetRepositoryUrl);
			public const string NugetApiKey = nameof(NugetApiKey);
			public const string NugetCacheDirectory = nameof(NugetCacheDirectory);

			public const string JenkinsServiceUrl = nameof(JenkinsServiceUrl);
			public const string JenkinsServicePassword = nameof(JenkinsServicePassword);
			public const string JenkinsUrl = nameof(JenkinsUrl);
			public const string JenkinsUserName = nameof(JenkinsUserName);
			public const string JenkinsApiToken = nameof(JenkinsApiToken);

			public const string FileStoreUrl = nameof(FileStoreUrl);
			public const string FileStoreUserName = nameof(FileStoreUserName);
			public const string FileStorePassword = nameof(FileStorePassword);

			internal static IEnumerable<string> Keys =>
				new[]
				{
					ActiveDirectoryDomain,
					ActiveDirectoryUserName,
					ActiveDirectoryPassword,
					ScmWebServiceUrl,
					CodeSigningTimeStampUrl,
					CodeSigningCertificateFileName,
					CodeSigningCertificatePassword,
					NugetRepositoryName,
					NugetRepositoryUrl,
					NugetApiKey,
					NugetCacheDirectory,
					JenkinsServiceUrl,
					JenkinsUrl,
					JenkinsUserName,
					JenkinsApiToken,
					FileStoreUrl,
					FileStoreUserName,
					FileStorePassword,
				};
		}

		protected IDictionary<string, string> Values { get; }
		protected GetSettings_TryGetValue OverrideTryGetValue { get; }

		public Settings(IDictionary<string, string> values, GetSettings_TryGetValue overrideTryGetValue)
		{
			Values = values;
			OverrideTryGetValue = overrideTryGetValue ?? ((string key, out string value) =>
			{
				value = null;
				return false;
			});
		}

		private SettingsActiveDirectory _activeDirectory = null;
		public SettingsActiveDirectory ActiveDirectory => _activeDirectory ??= new SettingsActiveDirectory(this);

		private SettingsScm _scm = null;
		public SettingsScm Scm => _scm ??= new SettingsScm(this);

		private SettingsCodeSigning _codeSigning = null;
		public SettingsCodeSigning CodeSigning => _codeSigning ??= new SettingsCodeSigning(this);

		private SettingsNuget _nuget = null;
		public SettingsNuget Nuget => _nuget ??= new SettingsNuget(this);

		private SettingsJenkins _jenkins = null;
		public SettingsJenkins Jenkins => _jenkins ??= new SettingsJenkins(this);

		private SettingsFileStore _fileStore = null;
		public SettingsFileStore FileStore => _fileStore ??= new SettingsFileStore(this);


		public string GetValue(string key, string defaultValue = null)
		{
			{
				if (OverrideTryGetValue(key, out var value))
				{
					return value;
				}
			}

			{
				if (Values.TryGetValue(key, out var value))
				{
					return value;
				}
			}

			return defaultValue;
		}

		public bool TryGetValue(string key, out string value)
		{
			{
				if (OverrideTryGetValue(key, out value))
				{
					return true;
				}
			}

			{
				if (Values.TryGetValue(key, out value))
				{
					return true;
				}
			}

			return false;
		}

		internal void SetValue(string key, string value)
		{
			Values.Remove(key);
			Values.Add(new KeyValuePair<string, string>(key, value));
		}

		public override string ToString() => string.Join("\r\n", Values.Select(keyValue => string.Format("{0} => {1}", keyValue.Key, keyValue.Value)));
	}
}
