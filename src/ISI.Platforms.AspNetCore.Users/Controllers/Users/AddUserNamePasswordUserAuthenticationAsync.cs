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
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Users.RouteNames.AddUserNamePasswordUserAuthenticationWithUserUuid, typeof(Routes.Users), "add-user-name-password-user-authentication/{userUuid}")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> AddUserNamePasswordUserAuthenticationAsync(Guid userUuid, System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new Models.Users.AddUserNamePasswordUserAuthenticationModel()
			{
				EditedUserNamePasswordUserAuthentication = new(),
			};

			var getUsersResponse = await AuthenticationApi.GetUsersAsync(new()
			{
				UserUuids = [userUuid],
			}, cancellationToken);

			viewModel.User = getUsersResponse.Users.NullCheckedFirstOrDefault();

			if (viewModel.User == null)
			{
				return NotFound();
			}

			viewModel.EditedUserNamePasswordUserAuthentication = new Models.Users.EditModels.AddUserNamePasswordUserAuthenticationEditModel()
			{
				UserUuid = userUuid,
			};

			return result ?? View(T4Files.Views.Users.AddUserNamePasswordUserAuthentication_cshtml, viewModel);
		}

		[Microsoft.AspNetCore.Mvc.AcceptVerbs(nameof(Microsoft.AspNetCore.Http.HttpMethods.Post))]
		[UsersAuthorize]
		[ISI.Extensions.AspNetCore.NamedRoute(Routes.Users.RouteNames.AddUserNamePasswordUserAuthentication, typeof(Routes.Users), "add-user-name-password-user-authentication")]
		[ApiExplorerSettings(IgnoreApi = true)]
		public virtual async Task<Microsoft.AspNetCore.Mvc.IActionResult> AddUserNamePasswordUserAuthenticationSaveAsync(Models.Users.EditModels.AddUserNamePasswordUserAuthenticationEditModel editedUserNamePasswordUserAuthentication, System.Threading.CancellationToken cancellationToken = default)
		{
			Microsoft.AspNetCore.Mvc.IActionResult result = null;

			var viewModel = new Models.Users.AddUserNamePasswordUserAuthenticationModel()
			{
				EditedUserNamePasswordUserAuthentication = editedUserNamePasswordUserAuthentication,
			};

			if (!string.IsNullOrWhiteSpace(editedUserNamePasswordUserAuthentication?.UserAuthenticationKey) && !string.IsNullOrWhiteSpace(editedUserNamePasswordUserAuthentication?.Password) && string.Equals(editedUserNamePasswordUserAuthentication.Password, editedUserNamePasswordUserAuthentication.ConfirmPassword, StringComparison.InvariantCulture))
			{
				await AuthenticationApi.CreateUserAuthenticationAsync(new()
				{
					UserUuid = editedUserNamePasswordUserAuthentication.UserUuid,
					UserAuthenticationKey = editedUserNamePasswordUserAuthentication.UserAuthenticationKey,
					Password = editedUserNamePasswordUserAuthentication.Password,
					RequestedByUserKey = GetUserUuid(),
				}, cancellationToken);
			}

			return RedirectToRoute(Routes.Users.RouteNames.UserWithUserUuid, new { userUuid = editedUserNamePasswordUserAuthentication.UserUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens) });
		}
	}
}