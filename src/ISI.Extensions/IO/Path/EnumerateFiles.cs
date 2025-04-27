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
 
using ISI.Extensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISI.Extensions
{
	public partial class IO
	{
		public partial class Path
		{
		public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, string[] ignorePatterns, int maxDepth = int.MaxValue)
		{
			return EnumerateFiles(path, searchPattern, ignorePatterns, 0, maxDepth, false);
		}


		private static IEnumerable<string> EnumerateFiles(string path, string searchPattern, string[] ignorePatterns, int depth, int maxDepth, bool stopIfFound)
		{
			if (depth >= maxDepth)
			{
				return [];
			}

			if ((ignorePatterns != null) && ignorePatterns.Any())
			{
				var filters = new List<Func<string, bool>>();

				foreach (var ignorePattern in ignorePatterns.Select(ignorePattern => ignorePattern.Trim()).Where(ignorePattern => !string.IsNullOrEmpty(ignorePattern)))
				{
					var startsWith = ignorePattern.EndsWith("*");
					var endsWith = ignorePattern.StartsWith("*");
					var pattern = ignorePattern.Trim('*');

					if (startsWith && endsWith)
					{
						if (!string.IsNullOrWhiteSpace(pattern))
						{
							filters.Add(fileName => (fileName.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) >= 0));
						}
					}
					else if (startsWith)
					{
						filters.Add(fileName => fileName.StartsWith(pattern, StringComparison.InvariantCultureIgnoreCase));
					}
					else if (endsWith)
					{
						filters.Add(fileName => fileName.EndsWith(pattern, StringComparison.InvariantCultureIgnoreCase));
					}
					else
					{
						filters.Add(fileName => string.Equals(fileName, pattern, StringComparison.InvariantCultureIgnoreCase));
					}
				}

				return EnumerateFiles(path, searchPattern, filters.ToArray(), depth + 1, maxDepth, stopIfFound);
			}

			return EnumerateFiles(path, searchPattern, Array.Empty<Func<string, bool>>(), depth + 1, maxDepth, stopIfFound);
		}

		private static IEnumerable<string> EnumerateFiles(string path, string searchPattern, Func<string, bool>[] ignorePatterns, int depth, int maxDepth, bool stopIfFound)
		{
			if (depth >= maxDepth)
			{
				return [];
			}

			var fileNames = new System.Collections.Concurrent.ConcurrentBag<string>();

			var pathInfo = new System.IO.DirectoryInfo(path);

			if (string.IsNullOrWhiteSpace(searchPattern))
			{
				foreach (var fileInfo in pathInfo.GetFiles().Where(fileInfo => !ignorePatterns.Any(ignorePattern => ignorePattern(fileInfo.Name))))
				{
					fileNames.Add(fileInfo.FullName);

					if (stopIfFound)
					{
						return fileNames;
					}
				}
			}
			else
			{
				foreach (var fileInfo in pathInfo.GetFiles(searchPattern, System.IO.SearchOption.TopDirectoryOnly).Where(fileInfo => !ignorePatterns.Any(ignorePattern => ignorePattern(fileInfo.Name))))
				{
					fileNames.Add(fileInfo.FullName);

					if (stopIfFound)
					{
						return fileNames;
					}
				}
			}

			if (depth < maxDepth)
			{
				var cancellationTokenSource = new System.Threading.CancellationTokenSource();

				try
				{
					Parallel.ForEach(
						pathInfo.GetDirectories().Where(directoryInfo => !ignorePatterns.Any(ignorePattern => ignorePattern(directoryInfo.Name))),
						new()
						{
							CancellationToken = cancellationTokenSource.Token,
						},
						directoryInfo =>
						{
							foreach (var fileName in EnumerateFiles(directoryInfo.FullName, searchPattern, ignorePatterns, depth + 1, maxDepth, stopIfFound))
							{
								fileNames.Add(fileName);

								if (stopIfFound)
								{
									cancellationTokenSource.Cancel();
								}
							}
						});

				}
				catch (OperationCanceledException operationCanceledException)
				{

				}
				catch (Exception exception)
				{
					throw;
				}
			}

			return fileNames;
		}
		}
	}
}