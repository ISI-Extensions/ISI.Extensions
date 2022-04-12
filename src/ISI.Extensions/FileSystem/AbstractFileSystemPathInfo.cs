#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	public partial class FileSystem
	{
		public abstract class AbstractFileSystemPathInfo : IFileSystemPathInfo
		{
			public abstract string Schema { get; }
			public abstract string DirectorySeparator { get; }
			public abstract System.Text.RegularExpressions.Regex AttributedPathRegex { get; }
			public abstract IFileSystemProvider FileSystemProvider { get; }

			public virtual string Server { get; protected set; } = string.Empty;
			public virtual string UserName { get; protected set; } = string.Empty;
			public virtual string Password { get; protected set; } = string.Empty;
			public virtual string Directory { get; protected set; } = string.Empty;
			public virtual string AttributedDirectory { get; protected set; }
			public virtual string AttributedDirectoryWithoutCredentials { get; protected set; }
			public virtual string PathName { get; protected set; } = string.Empty;
			public virtual string FullPathName { get; protected set; } = string.Empty;
			public virtual string AttributedFullPath { get; protected set; }
			public virtual string AttributedFullPathWithoutCredentials { get; protected set; }

			public virtual bool CanParse(string value)
			{
				return AttributedPathRegex.Match(value).Success;
			}

			#region Parse

			public virtual void Parse(string value)
			{
				Parse(value, true);
			}

			public virtual void Parse(string value, bool removeLeadingDirectorySeparator)
			{
				Server = string.Empty;
				UserName = string.Empty;
				Password = string.Empty;
				Directory = string.Empty;
				PathName = string.Empty;
				FullPathName = string.Empty;

				if (AttributedPathRegex != null)
				{
					value = value.Trim();

					while (value.EndsWith(DirectorySeparator))
					{
						value = value.Substring(0, value.Length - DirectorySeparator.Length).Trim();
					}

					var match = AttributedPathRegex.Match(value);

					if (match.Success)
					{
						AttributedDirectory = value;
						{
							var lastDirectorySeparatorIndex = AttributedDirectory.LastIndexOf(DirectorySeparator, StringComparison.InvariantCultureIgnoreCase);

							if (lastDirectorySeparatorIndex >= 0)
							{
								AttributedDirectory = AttributedDirectory.Substring(0, lastDirectorySeparatorIndex);
							}
						}

						AttributedFullPath = value;

						if (match.Groups.TryGetValue("server", out var serverGroup))
						{
							Server = serverGroup.Value;
						}

						if (match.Groups.TryGetValue("user", out var userNameGroup))
						{
							UserName = userNameGroup.Value;
						}

						if (match.Groups.TryGetValue("password", out var passwordGroup))
						{
							Password = passwordGroup.Value;
						}

						FullPathName = match.Groups["file"].Value.Trim();

						if (removeLeadingDirectorySeparator)
						{
							while (FullPathName.StartsWith(DirectorySeparator))
							{
								FullPathName = FullPathName.Substring(DirectorySeparator.Length).Trim();
							}
						}

						{
							var lastDirectorySeparatorIndex = FullPathName.LastIndexOf(DirectorySeparator, StringComparison.InvariantCultureIgnoreCase);

							if (lastDirectorySeparatorIndex >= 0)
							{
								Directory = FullPathName.Substring(0, lastDirectorySeparatorIndex);
								PathName = FullPathName.Substring(lastDirectorySeparatorIndex + 1);
							}
							else
							{
								PathName = FullPathName;
							}
						}

						#region AttributedDirectoryWithoutCredentials

						var attributedDirectoryWithoutCredentials = new System.Text.StringBuilder();

						attributedDirectoryWithoutCredentials.Append(Schema);

						if (!string.IsNullOrEmpty(Server))
						{
							attributedDirectoryWithoutCredentials.AppendFormat("{0}{1}", Server, DirectorySeparator);
						}

						attributedDirectoryWithoutCredentials.Append(Directory);

						AttributedDirectoryWithoutCredentials = attributedDirectoryWithoutCredentials.ToString();

						#endregion

						#region AttributedFullPathWithoutCredentials

						var attributedFullPathWithoutCredentials = new System.Text.StringBuilder();

						attributedFullPathWithoutCredentials.Append(Schema);

						if (!string.IsNullOrEmpty(Server))
						{
							attributedFullPathWithoutCredentials.AppendFormat("{0}{1}", Server, DirectorySeparator);
						}

						attributedFullPathWithoutCredentials.Append(FullPathName);

						AttributedFullPathWithoutCredentials = attributedFullPathWithoutCredentials.ToString();

						#endregion
					}
				}
			}

			public virtual void Parse(string server, string userName, string password, string directory, string pathName = null)
			{
				Server = server;
				UserName = userName;
				Password = password;

				#region Directory

				Directory = directory.Trim();
				while (Directory.StartsWith(DirectorySeparator))
				{
					Directory = Directory.Substring(DirectorySeparator.Length).Trim();
				}

				while (Directory.EndsWith(DirectorySeparator))
				{
					Directory = Directory.Substring(0, Directory.Length - DirectorySeparator.Length).Trim();
				}

				#endregion

				#region PathName

				if (pathName == null)
				{
					var lastDirectorySeparatorIndex = Directory.LastIndexOf(DirectorySeparator, StringComparison.InvariantCultureIgnoreCase);

					if (lastDirectorySeparatorIndex >= 0)
					{
						pathName = Directory.Substring(lastDirectorySeparatorIndex);
						Directory = Directory.Substring(0, lastDirectorySeparatorIndex);
					}
					else
					{
						pathName = Directory;
						Directory = string.Empty;
					}
				}

				PathName = pathName.Trim();
				while (PathName.StartsWith(DirectorySeparator))
				{
					PathName = PathName.Substring(DirectorySeparator.Length).Trim();
				}

				while (PathName.EndsWith(DirectorySeparator))
				{
					PathName = PathName.Substring(0, PathName.Length - DirectorySeparator.Length).Trim();
				}

				#endregion

				FullPathName = (string.IsNullOrWhiteSpace(Directory) ? pathName : string.Format("{0}{1}{2}", Directory, DirectorySeparator, PathName));

				#region userNamePassword

				var userNamePassword = userName;
				if (!string.IsNullOrWhiteSpace(userNamePassword))
				{
					if (!string.IsNullOrWhiteSpace(password))
					{
						userNamePassword += string.Format(":{0}", password);
					}

					userNamePassword += "@";
				}

				#endregion

				#region AttributedDirectory

				var attributedDirectory = new System.Text.StringBuilder();

				attributedDirectory.Append(Schema);

				attributedDirectory.Append(userNamePassword);

				if (!string.IsNullOrEmpty(Server))
				{
					attributedDirectory.AppendFormat("{0}{1}", Server, DirectorySeparator);
				}

				attributedDirectory.Append(Directory);

				AttributedDirectory = attributedDirectory.ToString();

				#endregion

				#region AttributedDirectoryWithoutCredentials

				var attributedDirectoryWithoutCredentials = new System.Text.StringBuilder();

				attributedDirectoryWithoutCredentials.Append(Schema);

				if (!string.IsNullOrEmpty(Server))
				{
					attributedDirectoryWithoutCredentials.AppendFormat("{0}{1}", Server, DirectorySeparator);
				}

				attributedDirectoryWithoutCredentials.Append(Directory);

				AttributedDirectoryWithoutCredentials = attributedDirectoryWithoutCredentials.ToString();

				#endregion

				#region AttributedFullPath

				var attributedFullPath = new System.Text.StringBuilder();

				attributedFullPath.Append(Schema);

				attributedFullPath.Append(userNamePassword);

				if (!string.IsNullOrEmpty(Server))
				{
					attributedFullPath.AppendFormat("{0}{1}", Server, DirectorySeparator);
				}

				attributedFullPath.Append(FullPathName);

				AttributedFullPath = attributedFullPath.ToString();

				#endregion

				#region AttributedFullPathWithoutCredentials

				var attributedFullPathWithoutCredentials = new System.Text.StringBuilder();

				attributedFullPathWithoutCredentials.Append(Schema);

				if (!string.IsNullOrEmpty(Server))
				{
					attributedFullPathWithoutCredentials.AppendFormat("{0}{1}", Server, DirectorySeparator);
				}

				attributedFullPathWithoutCredentials.Append(FullPathName);

				AttributedFullPathWithoutCredentials = attributedFullPathWithoutCredentials.ToString();

				#endregion
			}

			#endregion

			#region ObfuscatedAttributedFullPath

			public virtual string ObfuscatedAttributedFullPath(bool obfuscateUserName = true, string obfuscatedUserNameValue = null, bool obfuscatePassword = true, string obfuscatedPasswordValue = null)
			{
				var result = new System.Text.StringBuilder();

				result.Append(Schema);

				if (!string.IsNullOrWhiteSpace(UserName))
				{
					result.Append((obfuscateUserName ? obfuscatedUserNameValue : UserName) ?? string.Empty);

					if (!string.IsNullOrWhiteSpace(Password))
					{
						result.AppendFormat(":{0}", (obfuscatePassword ? obfuscatedPasswordValue : Password) ?? string.Empty);
					}

					result.Append("@");
				}

				if (!string.IsNullOrEmpty(Server))
				{
					result.AppendFormat("{0}{1}", Server, DirectorySeparator);
				}

				result.Append(FullPathName);

				return result.ToString();
			}

			#endregion

			public virtual IFileSystemPathInfo Clone()
			{
				var fileSystemPathInfo = Activator.CreateInstance(this.GetType()) as IFileSystemPathInfo;

				fileSystemPathInfo?.Parse(AttributedFullPath);

				return fileSystemPathInfo;
			}
		}
	}
}