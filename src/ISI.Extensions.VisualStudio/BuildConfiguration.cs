#region Copyright & License
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
using System.Text;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.VisualStudio
{
	public class BuildConfiguration
	{
		public string Configuration { get; set; }

		public MSBuildPlatform MSBuildPlatform { get; set; } = MSBuildPlatform.Automatic;

		public BuildConfiguration()
		{

		}

		public BuildConfiguration(string configuration, MSBuildPlatform msBuildPlatform)
		{
			Configuration = configuration;
			MSBuildPlatform = msBuildPlatform;
		}

		public BuildConfiguration(string buildConfiguration)
		{
			var parsedBuildConfiguration = buildConfiguration.Split(['|'], StringSplitOptions.RemoveEmptyEntries);

			Configuration = (parsedBuildConfiguration.Length > 0 ? parsedBuildConfiguration[0] : string.Empty);

			if (parsedBuildConfiguration.Length > 1)
			{
				if (string.Equals(parsedBuildConfiguration[1], "x64", StringComparison.InvariantCultureIgnoreCase))
				{
					MSBuildPlatform = MSBuildPlatform.x64;
				}
				else if (string.Equals(parsedBuildConfiguration[1], "x86", StringComparison.InvariantCultureIgnoreCase))
				{
					MSBuildPlatform = MSBuildPlatform.x86;
				}
				else
				{
					MSBuildPlatform = MSBuildPlatform.Automatic;
				}
			}
		}

		public override string ToString() => string.Format("{0}|{1}", Configuration, MSBuildPlatform.GetDescription());
	}
}
