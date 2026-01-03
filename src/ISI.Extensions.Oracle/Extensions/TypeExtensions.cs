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

namespace ISI.Extensions.Oracle.Extensions
{
	public static class TypeExtensions
	{
		public static global::Oracle.ManagedDataAccess.Client.OracleDbType GetOracleDbType(this Type type, bool throwExceptionIfTypeConversionNotFound = true)
		{
			global::Oracle.ManagedDataAccess.Client.OracleDbType result;

			var isNullable = (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));

			if (isNullable)
			{
				type = (new System.ComponentModel.NullableConverter(type)).UnderlyingType;
			}

			if (type == typeof(string))
			{
				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2;
			}
			else if (type == typeof(Guid))
			{
				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.Raw;
			}
			else if (type == typeof(bool))
			{
				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.Boolean;
			}
			else if (type == typeof(int))
			{
				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.Int32;
			}
			else if (type == typeof(long))
			{
				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.Int64;
			}
			else if (type == typeof(double))
			{
				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.Double;
			}
			else if (type == typeof(float))
			{
				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.Double;
			}
			else if (type == typeof(decimal))
			{
				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.Decimal;
			}
			else if (type == typeof(DateTime))
			{
				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.TimeStamp;
			}
			else
			{
				if (throwExceptionIfTypeConversionNotFound)
				{
					throw new ArgumentOutOfRangeException(nameof(type), $"Unsupported type: {type.FullName}");
				}

				result = global::Oracle.ManagedDataAccess.Client.OracleDbType.Object;
			}

			return result;
		}
	}
}
