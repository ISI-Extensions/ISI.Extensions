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
using ISI.Extensions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using DTOs = ISI.Platforms.AspNetCore.Users.Models.Users.SerializableModels;

namespace ISI.Platforms.AspNetCore.Users.Controllers
{
	public partial class UsersController
	{
		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[UsersAuthorize]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Users.RouteNames.GetUsers, typeof(Routes.Users), "get-users")]
		[Microsoft.AspNetCore.Mvc.ProducesResponseType(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, Type = typeof(DTOs.GetUsersResponse))]
		[ApiExplorerSettings(IgnoreApi = true)]
		public async Task<Microsoft.AspNetCore.Mvc.IActionResult> GetUsersAsync(int? offset, int? limit, string search, System.Threading.CancellationToken cancellationToken = default)
		{
			var response = new DTOs.GetUsersResponse();

			var listRolesResponse = await AuthorizationApi.ListRolesAsync(new(), cancellationToken);

			var listUsersResponse = await AuthenticationApi.ListUsersAsync(new(), cancellationToken);

			var userRoleLookup = listRolesResponse.Roles.ToNullCheckedDictionary(role => role.Role, role => role.Description, NullCheckDictionaryResult.Empty);

			var users = listUsersResponse.Users.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			response.TotalNotFiltered = users.Length;
			response.Total = response.TotalNotFiltered;

			users = users
				.OrderBy(user => user.LastName, StringComparer.InvariantCultureIgnoreCase).ThenBy(user => user.FirstName, StringComparer.InvariantCultureIgnoreCase)
				.Skip(offset ?? 0).Take(limit ?? 20).ToArray();
			
			response.Rows = users.ToNullCheckedArray(user => new DTOs.GetUsersResponseUser()
			{
				UserUuid = user.UserUuid,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Roles = string.Join(", ", user.Roles.Select(role =>
					{
						if (userRoleLookup.TryGetValue(role, out var roleDescription))
						{
							return roleDescription;
						}

						return null;
					})
					.Where(roleDescription => !string.IsNullOrWhiteSpace(roleDescription))
					.OrderBy(roleDescription => roleDescription, StringComparer.InvariantCultureIgnoreCase)),
			});

			return Ok(response);
		}
	}
}