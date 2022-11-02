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
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public class SystemInformation
	{
		public static string GetAssemblyVersion(System.Reflection.Assembly assembly)
		{
			string result = null;

			var customAttributes = assembly.GetCustomAttributes(true);

			#region InformationalVersion
			if ((result == null) && (customAttributes != null))
			{
				foreach (var customAttribute in customAttributes)
				{
					if (customAttribute is System.Reflection.AssemblyInformationalVersionAttribute)
					{
						var attribute = (System.Reflection.AssemblyInformationalVersionAttribute)customAttribute;
						result = attribute.InformationalVersion;
					}
				}
			}
			#endregion

			#region FileVersion
			if ((result == null) && (customAttributes != null))
			{
				foreach (var customAttribute in customAttributes)
				{
					if (customAttribute is System.Reflection.AssemblyFileVersionAttribute)
					{
						var attribute = (System.Reflection.AssemblyFileVersionAttribute)customAttribute;
						result = attribute.Version;
					}
				}
			}
			#endregion

			#region Version
			if ((result == null) && (customAttributes != null))
			{
				foreach (var customAttribute in customAttributes)
				{
					if (customAttribute is System.Reflection.AssemblyVersionAttribute)
					{
						var attribute = (System.Reflection.AssemblyVersionAttribute)customAttribute;
						result = attribute.Version;
					}
				}
			}
			#endregion

			return result ?? assembly.GetName().Version.ToString();
		}

		private static DateTime _jan1st2000 = new(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		public static DateTime? GetAssemblyBuildDate(System.Reflection.Assembly assembly)
		{
			var version = new System.Version(GetAssemblyVersion(assembly));

			var buildDateTimeFromVersion = _jan1st2000.Add(new(
				TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
				TimeSpan.TicksPerSecond * 2 * version.Revision)); /* seconds since midnight, (multiply by 2 to get original) */

			if (buildDateTimeFromVersion > new DateTime(2010, 1, 1))
			{
				return buildDateTimeFromVersion;
			}

			try
			{
				return System.IO.File.GetLastWriteTimeUtc(assembly.Location);
			}
#pragma warning disable CS0168 // Variable is declared but never used
			catch (Exception exception)
#pragma warning restore CS0168 // Variable is declared but never used
			{
			}

			return null;
		}

		private static bool? _isRunningIn64BitCLR = null;
		public static bool IsRunningIn64BitCLR()
		{
			if (!_isRunningIn64BitCLR.HasValue)
			{
				_isRunningIn64BitCLR = System.Runtime.InteropServices.Marshal.SizeOf(typeof(IntPtr)) == 8;
			}

			return _isRunningIn64BitCLR.GetValueOrDefault();
		}
	}
}
