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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using DTOs = ISI.Extensions.VisualStudio.DataTransferObjects.PackagerApi;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.VisualStudio
{
	public partial class PackagerApi
	{
		private enum ExcludeFileMatchType
		{
			ExactMatch,
			Contains,
			StartsWith,
			EndsWith,
		}

		private class ExcludeFileDefinition
		{
			public string Exclude { get; set; }
			public ExcludeFileMatchType MatchType { get; set; }
		}

		private ExcludeFileDefinition[] GetExcludeFileDefinitions(IEnumerable<string> excludeFiles)
		{
			var excludeFileDefinitions = new List<ExcludeFileDefinition>();

			foreach (var excludeFile in excludeFiles)
			{
				if (excludeFile.StartsWith("*") && excludeFile.EndsWith("*"))
				{
					excludeFileDefinitions.Add(new()
					{
						Exclude = excludeFile.Trim('*'),
						MatchType = ExcludeFileMatchType.Contains,
					});
				}
				else if (excludeFile.StartsWith("*"))
				{
					excludeFileDefinitions.Add(new()
					{
						Exclude = excludeFile.Trim('*'),
						MatchType = ExcludeFileMatchType.StartsWith,
					});
				}
				else if (excludeFile.EndsWith("*"))
				{
					excludeFileDefinitions.Add(new()
					{
						Exclude = excludeFile.Trim('*'),
						MatchType = ExcludeFileMatchType.EndsWith,
					});
				}
				else
				{
					excludeFileDefinitions.Add(new()
					{
						Exclude = excludeFile.Trim('*'),
						MatchType = ExcludeFileMatchType.ExactMatch,
					});
				}
			}

			return excludeFileDefinitions.ToArray();
		}

		private bool ShouldExclude(IEnumerable<ExcludeFileDefinition> excludeFileDefinitions, string fileName)
		{
			foreach (var excludeFileDefinition in excludeFileDefinitions)
			{
				switch (excludeFileDefinition.MatchType)
				{
					case ExcludeFileMatchType.ExactMatch:
						if (string.Equals(excludeFileDefinition.Exclude, fileName, StringComparison.InvariantCultureIgnoreCase))
						{
							return true;
						}
						break;

					case ExcludeFileMatchType.Contains:
						if (excludeFileDefinition.Exclude.IndexOf(fileName, StringComparison.InvariantCultureIgnoreCase) >= 0)
						{
							return true;
						}
						break;

					case ExcludeFileMatchType.StartsWith:
						if (excludeFileDefinition.Exclude.StartsWith(fileName, StringComparison.InvariantCultureIgnoreCase))
						{
							return true;
						}
						break;

					case ExcludeFileMatchType.EndsWith:
						if (excludeFileDefinition.Exclude.EndsWith(fileName, StringComparison.InvariantCultureIgnoreCase))
						{
							return true;
						}
						break;
				}
			}

			return false;
		}
	}
}