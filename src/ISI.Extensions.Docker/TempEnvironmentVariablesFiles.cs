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

namespace ISI.Extensions.Docker
{
	internal class TempEnvironmentVariablesFile : IDisposable
	{
		public string Content { get; }
		public string FullName { get; }

		private string _fileName = null;
		public string FileName
		{
			get
			{
				if (string.IsNullOrWhiteSpace(_fileName))
				{
					System.IO.File.WriteAllText(FullName, Content);

					_fileName = System.IO.Path.GetFileName(FullName);
				}

				return _fileName;
			}
		}

		private InvariantCultureIgnoreCaseStringDictionary<string> _environmentVariables = null;
		public InvariantCultureIgnoreCaseStringDictionary<string> GetEnvironmentVariables() => _environmentVariables ??= ParseEnvironmentContent(Content);

		public TempEnvironmentVariablesFile(string content, string composeDirectory)
		{
			if (string.IsNullOrWhiteSpace(composeDirectory))
			{
				composeDirectory = System.IO.Path.GetTempPath();
			}

			FullName = System.IO.Path.Combine(composeDirectory, $"Temp-{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}.env");

			Content = content;
		}

		public TempEnvironmentVariablesFile(InvariantCultureIgnoreCaseStringDictionary<string> environmentVariables, string composeDirectory)
		{
			if (string.IsNullOrWhiteSpace(composeDirectory))
			{
				composeDirectory = System.IO.Path.GetTempPath();
			}

			FullName = System.IO.Path.Combine(composeDirectory, $"Temp-{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}.env");

			Content = string.Join(Environment.NewLine, environmentVariables.Select(keyValue => $"{keyValue.Key}={keyValue.Value}"));
		}

		protected InvariantCultureIgnoreCaseStringDictionary<string> ParseEnvironmentContent(string content)
		{
			var environmentVariables = new InvariantCultureIgnoreCaseStringDictionary<string>();

			var lines = content.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

			for (var lineIndex = 0; lineIndex < lines.Length; lineIndex++)
			{
				var line = lines[lineIndex].Trim();

				if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("#"))
				{
					var lineParts = line.Split(['='], 2);

					if (lineParts.Length == 2)
					{
						if (environmentVariables.ContainsKey(lineParts[0]))
						{
							environmentVariables[lineParts[0]] = lineParts[1];
						}
						else
						{
							environmentVariables.Add(lineParts[0], lineParts[1]);
						}
					}
				}
			}

			return environmentVariables;
		}

		public void Dispose()
		{
			if (System.IO.File.Exists(FullName))
			{
				System.IO.File.Delete(FullName);
			}
		}
	}

	internal class TempEnvironmentVariablesFiles : IDisposable
	{
		internal TempEnvironmentVariablesFile[] EnvironmentFiles { get; }

		private InvariantCultureIgnoreCaseStringDictionary<string> _environmentVariables = null;
		internal InvariantCultureIgnoreCaseStringDictionary<string> EnvironmentVariables => _environmentVariables ??= GetEnvironmentVariables();

		public TempEnvironmentVariablesFiles(string composeDirectory, IEnumerable<string> environmentFileFullNames, IEnumerable<string> environmentFileContents, InvariantCultureIgnoreCaseStringDictionary<string> environmentVariables)
		{
			var environmentFiles = environmentFileFullNames.ToNullCheckedList(environmentFileFullName =>
			{
				if (!System.IO.File.Exists(environmentFileFullName))
				{
					throw new System.IO.FileNotFoundException($"File not found: \"{environmentFileFullName}\"");
				}

				return new TempEnvironmentVariablesFile(System.IO.File.ReadAllText(environmentFileFullName), composeDirectory);
			}, NullCheckCollectionResult.Empty);

			if (environmentFileContents.NullCheckedAny())
			{
				foreach (var environmentFileContent in environmentFileContents)
				{
					if (!string.IsNullOrWhiteSpace(environmentFileContent))
					{
						environmentFiles.Add(new TempEnvironmentVariablesFile(environmentFileContent, composeDirectory));
					}
				}
			}

			if (environmentVariables.NullCheckedAny())
			{
				environmentFiles.Add(new TempEnvironmentVariablesFile(environmentVariables, composeDirectory));
			}

			EnvironmentFiles = environmentFiles.ToNullCheckedArray();
		}

		public string[] GetFileNames() => EnvironmentFiles.ToNullCheckedArray(environmentFile => environmentFile.FileName);

		public string[] GetDockerComposeArguments() => GetFileNames().ToNullCheckedArray(environmentFileName => $"--env-file={environmentFileName}");

		protected InvariantCultureIgnoreCaseStringDictionary<string> GetEnvironmentVariables()
		{
			var environmentVariables = new InvariantCultureIgnoreCaseStringDictionary<string>();

			if (EnvironmentFiles.NullCheckedAny())
			{
				foreach (var environmentFile in EnvironmentFiles)
				{
					if (!string.IsNullOrWhiteSpace(environmentFile.Content))
					{
						environmentVariables.AddRange(environmentFile.GetEnvironmentVariables());
					}
				}
			}

			return environmentVariables;
		}

		public void Dispose()
		{
			foreach (var environmentFile in EnvironmentFiles)
			{
				environmentFile?.Dispose();
			}
		}
	}
}
