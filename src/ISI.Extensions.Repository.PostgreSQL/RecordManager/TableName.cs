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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;
using System.Linq.Expressions;
using ISI.Extensions.PostgreSQL.Extensions;
using ISI.Extensions.Repository.Extensions;
using ISI.Extensions.Repository.PostgreSQL.Extensions;
using Microsoft.Extensions.Configuration;

namespace ISI.Extensions.Repository.PostgreSQL
{
	public abstract partial class RecordManager<TRecord>
	{
		protected virtual string GetTableNameAlias(string alias = null)
		{
			if (string.IsNullOrWhiteSpace(alias))
			{
				alias = TableAlias;
			}

			alias = (string.IsNullOrWhiteSpace(alias) ? TableName : alias);

			return FormatTableNameAlias(alias);
		}

		protected virtual string FormatTableNameAlias(string alias)
		{
			return alias;
		}

		protected virtual string GetTableName(string alias = null, bool addAlias = true)
		{
			return FormatTableName(TableName, alias, addAlias);
		}

		protected virtual string FormatTableName(string tableName, string alias = null, bool addAlias = true)
		{
			if (string.IsNullOrWhiteSpace(alias) && (string.IsNullOrWhiteSpace(tableName) || string.Equals(tableName, TableName, StringComparison.InvariantCultureIgnoreCase)))
			{
				alias = TableAlias;
			}
			
			alias = FormatTableNameAlias(string.IsNullOrWhiteSpace(alias) ? TableName : alias);

			tableName = (string.IsNullOrWhiteSpace(TableNamePrefix) ? tableName : string.Format("{0}{1}", TableNamePrefix, tableName));

			if (string.IsNullOrWhiteSpace(Schema))
			{
				return string.Format("{0}{1}", tableName.PostgreSQLFormatName(), (addAlias ? string.Format(" {0}", alias) : string.Empty));
			}

			return string.Format("{0}{1}", string.Format("{0}.{1}", Schema.PostgreSQLFormatName(), tableName.PostgreSQLFormatName()), (addAlias ? string.Format(" {0}", alias) : string.Empty));
		}






		protected virtual string GetArchiveTableNameAlias(string alias = null)
		{
			if (string.IsNullOrWhiteSpace(alias))
			{
				alias = TableAlias;
			}

			alias = (string.IsNullOrWhiteSpace(alias) ? TableName : alias);

			return FormatArchiveTableNameAlias(string.Format("{0}{1}", alias, ArchiveTableSuffix));
		}

		protected virtual string FormatArchiveTableNameAlias(string alias)
		{
			return alias;
		}

		protected virtual string GetArchiveTableName(string alias = null, bool addAlias = true)
		{
			return FormatArchiveTableName(string.Format("{0}{1}", TableName, ArchiveTableSuffix), alias, addAlias);
		}

		protected virtual string FormatArchiveTableName(string tableName, string alias = null, bool addAlias = true)
		{
			if (string.IsNullOrWhiteSpace(alias) && (string.IsNullOrWhiteSpace(tableName) || string.Equals(tableName, TableName, StringComparison.InvariantCultureIgnoreCase)))
			{
				alias = TableAlias;
			}

			alias = FormatArchiveTableNameAlias(string.IsNullOrWhiteSpace(alias) ? tableName : alias);

			tableName = (string.IsNullOrWhiteSpace(TableNamePrefix) ? tableName : string.Format("{0}{1}", TableNamePrefix, tableName));

			if (string.IsNullOrWhiteSpace(Schema))
			{
				return string.Format("{0}{1}", tableName.PostgreSQLFormatName(), (addAlias ? string.Format(" {0}", alias) : string.Empty));
			}

			return string.Format("{0}{1}", string.Format("{0}.{1}", Schema.PostgreSQLFormatName(), tableName.PostgreSQLFormatName()), (addAlias ? string.Format(" {0}", alias) : string.Empty));
		}
	}
}