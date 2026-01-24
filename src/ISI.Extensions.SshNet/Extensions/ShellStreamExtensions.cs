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
using ISI.Extensions.Extensions;

//Sourced from https://github.com/sshnet/SSH.NET/issues/136
namespace ISI.Extensions.SshNet.Extensions
{
  public static class ShellStreamExtensions
  {
	  private const string endOfLine = "efba80ab-e75f-4533-8919-4d6d3aedf13c";

	  public static void WriteToShellStream(this Renci.SshNet.ShellStream shellStream, string command)
	  {
		  shellStream.WriteLine($"{command}; echo {endOfLine}");

		  while (shellStream.Length == 0)
		  {
			  System.Threading.Thread.Sleep(500);
		  }
	  }

	  public static string ReadFromShellStream(this Renci.SshNet.ShellStream shellStream)
	  {
		  var result = new StringBuilder();

		  var line = string.Empty;

		  while (!line.EndsWith(endOfLine))
		  {
			  line = shellStream.ReadLine();

				result.AppendLine(line);
		  }

		  return result.ToString();
	  }

	  public static string SendCommandToShellStream(this Renci.SshNet.ShellStream shellStream, string command)
	  {
			shellStream.WriteToShellStream(command);

			return shellStream.ReadFromShellStream();
	  }

	  public static void Sudo(this Renci.SshNet.ShellStream shellStream, string password)
	  {
		  var prompt = shellStream.Expect(new System.Text.RegularExpressions.Regex(@"[$]"));

		  shellStream.WriteLine("sudo su");

		  prompt = shellStream.Expect(new System.Text.RegularExpressions.Regex(@"([$#:])"));
		  
		  if (prompt.Contains(":"))
		  {
			  shellStream.WriteLine(password);

			  prompt = shellStream.Expect(new System.Text.RegularExpressions.Regex(@"[$#>]"));
		  }
	  }
	}
}
