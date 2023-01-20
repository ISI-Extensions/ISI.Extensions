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
using System.Threading.Tasks;

namespace ISI.Extensions.Repository
{
	public abstract partial class RecordManager<TRecord> : ISI.Extensions.Repository.IRecordManager<TRecord>
		where TRecord : class, IRecordManagerRecord, new()
	{
		protected Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }
		protected Microsoft.Extensions.Logging.ILogger Logger { get; }
		protected ISI.Extensions.DateTimeStamper.IDateTimeStamper DateTimeStamper { get; }
		protected ISI.Extensions.JsonSerialization.IJsonSerializer Serializer { get; }

		protected RecordManager(
			Microsoft.Extensions.Configuration.IConfiguration configuration,
			Microsoft.Extensions.Logging.ILogger logger,
			ISI.Extensions.DateTimeStamper.IDateTimeStamper dateTimeStamper,
			ISI.Extensions.JsonSerialization.IJsonSerializer serializer)
		{
			Configuration = configuration;
			Logger = logger;
			DateTimeStamper = dateTimeStamper;
			Serializer = serializer;
		}

		protected virtual void AddParameters<TValue>(IDictionary<string, object> parameters, string valueName, TValue[] values)
		{
			var index = 0;
			foreach (var value in values)
			{
				parameters.Add(string.Format("@{0}{1}", valueName, index++), value);
			}
		}

		protected virtual string IncludeParameters<TValue>(string valueName, IEnumerable<TValue> values)
		{
			return string.Join(", ", Enumerable.Range(0, values.Count()).Select(index => string.Format("@{0}{1}", valueName, index)));
		}

		protected virtual string DefaultOrderByClause => string.Empty;

		public abstract void CreateTable(CreateTableMode createTableMode = CreateTableMode.ErrorIfExists);
		public abstract void DropTable();
	}
}