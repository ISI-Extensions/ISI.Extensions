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
	[FileSystem.FileSystemProvider]
	public class S3FileSystemProvider : FileSystem.AbstractFileSystemProvider
	{
		internal static string _schema => "s3://";
		protected override string Schema => _schema;

		internal static string _directorySeparator => "/";
		protected override string DirectorySeparator => _directorySeparator;

		internal static readonly System.Text.RegularExpressions.Regex _attributedPathRegex = new(@"^" + _schema + @"((?<accesskey>.+?)(:(?<secretkey>.+))?@)?(?<endpointurl>.+?)(?<file>/.*)?$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
		protected override System.Text.RegularExpressions.Regex AttributedPathRegex => _attributedPathRegex;

		public override Type GetFileSystemPathType => typeof(IS3FileSystemPath);

		protected S3BlobClient GetClient(IS3FileSystemPath fileSystemPath)
		{
			return new(fileSystemPath.EndpointUrl, fileSystemPath.AccessKey, fileSystemPath.SecretKey, fileSystemPath.BucketName);
		}

		public override string Combine(string attributedFullName, string path2)
		{
			var fileSystemPath = ParseAttributedFullName(attributedFullName);

			var directory = (string.IsNullOrWhiteSpace(fileSystemPath.PathName) ? fileSystemPath.Directory : $"{fileSystemPath.Directory}{DirectorySeparator}{fileSystemPath.PathName}");

			IS3FileSystemPathDirectory fileSystemPathDirectory = new S3FileSystemPathDirectory();

			fileSystemPathDirectory.SetValues(fileSystemPath.EndpointUrl, fileSystemPath.AccessKey, fileSystemPath.SecretKey, fileSystemPath.BucketName, directory, path2);

			return fileSystemPathDirectory.AttributedFullPath();
		}

		protected virtual (string EndpointUrl, string AccessKey, string SecretKey, string BucketName, string Directory, string PathName) ParseAttributedFullName(string attributedFullName)
		{
			var fileSystemPath = (EndpointUrl: string.Empty, AccessKey: string.Empty, SecretKey: string.Empty, BucketName: string.Empty, Directory: string.Empty, PathName: string.Empty);

			attributedFullName = attributedFullName.Trim();

			while (attributedFullName.EndsWith(DirectorySeparator))
			{
				attributedFullName = attributedFullName.Substring(0, attributedFullName.Length - DirectorySeparator.Length).Trim();
			}

			var match = AttributedPathRegex.Match(attributedFullName);

			if (match.Success)
			{
				if (match.Groups.TryGetValue("endpointurl", out var serverGroup))
				{
					fileSystemPath.EndpointUrl = serverGroup.Value;
				}
				if (match.Groups.TryGetValue("accesskey", out var userNameGroup))
				{
					fileSystemPath.AccessKey = userNameGroup.Value;
				}
				if (match.Groups.TryGetValue("secretkey", out var passwordGroup))
				{
					fileSystemPath.SecretKey = passwordGroup.Value;
				}

				var fullPath = match.Groups["file"].Value.Trim();

				while (fullPath.StartsWith(DirectorySeparator))
				{
					fullPath = fullPath.TrimStart(DirectorySeparator).Trim();
				}

				var fullPathPieces = fullPath.Split([DirectorySeparator], 2, StringSplitOptions.RemoveEmptyEntries);

				if (fullPathPieces.Length > 0)
				{
					fileSystemPath.BucketName = fullPathPieces[0];

					if (fullPathPieces.Length > 1)
					{
						fullPath = fullPathPieces[1];
					}
				}

				var lastDirectorySeparatorIndex = fullPath.LastIndexOf(DirectorySeparator, StringComparison.InvariantCultureIgnoreCase);
				if (lastDirectorySeparatorIndex > 0)
				{
					fileSystemPath.Directory = fullPath.Substring(0, lastDirectorySeparatorIndex);
					fileSystemPath.PathName = fullPath.Substring(lastDirectorySeparatorIndex + DirectorySeparator.Length);
				}
				else
				{
					fileSystemPath.PathName = fullPath;
				}
			}

			return fileSystemPath;
		}

		public override FileSystem.IFileSystemPathFile GetFileSystemPathFile(string attributedFullName)
		{
			var parseAttributedFullName = ParseAttributedFullName(attributedFullName);

			var fileSystemPathFile = new S3FileSystemPathFile();

			fileSystemPathFile.SetValues(parseAttributedFullName.EndpointUrl, parseAttributedFullName.AccessKey, parseAttributedFullName.SecretKey, parseAttributedFullName.BucketName, parseAttributedFullName.Directory, parseAttributedFullName.PathName, null, null);

			return fileSystemPathFile;
		}

		public override FileSystem.IFileSystemPathDirectory GetFileSystemPathDirectory(string attributedFullName)
		{
			var parseAttributedFullName = ParseAttributedFullName(attributedFullName);

			var fileSystemPathDirectory = new S3FileSystemPathDirectory();

			fileSystemPathDirectory.SetValues(parseAttributedFullName.EndpointUrl, parseAttributedFullName.AccessKey, parseAttributedFullName.SecretKey, parseAttributedFullName.BucketName, parseAttributedFullName.Directory, parseAttributedFullName.PathName);

			return fileSystemPathDirectory;
		}

		protected virtual FileSystem.IFileSystemStream GetFileSystemStream()
		{
			return new S3FileSystemStream(GetClient);
		}

		public override IEnumerable<FileSystem.IFileSystemPath> GetDirectoryFileSystemPaths(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive)
		{
			if (fileSystemPathDirectory is IS3FileSystemPath s3fileSystemPath)
			{
				try
				{
					var client = GetClient(s3fileSystemPath);

					IEnumerable<FileSystem.IFileSystemPath> getDirectoryFileSystemInfos(FileSystem.IFileSystemPathDirectory directorySystemPathInfo, bool doRecursive)
					{
						var fileSystemInfos = new List<FileSystem.IFileSystemPath>();
						var fileSystemInfoDirectories = new List<FileSystem.IFileSystemPathDirectory>();

						var blobInfos = client.ListFilesAsync(new()
						{
							Prefix = ((IS3FileSystemPath)directorySystemPathInfo).BucketName,
						}).GetAwaiter().GetResult().BlobInfos.ToNullCheckedArray(NullCheckCollectionResult.Empty);

						foreach (var blobInfo in blobInfos)
						{
							var lastDirectorySeparatorIndex = blobInfo.FullName.LastIndexOf(DirectorySeparator, StringComparison.InvariantCultureIgnoreCase);
							var directory = (lastDirectorySeparatorIndex > 0 ? blobInfo.FullName.Substring(0, lastDirectorySeparatorIndex) : string.Empty);
							var pathName = (lastDirectorySeparatorIndex > 0 ? blobInfo.FullName.Substring(lastDirectorySeparatorIndex + DirectorySeparator.Length) : blobInfo.FullName);

							if (blobInfo.IsDirectory)
							{
								var fileSystemDirectory = new S3FileSystemPathDirectory();
								fileSystemDirectory.SetValues(s3fileSystemPath.EndpointUrl, s3fileSystemPath.AccessKey, s3fileSystemPath.SecretKey, s3fileSystemPath.BucketName, directory, pathName);
								fileSystemInfos.Add(fileSystemDirectory);
								fileSystemInfoDirectories.Add(fileSystemDirectory);
							}
							else
							{
								var fileSystemPathFile = new S3FileSystemPathFile();
								fileSystemPathFile.SetValues(s3fileSystemPath.EndpointUrl, s3fileSystemPath.AccessKey, s3fileSystemPath.SecretKey, s3fileSystemPath.BucketName, directory, pathName, blobInfo.LastModifiedDateTime, (long)blobInfo.Size);
								fileSystemInfos.Add(fileSystemPathFile);
							}
						}

						if (doRecursive)
						{
							foreach (var fileSystemInfoDirectory in fileSystemInfoDirectories)
							{
								fileSystemInfos.AddRange(getDirectoryFileSystemInfos(fileSystemInfoDirectory, doRecursive));
							}
						}

						return fileSystemInfos;
					}

					return getDirectoryFileSystemInfos(fileSystemPathDirectory, doRecursive);
				}
				catch (Exception exception)
				{
					throw exception;
				}
			}

			return Array.Empty<FileSystem.IFileSystemPath>();
		}

		public override void CreateDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory)
		{

		}

		public override void RemoveDirectory(FileSystem.IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive = true)
		{
			if (fileSystemPathDirectory is IS3FileSystemPath s3fileSystemPath)
			{
				try
				{
					var client = GetClient(s3fileSystemPath);

					var prefix = s3fileSystemPath.BucketName;
					if (!string.IsNullOrWhiteSpace(s3fileSystemPath.Directory))
					{
						prefix = $"{prefix}{DirectorySeparator}{s3fileSystemPath.Directory}";
					}
					if (!string.IsNullOrWhiteSpace(s3fileSystemPath.PathName))
					{
						prefix = $"{prefix}{DirectorySeparator}{s3fileSystemPath.PathName}";
					}

					var blobInfos = client.ListFilesAsync(new()
					{
						Prefix = prefix,
					}).GetAwaiter().GetResult().BlobInfos.ToNullCheckedArray(NullCheckCollectionResult.Empty);

					var directoryBlobInfo = blobInfos.NullCheckedFirstOrDefault(blobInfo => blobInfo.IsDirectory);

					if (directoryBlobInfo != null)
					{
						prefix = s3fileSystemPath.BucketName;
						if (!string.IsNullOrWhiteSpace(directoryBlobInfo.FullName))
						{
							prefix = $"{prefix}{DirectorySeparator}{directoryBlobInfo.FullName}";
						}

						blobInfos = client.ListFilesAsync(new()
						{
							Prefix = prefix,
							DoRecursive = true,
						}).GetAwaiter().GetResult().BlobInfos.ToNullCheckedArray(NullCheckCollectionResult.Empty);

						var isEmpty = !blobInfos.NullCheckedAny(blobInfo => blobInfo.IsDirectory);

						if (!isEmpty && doRecursive)
						{
							foreach (var blobInfo in blobInfos.NullCheckedWhere(blobInfo => !blobInfo.IsDirectory, NullCheckCollectionResult.Empty))
							{
								client.DeleteFileIfExistsAsync(new()
								{
									FullName = blobInfo.FullName,
								}).GetAwaiter().GetResult();
							}
						}
					}
				}
				catch (Exception exception)
				{
					throw exception;
				}
			}
		}

		public override void RemoveFile(FileSystem.IFileSystemPathFile fileSystemPathFile)
		{
			if (fileSystemPathFile is IS3FileSystemPath s3fileSystemPath)
			{
				try
				{
					var client = GetClient(s3fileSystemPath);

					var fullName = (string.IsNullOrWhiteSpace(s3fileSystemPath.Directory) ? s3fileSystemPath.PathName : $"{s3fileSystemPath.Directory}{DirectorySeparator}{s3fileSystemPath.PathName}");

					client.DeleteFileIfExistsAsync(new()
					{
						FullName = fullName,
					}).GetAwaiter().GetResult();
				}
				catch (Exception exception)
				{
					throw exception;
				}
			}
		}

		public override System.IO.Stream OpenRead(FileSystem.IFileSystemPathFile fileSystemPathFile, bool mustBeSeekable = false)
		{
			var stream = GetFileSystemStream();

			stream.OpenRead(fileSystemPathFile, mustBeSeekable);

			return stream as System.IO.Stream;
		}

		public override System.IO.Stream OpenWrite(FileSystem.IFileSystemPathFile fileSystemPathFile, bool createDirectories, bool overwrite, long fileSize = 0)
		{
			if (createDirectories)
			{
				CreateDirectory(fileSystemPathFile.GetParentFileSystemPathDirectory());
			}

			var stream = GetFileSystemStream();

			stream.OpenWrite(fileSystemPathFile, overwrite, fileSize);

			return stream as System.IO.Stream;
		}
	}
}
