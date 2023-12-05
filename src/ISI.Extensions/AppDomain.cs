#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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

namespace ISI.Extensions
{
	public class AppDomain
	{
		private static readonly HashSet<string> _customAppDomainCleanupPaths = new(StringComparer.InvariantCultureIgnoreCase);

		public static bool TryUseCustomAppDomain(UnhandledExceptionEventHandler unhandledExceptionEventHandler, out int exitCode)
		{
			exitCode = 0;

			if (System.AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				var customDomain = System.AppDomain.CreateDomain(string.Format("{0}-{1}", System.Reflection.Assembly.GetEntryAssembly().GetType().Name, Guid.NewGuid().Formatted(GuidExtensions.GuidFormat.WithHyphens)));

				if (unhandledExceptionEventHandler != null)
				{
					customDomain.UnhandledException += unhandledExceptionEventHandler;
				}

				exitCode = customDomain.ExecuteAssembly(System.Reflection.Assembly.GetEntryAssembly().Location);

				System.AppDomain.Unload(customDomain);

				if (_customAppDomainCleanupPaths.Any())
				{
					foreach (var cleanupPath in _customAppDomainCleanupPaths)
					{
						if (System.IO.Directory.Exists(cleanupPath))
						{
							try
							{
								System.IO.Directory.Delete(cleanupPath, true);
							}
							catch
							{
							}
						}
						else if (System.IO.File.Exists(cleanupPath))
						{
							try
							{
								System.IO.File.Delete(cleanupPath);
							}
							catch
							{
							}
						}
					}
				}

				return true;
			}

			return false;
		}

		public static void AddCustomAppDomainCleanupPath(string cleanupPath)
		{
			if (!System.AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				_customAppDomainCleanupPaths.Add(cleanupPath);
			}
		}
	}
}
