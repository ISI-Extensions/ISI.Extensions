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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.GoDrive.GoDriveFileSystem
{
	public abstract class GoDriveFileSystemPath : IGoDriveFileSystemPath
	{
		public abstract string Schema { get; }
		public abstract string DirectorySeparator { get; }

		public string Server { get; protected set; } = string.Empty;
		public string Directory { get; protected set; } = string.Empty;
		public string PathName { get; protected set; } = string.Empty;

		string FileSystem.IFileSystemPath.UserName => throw new NotImplementedException();
		string FileSystem.IFileSystemPath.Password => throw new NotImplementedException();

		protected virtual string BuildAttributedFullPath(bool showSchema, bool showServer)
		{
			var attributedFullPathBuilder = new System.Text.StringBuilder();

			if (showSchema)
			{
				attributedFullPathBuilder.Append(Schema);
			}

			if (showServer && !string.IsNullOrEmpty(Server))
			{
				attributedFullPathBuilder.AppendFormat("{0}{1}", Server, DirectorySeparator);
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

		public virtual string FullPath() => BuildAttributedFullPath(false, false);
		public virtual string AttributedFullPath() => BuildAttributedFullPath(true, true);
		public virtual string ObfuscatedAttributedFullPath(bool obfuscateUserName = true, string obfuscatedUserNameValue = null, bool obfuscatePassword = true, string obfuscatedPasswordValue = null) => BuildAttributedFullPath(true, true);

		public abstract FileSystem.IFileSystemPath Clone();

		public abstract FileSystem.IFileSystemPathDirectory GetParentFileSystemPathDirectory();

		public override string ToString() => FullPath();
	}
}

