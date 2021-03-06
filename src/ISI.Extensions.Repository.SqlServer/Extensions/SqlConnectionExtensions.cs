#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Repository.SqlServer.Extensions
{
	public static partial class SqlConnectionExtensions
	{
		public static async Task EnsureConnectionIsOpenAsync(this Microsoft.Data.SqlClient.SqlConnection connection)
		{
			if (connection.State != System.Data.ConnectionState.Open)
			{
				try
				{
					await connection.OpenAsync();
				}
				catch (Exception exception)
				{
					throw new Exception(string.Format("Error opening Connection to \"{0}\"", connection.ConnectionString), exception);
				}
			}
		}

		public static async Task<Version> GetServerVersionAsync(this Microsoft.Data.SqlClient.SqlConnection connection)
		{
			await connection.EnsureConnectionIsOpenAsync();

			var version = connection.ServerVersion;

			if (Version.TryParse(version, out var serverVersion))
			{
				return serverVersion;
			}

			var pieces = string.Format("{0}.....", version).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToNullCheckedArray( value => value.ToInt());

			return new Version(pieces[0], pieces[1], pieces[2], pieces[3]);
		}

		public static async Task<SqlServerCapabilities> GetSqlServerCapabilitiesAsync(this Microsoft.Data.SqlClient.SqlConnection connection)
		{
			var serverVersion = await connection.GetServerVersionAsync();

			return new SqlServerCapabilities()
			{
				ServerVersion = serverVersion,
				SupportsNativePaging = (serverVersion.Major >= 11),
			};
		}

		public static async Task<int> ExecuteNonQueryAsync(this Microsoft.Data.SqlClient.SqlConnection connection, string sql, string parameterName, object parameterValue, int? commandTimeout = null)
		{
			return await ExecuteNonQueryAsync(connection, sql, new KeyValuePair<string, object>(parameterName, parameterValue), commandTimeout);
		}

		public static async Task<int> ExecuteNonQueryAsync(this Microsoft.Data.SqlClient.SqlConnection connection, string sql, KeyValuePair<string, object> parameter, int? commandTimeout = null)
		{
			return await ExecuteNonQueryAsync(connection, sql, new[] { parameter }, commandTimeout);
		}

		public static async Task<int> ExecuteNonQueryAsync(this Microsoft.Data.SqlClient.SqlConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null, int? commandTimeout = null)
		{
			await connection.EnsureConnectionIsOpenAsync();

			using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
			{
				if (commandTimeout.HasValue)
				{
					command.CommandTimeout = commandTimeout.Value;
				}
				command.AddParameters(parameters);

				return await command.ExecuteNonQueryWithExceptionTracingAsync();
			}
		}

		public static async Task<TProperty> ExecuteScalarAsync<TProperty>(this Microsoft.Data.SqlClient.SqlConnection connection, string sql, string parameterName, object parameterValue, int? commandTimeout = null)
		{
			return await ExecuteScalarAsync<TProperty>(connection, sql, new KeyValuePair<string, object>(parameterName, parameterValue), commandTimeout);
		}

		public static async Task<TProperty> ExecuteScalarAsync<TProperty>(this Microsoft.Data.SqlClient.SqlConnection connection, string sql, KeyValuePair<string, object> parameter, int? commandTimeout = null)
		{
			return await ExecuteScalarAsync<TProperty>(connection, sql, new[] { parameter }, commandTimeout);
		}

		public static async Task<TProperty> ExecuteScalarAsync<TProperty>(this Microsoft.Data.SqlClient.SqlConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null, int? commandTimeout = null)
		{
			await connection.EnsureConnectionIsOpenAsync();

			using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
			{
				if (commandTimeout.HasValue)
				{
					command.CommandTimeout = commandTimeout.Value;
				}
				command.AddParameters(parameters);

				using (var dataReader = await command.ExecuteReaderWithExceptionTracingAsync())
				{
					if (await dataReader.ReadAsync())
					{
						var expectedType = typeof(TProperty);

						if (expectedType == typeof(string))
						{
							return (TProperty)(object)dataReader.GetString(0);
						}
						if (expectedType == typeof(Guid))
						{
							return (TProperty)(object)dataReader.GetGuid(0);
						}

						if (expectedType == typeof(int?))
						{
							return (TProperty)(object)dataReader.GetIntNullable(0);
						}
						if (expectedType == typeof(int))
						{
							return (TProperty)(object)dataReader.GetInt(0);
						}
						if (expectedType == typeof(bool?))
						{
							return (TProperty)(object)dataReader.GetBooleanNullable(0);
						}
						if (expectedType == typeof(bool))
						{
							return (TProperty)(object)dataReader.GetBoolean(0);
						}
						if (expectedType == typeof(long?))
						{
							return (TProperty)(object)dataReader.GetLongNullable(0);
						}
						if (expectedType == typeof(long))
						{
							return (TProperty)(object)dataReader.GetLong(0);
						}
						if (expectedType == typeof(double?))
						{
							return (TProperty)(object)dataReader.GetDoubleNullable(0);
						}
						if (expectedType == typeof(double))
						{
							return (TProperty)(object)dataReader.GetDouble(0);
						}
						if (expectedType == typeof(decimal?))
						{
							return (TProperty)(object)dataReader.GetDecimalNullable(0);
						}
						if (expectedType == typeof(decimal))
						{
							return (TProperty)(object)dataReader.GetDecimal(0);
						}
						if (expectedType == typeof(float?))
						{
							return (TProperty)(object)dataReader.GetFloatNullable(0);
						}
						if (expectedType == typeof(float))
						{
							return (TProperty)(object)dataReader.GetFloat(0);
						}
						if (expectedType == typeof(DateTime?))
						{
							return (TProperty)(object)dataReader.GetDateTimeNullable(0);
						}
						if (expectedType == typeof(DateTime))
						{
							return (TProperty)(object)dataReader.GetDateTime(0);
						}
						if (expectedType == typeof(TimeSpan?))
						{
							return (TProperty)(object)dataReader.GetTimeSpanNullable(0);
						}
						if (expectedType == typeof(TimeSpan))
						{
							return (TProperty)(object)dataReader.GetTimeSpan(0);
						}
						return (TProperty)dataReader[0];
					}
				}
			}

			return default(TProperty);
		}

		public static async Task<object> ExecuteScalarAsync(this Microsoft.Data.SqlClient.SqlConnection connection, string sql, string parameterName, object parameterValue, int? commandTimeout = null)
		{
			return await ExecuteScalarAsync(connection, sql, new KeyValuePair<string, object>(parameterName, parameterValue), commandTimeout);
		}

		public static async Task<object> ExecuteScalarAsync(this Microsoft.Data.SqlClient.SqlConnection connection, string sql, KeyValuePair<string, object> parameter, int? commandTimeout = null)
		{
			return await ExecuteScalarAsync(connection, sql, new[] { parameter }, commandTimeout);
		}

		public static async Task<object> ExecuteScalarAsync(this Microsoft.Data.SqlClient.SqlConnection connection, string sql, IEnumerable<KeyValuePair<string, object>> parameters = null, int? commandTimeout = null)
		{
			await connection.EnsureConnectionIsOpenAsync();

			using (var command = new Microsoft.Data.SqlClient.SqlCommand(sql, connection))
			{
				if (commandTimeout.HasValue)
				{
					command.CommandTimeout = commandTimeout.Value;
				}
				command.AddParameters(parameters);

				return await command.ExecuteScalarWithExceptionTracingAsync();
			}
		}
	}
}
