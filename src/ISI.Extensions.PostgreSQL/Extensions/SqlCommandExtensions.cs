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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ISI.Extensions.PostgreSQL.Extensions
{
	public static partial class SqlCommandExtensions
	{
		public static bool SendCommandsToLogger { get; set; } = false;

		private static Microsoft.Extensions.Logging.ILogger _logger;
		public static Microsoft.Extensions.Logging.ILogger Logger => _logger ??= ISI.Extensions.ServiceLocator.Current?.GetService<Microsoft.Extensions.Logging.ILogger>() ?? new ISI.Extensions.NullLogger();

		public static string GetFormattedCommand(this Npgsql.NpgsqlCommand command)
		{
			var message = string.Format("CmdText: {0}", command.CommandText);

			var parameters = new List<string>();
			foreach (Npgsql.NpgsqlParameter commandParameter in command.Parameters)
			{
				parameters.Add(string.Format("{0}: {1}", commandParameter.ParameterName, commandParameter.Value));
			}

			if (parameters.Any())
			{
				message = string.Format("{0}\nParameters:\n{1}", message, string.Join("\n", parameters));
			}

			return message;
		}


		public static Npgsql.NpgsqlParameter GetSqlParameter(string parameterName, object parameterValue)
		{
			if (parameterValue == null)
			{
				parameterValue = DBNull.Value;
			}
			else
			{
				var type = parameterValue.GetType();

				var isNullable = (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));

				if (isNullable)
				{
					type = (new System.ComponentModel.NullableConverter(type)).UnderlyingType;
				}

				if (SendCommandsToLogger)
				{
					Logger.LogInformation("  Parameter Name: \"{0}\" Value: \"{1}\" Type: \"{2}\"", parameterName, parameterValue, type.Name);
				}

				if (type == typeof(bool))
				{
					parameterValue = ((bool)parameterValue ? 1 : 0);
				}

				if (type == typeof(DateTime))
				{
					return new(parameterName, System.Data.SqlDbType.DateTime2)
					{
						Value = parameterValue
					};
				}
				
				if (type == typeof(string))
				{
					return new (parameterName, System.Data.SqlDbType.VarChar)
					{
						Value = parameterValue
					};
				}

				//if (type == typeof(string))
				//{
				//	if (SendCommandsToLogger)
				//	{
				//		Logger.LogInformation("  used System.Data.SqlDbType.VarChar");
				//	}

				//	return new(parameterName, System.Data.SqlDbType.VarChar, -1)
				//	{
				//		Value = parameterValue
				//	};
				//}
			}

			return new(parameterName, parameterValue);
		}

		public static Npgsql.NpgsqlParameter GetSqlParameter(this KeyValuePair<string, object> parameter)
		{
			return GetSqlParameter(parameter.Key, parameter.Value);
		}

		public static void AddParameter(this Npgsql.NpgsqlCommand command, string parameterName, object parameterValue)
		{
			command.Parameters.Add(GetSqlParameter(parameterName, parameterValue));
		}

		public static void AddParameter(this Npgsql.NpgsqlCommand command, KeyValuePair<string, object> parameter)
		{
			command.Parameters.Add(parameter.GetSqlParameter());
		}

		public static void AddParameters(this Npgsql.NpgsqlCommand command, IEnumerable<KeyValuePair<string, object>> parameters)
		{
			if (parameters.NullCheckedAny())
			{
				foreach (var parameter in parameters)
				{
					command.AddParameter(parameter);
				}
			}
		}

		public static async Task<int> ExecuteNonQueryWithExceptionTracingAsync(this Npgsql.NpgsqlCommand command, System.Threading.CancellationToken cancellationToken = default)
		{
			try
			{
				if (SendCommandsToLogger)
				{
					Logger.LogInformation(command.GetFormattedCommand());
				}

				return await command.ExecuteNonQueryAsync(cancellationToken);
			}
			catch (Exception exception)
			{
				var message = string.Format("Error: {0}\n{1}", exception.Message, command.GetFormattedCommand());

				Logger?.LogError(exception, message);

				exception = new(message, exception);

				throw exception;
			}
		}

		public static async Task<object> ExecuteScalarWithExceptionTracingAsync(this Npgsql.NpgsqlCommand command, System.Threading.CancellationToken cancellationToken = default)
		{
			try
			{
				if (SendCommandsToLogger)
				{
					Logger.LogInformation(command.GetFormattedCommand());
				}

				return await command.ExecuteScalarAsync(cancellationToken);
			}
			catch (Exception exception)
			{
				var message = string.Format("Error: {0}\n{1}", exception.Message, command.GetFormattedCommand());

				Logger?.LogError(exception, message);

				exception = new(message, exception);

				throw exception;
			}
		}

		public static async Task<Npgsql.NpgsqlDataReader> ExecuteReaderWithExceptionTracingAsync(this Npgsql.NpgsqlCommand command, System.Threading.CancellationToken cancellationToken = default)
		{
			try
			{
				if (SendCommandsToLogger)
				{
					Logger.LogInformation(command.GetFormattedCommand());
				}

				return await command.ExecuteReaderAsync(cancellationToken);
			}
			catch (Exception exception)
			{
				var message = string.Format("Error: {0}\n{1}", exception.Message, command.GetFormattedCommand());

				Logger?.LogError(exception, message);

				exception = new(message, exception);

				throw exception;
			}
		}
	}
}
