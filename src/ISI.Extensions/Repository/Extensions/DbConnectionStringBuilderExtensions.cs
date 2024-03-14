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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Repository.Extensions
{
	public static class DbConnectionStringBuilderExtensions
	{
		public static string GetValue(this System.Data.Common.DbConnectionStringBuilder dbConnectionStringBuilder, string key, bool removeWhenFound = false)
		{
			return GetValue(dbConnectionStringBuilder, new[] { key }, removeWhenFound);
		}

		public static string GetValue(this System.Data.Common.DbConnectionStringBuilder dbConnectionStringBuilder, string[] keys, bool removeWhenFound = false)
		{
			if ((keys != null) && keys.Any())
			{
				foreach (var key in keys.Where(key => !string.IsNullOrWhiteSpace(key)))
				{
					if (dbConnectionStringBuilder.TryGetValue(key, out var value))
					{
						if (removeWhenFound)
						{
							dbConnectionStringBuilder.Remove(key);
						}

						return string.Format("{0}", value);
					}
				}
			}

			return null;
		}

		public static string GetServerName(this System.Data.Common.DbConnectionStringBuilder dbConnectionStringBuilder) => GetValue(dbConnectionStringBuilder, new[] { "Data Source", "Server", "Address", "Addr", "Network Address", "Host" });
		public static int? GetServerPort(this System.Data.Common.DbConnectionStringBuilder dbConnectionStringBuilder) => dbConnectionStringBuilder.GetServerName()?.Split(new[] { ':' })?.LastOrDefault()?.ToIntNullable();
		public static string GetDatabaseName(this System.Data.Common.DbConnectionStringBuilder dbConnectionStringBuilder) => GetValue(dbConnectionStringBuilder, new[] { "Database", "Initial Catalog" });
		public static string GetUserName(this System.Data.Common.DbConnectionStringBuilder dbConnectionStringBuilder) => GetValue(dbConnectionStringBuilder, new[] { "UID", "User ID", "UserName" });
		public static string GetPassword(this System.Data.Common.DbConnectionStringBuilder dbConnectionStringBuilder) => GetValue(dbConnectionStringBuilder, new[] { "Password", "Pwd" });
	}
}
