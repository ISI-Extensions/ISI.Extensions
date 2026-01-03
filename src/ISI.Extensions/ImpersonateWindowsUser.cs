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
using System.Text;

namespace ISI.Extensions
{
	public class ImpersonateWindowsUser
	{
		[System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

		private enum LogonType
		{
			/// <summary>
			/// This logon type is intended for users who will be interactively using the computer, such as a user being logged on  
			/// by a terminal server, remote shell, or similar process.
			/// This logon type has the additional expense of caching logon information for disconnected operations;
			/// therefore, it is inappropriate for some client/server applications,
			/// such as a mail server.
			/// </summary>
			LOGON32_LOGON_INTERACTIVE = 2,

			/// <summary>
			/// This logon type is intended for high performance servers to authenticate plaintext passwords.

			/// The LogonUser function does not cache credentials for this logon type.
			/// </summary>
			LOGON32_LOGON_NETWORK = 3,

			/// <summary>
			/// This logon type is intended for batch servers, where processes may be executing on behalf of a user without
			/// their direct intervention. This type is also for higher performance servers that process many plaintext
			/// authentication attempts at a time, such as mail or Web servers.
			/// The LogonUser function does not cache credentials for this logon type.
			/// </summary>
			LOGON32_LOGON_BATCH = 4,

			/// <summary>
			/// Indicates a service-type logon. The account provided must have the service privilege enabled.
			/// </summary>
			LOGON32_LOGON_SERVICE = 5,

			/// <summary>
			/// This logon type is for GINA DLLs that log on users who will be interactively using the computer.
			/// This logon type can generate a unique audit record that shows when the workstation was unlocked.
			/// </summary>
			LOGON32_LOGON_UNLOCK = 7,

			/// <summary>
			/// This logon type preserves the name and password in the authentication package, which allows the server to make
			/// connections to other network servers while impersonating the client. A server can accept plaintext credentials
			/// from a client, call LogonUser, verify that the user can access the system across the network, and still
			/// communicate with other servers.
			/// NOTE: Windows NT:  This value is not supported.
			/// </summary>
			LOGON32_LOGON_NETWORK_CLEARTEXT = 8,

			/// <summary>
			/// This logon type allows the caller to clone its current token and specify new credentials for outbound connections.
			/// The new logon session has the same local identifier but uses different credentials for other network connections.
			/// NOTE: This logon type is supported only by the LOGON32_PROVIDER_WINNT50 logon provider.
			/// NOTE: Windows NT:  This value is not supported.
			/// </summary>
			LOGON32_LOGON_NEW_CREDENTIALS = 9,
		}

		public enum LogonProvider
		{
			/// <summary>
			/// Use the standard logon provider for the system.
			/// The default security provider is negotiate, unless you pass NULL for the domain name and the user name
			/// is not in UPN format. In this case, the default provider is NTLM.
			/// NOTE: Windows 2000/NT:   The default security provider is NTLM.
			/// </summary>
			LOGON32_PROVIDER_DEFAULT = 0,
			LOGON32_PROVIDER_WINNT35 = 1,
			LOGON32_PROVIDER_WINNT40 = 2,
			LOGON32_PROVIDER_WINNT50 = 3
		}


		public static void RunImpersonated(string userName, string password, bool checkForUserNameAndOrPassword, Action action)
		{
			RunImpersonated(null, userName, password, checkForUserNameAndOrPassword, action);
		}

		public static void RunImpersonated(string domainName, string userName, string password, bool checkForUserNameAndOrPassword, Action action)
		{
			userName = (userName ?? string.Empty).Replace("/", "\\");
			var userParsed = new List<string>(userName.Split('\\'));
			if (userParsed.Count < 1) userParsed.Insert(0, string.Empty);
			if (userParsed.Count < 2) userParsed.Add(string.Empty);

			var executed = false;

			if (!checkForUserNameAndOrPassword || !string.IsNullOrEmpty(userName) || !string.IsNullOrEmpty(password))
			{
				if (LogonUser(userName, domainName, password, (int)LogonType.LOGON32_LOGON_NEW_CREDENTIALS, (int)LogonProvider.LOGON32_PROVIDER_DEFAULT, out var safeTokenHandle))
				{
					var windowsIdentity = new System.Security.Principal.WindowsIdentity(safeTokenHandle.DangerousGetHandle());

					System.Security.Principal.WindowsIdentity.RunImpersonated(windowsIdentity.AccessToken, action);

					executed = true;

					windowsIdentity.Dispose();
					safeTokenHandle.Dispose();
				}
			}

			if (!executed)
			{
				action();
			}
		}

		private sealed class SafeTokenHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
		{
			private SafeTokenHandle()
				: base(true)
			{
			}

			[System.Runtime.InteropServices.DllImport("kernel32.dll")]
			[System.Runtime.ConstrainedExecution.ReliabilityContract(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.Success)]
			[System.Security.SuppressUnmanagedCodeSecurity]
			[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
			private static extern bool CloseHandle(IntPtr handle);

			protected override bool ReleaseHandle()
			{
				return CloseHandle(handle);
			}
		}
	}
}