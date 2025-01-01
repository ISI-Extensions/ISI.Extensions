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
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class IO
	{
		public partial class Path
		{
			private static string _pathRoot = null;
			public static string PathRoot => _pathRoot ??= System.IO.Path.GetPathRoot(System.Reflection.Assembly.GetExecutingAssembly().CodeBase.TrimStart("file:///"));

			private static string _dataRoot = null;
			public static string DataRoot => _dataRoot ??= GetDataRoot();

			private static string GetDataRoot()
			{
				var dataRoot = System.Environment.GetEnvironmentVariable("AppDataRoot");

				if (string.IsNullOrWhiteSpace(dataRoot))
				{
					var localAppData = System.Environment.GetEnvironmentVariable("LocalAppData");

					if (!string.IsNullOrWhiteSpace(localAppData))
					{
						dataRoot = System.IO.Path.Combine(localAppData, "Data");
					}
				}

				if (string.IsNullOrWhiteSpace(dataRoot) && (Environment.OSVersion.Platform == PlatformID.Unix))
				{
					dataRoot =  "/etc";
				}

				if (string.IsNullOrWhiteSpace(dataRoot))
				{
					dataRoot = System.IO.Path.Combine(PathRoot, "Data");
				}

				return string.Format("{0}{1}", dataRoot.TrimEnd(System.IO.Path.DirectorySeparatorChar), System.IO.Path.DirectorySeparatorChar);
			}
		}
	}
}