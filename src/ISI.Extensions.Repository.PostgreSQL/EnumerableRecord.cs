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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.PostgreSQL.Extensions;
using ISI.Extensions.Repository.PostgreSQL.Extensions;

namespace ISI.Extensions.Repository.PostgreSQL
{
	public delegate Npgsql.NpgsqlParameter GetSqlParameter(string parameterName, object parameterValue);

	public class EnumerableRecord<TRecord> : IEnumerable<TRecord>
		where TRecord : class, new()
	{
		protected ISI.Extensions.JsonSerialization.IJsonSerializer Serializer { get; }
		protected Npgsql.NpgsqlConnection Connection { get; }
		protected string Sql { get; }
		protected IDictionary<string, object> Parameters { get; }
		protected GetSqlParameter GetSqlParameter { get; }

		public EnumerableRecord(
			ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
			Npgsql.NpgsqlConnection connection, 
			string sql, 
			IDictionary<string, object> parameters, 
			GetSqlParameter getSqlParameter)
		{
			Serializer = serializer;
			Connection = connection;
			Sql = sql;
			Parameters = parameters;
			GetSqlParameter = getSqlParameter;
		}

		public class EnumeratorRecord : IEnumerator<TRecord>
		{
			protected ISI.Extensions.JsonSerialization.IJsonSerializer Serializer { get; }
			protected Npgsql.NpgsqlConnection Connection { get; set; }
			protected string Sql { get; }
			protected IDictionary<string, object> Parameters { get; }
			protected GetSqlParameter GetSqlParameter { get; }

			protected Npgsql.NpgsqlCommand Command { get; set; }
			protected Npgsql.NpgsqlDataReader DataReader { get; set; }

			private Reader<TRecord> _reader = null;

			public EnumeratorRecord(
				ISI.Extensions.JsonSerialization.IJsonSerializer serializer,
				Npgsql.NpgsqlConnection connection, 
				string sql,
				IDictionary<string, object> parameters, 
				GetSqlParameter getSqlParameter)
			{
				Serializer = serializer;
				Connection = connection;
				Sql = sql;
				Parameters = parameters;
				GetSqlParameter = getSqlParameter;

				Reset();
			}

			public void Reset()
			{
				Current = null;

				DataReader?.Close();
				DataReader?.Dispose();
				DataReader = null;

				Command?.Dispose();
				Command = null;
			}

			public TRecord Current { get; protected set; }


			public bool MoveNext()
			{
				if (Current == null)
				{
					Connection.EnsureConnectionIsOpenAsync().Wait();

					Command = new(Sql, Connection);

					Command.AddParameters(Parameters);

					DataReader = Command.ExecuteReaderWithExceptionTracingAsync().GetAwaiter().GetResult();
				}

				if (DataReader.Read())
				{
					var reader = _reader ??= ExpressionBuilder.GetReader(DataReader, RecordDescription.GetRecordDescription<TRecord>().PropertyDescriptions, Serializer);

					Current = reader(DataReader);
					return true;
				}

				Current = null;
				return false;
			}

			object IEnumerator.Current => Current;

			void IDisposable.Dispose()
			{
				Reset();

				Connection?.Close();
				Connection?.Dispose();
				Connection = null;
			}
		}

		public IEnumerator<TRecord> GetEnumerator() => new EnumeratorRecord(Serializer, Connection, Sql, Parameters, GetSqlParameter);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
