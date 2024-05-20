﻿#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
	internal class TempEnvironmentFile : IDisposable
	{
		public string FullName { get; }
		public string FileName { get; }

		public TempEnvironmentFile(string sourceFullName, string composeDirectory)
		{
			FileName = $"Temp-{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}.env";
			FullName = System.IO.Path.Combine(composeDirectory, FileName);

			System.IO.File.Copy(sourceFullName, FullName);
		}

		public TempEnvironmentFile(InvariantCultureIgnoreCaseStringDictionary<string> environmentVariables, string composeDirectory)
		{
			FileName = $"Temp-{Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.NoFormatting)}.env";
			FullName = System.IO.Path.Combine(composeDirectory, FileName);

			System.IO.File.WriteAllLines(FullName, environmentVariables.Select(keyValue => $"{keyValue.Key}={keyValue.Value}"));
		}

		public void Dispose()
		{
			System.IO.File.Delete(FullName);
		}
	}

	internal class TempEnvironmentFiles : IDisposable
	{
		protected TempEnvironmentFile[] EnvironmentFiles { get; }

		public TempEnvironmentFiles(string composeDirectory, IEnumerable<string> sourceFullNames, InvariantCultureIgnoreCaseStringDictionary<string> environmentVariables)
		{
			var environmentFiles = sourceFullNames.ToNullCheckedList(sourceFullName => new TempEnvironmentFile(sourceFullName, composeDirectory), NullCheckCollectionResult.Empty);

			if (environmentVariables.NullCheckedAny())
			{
				environmentFiles.Add(new TempEnvironmentFile(environmentVariables, composeDirectory));
			}

			EnvironmentFiles = environmentFiles.ToNullCheckedArray();

			foreach (var environmentFile in EnvironmentFiles)
			{
				Console.WriteLine(environmentFile.FullName);
			}
		}

		public string[] GetFileNames() => EnvironmentFiles.ToNullCheckedArray(environmentFile => environmentFile.FileName);

		public string[] GetDockerComposeArguments() => GetFileNames().ToNullCheckedArray(environmentFileName => $"--env-file={environmentFileName}");

		public void Dispose()
		{
			foreach (var environmentFile in EnvironmentFiles)
			{
				environmentFile?.Dispose();
			}
		}
	}
}
