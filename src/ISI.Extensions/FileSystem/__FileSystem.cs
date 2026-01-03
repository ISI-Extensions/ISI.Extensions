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
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions
{
	public partial class FileSystem
	{
		private static IFileSystemProvider[] _fileSystemProviders = null;
		private static IFileSystemProvider[] FileSystemProviders => _fileSystemProviders ??= ISI.Extensions.TypeLocator.Container.LocalContainer.GetImplementations<IFileSystemProvider>();

		private static Dictionary<Type, IFileSystemProvider> _fileSystemProviderByFileSystemPathType = null;
		private static Dictionary<Type, IFileSystemProvider> FileSystemProviderByFileSystemPathType => _fileSystemProviderByFileSystemPathType ??= FileSystemProviders.ToDictionary(fileSystemProvider => fileSystemProvider.GetFileSystemPathType, fileSystemProvider => fileSystemProvider);

		public static IFileSystemProvider GetFileSystemProvider(string attributedFullName)
		{
			IFileSystemProvider response = null;
			IFileSystemProvider defaultFileSystemProvider = null;

			foreach (var fileSystemProvider in FileSystemProviders)
			{
				if (fileSystemProvider is ISI.Extensions.SmbFileSystem.SmbFileSystemProvider)
				{
					if (Environment.OSVersion.Platform != PlatformID.Unix)
					{
						defaultFileSystemProvider = fileSystemProvider;
					}
					//ignore SmbFileSystemProvider when running in Linux
				}
				else if (fileSystemProvider is ISI.Extensions.UnixFileSystem.UnixFileSystemProvider)
				{
					if (Environment.OSVersion.Platform == PlatformID.Unix)
					{
						defaultFileSystemProvider = fileSystemProvider;
					}
					//ignore SmbFileSystemProvider when running in Linux
				}
				else
				{
					if (fileSystemProvider.CanParsePath(attributedFullName))
					{
						if ((response == null) || (fileSystemProvider.Priority > response.Priority))
						{
							response = fileSystemProvider;
						}
					}
				}
			}

			return response ?? defaultFileSystemProvider;
		}

		public static IFileSystemProvider GetFileSystemProvider(IFileSystemPath fileSystemPath)
		{
			var fileSystemPathType = fileSystemPath.GetType();

			{
				if (FileSystemProviderByFileSystemPathType.TryGetValue(fileSystemPathType, out var fileSystemProvider))
				{
					return fileSystemProvider;
				}
			}

			IFileSystemProvider response = null;
			IFileSystemProvider defaultFileSystemProvider = null;

			foreach (var fileSystemProvider in FileSystemProviders)
			{
				if ((Environment.OSVersion.Platform != PlatformID.Unix) && (fileSystemProvider is ISI.Extensions.SmbFileSystem.SmbFileSystemProvider))
				{
					defaultFileSystemProvider = fileSystemProvider;
				}
				else if ((Environment.OSVersion.Platform == PlatformID.Unix) && (fileSystemProvider is ISI.Extensions.UnixFileSystem.UnixFileSystemProvider))
				{
					defaultFileSystemProvider = fileSystemProvider;
				}
				else
				{
					if (fileSystemPathType.Implements(fileSystemProvider.GetFileSystemPathType))
					{
						if ((response == null) || (fileSystemProvider.Priority > response.Priority))
						{
							response = fileSystemProvider;
						}
					}
				}
			}

			response ??= defaultFileSystemProvider;

			if (!FileSystemProviderByFileSystemPathType.ContainsKey(fileSystemPathType))
			{
				FileSystemProviderByFileSystemPathType.Add(fileSystemPathType, response);
			}

			return response;
		}

		public static string Combine(string attributedFullName, string path2) => GetFileSystemProvider(attributedFullName).Combine(attributedFullName, path2);
		public static IFileSystemPathFile GetFileSystemPathFile(string attributedFullName) => GetFileSystemProvider(attributedFullName).GetFileSystemPathFile(attributedFullName);
		public static IFileSystemPathDirectory GetFileSystemPathDirectory(string attributedFullName) => GetFileSystemProvider(attributedFullName).GetFileSystemPathDirectory(attributedFullName);
		public static string GetObfuscatedAttributedFullPath(string attributedFullName, bool obfuscateUserName = true, string obfuscatedUserNameValue = null, bool obfuscatePassword = true, string obfuscatedPasswordValue = null) => GetFileSystemProvider(attributedFullName).GetFileSystemPathFile(attributedFullName).ObfuscatedAttributedFullPath(obfuscateUserName, obfuscatedUserNameValue, obfuscatePassword, obfuscatedPasswordValue);

		public static IEnumerable<IFileSystemPath> GetDirectoryFileSystemPaths(string directoryName, bool doRecursive) => GetFileSystemProvider(directoryName).GetDirectoryFileSystemPaths(directoryName, doRecursive);
		public static IEnumerable<IFileSystemPath> GetDirectoryFileSystemPaths(IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive) => GetFileSystemProvider(fileSystemPathDirectory).GetDirectoryFileSystemPaths(fileSystemPathDirectory, doRecursive);

		public static bool FileExists(string attributedFullName) => GetFileSystemProvider(attributedFullName).FileExists(attributedFullName);
		public static bool FileExists(IFileSystemPathFile fileSystemPathFile) => GetFileSystemProvider(fileSystemPathFile).FileExists(fileSystemPathFile);
		public static bool DirectoryExists(string attributedFullName) => GetFileSystemProvider(attributedFullName).DirectoryExists(attributedFullName);
		public static bool DirectoryExists(IFileSystemPathDirectory fileSystemPathDirectory) => GetFileSystemProvider(fileSystemPathDirectory).DirectoryExists(fileSystemPathDirectory);

		public static void CreateDirectory(string attributedFullName) => GetFileSystemProvider(attributedFullName).CreateDirectory(attributedFullName);
		public static void CreateDirectory(IFileSystemPathDirectory fileSystemPathDirectory) => GetFileSystemProvider(fileSystemPathDirectory).CreateDirectory(fileSystemPathDirectory);

		public static void RemoveDirectory(string attributedFullName, bool doRecursive = true) => GetFileSystemProvider(attributedFullName).RemoveDirectory(attributedFullName, doRecursive);
		public static void RemoveDirectory(IFileSystemPathDirectory fileSystemPathDirectory, bool doRecursive = true) => GetFileSystemProvider(fileSystemPathDirectory).RemoveDirectory(fileSystemPathDirectory, doRecursive);
		public static void RemoveFile(string attributedFullName) => GetFileSystemProvider(attributedFullName).RemoveFile(attributedFullName);
		public static void RemoveFile(IFileSystemPathFile fileSystemPathFile) => GetFileSystemProvider(fileSystemPathFile).RemoveFile(fileSystemPathFile);

		public static System.IO.Stream OpenRead(string attributedFullName, bool mustBeSeekable = false) => GetFileSystemProvider(attributedFullName).OpenRead(attributedFullName, mustBeSeekable);
		public static System.IO.Stream OpenRead(IFileSystemPathFile fileSystemPathFile, bool mustBeSeekable = false) => GetFileSystemProvider(fileSystemPathFile).OpenRead(fileSystemPathFile, mustBeSeekable);
		public static System.IO.Stream OpenWrite(string attributedFullName, bool createDirectories, bool overwrite, long fileSize = 0) => GetFileSystemProvider(attributedFullName).OpenWrite(attributedFullName, createDirectories, overwrite, fileSize);
		public static System.IO.Stream OpenWrite(IFileSystemPathFile fileSystemPathFile, bool createDirectories, bool overwrite, long fileSize = 0) => GetFileSystemProvider(fileSystemPathFile).OpenWrite(fileSystemPathFile, createDirectories, overwrite, fileSize);
	}
}