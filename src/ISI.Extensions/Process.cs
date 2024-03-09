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
using Microsoft.Extensions.Logging;

namespace ISI.Extensions
{
	public class Process
	{
		public class ExecuteShellRequest
		{
			public string WorkingDirectory { get; set; }
			public string ProcessExeFullName { get; set; }
			public IEnumerable<string> Arguments { get; set; }
			public IDictionary<string, string> EnvironmentVariables { get; set; }

			public override string ToString() => string.Format("\"{0}\" {1}", ProcessExeFullName, string.Join(" ", Arguments ?? Array.Empty<string>()));
		}

		public static void ExecuteShell(string processExeFullName, params string[] arguments)
		{
			ExecuteShell(new ExecuteShellRequest()
			{
				ProcessExeFullName = processExeFullName,
				Arguments = arguments,
			});
		}

		public static void ExecuteShell(ExecuteShellRequest request)
		{
			var processStartInfo = new System.Diagnostics.ProcessStartInfo(request.ProcessExeFullName)
			{
				Arguments = string.Join(" ", request.Arguments ?? Array.Empty<string>()),
				WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal,
				UseShellExecute = true,
				CreateNoWindow = false,
			};

			if (request.EnvironmentVariables.NullCheckedAny())
			{
				foreach (var environmentVariable in request.EnvironmentVariables)
				{
					if (processStartInfo.EnvironmentVariables.ContainsKey(environmentVariable.Key))
					{
						processStartInfo.EnvironmentVariables[environmentVariable.Key] = environmentVariable.Value;
					}
					else
					{
						processStartInfo.EnvironmentVariables.Add(environmentVariable.Key, environmentVariable.Value);
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(request.WorkingDirectory))
			{
				processStartInfo.WorkingDirectory = request.WorkingDirectory;

				processStartInfo.WorkingDirectory = System.IO.Path.GetFullPath(processStartInfo.WorkingDirectory);

				while (!string.IsNullOrEmpty(processStartInfo.WorkingDirectory) && !System.IO.Directory.Exists(processStartInfo.WorkingDirectory))
				{
					processStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(processStartInfo.WorkingDirectory);
				}
			}

			var process = new System.Diagnostics.Process
			{
				EnableRaisingEvents = false,
				StartInfo = processStartInfo,
			};
			process.Start();
		}


		public delegate void ProcessResponse_OnChange(bool isAppend, string output);

		public class ProcessRequest
		{
			public Microsoft.Extensions.Logging.ILogger Logger { get; set; }
			public string WorkingDirectory { get; set; }
			public string ProcessExeFullName { get; set; }
			public IEnumerable<string> Arguments { get; set; }
			public IDictionary<string, string> EnvironmentVariables { get; set; }

			public override string ToString() => string.Format("\"{0}\" {1}", ProcessExeFullName, string.Join(" ", Arguments ?? Array.Empty<string>()));
		}

		public class ProcessResponse
		{
			private string _output = string.Empty;
			public string Output
			{
				get => _output;
				set { _output = value; OnChange?.Invoke(false, value); }
			}

			public int ExitCode { get; set; }
			public bool Errored => (ExitCode != 0);

			public ProcessResponse_OnChange OnChange { get; set; } = null;

			public void Reset()
			{
				Output = string.Empty;
				ExitCode = 0;
			}

			public void AppendLine(string output)
			{
				output += "\r\n";
				_output += output;

				OnChange?.Invoke(true, output);
			}
		}

		public static ProcessResponse WaitForProcessResponse(string processExeFullName, params string[] arguments)
		{
			return WaitForProcessResponse(new ProcessRequest()
			{
				ProcessExeFullName = processExeFullName,
				Arguments = arguments,
			});
		}

		public static ProcessResponse WaitForProcessResponse(ProcessRequest request)
		{
			var response = new ProcessResponse();

			request.Logger ??= new ConsoleLogger();

			var output = new StringBuilder();

			var processStartInfo = new System.Diagnostics.ProcessStartInfo(request.ProcessExeFullName)
			{
				Arguments = string.Join(" ", request.Arguments ?? Array.Empty<string>()),
				WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
				UseShellExecute = false,
				CreateNoWindow = true,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
			};

			if (request.EnvironmentVariables.NullCheckedAny())
			{
				foreach (var environmentVariable in request.EnvironmentVariables)
				{
					if (processStartInfo.EnvironmentVariables.ContainsKey(environmentVariable.Key))
					{
						processStartInfo.EnvironmentVariables[environmentVariable.Key] = environmentVariable.Value;
					}
					else
					{
						processStartInfo.EnvironmentVariables.Add(environmentVariable.Key, environmentVariable.Value);
					}
				}
			}

			if (!string.IsNullOrWhiteSpace(request.WorkingDirectory))
			{
				processStartInfo.WorkingDirectory = request.WorkingDirectory;

				processStartInfo.WorkingDirectory = System.IO.Path.GetFullPath(processStartInfo.WorkingDirectory);

				while (!string.IsNullOrEmpty(processStartInfo.WorkingDirectory) && !System.IO.Directory.Exists(processStartInfo.WorkingDirectory))
				{
					processStartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(processStartInfo.WorkingDirectory);
				}
			}

			using (var process = System.Diagnostics.Process.Start(processStartInfo))
			{
				process.OutputDataReceived += (sender, dataReceivedEventArgs) =>
				{
					var data = dataReceivedEventArgs.Data;

					if (!string.IsNullOrWhiteSpace(data))
					{
						output.AppendLine(data);

						data = data.Replace("{", "{{").Replace("}", "}}");

						request.Logger.LogInformation(data);
					}
				};

				process.ErrorDataReceived += (sender, dataReceivedEventArgs) =>
				{
					var data = dataReceivedEventArgs.Data;

					if (!string.IsNullOrEmpty(data))
					{
						output.AppendLine(data);

						data = data.Replace("{", "{{").Replace("}", "}}");

						request.Logger.LogError(data);
					}
				};

				process.BeginOutputReadLine();
				process.BeginErrorReadLine();
				process.WaitForExit();

				response.ExitCode = process.ExitCode;

				response.Output = output.ToString();

				return response;
			}
		}
	}
}
