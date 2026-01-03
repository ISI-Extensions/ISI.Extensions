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
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ISI.Extensions.Extensions;

namespace ISI.Extensions
{
	public partial class IO
	{
		public partial class Path
		{
			[StructLayout(LayoutKind.Sequential)]
			struct RM_UNIQUE_PROCESS
			{
				public int dwProcessId;
				public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
			}

			const int RmRebootReasonNone = 0;
			const int CCH_RM_MAX_APP_NAME = 255;
			const int CCH_RM_MAX_SVC_NAME = 63;

			enum RM_APP_TYPE
			{
				RmUnknownApp = 0,
				RmMainWindow = 1,
				RmOtherWindow = 2,
				RmService = 3,
				RmExplorer = 4,
				RmConsole = 5,
				RmCritical = 1000
			}

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			struct RM_PROCESS_INFO
			{
				public RM_UNIQUE_PROCESS Process;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
				public string strAppName;

				[MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
				public string strServiceShortName;

				public RM_APP_TYPE ApplicationType;
				public uint AppStatus;
				public uint TSSessionId;
				[MarshalAs(UnmanagedType.Bool)] public bool bRestartable;
			}

			[DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
			private static extern int RmRegisterResources(uint pSessionHandle, uint nFiles, string[] rgsFilenames, uint nApplications, [In] RM_UNIQUE_PROCESS[] rgApplications, uint nServices, string[] rgsServiceNames);

			[DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
			static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);

			[DllImport("rstrtmgr.dll")]
			static extern int RmEndSession(uint pSessionHandle);

			[DllImport("rstrtmgr.dll")]
			static extern int RmGetList(uint dwSessionHandle, out uint pnProcInfoNeeded, ref uint pnProcInfo, [In, Out] RM_PROCESS_INFO[] rgAffectedApps, ref uint lpdwRebootReasons);

			// http://stackoverflow.com/questions/1304/how-to-check-for-file-lock
			public static System.Diagnostics.Process[] GetLockingProcesses(IEnumerable<string> paths)
			{
				var processes = new List<System.Diagnostics.Process>();

				var key = Guid.NewGuid().ToString();

				if (RmStartSession(out var handle, 0, key) != 0)
				{
					throw new("Could not begin restart session.  Unable to determine file locker.");
				}

				try
				{
					const int ERROR_MORE_DATA = 234;
					uint pnProcInfoNeeded = 0;
					uint pnProcInfo = 0;
					uint lpdwRebootReasons = RmRebootReasonNone;

					var resources = paths.ToNullCheckedArray(NullCheckCollectionResult.Empty);

					if (RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null) != 0)
					{
						throw new("Could not register resource.");
					}

					//Note: there's a race condition here -- the first call to RmGetList() returns
					//      the total number of process. However, when we call RmGetList() again to get
					//      the actual processes this number may have increased.
					var rmGetListResponse = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);
					if (rmGetListResponse == ERROR_MORE_DATA)
					{
						// Create an array to store the process results
						var processInfo = new RM_PROCESS_INFO[pnProcInfoNeeded];
						pnProcInfo = pnProcInfoNeeded;

						// Get the list
						if (RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons) == 0)
						{
							processes = new((int)pnProcInfo);

							// Enumerate all of the results and add them to the 
							// list to be returned
							for (var i = 0; i < pnProcInfo; i++)
							{
								try
								{
									processes.Add(System.Diagnostics.Process.GetProcessById(processInfo[i].Process.dwProcessId));
								}
								// catch the error -- in case the process is no longer running
								catch (ArgumentException)
								{
								}
							}
						}
						else
						{
							throw new("Could not list processes locking resource.");
						}
					}
					else if (rmGetListResponse != 0)
					{
						throw new("Could not list processes locking resource. Failed to get size of result.");
					}
				}
				finally
				{
					RmEndSession(handle);
				}

				return processes.ToArray();
			}
		}
	}
}