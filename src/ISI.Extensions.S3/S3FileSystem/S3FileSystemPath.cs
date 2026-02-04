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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.S3.S3FileSystem
{
	public abstract class S3FileSystemPath : IS3FileSystemPath
	{
		public virtual string Schema => S3FileSystemProvider._schema;
		public virtual string DirectorySeparator => S3FileSystemProvider._directorySeparator;

		public string EndpointUrl { get; protected set; } = string.Empty;
		public string AccessKey { get; protected set; } = string.Empty;
		public string SecretKey { get; protected set; } = string.Empty;
		public string BucketName { get; protected set; } = string.Empty;

		string FileSystem.IFileSystemPath.Server => EndpointUrl;
		string FileSystem.IFileSystemPath.UserName => AccessKey;
		string FileSystem.IFileSystemPath.Password => SecretKey;

		public string Directory { get; protected set; } = string.Empty;
		public string PathName { get; protected set; } = string.Empty;

		protected virtual string BuildAttributedFullPath(bool showSchema, bool showAccessKey, bool showSecretKey, bool showEndpointUrl, bool obfuscateAccessKey = true, string obfuscatedAccessKeyValue = null, bool obfuscateSecretKey = true, string obfuscatedSecretKeyValue = null)
		{
			var attributedFullPathBuilder = new System.Text.StringBuilder();

			if (showSchema)
			{
				attributedFullPathBuilder.Append(Schema);
			}

			var accessKey = (string.IsNullOrWhiteSpace(AccessKey) ? string.Empty : (obfuscateAccessKey ? obfuscatedAccessKeyValue : AccessKey)) ?? string.Empty;
			if (showAccessKey && !string.IsNullOrWhiteSpace(accessKey))
			{
				attributedFullPathBuilder.Append(accessKey);

				var secretkey = (string.IsNullOrWhiteSpace(SecretKey) ? string.Empty : (obfuscateSecretKey ? obfuscatedSecretKeyValue : SecretKey)) ?? string.Empty;
				if (showSecretKey && !string.IsNullOrWhiteSpace(secretkey))
				{
					attributedFullPathBuilder.AppendFormat(":{0}", secretkey);
				}

				attributedFullPathBuilder.Append("@");
			}

			if (showEndpointUrl && !string.IsNullOrEmpty(EndpointUrl))
			{
				attributedFullPathBuilder.AppendFormat("{0}{1}", EndpointUrl, DirectorySeparator);
			}

			if (!string.IsNullOrWhiteSpace(BucketName))
			{
				attributedFullPathBuilder.Append(BucketName);

				if (!Directory.EndsWith(DirectorySeparator))
				{
					attributedFullPathBuilder.Append(DirectorySeparator);
				}
			}

			if (!string.IsNullOrWhiteSpace(Directory))
			{
				attributedFullPathBuilder.Append(Directory);

				if (!Directory.EndsWith(DirectorySeparator))
				{
					attributedFullPathBuilder.Append(DirectorySeparator);
				}
			}

			if (!string.IsNullOrWhiteSpace(PathName))
			{
				attributedFullPathBuilder.Append(PathName);
			}

			return attributedFullPathBuilder.ToString();
		}

		protected virtual (string Directory, string PathName) GetParentPathParts()
		{
			var lastDirectorySeparatorIndex = Directory.LastIndexOf(DirectorySeparator, StringComparison.InvariantCultureIgnoreCase);
			if (lastDirectorySeparatorIndex > 0)
			{
				return (Directory: Directory.Substring(0, lastDirectorySeparatorIndex), PathName: Directory.Substring(lastDirectorySeparatorIndex + DirectorySeparator.Length));
			}

			return (Directory: string.Empty, PathName: Directory);
		}

		public virtual string FullPath() => BuildAttributedFullPath(false, false, false, false, true, null, true, null);
		public virtual string AttributedFullPath() => BuildAttributedFullPath(true, true, true, true, false, null, false, null);
		public virtual string ObfuscatedAttributedFullPath(bool obfuscateAccessKey = true, string obfuscatedAccessKeyValue = null, bool obfuscateSecretKey = true, string obfuscatedSecretKeyValue = null) => BuildAttributedFullPath(true, true, true, true, obfuscateAccessKey, obfuscatedAccessKeyValue, obfuscateSecretKey, obfuscatedSecretKeyValue);

		public abstract FileSystem.IFileSystemPath Clone();

		public virtual FileSystem.IFileSystemPathDirectory GetParentFileSystemPathDirectory()
		{
			var parentPathParts = GetParentPathParts();

			if (string.IsNullOrWhiteSpace(parentPathParts.PathName))
			{
				return null;
			}

			var fileSystemPath = new S3FileSystemPathDirectory();

			EndpointUrl = EndpointUrl;
			AccessKey = AccessKey;
			SecretKey = SecretKey;
			BucketName = BucketName;
			Directory = parentPathParts.Directory;
			PathName = parentPathParts.PathName;

			return fileSystemPath;
		}
	}
}