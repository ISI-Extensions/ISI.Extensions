#region Copyright & License
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
using System.Linq;
using System.Text;
using ISI.Extensions.Extensions;

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

			public const string CodeSigningDoCodeSigning = nameof(CodeSigningDoCodeSigning);
			public const string CodeSigningTimeStampUrl = nameof(CodeSigningTimeStampUrl);
			public const string CodeSigningTimeStampDigestAlgorithm = nameof(CodeSigningTimeStampDigestAlgorithm);
			public const string CodeSigningCertificateFingerprint = nameof(CodeSigningCertificateFingerprint);
			public const string CodeSigningCertificateFileName = nameof(CodeSigningCertificateFileName);
			public const string CodeSigningCertificatePassword = nameof(CodeSigningCertificatePassword);
			public const string CodeSigningCertificateTokenCertificateFileName = nameof(CodeSigningCertificateTokenCertificateFileName);
			public const string CodeSigningCertificateTokenCryptographicProvider = nameof(CodeSigningCertificateTokenCryptographicProvider);
			public const string CodeSigningCertificateTokenContainerName = nameof(CodeSigningCertificateTokenContainerName);
			public const string CodeSigningCertificateTokenPassword = nameof(CodeSigningCertificateTokenPassword);
			public const string CodeSigningCertificateTokenRevocationPassword = nameof(CodeSigningCertificateTokenRevocationPassword);
			public const string CodeSigningDigestAlgorithm = nameof(CodeSigningDigestAlgorithm);
			public const string CodeSigningRunAsync = nameof(CodeSigningRunAsync);
			public const string CodeSigningRemoteCodeSigningServiceApiUrl = nameof(CodeSigningRemoteCodeSigningServiceApiUrl);
			public const string CodeSigningRemoteCodeSigningServiceApiKey = nameof(CodeSigningRemoteCodeSigningServiceApiKey);

			public const string SBomAuthor = nameof(SBomAuthor);
			public const string SBomNamespace = nameof(SBomNamespace);

			public const string NugetApiUrl = nameof(NugetApiUrl);
			public const string NugetRepositoryName = nameof(NugetRepositoryName);
			public const string NugetRepositoryUrl = nameof(NugetRepositoryUrl);
			public const string NugetApiKey = nameof(NugetApiKey);
			public const string NugetUserName = nameof(NugetUserName);
			public const string NugetPassword = nameof(NugetPassword);
			public const string NugetNuSpecIconUrl = nameof(NugetNuSpecIconUrl);
			public const string NugetNuSpecCopyright = nameof(NugetNuSpecCopyright);
			public const string NugetNuSpecAuthor = nameof(NugetNuSpecAuthor);
			public const string NugetNuSpecOwner = nameof(NugetNuSpecOwner);

			public const string DependencyTrackApiUrl = nameof(DependencyTrackApiUrl);
			public const string DependencyTrackApiKey = nameof(DependencyTrackApiKey);

			public const string BuildArtifactsApiUrl = nameof(BuildArtifactsApiUrl);
			public const string BuildArtifactsApiKey = nameof(BuildArtifactsApiKey);
			public const string BuildArtifactsUserName = nameof(BuildArtifactsUserName);
			public const string BuildArtifactsPassword = nameof(BuildArtifactsPassword);

			public const string DockerRegistryDomainName = nameof(DockerRegistryDomainName);
			public const string DockerRegistryApiKey = nameof(DockerRegistryApiKey);
			public const string DockerRegistryUserName = nameof(DockerRegistryUserName);
			public const string DockerRegistryPassword = nameof(DockerRegistryPassword);
			public const string EnvironmentFileFullName = nameof(EnvironmentFileFullName);

			public const string VsExtensionsApiUrl = nameof(VsExtensionsApiUrl);
			public const string VsExtensionsApiKey = nameof(VsExtensionsApiKey);
			public const string VsExtensionsUserName = nameof(VsExtensionsUserName);
			public const string VsExtensionsPassword = nameof(VsExtensionsPassword);
			public const string VsExtensionsPublisherKey = nameof(VsExtensionsPublisherKey);
			public const string VsExtensionsPublisherPersonalAccessToken = nameof(VsExtensionsPublisherPersonalAccessToken);

			public const string JenkinsServiceApiUrl = nameof(JenkinsServiceApiUrl);
			public const string JenkinsServiceApiKey = nameof(JenkinsServiceApiKey);
			public const string JenkinsUrl = nameof(JenkinsUrl);
			public const string JenkinsUserName = nameof(JenkinsUserName);
			public const string JenkinsApiToken = nameof(JenkinsApiToken);

			public const string FileStoreUrl = nameof(FileStoreUrl);
			public const string FileStoreUserName = nameof(FileStoreUserName);
			public const string FileStorePassword = nameof(FileStorePassword);

			private static IEnumerable<string> _keys = null;
			internal static IEnumerable<string> Keys => _keys ??= GetKeys();
			private static IEnumerable<string> GetKeys()
			{
				return typeof(Key)
					.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy)
					.Where(fieldInfo => fieldInfo.IsLiteral && !fieldInfo.IsInitOnly && (fieldInfo.FieldType == typeof(string)))
					.Select(fieldInfo => (string)fieldInfo.GetRawConstantValue())
					.ToArray();
			}
		}

		public string SettingsFullName { get; protected set; }

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
		public SettingsActiveDirectory ActiveDirectory => _activeDirectory ??= new(this);

		private SettingsCodeSigning _codeSigning = null;
		public SettingsCodeSigning CodeSigning => _codeSigning ??= new(this);

		private SettingsSBom _sBom = null;
		public SettingsSBom SBom => _sBom ??= new(this);

		private SettingsNuget _nuget = null;
		public SettingsNuget Nuget => _nuget ??= new(this);

		private SettingsDependencyTrack _dependencyTrack = null;
		public SettingsDependencyTrack DependencyTrack => _dependencyTrack ??= new(this);

		private SettingsBuildArtifacts _buildArtifacts = null;
		public SettingsBuildArtifacts BuildArtifacts => _buildArtifacts ??= new(this);

		private SettingsDockerRegistry _dockerRegistry = null;
		public SettingsDockerRegistry DockerRegistry => _dockerRegistry ??= new(this);

		private SettingsVsExtensions _vsExtensions = null;
		public SettingsVsExtensions VsExtensions => _vsExtensions ??= new(this);

		private SettingsJenkins _jenkins = null;
		public SettingsJenkins Jenkins => _jenkins ??= new(this);

		private SettingsFileStore _fileStore = null;
		public SettingsFileStore FileStore => _fileStore ??= new(this);

		public string[] AllKeys => Values.Keys.ToNullCheckedArray(NullCheckCollectionResult.Empty);

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
			Values.Add(new(key, value));
		}

		public override string ToString() => string.Join("\r\n", Values.Select(keyValue => string.Format("{0} => {1}", keyValue.Key, keyValue.Value)));
	}
}
