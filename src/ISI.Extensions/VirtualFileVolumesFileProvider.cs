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

namespace ISI.Extensions
{
	public class VirtualFileVolumesFileProvider : Microsoft.Extensions.FileProviders.IFileProvider
	{
		private static IDictionary<string, IVirtualFileVolume> _virtualFileVolumes { get; } = new System.Collections.Concurrent.ConcurrentDictionary<string, IVirtualFileVolume>(StringComparer.InvariantCultureIgnoreCase);

		public IEnumerable<IVirtualFileVolume> VirtualFileVolumes => _virtualFileVolumes.Values;

		public static void RegisterEmbeddedVolume(Type typeToFindResourceAssembly)
		{
			var resourceAssembly = typeToFindResourceAssembly.Assembly;

			var embeddedVolumeNamespace = resourceAssembly.FullName.Split(new[] { ',' }).First();

			var pathPrefix = FormatPathPrefix(embeddedVolumeNamespace);

			if (!_virtualFileVolumes.ContainsKey(pathPrefix))
			{
				_virtualFileVolumes.Add(pathPrefix, new EmbeddedVolume(resourceAssembly, embeddedVolumeNamespace));
			}
		}

		public static string GetPathPrefix(Type typeToFindResourceAssembly)
		{
			var resourceAssembly = typeToFindResourceAssembly.Assembly;

			var embeddedVolumeNamespace = resourceAssembly.FullName.Split(new[] { ',' }).First();

			return FormatPathPrefix(embeddedVolumeNamespace);
		}

		protected static string FormatPathPrefix(string embeddedVolumeNamespace)
		{
			return string.Format("{0}{1}{0}", EmbeddedVolume.DirectorySeparator, embeddedVolumeNamespace);
		}

		protected static (string PathPrefix, string ResourcePath) ParseSubpath(string subpath)
		{
			var pieces = subpath.Substring(1).Split([EmbeddedVolume.DirectorySeparator], StringSplitOptions.RemoveEmptyEntries).ToList();

			var pathPrefix = FormatPathPrefix(pieces.First()); //ISI.Extensions.Configuration.GetUrlRoot(typeToFindResourceAssembly);

			if (_virtualFileVolumes.ContainsKey(pathPrefix))
			{
				pieces.RemoveAt(0);

				var path = string.Join(EmbeddedVolume.DirectorySeparator, pieces);

				var resourcePath = GetEmbeddedResourceName(path);

				return (PathPrefix: pathPrefix, ResourcePath: resourcePath);
			}

			return (PathPrefix: string.Empty, ResourcePath: string.Empty);
		}

		public static string GetEmbeddedResourceName(string fileName, bool isDirectoryName = false)
		{
			var pieces = new List<string>(fileName.Split(["/", "\\"], StringSplitOptions.RemoveEmptyEntries));

			{
				var index = pieces.IndexOf("..");
				while (index >= 0)
				{
					pieces.RemoveAt(index);
					pieces.RemoveAt(index - 1);
					index = pieces.IndexOf("..");
				}
			}

			var lastPieceIndex = pieces.Count - 1;

			if (lastPieceIndex > 0)
			{
				for (var index = 0; index <= lastPieceIndex; index++)
				{
					pieces[index] = ("0123456789".Contains(pieces[index].Substring(0, 1)) ? $"_{pieces[index]}" : pieces[index]);
					if (!isDirectoryName && (index < lastPieceIndex))
					{
						pieces[index] = pieces[index].Replace("-", "_");
					}
				}
			}

			fileName = string.Join(".", pieces).ToLower();

			return fileName.ToLower();
		}
		
		public Microsoft.Extensions.FileProviders.IFileInfo GetFileInfo(string subpath)
		{
			var parsedSubpath = ParseSubpath(subpath);

			if (_virtualFileVolumes.TryGetValue(parsedSubpath.PathPrefix, out var embeddedVolume))
			{
				return embeddedVolume.GetFileInfo(parsedSubpath.ResourcePath);
			}

			return new Microsoft.Extensions.FileProviders.NotFoundFileInfo(subpath);
		}

		public Microsoft.Extensions.FileProviders.IDirectoryContents GetDirectoryContents(string subpath)
		{
			return null;
		}

		public Microsoft.Extensions.Primitives.IChangeToken Watch(string filter)
		{
			return Microsoft.Extensions.FileProviders.NullChangeToken.Singleton;
		}
	}
}
