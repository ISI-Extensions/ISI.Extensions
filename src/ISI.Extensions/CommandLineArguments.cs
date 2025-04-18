﻿#region Copyright & License
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

namespace ISI.Extensions
{
	public class CommandLineArguments
	{
		public string Command { get; }

		private readonly InvariantCultureIgnoreCaseStringDictionary<string> _parameters = new ();
		private readonly IList<string> _values = new List<string>();

		public CommandLineArguments(string command, InvariantCultureIgnoreCaseStringDictionary<string> parameters = null, IList<string> values = null)
		{
			Command = command;
			_parameters = parameters ?? new ();
			_values = values ?? new List<string>();
		}

		public CommandLineArguments(string[] args, int originIndex = 0)
		{
			var parameterName = string.Empty;
			var isFirstArg = true;

			while (originIndex < args.Length)
			{
				var arg = args[originIndex++];

				if (isFirstArg && string.IsNullOrEmpty(Command) && !arg.StartsWith("-") && !arg.StartsWith("/") && !arg.StartsWith("\\"))
				{
					Command = arg;
				}
				else
				{
					if (arg.StartsWith("-") || arg.StartsWith("/") || arg.StartsWith("\\"))
					{
						parameterName = arg.TrimStart('-', '/', '\\');
						_parameters.Add(parameterName, string.Empty);
					}
					else
					{
						if (!string.IsNullOrEmpty(parameterName))
						{
							_parameters[parameterName] = arg;
							parameterName = string.Empty;
						}
						else
						{
							_values.Add(arg);
						}
					}
				}

				isFirstArg = false;
			}
		}

		public void AddParameter(string parameterName, IEnumerable<string> parameterValues, string delimiter = ";")
		{
			var parameterValue = string.Join(delimiter, parameterValues);

			_parameters.Add(parameterName, parameterValue);
		}

		public void AddParameter(string parameterName, string parameterValue)
		{
			AddParameter(parameterName, [parameterValue]);
		}

		public void AddValue(string value)
		{
			_values.Add(value);
		}

		public bool HasOption(string optionName) => _parameters.ContainsKey(optionName);

		public bool TryGetParameterValue(string parameterName, out string parameterValue) => _parameters.TryGetValue(parameterName, out parameterValue);

		public bool TryGetParameterValues(string parameterName, out string[] parameterValues, string[] delimiters, bool trimValues = true, bool removeEmptyValues = true)
		{
			if (_parameters.TryGetValue(parameterName, out var value))
			{
				parameterValues = value.Split(delimiters, StringSplitOptions.None);

				if (trimValues)
				{
					parameterValues = parameterValues.Select(v => v.Trim()).ToArray();
				}

				if (removeEmptyValues)
				{
					parameterValues = parameterValues.Where(v => !string.IsNullOrEmpty(v)).ToArray();
				}

				return true;
			}

			parameterValues = null;

			return false;
		}

		public bool TryGetParameterValues(string parameterName, out string[] parameterValues, string delimiter = ";", bool trimValues = true, bool removeEmptyValues = true) => TryGetParameterValues(parameterName, out parameterValues, [delimiter], trimValues, removeEmptyValues);

		public string GetParameterValue(string parameterName, string defaultValue = "")
		{
			if (TryGetParameterValue(parameterName, out var parameterValue))
			{
				return parameterValue;
			}

			return defaultValue;
		}

		public string[] GetParameterValues(string parameterName, string[] delimiters, bool trimValues = true, bool removeEmptyValues = true, string[] defaultValues = null)
		{
			if (TryGetParameterValues(parameterName, out var parameterValues, delimiters, trimValues, removeEmptyValues))
			{
				return parameterValues;
			}

			return defaultValues;
		}

		public string[] GetParameterValues(string parameterName, string delimiter = ";", bool trimValues = true, bool removeEmptyValues = true, string[] defaultValues = null) => GetParameterValues(parameterName, [delimiter], trimValues, removeEmptyValues, defaultValues);

		public string[] Values => _values.ToArray();

		public string ToArguments()
		{
			var arguments = new List<string>();

			arguments.Add(Command);
			arguments.Add(string.Join(" ", _parameters.Select(parameter => string.Format("\"-{0}\" \"{1}\"", parameter.Key, parameter.Value))));
			arguments.Add(string.Join(" ", _values.Select(value => string.Format("\"{0}\"", value))));

			var result = string.Join(" ", arguments);

			return result;
		}
	}
}
