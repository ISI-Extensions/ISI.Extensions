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

namespace ISI.Platforms.AspNetCore.Users.Controllers
{
	public partial class UsersController
	{
		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Get))]
		[UsersAuthorize]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Users.RouteNames.UserWithUserUuid, typeof(Routes.Users), "user/{userUuid}")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> UserAsync(Guid userUuid, System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new Models.Users.UserModel();

			var getUsersResponse = await AuthenticationApi.GetUsersAsync(new()
			{
				UserUuids = [userUuid],
			}, cancellationToken);

			var user = getUsersResponse.Users.NullCheckedFirstOrDefault();

			if (user == null)
			{
				return NotFound();
			}

			var listRolesResponse = await AuthorizationApi.ListRolesAsync(new(), cancellationToken);

			var findApiKeysResponse = await AuthenticationApi.FindApiKeysByUserUuidAsync(new()
			{
				UserUuids = [userUuid],
			}, cancellationToken);

			viewModel.EditedUser = new Models.Users.EditModels.UserEditModel()
			{
				UserUuid = user.UserUuid,
				FirstName = user?.FirstName,
				LastName = user?.LastName,
				EmailAddress = user?.EmailAddress,
				Roles = listRolesResponse.Roles.ToNullCheckedArray(userRole => new Models.Users.EditModels.UserRoleEditModel()
				{
					Role = userRole.Role,
					HasRole = user.Roles.Contains(userRole.Role),
					InheritedRole = user?.Roles?.Contains(userRole.Role) ?? false,
				}),
				IsActive = user.IsActive,
			};

			viewModel.UserAuthenticationTypeLookup = (await AuthenticationApi.ListUserAuthenticationTypesAsync(new(), cancellationToken)).UserAuthenticationTypes.ToNullCheckedDictionary(userAuthenticationType => userAuthenticationType.UserAuthenticationTypeUuid, userAuthenticationType => userAuthenticationType.Description, NullCheckDictionaryResult.Empty);

			viewModel.UserAuthentications = (await AuthenticationApi.FindUserAuthenticationsAsync(new() { UserUuids = [user.UserUuid] }, cancellationToken)).UserAuthentications.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			viewModel.ApiKeys = findApiKeysResponse.ApiKeys.ToNullCheckedArray(NullCheckCollectionResult.Empty);

			return result ?? View(T4Files.Views.Users.User_cshtml, viewModel);
		}

		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Post))]
		[UsersAuthorize]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Users.RouteNames.User, typeof(Routes.Users), "user")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> UserSaveAsync(Models.Users.EditModels.UserEditModel editedUser, System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new Models.Users.UserModel()
			{
				EditedUser = editedUser,
			};

			var getUsersResponse = await AuthenticationApi.GetUsersAsync(new()
			{
				UserUuids = [editedUser.UserUuid],
			}, cancellationToken);

			var user = getUsersResponse.Users.NullCheckedFirstOrDefault();

			if (user == null)
			{
				return NotFound();
			}

			user.Roles = editedUser.Roles.NullCheckedWhere(role => role.HasRole, NullCheckCollectionResult.Empty).ToNullCheckedArray(role => role.Role);

			user.IsActive = editedUser.IsActive;

			var setUsersResponse = await AuthenticationApi.SetUsersAsync(new()
			{
				Users = [user],
				RequestedByUserKey = GetUserUuid().GetValueOrDefault().Formatted(GuidExtensions.GuidFormat.WithHyphens),
			}, cancellationToken);


			user = setUsersResponse.Users.NullCheckedFirstOrDefault();

			if (user?.UserUuid != Guid.Empty)
			{
				result = RedirectToRoute(Routes.Users.RouteNames.Index);
			}

			return result ?? await UserAsync(editedUser.UserUuid, cancellationToken);
		}
	}
}